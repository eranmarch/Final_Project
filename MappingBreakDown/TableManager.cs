
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;


namespace MappingBreakDown
{
    class TableManager
    {
        public DataSet dsDataset { set;  get; }

        public DataTable dtgroups, dtregisters, dtfields;

        private FileManager flMan { get; set; }

        public bool saved { get; set; }

        public TableManager(bool only_for_table)
        {
            InitGroupTable();
            InitDataBasesParams();
        }

        public TableManager()
        {
            flMan = new FileManager();
            ReadDatabase();
        }

        public TableManager(string file_name)
        {
            flMan = new FileManager(file_name);
            //
            dsDataset = flMan.GetDataSet();
            dtgroups = dsDataset.Tables[0];
            dtregisters = dsDataset.Tables[1];
            dtfields = dsDataset.Tables[2];

        }

        private void InitGroupTable()
        {
            dtgroups = new DataTable();
            dtgroups.Columns.Add("Group", typeof(string));
            dtgroups.Rows.Add("");
        }

        private void InitDataBasesParams()
        {
            dtregisters = new DataTable();
            dtregisters.Columns.Add("Group", typeof(string));
            dtregisters.Columns.Add("Name", typeof(string));
            dtregisters.Columns.Add("Address", typeof(string));
            dtregisters.Columns.Add("MAIS", typeof(string));
            dtregisters.Columns.Add("LSB", typeof(string));
            dtregisters.Columns.Add("MSB", typeof(string));
            dtregisters.Columns.Add("Type", typeof(string));
            dtregisters.Columns.Add("FPGA", typeof(string));
            dtregisters.Columns.Add("Init", typeof(string));
            dtregisters.Columns.Add("Comment", typeof(string));
            //
            dtregisters.Columns.Add("IsComment", typeof(bool));
            dtregisters.Columns.Add("IsReserved", typeof(bool));
            dtregisters.Columns.Add("IsValid", typeof(bool));
            dtregisters.Columns.Add("Reason", typeof(string));
            dtregisters.Columns.Add("Index", typeof(int));
            dtregisters.Columns.Add("SecondaryIndex", typeof(int));
            dtfields = new DataTable();
            dtfields.Columns.Add("Group", typeof(string));
            dtfields.Columns.Add("Name", typeof(string));
            dtfields.Columns.Add("Address", typeof(string));
            dtfields.Columns.Add("MAIS", typeof(string));
            dtfields.Columns.Add("LSB", typeof(string));
            dtfields.Columns.Add("MSB", typeof(string));
            dtfields.Columns.Add("Type", typeof(string));
            dtfields.Columns.Add("FPGA", typeof(string));
            dtfields.Columns.Add("Init", typeof(string));
            dtfields.Columns.Add("Comment", typeof(string));
            //
            dtfields.Columns.Add("IsComment", typeof(bool));
            dtfields.Columns.Add("IsReserved", typeof(bool));
            dtfields.Columns.Add("IsValid", typeof(bool));
            dtfields.Columns.Add("Reason", typeof(string));
            dtfields.Columns.Add("Index", typeof(int));
            dtfields.Columns.Add("SecondaryIndex", typeof(int));
            dsDataset = new DataSet();
            dsDataset.Tables.Add(dtgroups);
            dsDataset.Tables.Add(dtregisters);
            dsDataset.Tables.Add(dtfields);
            DataRelation groupsRegsRelation = new DataRelation("GroupsRegistersRelation", dsDataset.Tables[0].Columns["Group"], dsDataset.Tables[1].Columns["Group"], true);
            DataRelation regsFieldsRelation = new DataRelation("GroupsFieldsRelation", dsDataset.Tables[1].Columns["Index"], dsDataset.Tables[2].Columns["Index"], true);
            groupsRegsRelation.Nested = true;
            regsFieldsRelation.Nested = true;
            dsDataset.Relations.Add(groupsRegsRelation);
            dsDataset.Relations.Add(regsFieldsRelation);
        }

        public List<string> getTypeOpts()
        {
            return flMan.valid_type;
        }

        public List<string> getFPGAOpts()
        {
            return flMan.valid_fpga;
        }

        /* Upon Insert to the table, allocate a new address to the register */
        public int FindAddress()
        {
            int i = 1, y;
            bool found;
            for (; i <= 1024; i++)
            {
                found = true;
                foreach (DataRow x in dtregisters.Rows)
                {
                    if (int.TryParse(x.Field<string>("Address"), out y))
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

        private DataRow getRegAtAddress(int Address)
        {
            foreach(DataRow r in getRegisters())
            {
                if (r.Field<string>("Address").Equals(Address.ToString()))
                    return r;
            }
            throw new IndexOutOfRangeException("Invalid address for register");
        }

        public bool createAndDocument(string path)
        {
            flMan.saveToFile(path);
            return flMan.file_saved;
        }

        public void UpdateDatabase()
        {
            //validateAddressDup();
            //FileStream fs = new FileStream(@"registers.txt", FileMode.Create, FileAccess.Write);
            //dsDataset.WriteXml(fs);//, XmlWriteMode.WriteSchema);

            //File.WriteAllText(@"registers.txt", dsDataset.GetXml());
            //fs.Close();

            flMan.saveFilePath();
        }

        public void ReadDatabase()
        {
            FileStream fs;
            try
            {
                fs = new FileStream(@"registers.txt", FileMode.Open, FileAccess.Read);
                throw new IOException();
                dsDataset = new DataSet();
                dsDataset.ReadXml(fs, XmlReadMode.InferSchema);
                dtgroups = dsDataset.Tables[0];
                dtregisters = dsDataset.Tables[1];
                dtfields = dsDataset.Tables[2];
                fs.Close();
            }
            catch (IOException e)
            {
                InitGroupTable();
                InitDataBasesParams();
            }
            
        }

        public void AddGroup(string group)
        {
            try
            {
                dtgroups.Rows.Add(group);
            }
            catch (ConstraintException c)
            {
                // do nothing
            }
        }

        public void addRow(string Name, string MAIS, string LSB, string MSB,
            string Type, string FPGA, string Init, string Comment, string Group,
            int Address = -1, bool add_as_field_to_last_reg = false, bool as_reserved = false)
        {
            // where to add ?
            DataTable collection;
            int index, secondary_index;
            string address_field, group_field;

            if (!Type.Equals("FIELD"))
            {
                group_field = Group;
                address_field = Address.ToString();
                index = regsCount();
                secondary_index = -1;
                collection = dtregisters;
            }
            else
            {
                collection = dtfields;
                DataRow reg;
                if (add_as_field_to_last_reg)
                {
                    reg = getRegisters()[regsCount() - 1];
                    index = regsCount() - 1;
                }
                else
                {
                    reg = getRegAtAddress(Address);
                    index = reg.Field<int>("Index");
                }
                address_field = reg.Field<string>("Address");
                secondary_index = reg.GetChildRows("GroupsFieldsRelation").Length;
                group_field = reg.Field<string>("Group");
            }

            DataRow new_row = collection.NewRow();
            new_row["Name"] = Name;
            new_row["Address"] = address_field;
            new_row["MAIS"] = MAIS;
            new_row["LSB"] = LSB;
            new_row["MSB"] = MSB;
            new_row["Type"] = Type;
            new_row["FPGA"] = FPGA;
            new_row["Init"] = Init;
            new_row["Comment"] = Comment;
            new_row["Group"] = group_field;
            new_row["Index"] = index;
            new_row["SecondaryIndex"] = secondary_index;
            new_row["Reason"] = "";
            new_row["IsComment"] = false;
            new_row["IsReserved"] = as_reserved;
            new_row["IsValid"] = true;

            collection.Rows.Add(new_row);
        }

        // Add as a register
        public void AddComment(string Group, string Comment)
        {
            DataRow new_row = dtregisters.NewRow();
            new_row["Name"] = "";
            new_row["Address"] = "";
            new_row["MAIS"] = "";
            new_row["LSB"] = "";
            new_row["MSB"] = "";
            new_row["Type"] = "";
            new_row["FPGA"] = "";
            new_row["Init"] = "";
            new_row["Comment"] = Comment;
            new_row["Group"] = Group;
            new_row["Index"] = regsCount();
            new_row["SecondaryIndex"] = -1;
            new_row["Reason"] = "";
            new_row["IsComment"] = true;
            new_row["IsReserved"] = false;
            new_row["IsValid"] = true;
            dtregisters.Rows.Add(new_row);
        }

        public bool nameValid(string new_name)
        {
            /* Check for VHDL names */
            if (flMan.IsReservedName(new_name.ToLower()))
                return false;

            /* check for name duplications */
            foreach (DataRow r in dtregisters.Rows)
            {
                if (new_name.Equals(r.Field<string>("Name")))
                    return false;
            }

            foreach (DataRow r in dtfields.Rows)
            {
                if (new_name.Equals(r.Field<string>("Name")))
                    return false;
            }

            return true;
        }

        public void editFields( int index,
            string mais,
            string lsb,
            string msb,
            string type,
            string fpga,
            string init,
            string comment,
            string group)
        {
            dtregisters.Rows[index].SetField("MAIS", mais);
            dtregisters.Rows[index].SetField("LSB", lsb);
            dtregisters.Rows[index].SetField("MSB", msb);
            dtregisters.Rows[index].SetField("Type", type);
            dtregisters.Rows[index].SetField("FPGA", fpga);
            dtregisters.Rows[index].SetField("Init", init);
            dtregisters.Rows[index].SetField("Comment", comment);
            dtregisters.Rows[index].SetField("Group", group);
        }

        public void editFields(Tuple<int,int> pair,
            string mais,
            string lsb,
            string msb,
            string type,
            string fpga,
            string init,
            string comment,
            string group)
        {
            foreach(DataRow r in dtfields.Rows)
            {
                if (r.Field<int>("Index") == pair.Item1 &&
                    r.Field<int>("SecondaryIndex") == pair.Item2)
                {
                    r.SetField("MAIS", mais);
                    r.SetField("LSB", lsb);
                    r.SetField("MSB", msb);
                    r.SetField("Type", type);
                    r.SetField("FPGA", fpga);
                    r.SetField("Init", init);
                    r.SetField("Comment", comment);
                    r.SetField("Group", group);
                }
            }
        }

        public void editReason(int index, string reason)
        {
            dtregisters.Rows[index].SetField("Reason", reason);
        }

        public void editReason(Tuple<int, int> pair, string reason)
        {
            foreach (DataRow r in dtfields.Rows)
            {
                if (r.Field<int>("Index") == pair.Item1 &&
                    r.Field<int>("SecondaryIndex") == pair.Item2)
                {
                    r.SetField("Reason", reason);
                }
            }
        }

        public void validateLogic()
        {
            foreach(DataRow r in getRegisters())
            {
                if (r.Field<bool>("IsComment") || r.Field<bool>("IsReserved"))
                    continue;

                validateRegBits(r);
            }
            validateAddressDup();
            validateNameDup();
        }

        private void validateAddressDup()
        {
            string address, name;
            bool set_ok;

            for (int i = 0; i < regsCount(); i++)
            {
                set_ok = true;
                if (dtregisters.Rows[i].Field<bool>("IsComment") ||
                    dtregisters.Rows[i].Field<bool>("IsReserved"))
                    continue;

                address = dtregisters.Rows[i].Field<string>("Address");
                name = dtregisters.Rows[i].Field<string>("Name");
                for (int j = 0; j < dtregisters.Rows.Count; j++)
                {
                    if (i == j)
                        continue;

                    if (dtregisters.Rows[j].Field<bool>("IsComment") ||
                        dtregisters.Rows[j].Field<bool>("IsReserved"))
                        continue;

                    if (address.Equals(dtregisters.Rows[j].Field<string>("Address")))
                    {
                        dtregisters.Rows[i].SetField("IsValid", false);
                        dtregisters.Rows[j].SetField("IsValid", false);

                        dtregisters.Rows[i].SetField("Reason",
                            "Address " + address + " is already in the list at register " +
                            dtregisters.Rows[j].Field<string>("Name"));

                        dtregisters.Rows[j].SetField("Reason",
                            "Address " + address + " is already in the list at register " +
                            dtregisters.Rows[i].Field<string>("Name"));

                        set_ok = false;
                        continue;
                    }
                    dtregisters.Rows[j].SetField("IsValid", true);
                    dtregisters.Rows[j].SetField("Reason", "");
                }
                if (set_ok)
                {
                    dtregisters.Rows[i].SetField("IsValid", true);
                    dtregisters.Rows[i].SetField("Reason", "");
                }
            }
        }

        private void validateNameDup()
        {
            for (int i = 0; i < getRegisters().Count; i++)
            {
                // search register with the same name
                for (int j = 0; j < getRegisters().Count; j++)
                {
                    if (i == j) // skip self
                        continue;

                    setDupName(getRegisters()[i], getRegisters()[j]);
                }

                // search fields
                for (int j =0; j < dtfields.Rows.Count; j++)
                    setDupName(getRegisters()[i], dtfields.Rows[j]);
                
            }

            // check duplicaitions between fields
            for (int i = 0; i < dtfields.Rows.Count; i++)
            {
                for (int j = 0; j < dtfields.Rows.Count; j++)
                {
                    if (i == j) // skip self
                        continue;

                    setDupName(dtfields.Rows[i], dtfields.Rows[j]);
                }
            }
        }

        private void setDupName(DataRow r1, DataRow r2)
        {
            string name = r1.Field<string>("Name");
            if (name.Equals(r2.Field<string>("Name")))
            {
                r1.SetField("IsValid", false);
                r2.SetField("IsValid", false);

                r1.SetField("Reason",
                    "Name " + name + " is a duplicate");

                r2.SetField("Reason",
                    "Name " + name + " is a duplicate");
            }
        }

        public static bool validateMSBLSB(int lsb, int msb)
        {
            return validateMSBLSB(lsb, msb, 0, 31);
        }

        private static bool validateMSBLSB(int lsb, int msb, int min_lsb, int max_msb)
        {
            return (lsb < msb) &&
                   (lsb >= min_lsb) &&
                   (msb <= max_msb);
        }

        public bool validateRegBits(DataRow reg)
        {
            int lsb = int.Parse(reg.Field<string>("LSB"));
            int msb = int.Parse(reg.Field<string>("MSB"));

            if (validateMSBLSB(lsb,msb))
            {
                reg.SetField("IsValid", false);
                reg.SetField("Reason", "MSB and LSB are invalid");
                return false;
            }

            DataRow[] fields = reg.GetChildRows("GroupsFieldsRelation");
            List<Interval> fieldsIntervals = new List<Interval>();
            foreach (DataRow f in fields)
            {
                if (f.Field<bool>("IsComment") ||
                    f.Field<bool>("IsReserved"))
                    continue;

                int f_lsb = int.Parse(f.Field<string>("LSB"));
                int f_msb = int.Parse(f.Field<string>("MSB"));

                fieldsIntervals.Add(new Interval(
                    f.Field<string>("Name"),
                    f_lsb,
                    f_msb));
            }

            Tuple<string, string> inter = Interval.IsIntersectList(fieldsIntervals);
            string field1 = inter.Item1, field2 = inter.Item2;

            if (!(field1.Equals("") && field2.Equals("")))
            {
                reg.SetField("IsValid", false);
                reg.SetField("Reason", "Field bits " + field1 + " and " + field2 + " intersect");
                return false;
            }

            return true;
        }

        public bool validateField(DataRow father_reg, int lsb, int msb, int SecondaryIndex = -1)
        {
            int min_lsb = int.Parse(father_reg.Field<string>("LSB"));
            int max_msb = int.Parse(father_reg.Field<string>("MSB"));

            // check if in required range
            if (!validateMSBLSB(lsb, msb, min_lsb, max_msb))
                return false;

            DataRow[] fields = father_reg.GetChildRows("GroupsFieldsRelation");

            List<Interval> fieldsIntervals = new List<Interval>();

            if (SecondaryIndex == -1)
                fieldsIntervals.Add(new Interval("New", lsb, msb));     // add the new field

            // check all others
            foreach (DataRow f in fields)
            {
                if (f.Field<bool>("IsComment") ||
                    f.Field<bool>("IsReserved") ||
                    !f.Field<bool>("IsValid"))
                    continue;

                int f_lsb, f_msb;

                if (f.Field<int>("SecondaryIndex") != SecondaryIndex)
                {
                    f_lsb = int.Parse(f.Field<string>("LSB"));
                    f_msb = int.Parse(f.Field<string>("MSB"));
                }
                else
                {
                    f_lsb = lsb;
                    f_msb = msb;
                }

                fieldsIntervals.Add(new Interval(f.Field<string>("Name"),f_lsb,f_msb));
            }

            Tuple<string, string> inter = Interval.IsIntersectList(fieldsIntervals);
            string field1 = inter.Item1, field2 = inter.Item2;

            if (!(field1.Equals("") && field2.Equals("")))
                return false;

            return true;
        }

        public void setRowsType(List<int> lst, string is_what)
        {
            DataRow[] fields;
            foreach (int index in lst)
            {
                foreach (DataRow r in getRegisters())
                {
                    if (r.Field<int>("Index") != index)
                        continue;

                    fields = r.GetChildRows("GroupsFieldsRelation");
                    switch (is_what)
                    {
                        case "Comment":
                            foreach (DataRow f in fields)
                                f.SetField("IsComment", true);
                            r.SetField("IsComment", true);
                            break;

                        case "UnComment":
                            r.SetField("IsComment", false);
                            break;

                        case "Reserved":
                            foreach (DataRow f in fields)
                                f.SetField("IsReserved", true);
                            r.SetField("IsReserved", true);
                            break;

                        case "UnReserved":
                            r.SetField("IsReserved", false);
                            break;

                        case "Valid":
                            r.SetField("IsValid", true);
                            break;

                        case "UnValid":
                            r.SetField("IsValid", false);
                            break;

                        default:
                            throw new ArgumentException("Invalid is_What argument");
                    }
                }
            }
        }

        public void setRowsType(List<Tuple<int,int>> lst, string is_what)
        {
            foreach (Tuple<int,int> pair in lst)
            {
                foreach (DataRow f in dtfields.Rows)
                {
                    if (f.Field<int>("Index") != pair.Item1 ||
                        f.Field<int>("SecondaryIndex") != pair.Item2)
                        continue;
                    
                    switch (is_what)
                    {
                        case "Comment":
                            f.SetField("IsComment", true);
                            break;

                        case "UnComment":
                            f.SetField("IsComment", false);
                            break;

                        case "Reserved":
                            f.SetField("IsReserved", true);
                            break;

                        case "UnReserved":
                            f.SetField("IsReserved", false);
                            break;

                        case "Valid":
                            f.SetField("IsValid", true);
                            break;

                        case "UnValid":
                            f.SetField("IsValid", false);
                            break;

                        default:
                            throw new ArgumentException("Invalid is_What argument");
                    }
                }
            }
        }

        public void setRow(DataRow row, string Name, string Comment, string Init, 
            string LSB, string MSB, string Type, string FPGA, string Group)
        {
            row.SetField("Name", Name);
            row.SetField("Comment", Comment);
            row.SetField("Init", Init);
            row.SetField("LSB", LSB);
            row.SetField("MSB", MSB);
            row.SetField("Type", Type);
            row.SetField("FPGA", FPGA);
            row.SetField("Group", Group);
            row.SetField("IsValid", true);
        }

        public void RemoveEntries(List<Tuple<int, int>> index_pairs, List<int> indices)
        {

            // remove selected fields first     // possible reversed!
            RemoveEntries(index_pairs);

            // then remove selected registers
            RemoveEntries(indices);
            
            dtregisters.AcceptChanges();
            dtfields.AcceptChanges();

            reIndexTable();
        }

        private void RemoveEntries(List<Tuple<int, int>> index_pairs)
        {
            foreach (Tuple<int, int> pair in index_pairs)
            {
                for (int row_num = dtfields.Rows.Count - 1; row_num >= 0; row_num--)
                    if (dtfields.Rows[row_num].Field<int>("Index") == pair.Item1 &&
                        dtfields.Rows[row_num].Field<int>("SecondaryIndex") == pair.Item2)
                    {
                        dtfields.Rows[row_num].Delete();
                        break;
                    }
            }
        }

        private void RemoveEntries(List<int> indices)
        {
            foreach (int indcs in indices)
            {
                for (int row_num = getRegisters().Count - 1; row_num >= 0; row_num--)
                {
                    if (dtregisters.Rows[row_num].Field<int>("Index") == indcs)
                    {
                        // delete the fields related to it
                        DataRow[] fields = dtregisters.Rows[row_num].GetChildRows("GroupsFieldsRelation");
                        for (int f_row_num = fields.Length - 1; f_row_num >= 0; f_row_num--)
                            fields[f_row_num].Delete();

                        // delete the register
                        dtregisters.Rows[row_num].Delete();
                    }
                }
            }
        }

        private void reIndexTable()
        {
            reIndexDtRegs();
            if (dtfields.Rows.Count > 0)
                reIndexDtFields();
        }

        private void reIndexDtRegs()
        {
            for (int i = 0; i < dtregisters.Rows.Count; i++)
                dtregisters.Rows[i].SetField("Index", i);
            
        }

        private void reIndexDtFields()
        {
            int new_index = 0;
            int cur_parent_index = dtfields.Rows[0].Field<int>("Index");

            for (int i = 0; i < dtfields.Rows.Count; i++)
            {
                if (cur_parent_index != dtfields.Rows[i].Field<int>("Index"))
                {
                    new_index = 0;
                    cur_parent_index = dtfields.Rows[i].Field<int>("Index");
                }

                dtfields.Rows[i].SetField<int>("SecondaryIndex", new_index);
                new_index++;
            }
            
        }

        public int regsCount()
        {
            return dtregisters.Rows.Count;
        }

        public DataRowCollection getGroups()
        {
            return dtgroups.Rows;
        }

        public DataRowCollection getRegisters()
        {
            return dtregisters.Rows;
        }

        public DataRow getField(Tuple<int,int> pair)
        {
            foreach(DataRow r in dtfields.Rows)
                if (r.Field<int>("Index") == pair.Item1 &&
                    r.Field<int>("SecondaryIndex") == pair.Item2)
                    return r;

            throw new IndexOutOfRangeException("Invalid <Index,SecondaryIndex> pair");
        }

        public List<string> getNames()
        {
            List<string> res = new List<string>();

            // registers
            foreach(DataRow row in getRegisters())
            {
                if (row.Field<bool>("IsComment") || row.Field<bool>("IsReserved"))
                    continue;

                res.Add(row.Field<string>("Name"));
            }
            // fields
            foreach (DataRow row in dtfields.Rows)
            {
                if (row.Field<bool>("IsComment") || row.Field<bool>("IsReserved"))
                    continue;

                res.Add(row.Field<string>("Name"));
            }
            return res;
        }

        public bool[] getColor(int index, int indexSec)
        {
            // row is a register
            if (indexSec == -1)
                return new bool[] {
                dtregisters.Rows[index].Field<bool>("IsValid"),
                dtregisters.Rows[index].Field<bool>("IsComment"),
                dtregisters.Rows[index].Field<bool>("IsReserved")};

            // row is a field
            foreach (DataRow r in dtfields.Rows)
            {
                if (r.Field<int>("Index") == index &&
                     r.Field<int>("SecondaryIndex") == indexSec)
                {
                    return new bool[] {
                        r.Field<bool>("IsValid"),
                        r.Field<bool>("IsComment"),
                        r.Field<bool>("IsReserved")};
                }
            }

            throw new ArgumentOutOfRangeException("Invalid <Index,SecondaryIndex> pair");
        }

        public string getPathToFile()
        {
            return flMan.path_to_file;
        }

        public bool openFileSuccessfull()
        {
            return flMan.file_opened;
        }
    }
}
