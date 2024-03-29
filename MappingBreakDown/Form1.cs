﻿using System;
using System.Diagnostics;
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

namespace MappingBreakDown
{
    public partial class MappingPackageAutomation : Form
    {

        public XmlSerializer xs;
        //public XmlSerializer ls;
        List<RegisterEntry> RegList;
        bool saved = true;

        public MappingPackageAutomation()
        {
            InitializeComponent();
            InitFields();
            xs = new XmlSerializer(typeof(List<RegisterEntry>));
            //ls = new XmlSerializer(typeof(List<string>));
            treeGridView1.Nodes.Add("");
            ReadDataBase();
            ColorInValid();
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
            //RegGroupOpts.SelectedIndex = 0;
            searchBox.Text = "";
        }

        /* Upon Insert to the table, allocate a new address to the register */
        private int FindAddress()
        {
            int i = 1, x;
            bool found;
            for (; i <= 1024; i++)
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
            treeGridView1.Nodes.Add(NewGroupText.Text);
            NewGroupText.Text = "";
        }

        private bool CheckDup(RegisterEntry new_entry)
        {
            int addr_new = new_entry.GetAddress();
            string name_new = new_entry.GetName();
            foreach (RegisterEntry item in RegList)
            {
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
            if (entry.GetRegType() != RegisterEntry.type_field.FIELD)
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
                ErrorMessage.Text = "Init field is empty, resort to default (0)";
                entry = new RegisterEntry(RegNameText.Text, -1, MAISOpts.Text, LSBOpts.Text, MSBOpts.Text, TypeOpts.Text, FPGAOpts.Text, "0", CommentText.Text, RegGroupOpts.Text);
            }
            else
            {
                ErrorMessage.Text = "Register named " + RegNameText.Text + " was added";
                entry = new RegisterEntry(RegNameText.Text, -1, MAISOpts.Text, LSBOpts.Text, MSBOpts.Text, TypeOpts.Text, FPGAOpts.Text, InitText.Text, CommentText.Text, RegGroupOpts.Text);
            }
            if (!InputValidation(entry))
                return;
            AddEntryToTable(entry);
            InitFields();
        }

        /* Open a file */
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileValidator fv = new FileValidator(openFileDialog1.FileName);
                if (fv.IsFileValid())
                {
                    PathToFile.Text = openFileDialog1.FileName;
                    File.WriteAllText(@"file_path.txt", openFileDialog1.FileName);
                    Console.WriteLine("Adding entries to table...");
                    AddManyRegisters(fv.GetRegList(), fv.GetGroups());
                }
            }
        }

        /* Edit a register */
        private void Load_Click(object sender, EventArgs e)
        {
            RegisterEntry re = null;
            TreeGridNode node = null;
            foreach (TreeGridNode item in treeGridView1.SelectedRows)
            {
                try
                {
                    re = RegList[(int)item.Cells["IndexColumn"].Value];
                    int index = (int)item.Cells["SecondaryIndexColumn"].Value;
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
                //InitFields();
                return;
            }
            Enum.TryParse(type, out RegisterEntry.type_field t);
            RegisterEntry.type_field s = entry.GetRegType();
            if ((s == RegisterEntry.type_field.FIELD && t != RegisterEntry.type_field.FIELD) ||
                s != RegisterEntry.type_field.FIELD && t == RegisterEntry.type_field.FIELD)
            {
                MessageBox.Show("Can't edit a field or create one using Load");
                TypeOpts.SelectedIndex = (int)s;
                return;
            }
            if (!RegisterEntry.IsValidLsbMsb(msb, lsb))
            {
                MessageBox.Show("Can't edit an entry to have LSB > MSB");
                //InitFields();
                return;
            }
            Enum.TryParse(fpga, out RegisterEntry.fpga_field r);
            entry.EditRegister(mais, lsb, msb, t, r, init, comment, group);
            OpenValidation();
            UpdateDataBase();
            EditCell(node, entry.GetTableEntry());
        }

        private void ColorNode(TreeGridNode node)
        {
            int index = (int)node.Cells["IndexColumn"].Value, indexSec = (int)node.Cells["SecondaryIndexColumn"].Value;
            RegisterEntry entry = RegList[index];
            //MessageBox.Show(entry.GetName() + ": [" + index + ", " + indexSec + "]");
            if (indexSec != -1)
                entry = entry.GetFields()[indexSec];
            for (int i = 0; i < treeGridView1.ColumnCount; i++)
                if (entry.GetIsComment())
                    node.Cells[i].Style.BackColor = Color.LimeGreen;
                else if (!entry.GetValid())
                    node.Cells[i].Style.BackColor = Color.Red;
                else
                    node.Cells[i].Style.BackColor = Color.White;
        }

        private void ColorInValid()
        {
            foreach (TreeGridNode group_node in treeGridView1.Nodes)
                foreach (TreeGridNode register in group_node.Nodes)
                {
                    ColorNode(register);
                    foreach (TreeGridNode field in register.Nodes)
                        ColorNode(field);
                }
        }

        private void UpdateDataBase()
        {
            FileStream fs = new FileStream(@"registers.txt", FileMode.Create, FileAccess.Write);
            xs.Serialize(fs, RegList);
            fs.Close();
            /*fs = new FileStream(@"groups.txt", FileMode.Create, FileAccess.Write);
            List<string> groups = new List<string>();
            foreach (string group in RegGroupOpts.Items)
                groups.Add(group);
            ls.Serialize(fs, groups);
            fs.Close();*/
            File.WriteAllText(@"file_path.txt", PathToFile.Text);

        }

        private void ReadDataBase()
        {
            FileStream fs;
            /*try
            {
                fs = new FileStream(@"groups.txt", FileMode.Open, FileAccess.Read);
                List<string> groups = (List<string>)ls.Deserialize(fs);
                foreach (string group in groups)
                {
                    RegGroupOpts.Items.Add(group);
                    treeGridView1.Nodes.Add(group);
                }
                fs.Close();
            }
            catch (Exception)
            {
                treeGridView1.Nodes.Add("");
            }*/
            try
            {
                Console.Write("Restoring registers from inner file 'registers.txt':");
                fs = new FileStream(@"registers.txt", FileMode.Open, FileAccess.Read);
                RegList = (List<RegisterEntry>)xs.Deserialize(fs);
                fs.Close();
                string group;
                foreach (RegisterEntry entry in RegList)
                {
                    group = entry.GetGroup();
                    if (!RegGroupOpts.Items.Contains(group))
                    {
                        RegGroupOpts.Items.Add(group);
                        treeGridView1.Nodes.Add(group);
                    }

                    UpdateTable(entry);
                    List<RegisterEntry> fields = entry.GetFields();
                    foreach (RegisterEntry field in fields)
                        UpdateTable(field);
                }
                Console.WriteLine(" SUCCESS");
            }
            catch (Exception e)
            {
                Console.WriteLine("FAILED\nException caught: " + e.Message + "\nReseting list...");
                RegList = new List<RegisterEntry>();
            }
            try
            {
                Console.Write("Restoring opened file from 'file_path.txt': ");
                PathToFile.Text = File.ReadAllText(@"file_path.txt");
                Console.WriteLine("SUCCESS");
            }
            catch (Exception e)
            {
                Console.WriteLine("FAILED\nException caught: " + e.Message + "\nReseting path...");
                PathToFile.Text = "";
            }
            //DataGridViewRow[] new_rows = { new DataGridViewRow(), new DataGridViewRow() };
            //treeGridView1.Rows.AddRange(InitializeArray<DataGridViewRow>(200));
        }
        T[] InitializeArray<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; ++i)
            {
                array[i] = new T();
            }
            return array;
        }

        private void EditCell(TreeGridNode cell, object[] ent)
        {
            for (int i = 0; i < ent.Length; i++)
                cell.Cells[i].Value = ent[i];
        }

        private void UpdateTable(RegisterEntry entry)
        {
            TreeGridNode node;
            string group = entry.GetGroup();
            object[] ent = entry.GetTableEntry();
            bool b, isField = entry.GetRegType() == RegisterEntry.type_field.FIELD;
            treeGridView1.Rows.AddRange();
            foreach (TreeGridNode group_node in treeGridView1.Nodes)
            {
                if (group_node.Cells["Registers"].Value.ToString().Equals(group))
                    if (!isField)
                    {
                        node = group_node.Nodes.Add(ent);
                        group_node.Expand();
                    }
                    else
                    {
                        TreeGridNode tmp = null;
                        group_node.Expand();
                        foreach (TreeGridNode reg in group_node.Nodes)
                        {
                            b = reg.GetIsExpanded();
                            reg.Expand();
                            if ((int)reg.Cells["IndexColumn"].Value == entry.GetIndex())
                            {
                                tmp = reg;
                                break;
                            }
                            else if (!b)
                                reg.Collapse();
                        }
                        if (tmp != null)
                        {
                            node = tmp.Nodes.Add(ent);
                            //group_node.Expand();
                            tmp.Expand();
                        }
                        break;
                    }
            }
        }

        private void AddEntryToTable(RegisterEntry entry, bool open = false)
        {
            bool isField = entry.GetRegType() == RegisterEntry.type_field.FIELD;
            if (isField)
            {
                RegList[entry.GetIndex()].AddField(entry);
            }
            else
            {
                entry.SetIndex(RegList.Count); // only outer index
                RegList.Add(entry);
            }
            if (!open)
                UpdateDataBase();
            UpdateTable(entry);
        }

        /*private void AddManyRegisters(List<RegisterEntry> entries, List<string> groups)
        {
            Console.Write("Adding groups: ");
            Queue<Queue<>>

            foreach (string group in groups)
                if (!RegGroupOpts.Items.Contains(group))
                {
                    RegGroupOpts.Items.Add(group);
                    treeGridView1.Nodes.Add(group);
                }

        }*/

        private void AddManyRegisters(List<RegisterEntry> entries, List<string> groups)
        {
            Console.Write("Adding groups: ");
            foreach (string group in groups)
                if (!RegGroupOpts.Items.Contains(group))
                {
                    RegGroupOpts.Items.Add(group);
                    treeGridView1.Nodes.Add(group);
                }
            Console.Write("DONE\n");
            List<RegisterEntry> fields;
            Console.Write("Adding registers: ");
            foreach (RegisterEntry entry in entries)
            {
                AddEntryToTable(entry, true);
                //MessageBox.Show(entry.GetName() + ", " + entry.GetSecondaryIndex().ToString());
                fields = entry.GetFields();
                foreach (RegisterEntry field in fields)
                {
                    //MessageBox.Show(field.GetName());
                    field.SetIndex(entry.GetIndex());
                    UpdateTable(field);
                }
            }
            Console.Write("DONE\nValidating logic with table: ");
            OpenValidation();
            UpdateDataBase();
            Console.WriteLine("DONE");
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
            foreach (TreeGridNode item in treeGridView1.SelectedRows)
            {
                try
                {
                    int i = (int)item.Cells["IndexColumn"].Value, j = (int)item.Cells["SecondaryIndexColumn"].Value;
                    if (j != -1)
                    {
                        s.Add(new Tuple<int, int>(i, j));
                        //MessageBox.Show(i.ToString() + ", " + j.ToString());
                    }
                    else
                    {
                        indices.Add(i);
                        //MessageBox.Show(i.ToString());
                    }
                    foreach (TreeGridNode group in treeGridView1.Nodes)
                    {
                        foreach (TreeGridNode sibling in group.Nodes)
                        {
                            if (j == -1)
                            {
                                //MessageBox.Show(sibling.Cells["IndexColumn"].Value.ToString());
                                if ((int)sibling.Cells["IndexColumn"].Value > i)
                                {
                                    sibling.Cells["IndexColumn"].Value = (int)sibling.Cells["IndexColumn"].Value - 1;
                                    foreach (TreeGridNode field in sibling.Nodes)
                                        field.Cells["IndexColumn"].Value = (int)field.Cells["IndexColumn"].Value - 1;
                                }
                            }
                            else
                            {
                                //MessageBox.Show(sibling.Cells["SecondaryIndexColumn"].Value.ToString());
                                if ((int)sibling.Cells["SecondaryIndexColumn"].Value > i)
                                    sibling.Cells["SecondaryIndexColumn"].Value = (int)sibling.Cells["SecondaryIndexColumn"].Value - 1;
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
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            string searchRes = searchBox.Text;
            foreach (TreeGridNode group_node in treeGridView1.Nodes)
            {
                group_node.Expand();
                foreach (TreeGridNode reg in group_node.Nodes)
                {
                    reg.Expand();
                    foreach (TreeGridNode field in reg.Nodes)
                    {
                        field.Visible = field.Cells["Registers"].Value.ToString().Contains(searchRes);
                    }
                    reg.Visible = reg.Cells["Registers"].Value.ToString().Contains(searchRes);
                }
            }
        }

        private String GetSpaces(int x)
        {
            return String.Concat(Enumerable.Repeat(" ", x));
        }

        private bool IsNum(String s)
        {
            return double.TryParse(s, out double num);
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
                TreeGridNode last_node = null;
                for (int i = treeGridView1.Nodes.Count - 1; i >= 0; i--)
                {
                    TreeGridNode last_group = treeGridView1.Nodes[i];
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
                RegisterEntry last = RegList[(int)last_node.Cells["IndexColumn"].Value];
                int k = (int)last_node.Cells["SecondaryIndexColumn"].Value;
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
                    ErrorMessage.Text = "File Saved!";

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
            foreach (TreeGridNode node in treeGridView1.Nodes)
                for (int i = node.Nodes.Count - 1; i >= 0; i--)
                    node.Nodes.Remove(node.Nodes[i]);
            for (int i = RegList.Count - 1; i >= 0; i--)
                RegList.RemoveAt(i);
            UpdateDataBase();
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

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (PathToFile.Text.Equals(""))
                return;
            if (saved)
            {
                PathToFile.Text = "";
                UpdateDataBase();
                Close();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to close the file without saving?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //SaveButton_Click(sender, e);
                    PathToFile.Text = "";
                    Close();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do nothing
                }
            }
        }

        private void TreeGridView1_SelectionChanged(object sender, EventArgs e)
        {
            RegisterEntry re = null;
            foreach (TreeGridNode item in treeGridView1.SelectedRows)
            {
                try
                {
                    re = RegList[(int)item.Cells["IndexColumn"].Value];
                    int index = (int)item.Cells["SecondaryIndexColumn"].Value;
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
                    ErrorMessage.Text = "";
                else if (!re.GetValid())
                    ErrorMessage.Text = re.GetReason();
                else
                    ErrorMessage.Text = "";
            }
        }

        private void TreeGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                Delete_Click(sender, e);
        }

        private void CommentButton_Click(object sender, EventArgs e)
        {
            List<int> indices = new List<int>();
            List<Tuple<int, int>> s = new List<Tuple<int, int>>();
            //List<RegisterEntry> fields;
            RegisterEntry entry;
            foreach (TreeGridNode item in treeGridView1.SelectedRows)
            {
                try
                {
                    int i = (int)item.Cells["IndexColumn"].Value, j = (int)item.Cells["SecondaryIndexColumn"].Value;
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
            foreach (TreeGridNode item in treeGridView1.SelectedRows)
            {
                try
                {
                    int i = (int)item.Cells["IndexColumn"].Value, j = (int)item.Cells["SecondaryIndexColumn"].Value;
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
        private void openManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string workingDir = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDir).Parent.FullName;
            Process.Start(projectDirectory + "\\MappingBreakDownMan.pdf");
        }

        private void MappingPackageAutomation_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseButton_Click(sender, e);
        }

        private void Size_Changed(object sender, System.EventArgs e)
        {
            
            treeGridView1.Size = new Size(Size.Width - 40, Size.Height - 384);
            panel5.Size = new Size(Size.Width - 40, treeGridView1.Size.Height + 304);
            ClearButton.Location = new Point(Size.Width - 68, 265);
        }
        private void AboutButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("MappingPackageAutomation Version 1.0\nCreated By Eran Marchesky and Eli Zeltser as a final project\n\nAdvisors: Dan Shalom and Eli Parente");
        }
    }
}
