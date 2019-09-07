using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;


namespace MappingBreakDown
{
    class FileManager
    {
        private string[] vhdl_names = { "abs", "access", "after", "alias", "all", "and", "architecture", "array", "assert", "attribute", "begin", "block", "body", "buffer", "bus", "case", "component", "configuration", "constant", "disconnect", "downto", "else", "elsif", "end", "entity", "exit", "file", "for", "function", "generate", "generic", "group", "guarded", "if", "impure", "in", "inertial", "inout", "is", "label", "library", "linkage", "literal", "loop", "map", "mod", "nand", "new", "next", "nor", "not", "null", "of", "on", "open", "or", "others", "out", "package", "port", "postponed", "procedure", "process", "pure", "range", "record", "register", "reject", "rem", "report", "return", "rol", "ror", "select", "severity", "signal", "shared", "sla", "sll", "sra", "srl", "subtype", "then", "to", "transport", "type", "unaffected", "units", "until", "use", "variable", "wait", "when", "while", "with", "xnor", "xor" };

        public List<string> valid_type { set; get; }
        public List<string> valid_fpga { set; get; }

        private bool last_flag { get; set; }

        public string path_to_file { get; set; }
        public bool file_opened { get; set; }
        public bool file_saved { get; set; }

        public TableManager dbMan { get; set; }

        public FileManager()
        {
            path_to_file = "";
            valid_type = new List<string> { "RD", "WR", "RD_WR", "FIELD" };
            valid_fpga = new List<string> { "G", "D", "A", "B", "C", "ABC", "ABCG" };
        }

        public FileManager(string path)
        {

            path_to_file = path;

            dbMan = new TableManager(false);
            if (!IsFileValid())
            {
                valid_type = new List<string> { "RD", "WR", "RD_WR", "FIELD" };
                valid_fpga = new List<string> { "G", "D", "A", "B", "C", "ABC", "ABCG" };
                file_opened = false;
            }
            else
                file_opened = true;
        }

        public DataSet GetDataSet()
        {
            return dbMan.dsDataset;
        }

        private void saveTemplate(string all_text)
        {
            string template = Regex.Replace(
                all_text,
                @"type\s+avalon_map_defenition\s+is\s+\((.*?)\s+last_deff\s*\)\s*;",
                "type        avalon_map_defenition  is  (\n0o0o0o0o0o0o0o0o0o0o0o0o0o00o0o0o0o0o0o00o0o0o0o0o0\n                last_deff) ;",
                RegexOptions.Singleline);
            template = Regex.Replace(
                template,
                @"constant\s+avalon_fields_table\s*:\s*fields_table_type\s*:=\s*\((.*?)\)\s*;",
                "constant    avalon_fields_table     : fields_table_type :=  (\n--               field name or port name, ADDRESS, MAIS, LSB, MSB, TYPE, FPGA, INIT\n0o0o0o0o0o0o0o0o0o0o0o0o0o00o0o0o0o0o0o00o0o0o0o0o0\n                );",
                RegexOptions.Singleline);

            File.WriteAllText(@"template.txt", template);
        }

        public bool IsReservedName(string name)
        {
            return vhdl_names.Contains(name);
        }

        private bool NamesCrossValid(List<string> lst1)
        {
            List<string> lst2 = dbMan.getNames();
            foreach (string name in lst2)
            {
                if (!lst1.Contains(name))
                {
                    MessageBox.Show("Register " + name + " doesn't appear in the first list");
                    return false;
                }
            }

            foreach (string name in lst1)
            {
                if (!lst2.Contains(name))
                {
                    MessageBox.Show("Register " + name + " doesn't appear in the first list");
                    return false;
                }
            }

            return true;
        }

        private bool IsFileValid()
        {
            string input = File.ReadAllText(path_to_file);

            // Get list of name definitions
            Match sliced = Regex.Match(input, @"type\s+avalon_map_defenition\s+is\s+\((.*?)\s+last_deff\s*\)\s*;",
                RegexOptions.Singleline);

            if (!sliced.Success)
            {
                MessageBox.Show("COMPILATION 1: could not find name definitions");
                return false;
            }

            // Get names list
            List<string> names = new List<string>();
            string cur_pattern = @"^\s*([a-zA-Z][a-zA-Z0-9_]*)\s*,";
            MatchCollection matches = Regex.Matches(sliced.Groups[1].ToString(), cur_pattern, RegexOptions.Multiline);

            foreach (Match match in matches)
                names.Add(match.Groups[1].Value);

            // Get list of RW_type
            sliced = Regex.Match(input, @"type\s+RW_type\s+is\s+\(((?:.*?)\))", RegexOptions.Singleline);

            if (!sliced.Success)
            {
                MessageBox.Show("COMPILATION 1: could not find RW_type");
                names = new List<string>();
                return false;
            }

            cur_pattern = @"([A-Z_]*)[,)]";
            matches = Regex.Matches(sliced.Groups[1].ToString(), cur_pattern);
            valid_type = new List<string>();
            foreach (Match match in matches)
                valid_type.Add(match.Groups[1].Value);

            // Get list of fpga_type
            sliced = Regex.Match(input, @"type\s+fpga_type\s+is\s+\(((?:.*?)\))\s*;");

            if (!sliced.Success)
            {
                MessageBox.Show("COMPILATION 1: could not find fpga_type");
                return false;
            }

            cur_pattern = @"([A-Z]*)[,\)]";
            matches = Regex.Matches(sliced.Groups[1].ToString(), cur_pattern);
            valid_fpga = new List<string>();
            foreach (Match match in matches)
                valid_fpga.Add(match.Groups[1].ToString());

            // Get the register entries
            sliced = Regex.Match(input, @"constant\s+avalon_fields_table\s*:\s*fields_table_type\s*:=\s*\((.*?)\)\s*;", RegexOptions.Singleline);

            if (!sliced.Success)
            {
                MessageBox.Show("COMPILATION 1: could not find avalon field table");
                return false;
            }

            string[] string_entries = sliced.Groups[1].ToString().Split("\n".ToCharArray());
            string group = "";


            char[] charsToTrim = { ' ', '\t', '\r' };
            int i;
            for (i = 0; i < string_entries.Length; i++)
            {
                string_entries[i] = string_entries[i].Trim(charsToTrim);

                if (string_entries[i].Equals(""))
                    continue;

                // skip this comment - its always the first line of the table for formatting
                if (Regex.Match(string_entries[i], @"^--\s*field name or port name").Success)
                    continue;

                // Check if "Group" comment
                Match match = Regex.Match(string_entries[i], @"^\s*--[Gg]\s*(.*)\s*");

                if (match.Success)
                {
                    group = match.Groups[1].ToString();
                    dbMan.AddGroup(group);
                    continue;
                }

                if (!entryParse(string_entries[i], group))
                {
                    MessageBox.Show("COMPILATION 2: cannot parse " + string_entries[i]);
                    return false;
                }

                if (last_flag)
                    break;
            }

            // last entry found - check next entries for comments
            if (last_flag)
            {
                for (i++;i< string_entries.Length; i++)
                {
                    string_entries[i] = string_entries[i].Trim(charsToTrim);

                    if (string_entries[i].Equals(""))
                        continue;

                    // Check if "Group" comment
                    Match match = Regex.Match(string_entries[i], @"^\s*--[Gg]\s*(.*)\s*");

                    if (match.Success)
                    {
                        group = match.Groups[1].ToString();
                        dbMan.AddGroup(group);
                        continue;
                    }

                    if (!entryParse(string_entries[i],group,true))
                    {
                        MessageBox.Show("COMPILATION 2: invalid format of last entry, check for '),' ");
                        return false;
                    }
                }
            }
            // last was not found - error
            else
            {
                MessageBox.Show("COMPILATION 3: invalid format of last entry, check for '),' ");
                return false;
            }

            // Logic Check
            if (!NamesCrossValid(names))
                return false;
            
            dbMan.validateLogic();

            // write template file
            saveTemplate(input);
            return true;
        }

        private bool entryParse(string str_entry, string cur_group, bool past_last = false)
        {
            // Check if line is a simple comment
            Match match = Regex.Match(str_entry, @"^\s*--([Rr])?\s*(.*)\s*");
            if (match.Success && !match.Groups[1].Success)
            {
                dbMan.AddComment(cur_group, match.Groups[2].ToString());
                return true;
            }
            string entry_pattern;
            if (!past_last)
                entry_pattern = @"\s*(--[Rr])?\s*\(";        // is reserved (1)
            else
                entry_pattern = @"\s*(--[Rr])\s*\(";        // is reserved (1)

            entry_pattern += @"([A-Za-z][A-Za-z0-9_]*)\s*,";    // name (2)
            entry_pattern += @"\s*(\d+)\s*,";                   // addres (3)
            entry_pattern += @"\s*([0-4])\s*,";                 // MAIS (4)
            entry_pattern += @"\s*(\d+)\s*,";                   // LSB (5)
            entry_pattern += @"\s*(\d+)\s*,";                   // MSB (6)
            entry_pattern += @"\s*([A-Z_]*)\s*,";               // Type (7)
            entry_pattern += @"\s*([A-Z_]*)\s*,";               // FPGA (8)
            entry_pattern += @"\s*(.+)\)";                      // init (9)

            // last comma and possible comment (10, 11)
            entry_pattern += @"\s*(,)?\s*(?:[ \t]*--\s*(.*)[ \t]*)?"; 

            // Split by regex
            match = Regex.Match(str_entry, entry_pattern);

            // could not parse - syntax error
            if (!match.Success)
                return false;

            GroupCollection fields = match.Groups;
            string comment = "";

            // past the last and not a comment
            if (past_last && !fields[1].Success)
                return false;

            if (!valid_type.Contains(fields[7].ToString().ToUpper()) ||
                !valid_fpga.Contains(fields[8].ToString().ToUpper()))
                return false;

            // if no comma - then this entry is the last - raise the flag
            if (!last_flag)
                last_flag = !fields[10].Success;

            if (fields[11].Success)
                comment = fields[11].ToString();

            // Add to registers table or fields table
            dbMan.addRow(
                fields[2].ToString(),
                fields[4].ToString(),
                fields[5].ToString(),
                fields[6].ToString(),
                fields[7].ToString(),
                fields[8].ToString(),
                fields[9].ToString(),
                fields[11].ToString(),
                cur_group,
                int.Parse(fields[3].ToString()),
                true,
                fields[1].Success);

            return true;
        }

        public void saveToFile(string path)
        {
            path_to_file = path + ".vhd";
            StreamReader file;
            try
            {
                if (File.Exists("template.txt"))
                    file = new StreamReader("template.txt");
                else
                    file = new StreamReader("mycorrect.txt");

                string line;
                string res = "";
                string title = Path.GetFileNameWithoutExtension(path_to_file);
                string date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                string introDec = "Original path: " + path_to_file + "</br>The following is a documentation for " + title + ". The table " +
                    "contains the registers created using the GUI.";

                string doc = "<html><head><title>" + title + " Documentation" + "</title>";

                doc += "<style>table, th, td { border: 1px solid black; } th, td {padding: 5px; text-align: center;}" + "</style></head><body>";
                doc += "<h1><font face = 'arial'><u>Documentation For " + title + "</h1></u>";
                doc += date;
                doc += "<h2>" + date + "<br/>" + introDec + "</h2>";
                doc += "<table style='width: 100 %'>";
                doc += "<tr><th>Name</th>";
                doc += "<th>Group</th>";
                doc += "<th>Address</th>";
                doc += "<th>Mais</th>";
                doc += "<th>LSB</th>";
                doc += "<th>MSB</th>";
                doc += "<th>TYPE</th>";
                doc += "<th>FPGA</th>";
                doc += "<th>Init</th>";
                doc += "<th>Comment</th>";
                doc += "</tr>";

                while ((line = file.ReadLine()) != null)
                {
                    if (line.Length == 0)
                    {
                        res += "\n";
                        continue;
                    }

                    if ('#' == line[0])
                        continue;

                    if (line.Equals("0o0o0o0o0o0o0o0o0o0o0o0o0o00o0o0o0o0o0o00o0o0o0o0o0"))
                        break;

                    res += line + "\n";
                }
                
                doc += getHTMLEntry();
                doc += "</table></font></body></html>";
                res += getNameDeclerations();
                while ((line = file.ReadLine()) != null)
                {
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
                res += getEntrys();
                while ((line = file.ReadLine()) != null)
                {
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
                    File.WriteAllText(path + ".vhd", res);
                    File.WriteAllText(path + "_Documentation.html", doc);
                    file_saved = true;
                }

                catch
                {
                    //MessageBox.Show("Invalid Path to File");
                }
            }

            catch (IOException t)
            {
                //MessageBox.Show("ArgumentException " + t.ToString());
            }
        }

        private string getNameDeclerations()
        {
            string res = "";

            foreach (DataRow g in dbMan.getGroups())
            {
                if (g.GetChildRows("GroupsRegistersRelation").Length == 0)
                    continue;

                res += "\t\t--G " + g.Field<string>("Group") + "\n";

                foreach (DataRow r in g.GetChildRows("GroupsRegistersRelation"))
                {
                    if (r.Field<bool>("IsComment"))
                    {
                        res += "\t\t-- " + r.Field<string>("Comment") + "\n";
                        continue;
                    }

                    if (r.Field<bool>("IsReserved"))
                        res += "--R ";

                    res += "\t\t\t\t\t" + r.Field<string>("Name") + ",\n";

                    foreach (DataRow f in r.GetChildRows("GroupsFieldsRelation"))
                    {
                        if (f.Field<bool>("IsComment"))
                            res += "\t\t\t-- " + f.Field<string>("Comment") + "\n";

                        if (f.Field<bool>("IsReserved"))
                            res += "--R";

                        res += "\t\t\t\t\t\t" + f.Field<string>("Name") + ",\n";
                    }
                }
            }
            return res;
        }

        private string getEntrys()
        {
            string res = "";
            int total_reg_count = 0;

            // Find the last entry that is not a comment
            Tuple<int, int> last_index_pair = new Tuple<int, int>(-1,-1);
            int accumulated_reg_index = dbMan.getRegisters().Count - 1;
            bool last_found = false;

            DataRowCollection groups = dbMan.getGroups();
            for (int k = groups.Count - 1; k>=0; k--)
            {
                if (groups[k].GetChildRows("GroupsRegistersRelation").Length == 0)
                    continue;
                DataRow[] regs = groups[k].GetChildRows("GroupsRegistersRelation");
                for (int i = regs.Length - 1; i >= 0; i--, accumulated_reg_index--)
                {
                    if (regs[i].Field<bool>("IsComment") ||
                        regs[i].Field<bool>("IsReserved"))
                        continue;

                    DataRow[] fields = regs[i].GetChildRows("GroupsFieldsRelation");
                    for (int j = fields.Length - 1; j >= 0; j--)
                    {

                        if (fields[j].Field<bool>("IsComment") ||
                            fields[j].Field<bool>("IsReserved"))
                            continue;

                        last_found = true;
                        last_index_pair = new Tuple<int, int>(accumulated_reg_index, j);
                        break;

                    }
                    if (last_found)
                        break;
                    
                    last_index_pair = new Tuple<int, int>(accumulated_reg_index, -1);
                    break;

                }
                if (last_found)
                    break;
            }

            foreach (DataRow g in dbMan.getGroups())
            {
                if (g.GetChildRows("GroupsRegistersRelation").Length == 0)
                    continue;

                res += "\t\t--G " + g.Field<string>("Group") + "\n";

                DataRow[] regs = g.GetChildRows("GroupsRegistersRelation");

                for (int i = 0; i< regs.Length; i++, total_reg_count++)
                {
                    DataRow[] fields = regs[i].GetChildRows("GroupsFieldsRelation");

                    // Add register entry
                    if (last_index_pair.Item2 == -1)    // i.e last isn't a field
                        res += entryToString(regs[i],
                            total_reg_count == last_index_pair.Item1);

                    else
                        res += entryToString(regs[i]);

                    for (int j = 0; j < fields.Length; j++)
                    {
                        // Add field entry
                        if (last_index_pair.Item2 == -1)    // i.e last isn't a field
                            res += entryToString(fields[j]);

                        else
                            res += entryToString(fields[j],
                                total_reg_count == last_index_pair.Item1 && j == last_index_pair.Item2);
                    }
                }
            }
            return res;
        }

        private string entryToString(DataRow entry, bool last = false)
        {
            string res = "";
            string Comment = entry.Field<string>("Comment");

            if (entry.Field<bool>("IsComment"))
                return "\t\t-- " + Comment + "\n";

            if (entry.Field<bool>("IsReserved"))
                res += "--R";

            string Name = entry.Field<string>("Name");
            string adrs = entry.Field<string>("Address");
            string MAIS = entry.Field<string>("MAIS");
            string lsb = entry.Field<string>("LSB");
            string msb = entry.Field<string>("MSB");
            string Type = entry.Field<string>("Type");
            string FPGA = entry.Field<string>("FPGA");
            string Init = entry.Field<string>("Init");

            if (entry.Field<string>("Type").Equals("FIELD"))
                res += "\t\t\t\t\t" + "(" + Name + getSpaces(35 - Name.Length) + ",";

            else
                res += "\t\t\t\t" + "(" + Name + getSpaces(39 - Name.Length) + ",";

            res += getSpaces(8 - adrs.Length) + adrs + ",";
            res += "  " + MAIS.ToString() + ",";
            res += getSpaces(3 - lsb.Length) + lsb + "," + getSpaces(3 - msb.Length) + msb + ",";
            res += " " + Type + getSpaces(5 - Type.Length) + ",";
            res += " " + FPGA + getSpaces(4 - FPGA.Length) + ",";

            if (int.TryParse(Init, out int x))
                res += getSpaces(Math.Max(4 - Init.Length, 0)) + Init + ")";

            else
                res += Init + ")";

            if (!last)
                res += ",";

            if (Comment != "")
                res += "\t-- " + Comment;

            res += "\n";
            return res;
        }

        // Returns x spaces
        public static string getSpaces(int x)
        {
            return string.Concat(Enumerable.Repeat(" ", x));
        }

        private string getHTMLEntry()
        {
            string res = "";

            foreach (DataRow g in dbMan.getGroups())
            {
                if (g.GetChildRows("GroupsRegistersRelation").Length == 0)
                    continue;

                
                DataRow[] regs = g.GetChildRows("GroupsRegistersRelation");

                for (int i = 0; i < regs.Length; i++)
                {
                    res += entryToXML(regs[i]);
                    DataRow[] fields = regs[i].GetChildRows("GroupsRegistersRelation");

                    for (int j = 0; j < fields.Length; j++)
                        res += entryToXML(fields[j]);
                }
            }
            return res;
        }

        private string entryToXML(DataRow entry)
        {
            string res = "";

            if (entry.Field<bool>("IsComment"))
                res += "<tr bgcolor = 'green'>";

            else if (entry.Field<bool>("IsReserved"))
                res += "<tr bgcolor = 'blue'>";

            else if (!entry.Field<bool>("IsValid"))
                res += "<tr bgcolor = 'red'>";

            else
                res += "<tr>";
            res += "<td>" + entry.Field<string>("Name");
            res += "</td><td>" + entry.Field<string>("Group");
            res += "</td><td>" + entry.Field<string>("Address");
            res += "</td><td>" + entry.Field<string>("MAIS");
            res += "</td><td>" + entry.Field<string>("LSB");
            res += "</td><td>" + entry.Field<string>("MSB");
            res += "</td><td>" + entry.Field<string>("Type");
            res += "</td><td>" + entry.Field<string>("FPGA");
            res += "</td><td>" + entry.Field<string>("Init");
            res += "</td><td>" + entry.Field<string>("Comment");
            return res;
        }
    }
}