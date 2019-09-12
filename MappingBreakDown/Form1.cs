using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Drawing;
using HierarchicalGrid;
using System.Diagnostics;

namespace MappingBreakDown
{
    public partial class MappingBreakDown : Form
    {

        public XmlSerializer xs;
        
        private DataGridSource GridSource;
        List<string> displayColumns;
        
        private TableManager tbMan;

        List<int> indices;
        List<Tuple<int, int>> index_pairs;

        public MappingBreakDown()
        {
            InitializeComponent();
        }

        private void MappingBreakDown_Shown(object sender, EventArgs e)
        {
            InitFields();
            ErrorMessage.Text = "Message: ";
            InitDispColumns();

            tbMan = new TableManager(true);
            //
            TypeOpts.DataSource = tbMan.getTypeOpts();
            FPGAOpts.DataSource = tbMan.getFPGAOpts();
            SetTable();
            PathToFileLabel.Text = "Path: ";
        }

        private void InitDispColumns()
        {
            displayColumns = new List<string>();
            displayColumns.Add("Group");
            displayColumns.Add("Name");
            displayColumns.Add("Address");
            displayColumns.Add("MAIS");
            displayColumns.Add("LSB");
            displayColumns.Add("MSB");
            displayColumns.Add("Type");
            displayColumns.Add("FPGA");
            displayColumns.Add("Init");
            displayColumns.Add("Comment");
            //
            displayColumns.Add("IsComment");
            displayColumns.Add("IsReserved");
            displayColumns.Add("IsValid");
            displayColumns.Add("Reason");
            displayColumns.Add("Index");
            displayColumns.Add("SecondaryIndex");
        }

        private void SetTable()
        {
            GridSource = new DataGridSource(
                tbMan.dsDataset,
                displayColumns,
                new List<GroupColumn>());   // group columns not really needed now

            hierarchicalGridView1.DataSource = GridSource;
            hierarchicalGridView1.Columns["Reason"].Visible = false;
            //
            hierarchicalGridView1.Columns["IsComment"].Visible = false;
            hierarchicalGridView1.Columns["IsReserved"].Visible = false;
            hierarchicalGridView1.Columns["IsValid"].Visible = false;
            hierarchicalGridView1.Columns["Index"].Visible = false;
            hierarchicalGridView1.Columns["SecondaryIndex"].Visible = false;
            expandNodes();
            reColorNodes();
        }

        /* Default values for each register */
        private void InitFields()
        {
            LSBOpts.Text = "0";
            MSBOpts.Text = "31";
            MAISOpts.Text = "0";
            TypeOpts.Text = "RD";
            FPGAOpts.Text = "G";
            InitText.Text = "";
            RegNameText.Text = "";
            CommentText.Text = "";
            //NewGroupText.Text = "";
            searchBox.Text = "";
        }

        /* Insert register to the table */
        private void InsertButton_Click(object sender, EventArgs e)
        {
            // add group to table, if not already in table
            tbMan.AddGroup(NewGroupText.Text);
            string grp = NewGroupText.Text;

            // If the IsComment checkbox is checked - add the comment
            if (IsCommentCheckBox.Checked)
            {
                tbMan.AddComment(grp, CommentText.Text);
                ErrorMessage.Text = "Message: A comment has been added";
                InitFields();
                SetTable();
                return;
            }

            // Validate syntactically
            if (!InputValidator())
                return;

            // init field may be empty, resort to default "0"
            string init_string;
            if (InitText.Text.Equals(""))
                init_string = "0";

            else
                init_string = InitText.Text;

            /* Find new address if register, if FIELD type then find 
            *       the register it belongs to using Form2.cs     
            */
            int address;

            if (!TypeOpts.Text.Equals("FIELD"))
            {
                // validate lsb & msb logic
                if (!TableManager.validateMSBLSB(
                    int.Parse(LSBOpts.Text),
                    int.Parse(MSBOpts.Text)))
                {
                    MessageBox.Show("Invalid MSB or LSB");
                    return;
                }

                // Find free address
                address = tbMan.FindAddress();
                if (address == -1)
                {
                    MessageBox.Show("Unable to add register, no free slot in memory");
                    return;
                }

                // add a register to the registers table
                tbMan.addRow(
                    RegNameText.Text,
                    MAISOpts.Text,
                    LSBOpts.Text,
                    MSBOpts.Text,
                    TypeOpts.Text,
                    FPGAOpts.Text,
                    init_string,
                    CommentText.Text,
                    grp,
                    address);
            }
            else
            {
                using (ChooseAddressPrompt prompt = new ChooseAddressPrompt(tbMan.getRegisters()))
                {
                    if (prompt.ShowDialog() == DialogResult.OK)
                    {
                        // validate lsb & msb logic
                        if (!tbMan.validateField(
                                tbMan.getRegisters()[prompt.Index],
                                int.Parse(LSBOpts.Text),
                                int.Parse(MSBOpts.Text)))
                        {
                            MessageBox.Show("Invalid MSB or LSB");
                            return;
                        }

                        address = prompt.Chosen_address;

                        // add a field to the fields table
                        tbMan.addRow(
                            RegNameText.Text,
                            MAISOpts.Text,
                            LSBOpts.Text,
                            MSBOpts.Text,
                            TypeOpts.Text,
                            FPGAOpts.Text,
                            init_string,
                            CommentText.Text,
                            grp,
                            int.Parse(tbMan.getRegisters()[prompt.Index].Field<string>("Address")));
                    }

                    else
                    {
                        ErrorMessage.Text = "Message: Operation Cancled";
                        return;
                    }
                }
            }
            
            if (init_string.Equals("0"))
                ErrorMessage.Text = "Message: Init field is empty, resort to default (0)";

            else if (TypeOpts.Text.Equals("FIELD"))
                ErrorMessage.Text = "Message: Field named " + RegNameText.Text + " was added";

            else
                ErrorMessage.Text = "Message: Register named " + RegNameText.Text + " was added";

            InitFields();
            SetTable();

        }

        private void expandNodes()
        {
            foreach (HierarchicalGridNode group_node in hierarchicalGridView1.Nodes)
            {
                group_node.Expand();
                foreach (HierarchicalGridNode register_node in group_node.Nodes)
                {
                    register_node.Expand();
                    foreach (HierarchicalGridNode field_node in register_node.Nodes)
                        field_node.Expand();
                }
            }
        }

        /* Check the register can be added to the chart */
        private bool InputValidator()
        {
            if (RegNameText.Text.Equals(""))
            {
                MessageBox.Show("Invalid register name: Empty name");
                return false;
            }

            if (Regex.IsMatch(RegNameText.Text, @"^\d"))
            {
                MessageBox.Show("Register name can't begin with a digit");
                return false;
            }

            if (!tbMan.nameValid(RegNameText.Text))
            {
                MessageBox.Show(
                    "Name "+ RegNameText.Text +" already in the list or is a reserved name");
                return false;
            }

            if (TypeOpts.Text.Equals("FIELD") && tbMan.regsCount() == 0)
            {
                MessageBox.Show("You must have at least one register to add a field");
                return false;
            }

            return true;
        }

        /* Open a file */
        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                TableManager OpenedtbMan = new TableManager(openFileDialog1.FileName);
                //
                if (OpenedtbMan.openFileSuccessfull())
                {
                    tbMan = OpenedtbMan;
                    TypeOpts.DataSource = tbMan.getTypeOpts();
                    FPGAOpts.DataSource = tbMan.getFPGAOpts();
                    //
                    PathToFileLabel.Text = "Path: " + Path.GetFileName(tbMan.getPathToFile());
                    Text = "MappingBreakDown" + " - " + Path.GetFileName(tbMan.getPathToFile());
                    //
                    SetTable();
                }
            }
        }

        /* Edit a register */
        private void Load_Click(object sender, EventArgs e)
        {
            getSelectedRows();

            if (! ((indices.Count == 1 && index_pairs.Count == 0) ||
                    (indices.Count == 0 && index_pairs.Count == 1)))
            {
                MessageBox.Show("Invalid selection, please select exactly one register or field");
                return;
            }

            DataRow selected;
            try
            {
                if (indices.Count == 1)
                    selected = tbMan.getRegisters()[indices.First()];

                else if (index_pairs.Count == 1)
                    selected = tbMan.getField(index_pairs.First());

                else
                    return;
            }
            catch (IndexOutOfRangeException t)
            {
                // then its just an arbirtaty error
                return;
            }

            if (!RegNameText.Text.Equals(selected.Field<string>("Name"))
                && !tbMan.nameValid(RegNameText.Text))
            {
                MessageBox.Show("Invalid Name, possible VHDL reserved name or duplicate");
                return;
            }
            
            // register msb & lsb logic check
            if (!selected.Field<string>("Type").Equals("FIELD"))
            {
                if (!TableManager.validateMSBLSB(
                    int.Parse(LSBOpts.Text),
                    int.Parse(MSBOpts.Text)))
                {
                    MessageBox.Show("Invalid MSB or LSB");
                    return;
                }
            }
            // Field msb and lsb are more complex
            else
            {
                if (!tbMan.validateField(
                        selected.GetParentRow("GroupsFieldsRelation"),
                        int.Parse(LSBOpts.Text),
                        int.Parse(MSBOpts.Text),
                        selected.Field<int>("SecondaryIndex")))
                {
                    MessageBox.Show("Invalid MSB or LSB");
                    return;
                }
            }
            
            if (!TypeOpts.Text.Equals(selected.Field<string>("Type")) &&
                (selected.Field<string>("Type").Equals("FIELD") ||
                TypeOpts.Text.Equals("FIELD")))
            {
                MessageBox.Show("Cannot change to type to FIELD or from FIELD");
                return;
            }

            // add group to table, if not already in table
            tbMan.AddGroup(NewGroupText.Text);
            string group = NewGroupText.Text;

            tbMan.setRow(
                selected,
                RegNameText.Text,
                CommentText.Text,
                InitText.Text,
                LSBOpts.Text,
                MSBOpts.Text,
                TypeOpts.Text,
                FPGAOpts.Text,
                group);

            ErrorMessage.Text = "Message: " + "Register was reloaded";

            SetTable();
        }

        private void ColorNode(HierarchicalGridNode node)
        {
            int index = (int)node.Cells[14].Value;
            int indexSec = (int)node.Cells[15].Value;

            bool[] bool_arr = tbMan.getColor(index, indexSec);

            for (int i = 0; i < hierarchicalGridView1.ColumnCount; i++)
            {
                if (bool_arr[2])        // IsReserved field
                    node.Cells[i].Style.BackColor = Color.Blue;

                else if (bool_arr[1])   // IsComment field
                    node.Cells[i].Style.BackColor = Color.LimeGreen;

                else if (!bool_arr[0])  //IsValid field
                    node.Cells[i].Style.BackColor = Color.Red;

                else
                    node.Cells[i].Style.BackColor = Color.White;
            }
        }

        private void reColorNodes()
        {
            foreach (HierarchicalGridNode group_node in hierarchicalGridView1.Nodes)
                foreach (HierarchicalGridNode register in group_node.Nodes)
                {
                    ColorNode(register);
                    foreach (HierarchicalGridNode field in register.Nodes)
                        ColorNode(field);
                }
        }

        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "VHD files (*.vhd)|*.vhd",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (tbMan.createAndDocument(Path.ChangeExtension(saveFileDialog1.FileName, null)))
                    MessageBox.Show("File was successfully saved!");
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            getSelectedRows();

            tbMan.RemoveEntries( index_pairs, indices);
            tbMan.validateLogic();

            InitFields();
            SetTable();
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            string searchRes = searchBox.Text;
            foreach (HierarchicalGridNode group_node in hierarchicalGridView1.Nodes)
            {
                group_node.Expand();
                foreach (HierarchicalGridNode reg in group_node.Nodes)
                {
                    reg.Expand();
                    foreach (HierarchicalGridNode field in reg.Nodes)
                        field.Visible = field.Cells["Name"].Value.ToString().Contains(searchRes);
                    
                    reg.Visible = reg.Cells["Name"].Value.ToString().Contains(searchRes);
                }
            }
        }

        private string GetSpaces(int x)
        {
            return string.Concat(Enumerable.Repeat(" ", x));
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (tbMan.getPathToFile().Equals(""))
            {
                SaveAsButton_Click(sender, e);
                return;
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            InitFields();
            tbMan = new TableManager(false);
            SetTable();
            //
            Text = "MappingBreakDown";
            PathToFileLabel.Text = "Path: ";
        }

        private void RegNameText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                InsertButton_Click(sender, e);
        }

        private void NewGroupText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                tbMan.AddGroup(NewGroupText.Text);
        }

        private void CommentText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                InsertButton_Click(sender, e);
        }

        private void InitText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                InsertButton_Click(sender, e);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (tbMan.getPathToFile().Equals(""))
                Close();
            if (tbMan.saved)
            {
                //tbMan.path_to_file = "";
                File.WriteAllText(@"file_path.txt", "");
                Close();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to close the file without saving?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //SaveButton_Click(sender, e);
                    //tbMan.path_to_file = "";
                    File.WriteAllText(@"file_path.txt", "");
                    Close();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do nothing
                }
            }
        }

        private void HierarchialGridView1_SelectionChanged(object sender, EventArgs e)
        {
            getSelectedRows();

            if (!((indices.Count == 1 && index_pairs.Count == 0) ||
                    (indices.Count == 0 && index_pairs.Count == 1)))
                return;

            DataRow selected;
            try
            {
                if (indices.Count == 1)
                    selected = tbMan.getRegisters()[indices.First()];

                else if (index_pairs.Count == 1)
                    selected = tbMan.getField(index_pairs.First());

                else
                    return;
                
            }
            catch (IndexOutOfRangeException t)
            {
                // then its just an arbirtaty error
                return;
            }
            
            RegNameText.Text = selected.Field<string>("Name");
            CommentText.Text = selected.Field<string>("Comment");
            InitText.Text = selected.Field<string>("Init");
            LSBOpts.Text = selected.Field<string>("LSB");
            MSBOpts.Text = selected.Field<string>("MSB");
            TypeOpts.Text = selected.Field<string>("Type");
            FPGAOpts.Text = selected.Field<string>("FPGA");
            NewGroupText.Text = selected.Field<string>("Group");

            ErrorMessage.Text = "Message: " + selected.Field<string>("Reason");

        }

        private void HierarchialGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                Delete_Click(sender, e);
        }

        private void frm_sizeChanged(object sender, EventArgs e)
        {
            Size margin_panel_size = new Size(Width - 40,Height - 80);
            Size table_size = new Size(margin_panel_size.Width-6, margin_panel_size.Height - 320);
            panel5.Size = margin_panel_size;
            hierarchicalGridView1.Size = table_size;
            ClearButton.Location = new Point(Width - 70, ClearButton.Location.Y);
        }

        private void getSelectedRows()
        {
            indices = new List<int>();
            index_pairs = new List<Tuple<int, int>>();
            foreach (HierarchicalGridNode item in hierarchicalGridView1.SelectedRows)
            {
                try
                {
                    int index = (int)item.Cells["Index"].Value;
                    int secondary_index = (int)item.Cells["SecondaryIndex"].Value;

                    if (secondary_index != -1)
                        index_pairs.Add(new Tuple<int, int>(index, secondary_index));

                    else
                        indices.Add(index);
                }
                catch (Exception)
                {
                    //nothing
                }
            }
        }

        private void CommentButton_Click(object sender, EventArgs e)
        {
            getSelectedRows();

            tbMan.setRowsType(indices, "Reserved");
            tbMan.setRowsType(index_pairs, "Reserved");

            searchBox.Text = "";
            //
            SetTable();
        }

        private void UnCommentButton_Click(object sender, EventArgs e)
        {
            getSelectedRows();

            tbMan.setRowsType(indices, "UnReserved");
            tbMan.setRowsType(index_pairs, "UnReserved");

            tbMan.validateLogic();

            searchBox.Text = "";

            SetTable();
        }

        private void help_MenuButtonClick(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@"man\\MappingBreakDownMan.pdf");
            }
            catch(IOException t)
            {
                MessageBox.Show("Could not find manual");
            }
        }

        private void About_MenuButtonClick(object senedr, EventArgs e)
        {
            string text = "MappingPackageAutomation Version 2.0\n";
            text += "Created By Eran Marchesky and Eli Zeltser as a final project\n\n";
            text += "Advisors: Dan Shalom and Eli Parente\n";
            text += "Eli Zeltser: elizeltser97 @gmail.com\n";
            text += "Eran Marchesky: eranmarch@gmail.com";
            MessageBox.Show(text);
        }

        private void MappingBreakDown_FormClosing(object sender, FormClosingEventArgs e)
        {
            tbMan.UpdateDatabase();
        }

        private void CommentButton_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.CommentButton, "Comment selected rows");
        }

        private void UnCommentButton_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.UnCommentButton, "Un-Comment selected rows");
        }

        private void ClearButton_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.ClearButton, "Clear table");
        }

        private void PathToFileLabel_MouseHover(object sender, EventArgs e)
        {
            if (tbMan.getPathToFile().Equals(""))
                return;

            ToolTip ToolTip1 = new ToolTip();
            string tip = "Full path: " + tbMan.getPathToFile();
            ToolTip1.SetToolTip(this.PathToFileLabel, tip);
        }

        private void IsCommentCheckBox_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.IsCommentCheckBox, "Set as comment");
        }

        private void InsertButton_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.InsertButton, "Insert entry");
        }

        private void Delete_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.Delete, "Delete selected entries");
        }

        private void Load_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.Load, "Change selected entry");
        }

        private void label10_MouseHover(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(this.label10, "Search entries by name");
        }
    }
}
