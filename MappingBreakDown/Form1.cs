using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

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
        }

        /* Upon Insert to the table, allocate a new address to the register */
        private int FindAddress()
        {
            int i = 0, x;
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

        private bool inputValidation(RegisterEntry entry, string type, string fpga, bool add, bool load)
        {
            int addr;

            if (!type.Equals("FIELD"))
            {
                addr = FindAddress();
                if (addr == -1)
                {
                    MessageBox.Show("Unable to add register, no free slot in memory");
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

                using (ChooseAddressPrompt prompt = new ChooseAddressPrompt(GetRegistersArray()))
                {
                    if (prompt.ShowDialog() == DialogResult.OK)
                    {
                        addr = prompt.chosen_address;
                    }
                }
            }

            if (!entry.IsValidLsbMsb())
            {
                MessageBox.Show("LSB is greater then MSB");
                InitFields();
                return false;
            }

            if (!load)
            {
                foreach (RegisterEntry item in RegList)
                {
                    if (item.Name.Equals(entry.Name))
                    {
                        MessageBox.Show("Register name already in the list");
                        InitFields();
                        return false;
                    }
                }
            }
            return true;
        }

        private void InsertButton_Click(object sender, EventArgs e)
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
            if (!inputValidation(entry, type, fpga, true, false))
                return;
            addEntryToTable(entry);
            InitFields();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.NewGroupText.Text.Equals(""))
                return;
            foreach (string item in this.RegGroupOpts.Items)
            {
                if (item.Equals(this.NewGroupText.Text))
                {
                    MessageBox.Show("Group name already exists");
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
                //System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                //MessageBox.Show(sr.ReadToEnd());
                PathToFile.Text = openFileDialog1.FileName;
                //sr.Close();
                FileValidator fv = new FileValidator(openFileDialog1.FileName);
                this.addManyRegisters(fv.Registers.ToList());


                //using (ChooseAddressPrompt prompt = new ChooseAddressPrompt(GetRegistersArray()))
                //{
                //    if (prompt.ShowDialog() == DialogResult.OK)
                //    {
                //        addr = int.Parse(prompt.chosen_address);
                //    }
                //}
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            /*RegisterEntry[] arr = new RegisterEntry[10];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new RegisterEntry("reg_" + i.ToString(), i + 1, 0, 0, 31, RegisterEntry.type_field.RD, RegisterEntry.fpga_field.A, "asd", "Asdasd", "sdfsdf");
            }
            addManyRegisters(arr.ToList());*/

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
            if (!inputValidation(entry, type, fpga, true, true))
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

            
            FileStream fs = new FileStream(@"jack.txt", FileMode.Create, FileAccess.Write);
            xs.Serialize(fs, RegList);
            fs.Close();

            //fs = new FileStream(@"jack.txt", FileMode.Open, FileAccess.Read);
            //RegList = (List<RegisterEntry>)xs.Deserialize(fs);

            //dataGridView1.DataSource = RegList;
            //fs.Close();
            //textBox2.Text = "";
            //RegShow = RegList;
            //InitFields();

            fs = new FileStream(@"show.txt", FileMode.Create, FileAccess.Write);
            xs.Serialize(fs, RegShow);
            fs.Close();

            fs = new FileStream(@"show.txt", FileMode.Open, FileAccess.Read);
            RegShow = (List<RegisterEntry>)xs.Deserialize(fs);

            dataGridView1.DataSource = RegShow;
            fs.Close();
        }

        private void addEntryToTable(RegisterEntry entry)
        {
            FileStream fs = new FileStream(@"jack.txt", FileMode.Create, FileAccess.Write);
            RegList.Add(entry);
            xs.Serialize(fs, RegList);
            fs.Close();

            fs = new FileStream(@"jack.txt", FileMode.Open, FileAccess.Read);
            RegList = (List<RegisterEntry>)xs.Deserialize(fs);

            dataGridView1.DataSource = RegList;
            fs.Close();
            textBox2.Text = "";
            RegShow = RegList;
        }

        private void addManyRegisters(List <RegisterEntry> entries)
        {
            foreach (RegisterEntry entry in entries){
                if (!inputValidation(entry, entry.Type.ToString("G"), entry.FPGA.ToString("G"), false, false))
                    return;
                addEntryToTable(entry);
                InitFields();
            }
        }

        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            //Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                PathToFile.Text = saveFileDialog1.FileName;
                SaveButton_Click(sender, e);
                //button5_Click(sender, e);
                /*if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    myStream.Close();
                }*/
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

        private void Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                RegList.RemoveAt(item.Index);
               // MessageBox.Show(item.ToString());
                //dataGridView1.Rows.RemoveAt(item.Index);
            }
            FileStream fs = new FileStream(@"jack.txt", FileMode.Create, FileAccess.Write);
            xs.Serialize(fs, RegList);
            fs.Close();

            fs = new FileStream(@"jack.txt", FileMode.Open, FileAccess.Read);
            RegList = (List<RegisterEntry>)xs.Deserialize(fs);

            dataGridView1.DataSource = RegList;
            fs.Close();
            RegShow = RegList;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            String searchRes = textBox2.Text;
            RegShow = new List<RegisterEntry>();
            foreach (RegisterEntry entry in RegList)
            {
                if (entry.Name.StartsWith(searchRes))
                    RegShow.Add(entry);
            }
            FileStream fs = new FileStream(@"show.txt", FileMode.Create, FileAccess.Write);
            xs.Serialize(fs, RegShow);
            fs.Close();

            fs = new FileStream(@"show.txt", FileMode.Open, FileAccess.Read);
            RegShow = (List<RegisterEntry>)xs.Deserialize(fs);

            dataGridView1.DataSource = RegShow;
            fs.Close();
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
            String ___reg_name___ = getSpaces(16) + "(" + reg + getSpaces((56 - ((17 + reg.Length))));
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
            System.IO.StreamReader file;
            try
            {
                file = new System.IO.StreamReader("mycorrect.txt");
                string line;
                String res = "";
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
                String reg, addr, mais, lsb, msb, type, fpga, init;
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
                            names += "\t\t\t\t" + reg + ",\n";
                            if (index++ != RegList.Count - 1)
                                prop += getString(reg, addr, mais, lsb, msb, type, fpga, init) + ",\n";
                            else
                                prop += getString(reg, addr, mais, lsb, msb, type, fpga, init) + "\n";
                        }
                    }
                }
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
                System.IO.File.WriteAllText(PathToFile.Text, res);
            }
            catch (IOException t)
            {
                MessageBox.Show("ArgumentException " + t.ToString());
            }
        }

        private void RegGroupOpts_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
