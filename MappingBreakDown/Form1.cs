using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MappingBreakDown
{
    public partial class MappingPackageAutomation : Form
    {
        public XmlSerializer xs;
        List<RegisterEntry> RegList;
        List<RegisterEntry> RegShow;
        public MappingPackageAutomation()
        {
            InitializeComponent();
            InitFields();
            RegList = new List<RegisterEntry>();
            RegShow = new List<RegisterEntry>();
            xs = new XmlSerializer(typeof(List<RegisterEntry>));
        }

        public RegisterEntry[] GetRegistersArray()
        {
            return RegList.ToArray();
        }

        /* Default values for each register */
        private void InitFields()
        {
            this.LSBOpts.Text = "0";
            this.MSBOpts.Text = "31";
            this.MAISOpts.Text = "0";
            this.TypeOpts.Text = "RD";
            this.FPGAOpts.Text = "G";
            this.InitText.Text = "";
            RegNameText.Text = "";
            CommentText.Text = "";
            RegGroupOpts.SelectedIndex = 0;
        }

        /* Upon Insert to the table, allocate a new address to the register */
        private int FindAddress()
        {
            int i = 1, x;
            for (; i <= 1023; i++)
            {
                bool found = true;
                foreach (RegisterEntry l in RegList)
                {
                    x = l.Address;
                    if (x == i)
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }
            return -1;
        }

        private bool inputValidation(RegisterEntry entry, string type, string fpga, bool add, bool load, bool open, List<RegisterEntry> lst = null)
        {
            int addr;
            if (open)
            {
                bool test = false;
                foreach (RegisterEntry new_entry in lst)
                {
                    int addr_new = new_entry.Address;
                    foreach (RegisterEntry item in RegList)
                    {
                        if (item.Address == addr_new)
                        {
                            test = true;
                            break;
                        }

                    }
                    if (test)
                    {
                        MessageBox.Show("Address " + addr_new + " is already in the list");
                        InitFields();
                        return false;
                    }
                }
                return true;
            }

            if (!type.Equals("FIELD"))
            {
                addr = FindAddress();
                if (addr == -1)
                {
                    MessageBox.Show("Unable to add register " + entry.Name + " (" + entry.Address + "), no free slot in memory");
                    return false;
                }
                if (add)
                    entry.Address = addr;
            }
            else
            {
                if (RegList.Count == 0)
                {
                    MessageBox.Show("There are no registers in the list");
                    return false;
                }

                if (!open)
                {
                    RegisterEntry[] regArr = RegList.Where(val => !val.Name.Equals(entry.Name)).ToArray();
                    using (ChooseAddressPrompt prompt = new ChooseAddressPrompt(regArr))
                    {
                        if (prompt.ShowDialog() == DialogResult.OK)
                        {
                            addr = prompt.chosen_address;
                            entry.Address = addr;
                        }
                        else
                            return false;
                    }
                }
            }

            if (!entry.IsValidLsbMsb())
            {
                MessageBox.Show("Register " + entry.Name + " (" + entry.Address + "): LSB is greater than MSB");
                InitFields();
                return false;
            }

            if (!load)
            {
                //MessageBox.Show("CHECK");
                foreach (RegisterEntry item in RegList)
                {
                    if (item.Name.Equals(entry.Name))
                    {
                        MessageBox.Show("Register " + entry.Name + " (" + item.Address + ") is already in the list");
                        InitFields();
                        return false;
                    }
                }
            }
            return true;
        }

        /* Insert register to the table */
        private void InsertButton_Click(object sender, EventArgs e)
        {
            if (this.RegNameText.Text.Equals(""))
            {
                MessageBox.Show("Invalid register name: Empty name");
                InitFields();
                return;
            }

            string name = this.RegNameText.Text;
            string mais = this.MAISOpts.Text;
            string lsb = this.LSBOpts.Text;
            string msb = this.MSBOpts.Text;
            string type = this.TypeOpts.Text;
            string fpga = this.FPGAOpts.Text;
            string init = this.InitText.Text;
            string comment = this.CommentText.Text;
            string group = this.RegGroupOpts.Text;
            int addr = -1;

            RegisterEntry entry = new RegisterEntry(name, addr, mais, lsb, msb, type, fpga, init, comment, group);
            if (!inputValidation(entry, type, fpga, true, false, false))
                return;
            addEntryToTable(entry);
            InitFields();
        }

        /* Add a new group */
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.NewGroupText.Text.Equals(""))
                return;
            foreach (string item in this.RegGroupOpts.Items)
            {
                if (item.Equals(this.NewGroupText.Text))
                {
                    MessageBox.Show("Group " + NewGroupText.Text + " already exists");
                    this.NewGroupText.Text = "";
                    return;
                }
            }
            this.RegGroupOpts.Items.Add(this.NewGroupText.Text);
            this.NewGroupText.Text = "";
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathToFile.Text = openFileDialog1.FileName;
                FileValidator fv = new FileValidator(openFileDialog1.FileName);
                fv.IsFileValid();
                this.addManyRegisters(fv.Registers.ToList());
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            if (this.RegNameText.Text.Equals(""))
            {
                MessageBox.Show("Invalid register name");
                InitFields();
                return;
            }

            string name = this.RegNameText.Text;
            string mais = this.MAISOpts.Text;
            string lsb = this.LSBOpts.Text;
            string msb = this.MSBOpts.Text;
            string type = this.TypeOpts.Text;
            string fpga = this.FPGAOpts.Text;
            string init = this.InitText.Text;
            string comment = this.CommentText.Text;
            string group = this.RegGroupOpts.Text;
            int addr = -1;
  
            RegisterEntry entry = new RegisterEntry(name, addr, mais, lsb, msb, type, fpga, init, comment, group);
            if (!inputValidation(entry, type, fpga, false, true, false))
                return;

            bool b = false;
            for (int i = 0; i < RegList.Count; i++)
            {
                if (RegList[i].Name.Equals(name))
                {
                    RegList[i].MAIS = Int32.Parse(mais);
                    RegList[i].LSB = Int32.Parse(lsb);
                    RegList[i].MSB = Int32.Parse(msb);
                    RegisterEntry.type_field t;
                    RegisterEntry.fpga_field r;
                    Enum.TryParse(type, out t);
                    Enum.TryParse(fpga, out r);
                    //MessageBox.Show("After: " + t + ", Before: " + RegList[i].Type + ", " + RegisterEntry.type_field.FIELD);
                    if (RegList[i].Type == RegisterEntry.type_field.FIELD && t != RegisterEntry.type_field.FIELD)
                        RegList[i].Address = FindAddress();
                    //MessageBox.Show("new address for " + RegList[i].Name + ": " + RegList[i].Address);
                    RegList[i].Type = t;
                    RegList[i].FPGA = r;
                    RegList[i].Init = init;
                    RegList[i].Comment = comment;
                    RegList[i].Group = group;
                    
                    for (int j = 0; j < RegShow.Count; j++)
                    {
                        if (RegShow[j].Name.Equals(name))
                        {
                            RegShow[j].MAIS = Int32.Parse(mais);
                            RegShow[j].LSB = Int32.Parse(lsb);
                            RegShow[j].MSB = Int32.Parse(msb);
                            RegShow[j].Type = t;
                            RegShow[j].FPGA = r;
                            RegShow[j].Init = init;
                            RegShow[j].Comment = comment;
                            RegShow[j].Group = group;
                            RegShow[j].Address = RegList[i].Address;
                        }
                    }
                    b = true;
                    break;
                }
            }

            if (!b)
            {
                MessageBox.Show("No such register " + name);
                return;
            }

            updateXML(false, true, false, false, false);
        }

        private void updateXML(bool insert, bool load, bool delete, bool serach, bool restore)
        {
            FileStream fs;
            RegList = RegList.OrderBy(y => y.Group).ThenBy(y => y.Address).ToList();
            RegShow = RegShow.OrderBy(y => y.Group).ThenBy(y => y.Address).ToList();
            if (insert || load || delete)
            {
                fs = new FileStream(@"jack.txt", FileMode.Create, FileAccess.Write);
                xs.Serialize(fs, RegList);
                fs.Close();
            }
            if (insert || delete || restore)
            {
                fs = new FileStream(@"jack.txt", FileMode.Open, FileAccess.Read);
                RegList = (List<RegisterEntry>)xs.Deserialize(fs);
                fs.Close();
                if (insert || delete)
                {
                    RegShow = RegList;
                    dataGridView1.DataSource = RegList;
                    fs = new FileStream(@"show.txt", FileMode.Create, FileAccess.Write);
                    xs.Serialize(fs, RegShow);
                    fs.Close();
                }
            }
            if (load || serach)
            {
                fs = new FileStream(@"show.txt", FileMode.Create, FileAccess.Write);
                xs.Serialize(fs, RegShow);
                fs.Close();
            }
            if (load || serach || restore)
            {
                fs = new FileStream(@"show.txt", FileMode.Open, FileAccess.Read);
                RegShow = (List<RegisterEntry>)xs.Deserialize(fs);
                fs.Close();
                dataGridView1.DataSource = RegShow;
            }
        }

        private void addEntryToTable(RegisterEntry entry)
        {
            RegList.Add(entry);
            updateXML(true, false, false, false, false);
        }

        private void addManyRegisters(List<RegisterEntry> entries)
        {
            if (!inputValidation(null, "", "", false, false, true, entries))
            {
                InitFields();
                return;
            }
            foreach (RegisterEntry entry in entries)
            {
                //if (!inputValidation(entry, entry.Type.ToString("G"), entry.FPGA.ToString("G"), false, false, true))
                //    return;
                addEntryToTable(entry);
            }
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
            if (this.RegGroupOpts.Items.Count == 0)
                MessageBox.Show("You must first add a group name");
        }

        private int findIndex(string name)
        {
            for (int i = 0; i < RegList.Count; i++)
            {
                if (RegList[i].Name.Equals(name))
                    return i;
            }
            return -1;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            List<int> indices = new List<int>();
            int i, addr;
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                i = findIndex(((RegisterEntry)item.DataBoundItem).Name);
                if (RegList[i].Type != RegisterEntry.type_field.FIELD) {
                    addr = ((RegisterEntry)item.DataBoundItem).Address;
                    for (int j = 0; j < RegList.Count; j++)
                    {
                        if (RegList[j].Address == addr && j != i &&
                            RegList[j].Type == RegisterEntry.type_field.FIELD && !indices.Contains(j))
                            indices.Add(j);
                    }
                }
                if (!indices.Contains(i))
                    indices.Add(i);
            }
            foreach (int index in indices.OrderByDescending(v => v))
                RegList.RemoveAt(index);

            searchBox.Text = "";
            updateXML(false, false, true, false, false);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            String searchRes = searchBox.Text;
            RegShow = new List<RegisterEntry>();
            foreach (RegisterEntry entry in RegList)
            {
                if (entry.Name.StartsWith(searchRes))
                    RegShow.Add(entry);
            }
            updateXML(false, false, false, true, false);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            RegisterEntry re = null;
            if (RegShow != null && RegShow.Count() != 0)
            {
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    re = RegShow[item.Index];
                    break;
                }
                if (re != null)
                {
                    RegNameText.Text = re.Name;
                    CommentText.Text = re.Comment;
                    InitText.Text = re.Init;
                    LSBOpts.SelectedIndex = LSBOpts.FindStringExact(re.LSB.ToString());
                    MSBOpts.SelectedIndex = MSBOpts.FindStringExact(re.MSB.ToString());
                    MAISOpts.SelectedIndex = MAISOpts.FindStringExact(re.MAIS.ToString());
                    TypeOpts.SelectedIndex = TypeOpts.FindStringExact(re.Type.ToString());
                    FPGAOpts.SelectedIndex = FPGAOpts.FindStringExact(re.FPGA.ToString());
                    RegGroupOpts.SelectedIndex = RegGroupOpts.FindStringExact(re.Group);
                }
            }
        }

        private void RegNameText_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private String getSpaces(int x)
        {
            return String.Concat(Enumerable.Repeat(" ", x));
        }

        private bool isNum(String s)
        {
            double num;
            return double.TryParse(s, out num);
        }

        private String getString(String reg, String addr, String mais, String lsb, String msb, String type, String fpga, String init)
        {
            int spaces;
            if (type.Equals("FIELD"))
                spaces = 4;
            else
                spaces = 0;
            String ___reg_name___ = getSpaces(16) + "(" + reg + getSpaces((56 - spaces - ((17 + reg.Length))));
            String __address = getSpaces(8 - addr.Length) + addr;
            String __mais = getSpaces(3 - mais.Length) + mais;
            String __lsb__msb = getSpaces(3 - lsb.Length) + lsb + "," + getSpaces(3 - msb.Length) + msb;
            String _type__ = " " + type + getSpaces(5 - type.Length);
            String _fpga__ = " " + fpga + getSpaces(4 - fpga.Length);
            String __init;
            if (isNum(init))
                __init = getSpaces(5 - init.Length) + init;
            else
                __init = init;
            return ___reg_name___ + "," + __address + "," + __mais + "," + __lsb__msb + "," + _type__ + "," + _fpga__ + "," + __init + ")";
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (PathToFile.Text.Equals(""))
            {
                SaveAsButton_Click(sender, e);
                return;
            }
            System.IO.StreamReader file;
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
                String reg, addr, mais, lsb, msb, type, fpga, init, comment;
                String prop = "", names = "";
                int index = 0;
                foreach (string group in RegGroupOpts.Items)
                {
                    names += "\t\t -- " + group + "\n";
                    prop += "\t\t -- " + group + "\n";
                    foreach (RegisterEntry l in RegList)
                    {
                        if (l.Group.Equals(group))
                        {
                            reg = l.Name;
                            addr = l.Address.ToString();
                            mais = l.MAIS.ToString();
                            lsb = l.LSB.ToString();
                            msb = l.MSB.ToString();
                            type = l.Type.ToString();
                            fpga = l.FPGA.ToString();
                            init = l.Init;
                            comment = l.Comment;
                            if (l.Type.Equals(RegisterEntry.type_field.FIELD))
                            {
                                names += "\t";
                                prop += "\t";
                            }
                            names += "\t\t\t\t" + reg + ",\n";
                            if (index++ != RegList.Count - 1)
                                prop += getString(reg, addr, mais, lsb, msb, type, fpga, init) + ",\t" + comment + "\n";
                            else
                                prop += getString(reg, addr, mais, lsb, msb, type, fpga, init) + "\t" + comment + "\n";
                            doc += "<tr><td>" + reg + "</td><td>" + l.Group + "</td><td>" + addr + "</td><td>" + mais + "</td><td>" + lsb + "</td><td>" + msb + "</td><td>" + type + "</td>";
                            doc += "<td>" + fpga + "</td><td>" + init + "</td><td>" + comment + "</td></tr>";
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

        private void Button2_Click_1(object sender, EventArgs e)
        {
            updateXML(false, false, false, false, true);
        }
    }
}
