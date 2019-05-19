using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MappingBreakDown
{
    public partial class MappingPackageAutomation : Form
    {
        public XmlSerializer xs;
        List<RegisterEntry> RegList;
        List<RegisterEntry> RegShow;
        bool saved = false;

        public MappingPackageAutomation()
        {
            InitializeComponent();
            InitFields();
            ErrorMessage.Text = "> ";
            xs = new XmlSerializer(typeof(List<RegisterEntry>));
            UpdateXML(false, false, false, false, true);
            //UpdateXML(false, false, true, false, false);
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
            int i = 1, x;
            bool found;
            for (; i <= 1023; i++)
            {
                found = true;
                foreach (RegisterEntry l in RegList)
                {
                    x = l.GetAddress();
                    if (x == i)
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

        /* Find Register entry at a given address */
        private RegisterEntry FindAtAddress(int address, bool real)
        {
            List<RegisterEntry> search = RegList;
            if (!real)
                search = RegShow;
            foreach (RegisterEntry entry in search)
                if (entry.GetAddress() == address)
                    return entry;
            return null;
        }

        /* Add a new group */
        private void AddGroupButton_Click(object sender, EventArgs e)
        {
            if (NewGroupText.Text.Equals(""))
                return;
            foreach (string item in RegGroupOpts.Items)
            {
                if (item.Equals(NewGroupText.Text))
                {
                    MessageBox.Show("Group " + NewGroupText.Text + " already exists");
                    NewGroupText.Text = "";
                    return;
                }
            }
            RegGroupOpts.Items.Add(NewGroupText.Text);
            NewGroupText.Text = "";
        }

        private bool CheckDup(RegisterEntry new_entry, bool delete = false)
        {
            int addr_new = new_entry.GetAddress();
            string name_new = new_entry.GetName();
            foreach (RegisterEntry item in RegList)
            {
                if (item.GetIsComment() || item == new_entry)
                    continue;
                string reason;
                if (item.GetName().Equals(name_new))
                {
                    reason = "Name " + name_new + " is already in the list";
                    if (delete)
                    {
                        new_entry.SetReason(reason);
                        new_entry.SetValid(false);
                    }
                    else
                        MessageBox.Show(reason);
                    return false;
                }
                if (item.GetAddress() == addr_new)
                {
                    reason = "Address " + addr_new + " is already in the list";
                    if (delete)
                    {
                        new_entry.SetReason(reason);
                        new_entry.SetValid(false);
                    }
                    else
                        MessageBox.Show(reason);
                    return false;
                }
            }
            return true;
        }

        /* Validate Opened file */
        private void OpenValidation(List<RegisterEntry> lst = null, bool delete = true)
        {
            if (delete)
                lst = RegList;
            foreach (RegisterEntry new_entry in lst)
            {
                if (CheckDup(new_entry, delete))
                {
                    if (delete)
                    {
                        if (!InputValidation(new_entry, false, false))
                        {
                            new_entry.SetReason("The register " + new_entry.GetName() + "(" + new_entry.GetAddress() + ") has MSB < LSB");
                            new_entry.SetValid(false);
                        }
                        else
                        {
                            new_entry.SetValid(true);
                            new_entry.SetReason("");
                        }
                    }
                }
            }
        }

        /* Check the a register can be added to the chart */
        private bool InputValidation(RegisterEntry entry, bool add, bool print = true)
        {
            if (add)
            {
                if (entry.GetRegType() != RegisterEntry.type_field.FIELD)
                {
                    int index = FindIndex(entry.GetName(), true);
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
                    int addr = -1;
                    RegisterEntry item;
                    using (ChooseAddressPrompt prompt = new ChooseAddressPrompt(RegList.ToArray()))
                    {
                        if (prompt.ShowDialog() == DialogResult.OK)
                        {
                            addr = prompt.Chosen_address;
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
                }
            }

            if (!entry.IsValidLsbMsb())
            {
                if (print)
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

            RegisterEntry entry = new RegisterEntry(RegNameText.Text, -1, MAISOpts.Text, LSBOpts.Text, MSBOpts.Text, TypeOpts.Text, FPGAOpts.Text, InitText.Text, CommentText.Text, RegGroupOpts.Text);
            if (!InputValidation(entry, true))
                return;
            AddEntryToTable(entry);
            InitFields();
        }

        /* Open a file */
        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileValidator fv = new FileValidator(openFileDialog1.FileName);
                if (fv.IsFileValid())
                {
                    PathToFile.Text = openFileDialog1.FileName;
                    AddManyRegisters(fv.GetRegList(), fv.GetGroups());
                }
            }
        }

        /* Edit a register */
        private void Load_Click(object sender, EventArgs e)
        {
            /*if (RegNameText.Text.Equals(""))
            {
                MessageBox.Show("Invalid register name");
                InitFields();
                return;
            }*/
            RegisterEntry re = null;
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                re = RegShow[item.Index];
                break;
            }
            if (re == null)
            {
                MessageBox.Show("Please select a register in order to edit");
                return;
            }
            string name = RegNameText.Text;
            string mais = MAISOpts.Text;
            string lsb = LSBOpts.Text;
            string msb = MSBOpts.Text;
            string type = TypeOpts.Text;
            string fpga = FPGAOpts.Text;
            string init = InitText.Text;
            string comment = CommentText.Text;
            string group = RegGroupOpts.Text;
            int addr = -1;

            RegisterEntry entry = new RegisterEntry(name, addr, mais, lsb, msb, type, fpga, init, comment, group);

            int i = FindIndex(name, true, re, true);
            if (i == -1)
            {
                MessageBox.Show("No such register " + name);
                InitFields();
                return;
            }
            if (RegList[i].GetIsComment())
            {
                MessageBox.Show("This register is a comment and can't be edited");
                InitFields();
                return;
            }
            Enum.TryParse(type, out RegisterEntry.type_field t);
            if ((RegList[i].GetRegType() == RegisterEntry.type_field.FIELD && t != RegisterEntry.type_field.FIELD) ||
                RegList[i].GetRegType() != RegisterEntry.type_field.FIELD && t == RegisterEntry.type_field.FIELD)
            {
                MessageBox.Show("Can't edit a field or create one using Load");
                InitFields();
                return;
            }
            if (!InputValidation(entry, false))
                return;
            Enum.TryParse(fpga, out RegisterEntry.fpga_field r);
            RegList[i].EditRegister(mais, lsb, msb, t, r, init, comment, group);
            i = FindIndex(name, false, re, true);
            RegShow[i].EditRegister(mais, lsb, msb, t, r, init, comment, group);
            OpenValidation();
            UpdateXML(false, true, false, false, false);
        }

        /* PROBLEM HERE */
        public void FlattenList()
        {
            //IEnumerable<RegisterEntry> jack = RegShow.SelectMany(val => val.GetFields());
            //List<RegisterEntry> onlyFIELDS = jack.ToList();
            List<RegisterEntry> res = new List<RegisterEntry>();
            var xml = XElement.Parse(@"show.txt");

            foreach (RegisterEntry reg in RegList)
            {
                res.Add(reg);
                res.Concat(reg.GetFields());
            }
        }
        /* PROBLEM HERE */

        private void ColorInValid()
        {
            for (int i = 0; i < RegShow.Count; i++)
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    if (RegShow[i].GetIsComment())
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.Gray;
                    else if (!RegShow[i].GetValid())
                    {
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.Red;
                        //Console.WriteLine(RegShow[i]);
                    }
                    else
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.White;
        }

        private void UpdateDataBase()
        {
            FileStream fs = new FileStream(@"jack.txt", FileMode.Create, FileAccess.Write);
            xs.Serialize(fs, RegList);
            fs.Close();
        }

        private void ReadDataBase()
        {
            FileStream fs = new FileStream(@"jack.txt", FileMode.Open, FileAccess.Read);
            RegList = (List<RegisterEntry>)xs.Deserialize(fs);
            fs.Close();
        }

        private void UpdateShow()
        {
            FileStream fs = new FileStream(@"show.txt", FileMode.Create, FileAccess.Write);
            xs.Serialize(fs, RegShow);
            fs.Close();
        }

        private void ReadShow()
        {
            FileStream fs = new FileStream(@"show.txt", FileMode.Open, FileAccess.Read);
            RegShow = (List<RegisterEntry>)xs.Deserialize(fs);
            fs.Close();
        }

        /* Update inner files */
        private void UpdateXML(bool insert, bool load, bool delete, bool search, bool restore)
        {
            //RegList = RegList.OrderBy(y => y.GetGroup()).ThenBy(y => y.GetAddress()).ThenBy(y => y.GetRegType()).ThenBy(y => y.GetLSB()).ToList();
            //RegShow = RegShow.OrderBy(y => y.GetGroup()).ThenBy(y => y.GetAddress()).ThenBy(y => y.GetRegType()).ThenBy(y => y.GetLSB()).ToList();
            if (insert || load || delete)
            {
                UpdateDataBase();
            }
            if (insert || delete || restore)
            {
                ReadDataBase();
                if (insert || delete)
                {
                    RegShow = RegList;
                    //FlattenList();
                    dataGridView1.DataSource = RegList;
                    dataGridView1.Columns["IsValid"].Visible = false;
                    dataGridView1.Columns["IsComment"].Visible = false;
                    dataGridView1.Columns["Reason"].Visible = false;
                    ColorInValid();
                    UpdateShow();
                }
            }
            if (load || search)
            {
                UpdateShow();
            }
            if (load || search || restore)
            {
                ReadShow();
                //FlattenList();
                dataGridView1.DataSource = RegShow;
                dataGridView1.Columns["IsValid"].Visible = false;
                dataGridView1.Columns["IsComment"].Visible = false;
                dataGridView1.Columns["Reason"].Visible = false;
                ColorInValid();
            }
        }

        private void AddEntryToTable(RegisterEntry entry)
        {
            if (entry.GetRegType() == RegisterEntry.type_field.FIELD)
            {
                RegisterEntry entf = FindAtAddress(entry.GetAddress(), true);
                entf.AddField(entry);
            }
            else
                RegList.Add(entry);
            UpdateXML(true, false, false, false, false);
        }

        private void AddManyRegisters(List<RegisterEntry> entries, List<string> groups)
        {
            OpenValidation(entries, false);
            foreach (RegisterEntry entry in entries)
                AddEntryToTable(entry);

            foreach (string group in groups)
                if (!RegGroupOpts.Items.Contains(group))
                    RegGroupOpts.Items.Add(group);
        }

        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                PathToFile.Text = saveFileDialog1.FileName;
                SaveButton_Click(sender, e);
            }
        }

        private void RegGroupOpts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RegGroupOpts_MouseDown(object sender, MouseEventArgs e)
        {
            if (RegGroupOpts.Items.Count == 0)
                MessageBox.Show("You must first add a group name");
        }

        private int FindIndex(string name, bool real, RegisterEntry entry = null, bool byObj = false)
        {
            List<RegisterEntry> search = RegList;
            if (!real)
                search = RegShow;
            for (int i = 0; i < search.Count; i++)
                if ((byObj && search[i].CompareTo(entry) == 0) || !byObj && search[i].GetName().Equals(name))
                    return i;
            return -1;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            List<int> indices = new List<int>();
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                int i = FindIndex("", true, (RegisterEntry)item.DataBoundItem, true);
                // Insert here field deletion
                if (!indices.Contains(i))
                    indices.Add(i);
            }
            /*string print = "";
            foreach (int index in indices)
                print += index;
            MessageBox.Show(print);*/
            foreach (int index in indices.OrderByDescending(v => v))
                RegList.RemoveAt(index);

            searchBox.Text = "";
            OpenValidation();
            UpdateXML(false, false, true, false, false);
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            String searchRes = searchBox.Text;
            RegShow = new List<RegisterEntry>();
            foreach (RegisterEntry entry in RegList)
                if (entry.GetName().Contains(searchRes))
                    RegShow.Add(entry);
            UpdateXML(false, false, false, true, false);
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (RegShow != null && RegShow.Count() != 0)
            {
                RegisterEntry re = null;
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    re = RegShow[item.Index];
                    break;
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

                    if (!re.GetValid())
                        ErrorMessage.Text = "[@] " + re.GetReason();
                    else
                        ErrorMessage.Text = "> ";
                }
            }
        }

        private void RegNameText_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {

        }

        private String getSpaces(int x)
        {
            return String.Concat(Enumerable.Repeat(" ", x));
        }

        private bool IsNum(String s)
        {
            double num;
            return double.TryParse(s, out num);
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
                file = new System.IO.StreamReader("mycorrect.txt");
                string line;
                String res = "";
                string title = Path.GetFileNameWithoutExtension(PathToFile.Text);
                String introDec = "Original path: " + PathToFile.Text + "</br>The following is a documentation for " + title + ". The table " +
                    "contains the registers created using the GUI.";
                //MessageBox.Show(Path.GetFileNameWithoutExtension(PathToFile.Text));
                string doc = "<html><head><title>" + title + " Documentation" + "</title>";
                doc += "<style>table, th, td { border: 1px solid black; } th, td {padding: 5px; text-align: center;}" + "</style></head><body>";
                doc += "<h1><font face = 'arial'><u>Documentation For " + title + "</h1></u>";
                doc += "<h2>" + introDec + "</h2>";
                doc += "<table style='width: 100 %'>";
                doc += "<tr><th>Name</th><th>Group</th><th>Address</th><th>Mais</th><th>LSB</th><th>MSB</th><th>TYPE</th><th>FPGA</th><th>Init</th><th>Comment</th></tr>";
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
                String prop = "", names = "", comment, reg;
                int index = 0;
                foreach (string group in RegGroupOpts.Items)
                {
                    names += "\t\t -- " + group + "\n";
                    prop += "\t\t -- " + group + "\n";
                    foreach (RegisterEntry l in RegList)
                    {
                        if (l.GetGroup().Equals(group))
                        {
                            reg = l.ToString();
                            comment = l.GetComment();
                            if (l.GetRegType().Equals(RegisterEntry.type_field.FIELD))
                            {
                                names += "\t";
                                prop += "\t";
                            }
                            names += "\t\t\t\t" + l.GetName() + ",\n";
                            if (index++ != RegList.Count - 1)
                                prop += reg + ",\t" + "-- " + comment + "\n";
                            //prop += l.ToString();
                            else
                                prop += reg + "\t" + "-- " + comment + "\n";
                            doc += "<tr><td>" + l.GetName() + "</td><td>" + l.GetGroup() + "</td><td>" + l.GetAddress().ToString() + "</td><td>" + l.GetMAIS().ToString() + "</td><td>" +
                                l.GetLSB().ToString() + "</td><td>" + l.GetMSB().ToString() + "</td><td>" + l.GetRegType().ToString() + "</td>";
                            doc += "<td>" + l.GetFPGA().ToString() + "</td><td>" + l.GetInit() + "</td><td>" + comment + "</td></tr>";
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
                    System.IO.File.WriteAllText(PathToFile.Text, res);
                    //MessageBox.Show(Path.GetDirectoryName(PathToFile.Text) + "\\" + title + "_doc.txt");
                    System.IO.File.WriteAllText(Path.GetDirectoryName(PathToFile.Text) + "\\" + title + "_doc.html", doc);
                    saved = true;
                    ErrorMessage.Text = "[#] File Saved!";

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

        private void RegGroupOpts_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void MappingPackageAutomation_Load(object sender, EventArgs e)
        {

        }

        private void Clear_Click(object sender, EventArgs e)
        {
            for (int i = RegList.Count - 1; i >= 0; i--)
                RegList.RemoveAt(i);
            UpdateXML(false, false, true, false, false);
            InitFields();
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

        private void DataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                Delete_Click(sender, e);
        }

        private void MappingPackageAutomation_Load_1(object sender, EventArgs e)
        {

        }

        private void lable5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void MSBOpts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void MAISOpts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void TypeOpts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FPGAOpts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LSBOpts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (PathToFile.Text.Equals(""))
                return;
            if (saved)
                PathToFile.Text = "";
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to close the file without saving?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //SaveButton_Click(sender, e);
                    PathToFile.Text = "";
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do nothing
                }
            }
        }
    }
}
