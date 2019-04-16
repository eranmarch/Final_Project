using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MappingBreakDown
{
    class FileValidator
    {
        public RegisterEntry[] Registers { get; set; }

        private String path_to_file;
        private char[] charsToTrimGlobal = { ' ', '\t' };
        private String[] keywords = { "signal", "bus", "component", "wait" };
        private String[] field_type = { "name", "address", "MAIS", "LSB", "MSB", "type", "FPGA", "INIT" };
        private String[] valid_type = { "rd", "wr", "rd_wr", "field" };
        private String[] valid_fpga = { "g", "d", "a", "b", "c", "abc", "abcg" };
        private String path_to_correct = "mycorrect.txt";

        enum Cmp_mod { Start, Reg_names, Middle, Reg_entrys, End };
        //enum Reg_entry_field { Name, Address, MAIS, LSB, MSB, Type, FPGA, INIT, comment };

        public FileValidator(String path_to_file)
        {
            this.path_to_file = path_to_file;
            //IsFileValid();
        }

        public List<RegisterEntry> GetRegList()
        {
            return this.Registers.ToList();
        }

        private String TrimAndLower(String str)
        {
            str = str.Trim(charsToTrimGlobal);
            str = str.ToLower();
            return str;
        }

        private String RemoveComment(String str)
        {
            int comment_index = 0;
            if (-1 != (comment_index = str.IndexOf("--")))
            {
                str = str.Substring(0, comment_index).TrimEnd(charsToTrimGlobal);
            }
            return str;
        }

        private String[] SubArray(String[] arr, int start, int length)
        {
            String[] res = new String[length];
            for (int i = 0; i < length; i++)
            {
                res[i] = arr[start + i];
            }
            return res;
        }

        private bool IsValidRegEntry(String entry)
        {
            entry = RemoveComment(entry);
            if (entry.Equals(""))
                return true;
            String[] fields = entry.Split(',');
            int numeric_field_value = -1;
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].Trim(charsToTrimGlobal);
                if (i == 0) //name field
                {
                    if ((fields[i].LastIndexOf('(') != 0) ||
                        (fields[i].IndexOf('(') != (fields[i].LastIndexOf('('))))
                    {
                        MessageBox.Show("Register named " + fields[0] + " has invalid " + fields[i]);
                        return false;
                    }
                    fields[i] = fields[i].TrimStart('(');

                }
                // numeric field
                if ((i > 0 && i < 5) && !Int32.TryParse(fields[i], out numeric_field_value))
                {
                    MessageBox.Show("Register named " + fields[0] + " has invalid " + field_type[i]);
                    return false;
                }
                if (i == 5) //type field
                {
                    int j;
                    for (j = 0; j < valid_type.Length && !fields[i].Equals(valid_type[j]); j++) { }
                    if (j == valid_type.Length)
                    {
                        MessageBox.Show("Register named " + fields[0] + " has invalid " + field_type[i]);
                        return false;
                    }
                }
                if (i == 6) //fpga field
                {
                    int j;
                    for (j = 0; j < valid_fpga.Length && !fields[i].Equals(valid_fpga[j]); j++) { }
                    if (j == valid_fpga.Length)
                    {
                        MessageBox.Show("Register named " + fields[0] + " has invalid " + field_type[i]);
                        return false;
                    }
                }
                if (i == 7) //init field
                {
                    if ((fields[i].LastIndexOf(')') != fields[i].Length - 1) ||
                        (fields[i].IndexOf(')') != (fields[i].LastIndexOf(')'))))
                    {
                        MessageBox.Show("Register named " + fields[0] + " has invalid " + fields[i][i]);
                        return false;
                    }
                    fields[i] = fields[i].TrimEnd(')');
                }
            }
            return true;

        }

        private bool IsValidRegName(String reg_name, char stop)
        {
            reg_name = RemoveComment(reg_name);
            if (reg_name.Equals(""))
                return true;
            String trimmed_regname = reg_name.TrimStart(charsToTrimGlobal);
            if (!Char.IsLetter(trimmed_regname[0]))
                return false;
            if (reg_name.LastIndexOf(',') != reg_name.Length - 1 || reg_name.IndexOf(',') != reg_name.LastIndexOf(','))
                return false;
            int i;
            for (i = 1; i < trimmed_regname.Length - 1; i++)
                if (!(Char.IsLetter(trimmed_regname[i]) || Char.IsDigit(trimmed_regname[i]) || trimmed_regname[i] == '_' || trimmed_regname[i] == '-'))
                    return false;
            String sliced = trimmed_regname.Substring(0, trimmed_regname.Length - 1).Trim(charsToTrimGlobal);
            foreach (String keyword in keywords)
                if (keyword.Equals(sliced))
                    return false;
            return true;
        }
        private bool isNotComment(String str)
        {
            return !str.StartsWith("#");
        }
        private bool isNotCommentMakaf(String str)
        {
            return !str.StartsWith("--");
        }

        public bool IsFileValid()
        {
            String[] lines_correct = File.ReadAllLines(path_to_correct);
            String[] lines = File.ReadAllLines(path_to_file);
            for (int i = 0; i < lines_correct.Length; i++)
                lines_correct[i] = TrimAndLower(lines_correct[i]);
            for (int i = 0; i < lines.Length; i++)
                lines[i] = TrimAndLower(lines[i]);
            lines_correct = Array.FindAll<String>(lines_correct, isNotComment).ToArray<String>();
            lines = Array.FindAll<String>(lines, isNotComment).ToArray<String>();
            int run_state = (int)Cmp_mod.Start;
            int j = 0;
            int reg_names_start = 0;
            int reg_entries_start = 0;
            int reg_names_length = 0;
            int reg_entries_length = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                lines_correct[j] = RemoveComment(lines_correct[j]);
                while (j < lines_correct.Length && lines_correct[j].Equals(""))
                    j++;
                if (lines_correct[j].Equals("0o0o0o0o0o0o0o0o0o0o0o0o0o00o0o0o0o0o0o00o0o0o0o0o0"))
                {
                    j = j + 2;
                    run_state++;
                }
                if (run_state == (int)Cmp_mod.Reg_names || run_state == (int)Cmp_mod.Reg_entrys)
                {

                    if (run_state == (int)Cmp_mod.Reg_names)
                        reg_names_start = i;
                    else
                        reg_entries_start = i;
                    int k;
                    for (k = i; k < lines.Length && !lines_correct[j - 1].Equals(lines[k]); k++)
                    {
                        if (run_state == (int)Cmp_mod.Reg_names && !IsValidRegName(lines[k], ','))
                        {
                            MessageBox.Show("Invalid register name" + lines[k] + " at line " + (k + 1).ToString());
                            return false;
                        }
                        if (run_state == (int)Cmp_mod.Reg_entrys && !IsValidRegEntry(lines[k]))
                        {
                            //MessageBox.Show("Invalid register entry " + lines[k] + " at line " + (k + 1).ToString());
                            return false;
                        }
                    }
                    if (run_state == (int)Cmp_mod.Reg_names)
                        reg_names_length = k - i;
                    else
                        reg_entries_length = k - i;
                    run_state++;
                    i = k;
                }
                else
                {
                    lines[i] = RemoveComment(lines[i]);
                    while (i < lines.Length && lines[i].Equals(""))
                        i++;
                    if (!lines_correct[j].Equals(lines[i]))
                    {
                        MessageBox.Show("Invalid file\n" + lines[i] + "\n" + lines_correct[j]);
                        return false;
                    }
                    j++;
                }
            }
            String[] Reg_names = SubArray(lines, reg_names_start, reg_names_length);
            String[] Reg_entries = SubArray(lines, reg_entries_start, reg_entries_length);
            if (ValidRegLogic(Array.FindAll<String>(Reg_names, isNotCommentMakaf).ToArray<String>(), Reg_entries))
            {
                //MessageBox.Show("OK");
                return true;
            }
            return false;
        }

        private String NameDuplicate(String[] lst)
        {
            for (int i = 0; i < lst.Length; i++)
            {
                for (int j = 0; j < lst.Length; j++)
                {
                    if (j != i && lst[j].Equals(lst[i]))
                        return lst[j];
                }
            }
            return null;
        }

        private int AddressDuplicate(RegisterEntry[] lst)
        {
            int i, j;
            for (i = 0; i < lst.Length; i++)
            {
                if (lst[i].GetRegType() == RegisterEntry.type_field.FIELD)
                    continue;
                for (j = 0; j < lst.Length; j++)
                {
                    if (lst[j].GetRegType() == RegisterEntry.type_field.FIELD)
                        continue;
                    if ((i != j) &&
                        (lst[i].GetFPGA() == lst[j].GetFPGA()) &&
                        (lst[i].GetAddress() == lst[j].GetAddress()))
                        break;
                }
                if (j != lst.Length)
                    break;
            }
            return (i == lst.Length) ? -1 : i;
        }

        private String NamesCrossValid(String[] reg_names, String[] reg_entries_names)
        {
            for (int i = 0; i < reg_names.Length; i++)
            {
                int j;
                for (j = 0; j < reg_entries_names.Length; j++)
                {
                    if (reg_names[i].Equals(reg_entries_names[j]))
                        break;
                }
                if (j == reg_entries_names.Length)
                    return reg_names[i];
            }

            for (int i = 0; i < reg_entries_names.Length; i++)
            {
                int j;
                for (j = 0; j < reg_names.Length; j++)
                {
                    if (reg_entries_names[i].Equals(reg_names[j]))
                        break;
                }
                if (j == reg_names.Length)
                    return reg_entries_names[i];
            }
            return null;
        }

        private bool FieldValidation()
        {
            int sum;
            bool any;
            foreach (RegisterEntry entry in Registers)
            {
                sum = 0;
                any = false;
                List<Interval> fieldsIntervals = null;
                if (entry.Type != RegisterEntry.type_field.FIELD)
                {
                    foreach (RegisterEntry item in Registers)
                    {
                        if (item.Address == entry.Address && item.Type == RegisterEntry.type_field.FIELD)
                        {
                            if (!any)
                            {
                                any = true;
                                fieldsIntervals = new List<Interval>();
                            }
                            fieldsIntervals.Add(new Interval(item.Name, item.LSB, item.MSB));
                            sum += item.MSB - item.LSB + 1;
                        }
                    }
                    if (any && sum != entry.MSB - entry.LSB + 1)
                    {
                        MessageBox.Show("Fields bits of register " + entry.Name + " (" + entry.Address + "), don't sum up correctly");
                        return false;
                    }
                    if (any)
                    {
                        Tuple<string, string> inter = Interval.IsIntersectList(fieldsIntervals);
                        string field1 = inter.Item1, field2 = inter.Item2;
                        if (!(field1.Equals("") && field2.Equals("")))
                        {
                            MessageBox.Show("Fields " + field1 + " and " + field2 + " intersect in register " + entry.Name + " (" + entry.Address + ")");
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool ValidRegLogic(String[] Reg_names, String[] Reg_entries)
        {
            for (int i = 0; i < Reg_names.Length; i++)
                Reg_names[i] = Reg_names[i].Trim(',');
            int reg_count = Array.FindAll<String>(Reg_entries, isNotCommentMakaf).ToArray<String>().Length;
            Registers = new RegisterEntry[reg_count];
            String[] entries_names = new String[reg_count];
            String group = "";
            int j = 0;
            for (int i = 0; i < Reg_entries.Length; i++)
            {
                if (!isNotCommentMakaf(Reg_entries[i]))
                {
                    group = Reg_entries[i].Trim('-');
                    continue;
                }
                Registers[j] = RegisterEntry.RegEntryParse(Reg_entries[i], group);
                entries_names[j] = Registers[j].GetName();
                if (!Registers[j].IsValidAddress())
                {
                    MessageBox.Show("The register " + Registers[j].GetName() + " has invalid address " + Registers[j].GetAddress());
                    return false;
                }
                if (!Registers[j].IsValiMAIS())
                {
                    MessageBox.Show("The register " + Registers[j].GetName() + " has invalid MAIS field " + Registers[j].GetMAIS());
                    return false;
                }
                j++;
            }
            String dup = NameDuplicate(Reg_names);
            if (dup != null)
            {
                MessageBox.Show("The register " + dup + " is refrenced more than once in the declerations segment");
                return false;
            }

            dup = NameDuplicate(entries_names);
            if (dup != null)
            {
                MessageBox.Show("The register " + dup + " is refrenced more than once in the entries segment");
                return false;
            }

            String not_cross_name = NamesCrossValid(Reg_names, entries_names);
            if (null != not_cross_name)
            {
                MessageBox.Show("The register " + not_cross_name + " is refrenced in only one list");
                return false;
            }
            int adrs_dup = AddressDuplicate(Registers);
            if (-1 != adrs_dup)
            {
                MessageBox.Show("The address " + adrs_dup + " is already full");
                return false;
            }
            if (!FieldValidation())
                return false;
            return true;
        }
    }
}
