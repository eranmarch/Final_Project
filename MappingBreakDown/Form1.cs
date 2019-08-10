using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using AdvancedDataGridView;
using System.Runtime;
using System.Drawing;
using HierarchicalGrid;
using System.Diagnostics;

namespace MappingBreakDown
{
    public partial class MappingBreakDown : Form
    {

        public XmlSerializer xs;
        List<RegisterEntry> RegList;
        bool saved = true;
        bool changed = false;

        DataTable dtgroups, dtregisters, dtfields;
        DataGridSource GridSource;
        List<string> displayColumns = new List<string>();
        List<GroupColumn> groupColumns = new List<GroupColumn>();

        public MappingBreakDown()
        {
            InitializeComponent();
            InitFields();
            ErrorMessage.Text = "Message: ";
            xs = new XmlSerializer(typeof(List<RegisterEntry>));
            InitDataBasesParams();
            ReadDataBase();
            ColorInValid();
        }

        private void InitDataBasesParams()
        {
            dtgroups = new DataTable();
            dtgroups.Columns.Add("Group", typeof(string));
            dtgroups.Rows.Add("");
            dtregisters = new DataTable();
            dtregisters.Columns.Add("Group", typeof(string));
            dtregisters.Columns.Add("Name", typeof(string));
            dtregisters.Columns.Add("Address", typeof(int));
            dtregisters.Columns.Add("MAIS", typeof(int));
            dtregisters.Columns.Add("LSB", typeof(int));
            dtregisters.Columns.Add("MSB", typeof(int));
            dtregisters.Columns.Add("Type", typeof(string));
            dtregisters.Columns.Add("FPGA", typeof(string));
            dtregisters.Columns.Add("Init", typeof(string));
            dtregisters.Columns.Add("Comment", typeof(string));
            dtregisters.Columns.Add("IsValid", typeof(bool));
            dtregisters.Columns.Add("Reason", typeof(string));
            dtregisters.Columns.Add("Index", typeof(int));
            dtregisters.Columns.Add("SecondaryIndex", typeof(int));
            dtfields = new DataTable();
            dtfields.Columns.Add("Group", typeof(string));
            dtfields.Columns.Add("Name", typeof(string));
            dtfields.Columns.Add("Address", typeof(int));
            dtfields.Columns.Add("MAIS", typeof(int));
            dtfields.Columns.Add("LSB", typeof(int));
            dtfields.Columns.Add("MSB", typeof(int));
            dtfields.Columns.Add("Type", typeof(string));
            dtfields.Columns.Add("FPGA", typeof(string));
            dtfields.Columns.Add("Init", typeof(string));
            dtfields.Columns.Add("Comment", typeof(string));
            dtfields.Columns.Add("IsValid", typeof(bool));
            dtfields.Columns.Add("Reason", typeof(string));
            dtfields.Columns.Add("Index", typeof(int));
            dtfields.Columns.Add("SecondaryIndex", typeof(int));
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
            displayColumns.Add("IsValid");
            displayColumns.Add("Reason");
            displayColumns.Add("Index");
            displayColumns.Add("SecondaryIndex");
            DataSet dsDataset = new DataSet();
            dsDataset.Tables.Add(dtgroups);
            dsDataset.Tables.Add(dtregisters);
            dsDataset.Tables.Add(dtfields);
            DataRelation groupsRegsRelation = new DataRelation("GroupsRegistersRelation", dsDataset.Tables[0].Columns["Group"], dsDataset.Tables[1].Columns["Group"], true);
            DataRelation regsFieldsRelation = new DataRelation("GroupsFieldsRelation", dsDataset.Tables[1].Columns["Index"], dsDataset.Tables[2].Columns["Index"], true);
            groupsRegsRelation.Nested = true;
            regsFieldsRelation.Nested = true;
            dsDataset.Relations.Add(groupsRegsRelation);
            dsDataset.Relations.Add(regsFieldsRelation);
            GridSource = new DataGridSource(dsDataset, displayColumns, groupColumns);
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
            RegGroupOpts.SelectedIndex = 0;
            searchBox.Text = "";
        }

        /* Upon Insert to the table, allocate a new address to the register */
        private int FindAddress()
        {
            int i = 1, y;
            bool found;
            for (; i <= 1024; i++)
            {
                found = true;
                foreach (DataRow x in dtregisters.Rows)
                {
                    y = x.Field<int>("Address");
                    if (y == i)
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                    return i;
            }
            return -1;
        }

        /* Add a new group */
        private void AddGroupButton_Click(object sender, EventArgs e)
        {
            if (NewGroupText.Text.Equals(""))
                return;
            foreach (DataRow grp in dtgroups.Rows)
            {
                string item = grp.Field<string>("Group");
                if (item.Equals(NewGroupText.Text))
                {
                    MessageBox.Show("Group " + NewGroupText.Text + " already exists");
                    NewGroupText.Text = "";
                    return;
                }
            }
            dtgroups.Rows.Add(NewGroupText.Text);
            //RegGroupOpts.DataSource = dtgroups.AsEnumerable().Select(r => r.Field<string>("Group")).ToList();
            RegGroupOpts.Items.Add(NewGroupText.Text);
            hierarchicalGridView1.DataSource = GridSource;
            NewGroupText.Text = "";
        }

        private bool CheckDup(RegisterEntry new_entry)
        {
            int addr_new = new_entry.GetAddress();
            string name_new = new_entry.GetName();
            foreach (RegisterEntry item in RegList)
            {
                //foreach (DataRow row in dtregisters.Rows)
                //{
                    //RegisterEntry item = new RegisterEntry(row);
                    if (item.GetIsComment() || item == new_entry)
                        continue;
                    if (item.GetName().Equals(name_new))
                    {
                        new_entry.SetReason("Name " + name_new + " is already in the list at address " + item.GetAddress().ToString());
                        new_entry.SetValid(false);
                        return false;
                    }
                    if (item.GetAddress() == addr_new)
                    {
                        new_entry.SetReason("Address " + addr_new + " is already in the list at register " + item.GetName());
                        new_entry.SetValid(false);
                        return false;
                    }
                //}
            }
            return true;
        }

        /* Validate Opened file */
        private void OpenValidation()
        {
            foreach (RegisterEntry new_entry in RegList)
                if (CheckDup(new_entry))
                    if (FileValidator.ValidEntry(new_entry))
                    {
                        new_entry.SetValid(true);
                        new_entry.SetReason("");
                    }
            ColorInValid();
        }

        /* Check the a register can be added to the chart */
        private bool InputValidation(RegisterEntry entry)
        {
            if (entry.GetName()[0] >= '0' && entry.GetName()[0] <= '9')
            {
                MessageBox.Show("Register name can't begin with a digit");
                return false;
            }
            if (!entry.GetRegType().Equals("FIELD"))
            {
                int index = -1;
                for (int i = 0; i < RegList.Count; i++)
                    if (RegList[i].GetName().Equals(entry.GetName()))
                        index = i;
                if (index != -1)
                {
                    MessageBox.Show("Register " + entry.GetName() + " (" + RegList[index].GetAddress() + ") is already in the list");
                    return false;
                }
                int addr = FindAddress();
                if (addr == -1)
                {
                    MessageBox.Show("Unable to add register " + entry.GetName() + ", no free slot in memory");
                    return false;
                }
                entry.SetAddress(addr);
            }
            else
            {
                if (RegList.Count == 0)
                {
                    MessageBox.Show("There are no registers in the list");
                    return false;
                }
                int addr = -1, index = -1;
                RegisterEntry item;
                using (ChooseAddressPrompt prompt = new ChooseAddressPrompt(RegList.ToArray()))
                {
                    if (prompt.ShowDialog() == DialogResult.OK)
                    {
                        addr = prompt.Chosen_address;
                        index = prompt.Index;
                        item = RegList[prompt.Index];
                    }
                    else
                        return false;
                }
                List<RegisterEntry> fields = item.GetFields();
                foreach (RegisterEntry field in fields)
                    if (field.GetName().Equals(entry.GetName()))
                    {
                        MessageBox.Show("Field " + entry.GetName() + " (" + item.GetAddress() + ") is already in the list of " + item.GetName());
                        return false;
                    }
                entry.SetAddress(addr);
                entry.SetIndex(index);
            }

            if (!entry.IsValidLsbMsb())
            {
                MessageBox.Show("Can't insert register " + entry.GetName() + " with LSB greater than MSB");
                return false;
            }
            return true;
        }

        /* Insert register to the table */
        private void InsertButton_Click(object sender, EventArgs e)
        {
            if (RegNameText.Text.Equals(""))
            {
                MessageBox.Show("Invalid register name: Empty name");
                InitFields();
                return;
            }
            RegisterEntry entry;
            if (InitText.Text.Equals(""))
            {
                ErrorMessage.Text = "Message: Init field is empty, resort to default (0)";
                entry = new RegisterEntry(RegNameText.Text, -1, MAISOpts.Text, LSBOpts.Text, MSBOpts.Text, TypeOpts.Text, FPGAOpts.Text, "0", CommentText.Text, RegGroupOpts.Text);
            }
            else
            {
                entry = new RegisterEntry(RegNameText.Text, -1, MAISOpts.Text, LSBOpts.Text, MSBOpts.Text, TypeOpts.Text, FPGAOpts.Text, InitText.Text, CommentText.Text, RegGroupOpts.Text);
            }
            if (!InputValidation(entry))
                return;
            AddEntryToTable(entry);
            changed = true;
            ErrorMessage.Text = "Message: Register named " + RegNameText.Text + " was added";
            InitFields();
            bool finish = false;
            foreach (HierarchicalGridNode group_node in hierarchicalGridView1.Nodes)
            {
                if (group_node.Cells["Group"].Value.ToString().Equals(entry.Group))
                {
                    group_node.Expand();
                    foreach (HierarchicalGridNode register_node in group_node.Nodes)
                    {
                        bool finish_inner = false;
                        if (register_node.Cells["Index"].Value.Equals(entry.Index)){
                            if (entry.SecondaryIndex != -1) // field
                            {
                                register_node.Expand();
                                foreach (HierarchicalGridNode field_node in register_node.Nodes)
                                {
                                    if (field_node.Cells["SecondaryIndex"].Value.Equals(entry.SecondaryIndex))
                                    {
                                        hierarchicalGridView1.FirstDisplayedScrollingRowIndex = field_node.Index + register_node.Index + group_node.Index;
                                        finish = true;
                                        finish_inner = true;
                                        break;
                                    }
                                }
                            }
                            else // register
                            {
                                hierarchicalGridView1.FirstDisplayedScrollingRowIndex = register_node.Index + group_node.Index;
                                finish = true;
                                break;
                            }
                        }
                        if (finish_inner)
                            break;
                    }
                }
                if (finish)
                    break;
            }
            saved = false;
        }

        /* Open a file */
        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileValidator fv = new FileValidator(openFileDialog1.FileName);
                //fv.IsFileValid1();
                if (fv.IsFileValid1())
                {
                    TypeOpts.DataSource = fv.f_type;
                    FPGAOpts.DataSource = fv.field_type1;
                    if (changed)
                    {
                        DialogResult res = MessageBox.Show("Would you like to clear current table (without saving!)", "Warning", MessageBoxButtons.YesNo);
                        if (res == DialogResult.Yes)
                            Clear_Click(sender, e);
                    }
                    PathToFile.Text = openFileDialog1.FileName;
                    this.Text = openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\")) + " - MappingBreakDown";
                    File.WriteAllText(@"file_path.txt", openFileDialog1.FileName);
                    AddManyRegisters(fv.GetRegList(), fv.GetGroups());
                }
            }
        }

        /* Edit a register */
        private void Load_Click(object sender, EventArgs e)
        {
            RegisterEntry re = null;
            HierarchicalGridNode node = null;
            foreach (HierarchicalGridNode item in hierarchicalGridView1.SelectedRows)
            {
                try
                {
                    re = RegList[(int)item.Cells["Index"].Value];
                    int index = (int)item.Cells["SecondaryIndex"].Value;
                    if (index != -1)
                        re = re.GetFields()[index];
                    node = item;
                    break;
                }
                catch (NullReferenceException)
                {
                    //do nothing for groups
                    return;
                }
            }
            if (re == null)
            {
                MessageBox.Show("Please select a register in order to edit");
                return;
            }
            string name = RegNameText.Text;
            if (!re.GetName().Equals(name))
            {
                MessageBox.Show("You can't edit a register's name");
                RegNameText.Text = re.GetName();
                return;
            }
            string mais = MAISOpts.Text;
            string lsb = LSBOpts.Text;
            string msb = MSBOpts.Text;
            string type = TypeOpts.Text;
            string fpga = FPGAOpts.Text;
            string init = InitText.Text;
            string comment = CommentText.Text;
            string group = RegGroupOpts.Text;
            RegisterEntry entry;
            int i = re.GetIndex(), j = re.GetSecondaryIndex();
            if (i == -1)
            {
                MessageBox.Show("No such register " + name);
                InitFields();
                return;
            }
            entry = RegList[i];
            if (j != -1)
                entry = entry.GetFields()[j];
            if (entry.GetIsComment())
            {
                MessageBox.Show("This register is a comment and can't be edited");
                return;
            }
            /*Enum.TryParse(type, out RegisterEntry.type_field t);
            RegisterEntry.type_field s = entry.GetRegType();
            if ((s == RegisterEntry.type_field.FIELD && t != RegisterEntry.type_field.FIELD) ||
                s != RegisterEntry.type_field.FIELD && t == RegisterEntry.type_field.FIELD)*/
            if (entry.GetRegType().Equals("FIELD") && !type.Equals("FIELD")||
                !entry.GetRegType().Equals("FIELD") && type.Equals("FIELD"))
            {
                MessageBox.Show("Can't edit a field or create one using Load");
                //TypeOpts.SelectedIndex = (int)s;
                return;
            }
            if (!RegisterEntry.IsValidLsbMsb(msb, lsb))
            {
                MessageBox.Show("Can't edit an entry to have LSB > MSB");
                return;
            }
            entry.EditRegister(mais, lsb, msb, type, fpga, init, comment, group);
            OpenValidation();
            UpdateDataBase();
            EditCell(node, entry.GetTableEntry());
            saved = false;
        }

        private void ColorNode(HierarchicalGridNode node)
        {
            int index = (int)node.Cells[12].Value, indexSec = (int)node.Cells[13].Value;
            RegisterEntry entry = RegList[index];
            if (indexSec != -1)
                entry = entry.GetFields()[indexSec];
            for (int i = 0; i < hierarchicalGridView1.ColumnCount; i++)
                if (entry.GetIsComment())
                    node.Cells[i].Style.BackColor = Color.LimeGreen;
                else if (!entry.GetValid())
                    node.Cells[i].Style.BackColor = Color.Red;
                else
                    node.Cells[i].Style.BackColor = Color.White;
        }

        private void ColorInValid()
        {
            foreach (HierarchicalGridNode group_node in hierarchicalGridView1.Nodes)
                foreach (HierarchicalGridNode register in group_node.Nodes)
                {
                    ColorNode(register);
                    foreach (HierarchicalGridNode field in register.Nodes)
                        ColorNode(field);
                }
        }

        private void UpdateDataBase()
        {
            FileStream fs = new FileStream(@"registers.txt", FileMode.Create, FileAccess.Write);
            xs.Serialize(fs, RegList);
            fs.Close();
            File.WriteAllText(@"file_path.txt", PathToFile.Text);
        }

        private void ReadDataBase()
        {
            FileStream fs;
            try
            {
                fs = new FileStream(@"registers.txt", FileMode.Open, FileAccess.Read);
                RegList = (List<RegisterEntry>)xs.Deserialize(fs);
                fs.Close();
                UpdateTable();
            }
            catch (Exception e)
            {
                RegList = new List<RegisterEntry>();
                hierarchicalGridView1.Nodes.Add("");
            }
            try
            {
                PathToFile.Text = File.ReadAllText(@"file_path.txt");
            }
            catch (Exception e)
            {
                PathToFile.Text = "";
            }
        }

        private void EditCell(HierarchicalGridNode cell, object[] ent)
        {
            for (int i = 0; i < ent.Length; i++)
                cell.Cells[i].Value = ent[i];
        }

        private void UpdateTable()
        {
            string group;
            foreach (RegisterEntry entry in RegList)
            {
                group = entry.GetGroup();
                if (!RegGroupOpts.Items.Contains(group))
                {
                    RegGroupOpts.Items.Add(group);
                    dtgroups.Rows.Add(group);
                }
                //entry.Index = RegList.IndexOf(entry);
                object[] ent = entry.GetTableEntry();
                dtregisters.Rows.Add(ent);

                List<RegisterEntry> fields = entry.GetFields();
                foreach (RegisterEntry field in fields)
                {
                    //field.Index = entry.Index;
                    //field.SecondaryIndex = entry.GetFields().IndexOf(field);
                    dtfields.Rows.Add(field.GetTableEntry());
                }
            }
            //RegGroupOpts.DataSource = dtgroups.AsEnumerable().Select(r => r.Field<string>("Group")).ToList();
            
            hierarchicalGridView1.DataSource = GridSource;
            hierarchicalGridView1.Columns["Reason"].Visible = false;
            hierarchicalGridView1.Columns["IsValid"].Visible = false;
            hierarchicalGridView1.Columns["Index"].Visible = false;
            hierarchicalGridView1.Columns["SecondaryIndex"].Visible = false;
        }

        private void AddEntryToTable(RegisterEntry entry, bool open = false)
        {
            bool isField = entry.GetRegType().ToUpper().Equals("FIELD");
            if (isField)
            {
                RegList[entry.GetIndex()].AddField(entry);
                dtfields.Rows.Add(entry.GetTableEntry());
            }
            else
            {
                entry.SetIndex(RegList.Count); // only outer index
                dtregisters.Rows.Add(entry.GetTableEntry());
                RegList.Add(entry);
            }
            if (!open)
            {
                UpdateDataBase();
                hierarchicalGridView1.DataSource = GridSource;
            }
        }

        private void AddManyRegisters(List<RegisterEntry> entries, List<string> groups)
        {
            foreach (string group in groups)
                if (!RegGroupOpts.Items.Contains(group))
                {
                    RegGroupOpts.Items.Add(group);
                    dtgroups.Rows.Add(group);
                }
            //RegGroupOpts.DataSource = dtgroups.AsEnumerable().Select(r => r.Field<string>("Group")).ToList();
            List<RegisterEntry> fields;
            foreach (RegisterEntry entry in entries)
            {
                AddEntryToTable(entry, true);
                fields = entry.GetFields();
                foreach (RegisterEntry field in fields)
                {
                    field.SetIndex(entry.GetIndex());
                    dtfields.Rows.Add(field.GetTableEntry());
                }
            }
            OpenValidation();
            UpdateDataBase();
            hierarchicalGridView1.DataSource = GridSource;
            hierarchicalGridView1.Columns["Reason"].Visible = false;
            hierarchicalGridView1.Columns["IsValid"].Visible = false;
            hierarchicalGridView1.Columns["Index"].Visible = false;
            hierarchicalGridView1.Columns["SecondaryIndex"].Visible = false;
            ColorInValid();
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
                PathToFile.Text = saveFileDialog1.FileName;
                File.WriteAllText(@"file_path.txt", saveFileDialog1.FileName);
                SaveButton_Click(sender, e);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            List<int> indices = new List<int>();
            List<Tuple<int, int>> s = new List<Tuple<int, int>>();
            List<RegisterEntry> fields;
            foreach (HierarchicalGridNode item in hierarchicalGridView1.SelectedRows)
            {
                try
                {
                    int i = (int)item.Cells["Index"].Value, j = (int)item.Cells["SecondaryIndex"].Value;
                    if (j != -1)
                    {
                        s.Add(new Tuple<int, int>(i, j));
                    }
                    else
                    {
                        indices.Add(i);
                    }
                    foreach (HierarchicalGridNode group in hierarchicalGridView1.Nodes)
                    {
                        foreach (HierarchicalGridNode sibling in group.Nodes)
                        {
                            if (j == -1)
                            {
                                //MessageBox.Show(sibling.Cells["Index"].Value.ToString());
                                if ((int)sibling.Cells[12].Value > i)
                                {
                                    sibling.Cells[12].Value = (int)sibling.Cells[12].Value - 1;
                                    foreach (HierarchicalGridNode field in sibling.Nodes)
                                        field.Cells[12].Value = (int)field.Cells[12].Value - 1;
                                }
                            }
                            else
                            {
                                //MessageBox.Show(sibling.Cells["SecondaryIndex"].Value.ToString());
                                if ((int)sibling.Cells[13].Value > i)
                                    sibling.Cells[13].Value = (int)sibling.Cells[13].Value - 1;
                            }
                        }
                    }
                    item.Parent.Nodes.Remove(item);
                }
                catch (Exception)
                {
                    //nothing
                }
            }
            s = s.OrderByDescending(v => v.Item2).ToList();
            foreach (Tuple<int, int> index in s)
            {
                int j = index.Item2;
                fields = RegList[index.Item1].GetFields();
                for (int i = j + 1; i < fields.Count; i++)
                {
                    //MessageBox.Show(i.ToString() + " < " + fields.Count.ToString());
                    fields[i].SecondaryIndex--;
                }
                fields.RemoveAt(j);
            }
            foreach (int index in indices.OrderByDescending(v => v))
            {
                RegisterEntry entry;
                for (int i = index + 1; i < RegList.Count; i++)
                {
                    entry = RegList[i];
                    entry.Index--;
                    fields = entry.GetFields();
                    foreach (RegisterEntry field in fields)
                        field.Index--;
                }
                RegList.RemoveAt(index);
            }

            searchBox.Text = "";
            OpenValidation();
            UpdateDataBase();
            saved = false;
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
                    {
                        field.Visible = field.Cells["Name"].Value.ToString().Contains(searchRes);
                    }
                    reg.Visible = reg.Cells["Name"].Value.ToString().Contains(searchRes);
                }
            }
        }

        private string GetSpaces(int x)
        {
            return string.Concat(Enumerable.Repeat(" ", x));
        }

        private bool IsNum(string s)
        {
            return double.TryParse(s, out double num);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (PathToFile.Text.Equals(""))
            {
                SaveAsButton_Click(sender, e);
                return;
            }
            StreamReader file;
            try
            {
                file = new StreamReader("mycorrect.txt");
                string line;
                string res = "";
                string title = Path.GetFileNameWithoutExtension(PathToFile.Text);
                string date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                string introDec = "Original path: " + PathToFile.Text + "</br>The following is a documentation for " + title + ". The table " +
                    "contains the registers created using the GUI.";
                //MessageBox.Show(Path.GetFileNameWithoutExtension(PathToFile.Text));
                string doc = "<html><head><title>" + title + " Documentation" + "</title>";
                doc += "<style>table, th, td { border: 1px solid black; } th, td {padding: 5px; text-align: center;}" + "</style></head><body>";
                doc += "<h1><font face = 'arial'><u>Documentation For " + title + "</h1></u>";
                doc += date;
                doc += "<h2>" + date + "<br/>" + introDec + "</h2>";
                doc += "<table style='width: 100 %'>";
                doc += "<tr><th>Name</th><th>Group</th><th>Address</th><th>Mais</th><th>LSB</th><th>MSB</th><th>TYPE</th><th>FPGA</th><th>Init</th><th>Comment</th></tr>";
                HierarchicalGridNode last_node = null;
                for (int i = hierarchicalGridView1.Nodes.Count - 1; i >= 0; i--)
                {
                    HierarchicalGridNode last_group = hierarchicalGridView1.Nodes[i];
                    if (last_group.HasChildren)
                    {
                        last_node = last_group.Nodes[last_group.Nodes.Count - 1];
                        if (last_node.HasChildren)
                        {
                            last_node = last_node.Nodes[last_node.Nodes.Count - 1];
                        }
                        break;
                    }
                }
                RegisterEntry last = RegList[(int)last_node.Cells[12].Value];
                int k = (int)last_node.Cells[13].Value;
                if (k != -1)
                {
                    last = last.GetFields()[k];
                    //MessageBox.Show()
                }
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Length == 0)
                    {
                        res += "\n";
                        continue;
                    }
                    //System.Console.WriteLine(line);
                    if ('#' == line[0])
                        continue;
                    if (line.Equals("0o0o0o0o0o0o0o0o0o0o0o0o0o00o0o0o0o0o0o00o0o0o0o0o0"))
                        break;
                    res += line + "\n";
                }
                string prop = "", names = "";
                foreach (string group in RegGroupOpts.Items)
                {
                    names += "\t\t -- " + group + "\n";
                    prop += "\t\t -- " + group + "\n";
                    foreach (RegisterEntry l in RegList)
                    {
                        if (l.GetGroup().Equals(group))
                        {
                            List<RegisterEntry> fields = l.GetFields();

                            names += l.toName();

                            prop += l.ToEntry(l == last);

                            doc += l.ToXMLstring();

                            foreach (RegisterEntry f in fields)
                            {
                                names += f.toName();

                                prop += f.ToEntry(f == last);

                                doc += f.ToXMLstring();
                            }
                        }
                    }

                }

                doc += "</table></font></body></html>";
                res += names;
                while ((line = file.ReadLine()) != null)
                {
                    //System.Console.WriteLine(line);
                    if (line.Length == 0)
                    {
                        res += "\n";
                        continue;
                    }
                    if ('#' == line[0])
                        continue;
                    if (line.Equals("0o0o0o0o0o0o0o0o0o0o0o0o0o00o0o0o0o0o0o00o0o0o0o0o0"))
                        break;
                    res += line + '\n';
                }
                res += prop;
                while ((line = file.ReadLine()) != null)
                {
                    //System.Console.WriteLine(line);
                    if (line.Length == 0)
                    {
                        res += "\n";
                        continue;
                    }
                    if ('#' == line[0])
                        continue;
                    res += line + '\n';
                }
                file.Close();
                try
                {
                    File.WriteAllText(PathToFile.Text, res);
                    //MessageBox.Show(Path.GetDirectoryName(PathToFile.Text) + "\\" + title + "_doc.txt");
                    File.WriteAllText(Path.GetDirectoryName(PathToFile.Text) + "\\" + title + "_doc.html", doc);
                    saved = true;
                    ErrorMessage.Text = "Message: File Saved!";

                }
                catch
                {
                    MessageBox.Show("Invalid Path to File");
                }
            }
            catch (IOException t)
            {
                MessageBox.Show("ArgumentException " + t.ToString());
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            RegList.Clear();
            dtfields.Clear();
            dtregisters.Clear();
            //dtgroups.Clear(); 
            UpdateDataBase();
            InitFields();
            hierarchicalGridView1.DataSource = GridSource;
            changed = false;
        }

        private void RegNameText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                InsertButton_Click(sender, e);
        }

        private void NewGroupText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                AddGroupButton_Click(sender, e);
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
            if (PathToFile.Text.Equals(""))
                Close();
            if (saved)
            {
                PathToFile.Text = "";
                File.WriteAllText(@"file_path.txt", "");
                Close();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to close the file without saving?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //SaveButton_Click(sender, e);
                    PathToFile.Text = "";
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
            RegisterEntry re = null;
            foreach (HierarchicalGridNode item in hierarchicalGridView1.SelectedRows)
            {
                try
                {
                    re = RegList[(int)item.Cells["Index"].Value];
                    int index = (int)item.Cells["SecondaryIndex"].Value;
                    if (index != -1)
                    {
                        re = re.GetFields()[index];
                        //MessageBox.Show(re.GetIndex().ToString() + ", " + index);
                    }
                    else
                    {
                        //MessageBox.Show(re.GetIndex().ToString());
                    }
                    break;
                }
                catch (NullReferenceException)
                {
                    //do nothing for groups
                    return;
                }
            }
            if (re != null)
            {
                RegNameText.Text = re.GetName();
                CommentText.Text = re.GetComment();
                InitText.Text = re.GetInit();
                int index = LSBOpts.FindStringExact(re.GetLSB().ToString());
                if (index == -1)
                    index = 0;
                LSBOpts.SelectedIndex = index;
                index = LSBOpts.FindStringExact(re.GetLSB().ToString());
                if (index == -1)
                    index = 0;
                LSBOpts.SelectedIndex = index;
                index = MSBOpts.FindStringExact(re.GetMSB().ToString());
                if (index == -1)
                    index = 31;
                MSBOpts.SelectedIndex = index;
                index = MAISOpts.FindStringExact(re.GetMAIS().ToString());
                if (index == -1)
                    index = 0;
                MAISOpts.SelectedIndex = index;
                index = TypeOpts.FindStringExact(re.GetRegType().ToString());
                if (index == -1)
                    index = 0;
                TypeOpts.SelectedIndex = index;
                index = FPGAOpts.FindStringExact(re.GetFPGA().ToString());
                if (index == -1)
                    index = 0;
                FPGAOpts.SelectedIndex = index;
                RegGroupOpts.SelectedIndex = RegGroupOpts.FindStringExact(re.GetGroup());

                if (re.GetIsComment())
                    ErrorMessage.Text = "Message: ";
                else if (!re.GetValid())
                    ErrorMessage.Text = "Message: " + re.GetReason();
                else
                    ErrorMessage.Text = "Message: ";
            }
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


        private void CommentButton_Click(object sender, EventArgs e)
        {
            List<int> indices = new List<int>();
            List<Tuple<int, int>> s = new List<Tuple<int, int>>();
            //List<RegisterEntry> fields;
            RegisterEntry entry;
            foreach (HierarchicalGridNode item in hierarchicalGridView1.SelectedRows)
            {
                try
                {
                    int i = (int)item.Cells["Index"].Value, j = (int)item.Cells["SecondaryIndex"].Value;
                    entry = RegList[i];
                    if (j != -1)
                        entry = entry.GetFields()[j];
                    entry.SetIsComment(true);
                }
                catch (Exception)
                {
                    //nothing
                }

            }

            searchBox.Text = "";
            OpenValidation();
            UpdateDataBase();
        }

        private void UnCommentButton_Click(object sender, EventArgs e)
        {
            List<int> indices = new List<int>();
            List<Tuple<int, int>> s = new List<Tuple<int, int>>();
            //List<RegisterEntry> fields;
            RegisterEntry entry;
            foreach (HierarchicalGridNode item in hierarchicalGridView1.SelectedRows)
            {
                try
                {
                    int i = (int)item.Cells["Index"].Value, j = (int)item.Cells["SecondaryIndex"].Value;
                    entry = RegList[i];
                    if (j != -1)
                        entry = entry.GetFields()[j];
                    entry.SetIsComment(false);
                }
                catch (Exception)
                {
                    //nothing
                }

            }
            searchBox.Text = "";
            OpenValidation();
            UpdateDataBase();
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
            MessageBox.Show(
                    "MappingPackageAutomation Version 1.2\nCreated By Eran Marchesky and Eli Zeltser as a final project\n\nAdvisors: Dan Shalom and Eli Parente");
        }
    }
}
