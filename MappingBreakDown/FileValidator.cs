using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;


namespace MappingBreakDown
{
    class FileValidator
    {
        private List<RegisterEntry> Registers { get; set; }
        private List<string> Groups;
        private List<string> names;

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
        }

        public List<RegisterEntry> GetRegList()
        {
            return this.Registers;
        }

        public List<string> getGroups()
        {
            return Groups;
        }

        /* Need to delete */
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

        private bool isNotComment(String str)
        {
            return !str.StartsWith("#");
        }

        private bool isNotCommentMakaf(String str)
        {
            return !str.StartsWith("--");
        }

        /* Need to delete */

        private bool IsValidRegName(String reg_name)
        {
            string pattern = @"^[ \t]*([a-zA-Z][a-zA-Z0-9]*)[ \t]*,[ \t]*";
            Match result = Regex.Match(reg_name, pattern);
            if (result.Success)
            {
                string[] substrings = Regex.Split(reg_name, pattern);
                names.Add(substrings[1]);
                return true;
            }
            return false;
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
                        if (run_state == (int)Cmp_mod.Reg_names)
                        {
                            if (!IsValidRegName(lines[k]))
                            {
                                MessageBox.Show("Parsing error at line " + (k + 1));
                                return false;
                            }
                        }
                        else if (run_state == (int)Cmp_mod.Reg_entrys)
                        {
                            RegisterEntry entry = RegisterEntry.RegEntryParse(lines[k], "");
                            if (entry != null)
                                Registers.Add(entry);
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
                    {
                        Registers[j].isValid = false;
                        return lst[j];
                    }
                }
            }
            return null;
        }

        private bool AddressDuplicate()
        {
            for (int i = 1; i < Registers.Length; i++)
                for (int j = 0; j < i; j++)
                {
                    if (Registers[i].GetAddress() == Registers[j].GetAddress())
                    {
                        Registers[i].SetReason("Register " + Registers[i].GetName() + " has the same address as register " + Registers[j].GetName());
                        Registers[i].SetValid(false);
                        //return false;
                    }
                    if (Registers[i].GetName().Equals(Registers[j].GetName()))
                    {
                        Registers[i].SetReason("Register " + Registers[i].GetName() + "(" + Registers[i].GetAddress() + ")" + " appears at address " + Registers[j].GetAddress() + " already");
                        Registers[i].SetValid(false);
                        //return false;
                    }
                }
            return true;
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
                {
                    Registers[i].isValid = false;
                    return reg_names[i];
                }
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
                {
                    Registers[i].isValid = false;
                    return reg_entries_names[i];
                }
            }
            return null;
        }

        private bool FieldValidation()
        {
            foreach (RegisterEntry entry in Registers)
                if (!entry.FieldValidation())
                {
                    //return false;
                }
            return true;
        }

        private bool ValidRegLogic(String[] Reg_names, String[] Reg_entries)
        {
            for (int i = 0; i < Reg_names.Length; i++)
                Reg_names[i] = Reg_names[i].Trim(',');
            int reg_count = Array.FindAll<String>(Reg_entries, isNotCommentMakaf).ToArray<String>().Length;
            Registers = new RegisterEntry[reg_count];
            Groups = new List<string>();
            String[] entries_names = new String[reg_count];
            String group = "";
            int j = 0;
            for (int i = 0; i < Reg_entries.Length; i++)
            {
                if (!isNotCommentMakaf(Reg_entries[i]))
                {
                    group = Reg_entries[i].Trim('-', ' ', '\t');
                    continue;
                }
                Registers[j] = RegisterEntry.RegEntryParse(Reg_entries[i], group);
                if (!Groups.Contains(group))
                    Groups.Add(group);
                entries_names[j] = Registers[j].GetName();
                if (!Registers[j].IsValidAddress())
                {
                    MessageBox.Show("The register " + Registers[j].GetName() + " has invalid address " + Registers[j].GetAddress());
                    Registers[j].isValid = false;
                    return false;
                }
                if (!Registers[j].IsValiMAIS())
                {
                    MessageBox.Show("The register " + Registers[j].GetName() + " has invalid MAIS field " + Registers[j].GetMAIS());
                    Registers[j].isValid = false;
                    return false;
                }
                j++;
            }

            String not_cross_name = NamesCrossValid(Reg_names, entries_names);
            if (null != not_cross_name)
            {
                MessageBox.Show("The register " + not_cross_name + " is refrenced in only one list");
                return false;
            }
            AddressDuplicate();
            FieldValidation();
            return true;
        }
    }
}
