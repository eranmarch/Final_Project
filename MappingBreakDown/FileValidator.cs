﻿using System;
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
        readonly string pattern = @"^[ \t]*--[ \t]*(.*)[ \t]*";

        enum Cmp_mod { Start, Reg_names, Middle, Reg_entrys, End };

        public FileValidator(String path_to_file)
        {
            this.path_to_file = path_to_file;
            Registers = new List<RegisterEntry>();
            Groups = new List<string>();
            names = new List<string>();
        }

        public List<RegisterEntry> GetRegList()
        {
            return Registers;
        }

        public List<string> GetGroups()
        {
            return Groups;
        }

        public List<string> GetNames()
        {
            return names;
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

        private bool IsNotComment(String str)
        {
            return !str.StartsWith("#");
        }

        private bool isNotCommentMakaf(String str)
        {
            return !str.StartsWith("--");
        }

        private bool IsValidRegName(String reg_name)
        {
            string pattern = @"^[ \t]*([a-zA-Z][a-zA-Z0-9_]*)[ \t]*,[ \t]*";
            Match result = Regex.Match(reg_name, pattern);
            if (result.Success)
            {
                string[] substrings = Regex.Split(reg_name, pattern);
                //MessageBox.Show(substrings[1] + " " + substrings[1].Length.ToString());
                names.Add(substrings[1]);
                return true;
            }
            return false;
        }

        private RegisterEntry FindAtAdress(int i)
        {
            foreach (RegisterEntry entry in Enumerable.Reverse(Registers))
                if (entry.GetAddress() == i)
                    return entry;
            return null;
        }

        public bool IsFileValid()
        {
            // Prepare texts for comparison: remove comments and convert to lower case
            Console.WriteLine("Preparing to compile " + path_to_file);
            string[] lines_correct = File.ReadAllLines(path_to_correct);
            string[] lines = File.ReadAllLines(path_to_file);
            for (int i = 0; i < lines_correct.Length; i++)
                lines_correct[i] = TrimAndLower(lines_correct[i]);
            for (int i = 0; i < lines.Length; i++)
                lines[i] = TrimAndLower(lines[i]);
            lines_correct = Array.FindAll(lines_correct, IsNotComment).ToArray();
            lines = Array.FindAll(lines, IsNotComment).ToArray();

            // Init state of comparison
            int run_state = (int)Cmp_mod.Start;
            int j = 0;

            // Parsing Analysis: Compare
            Console.WriteLine("Compiling...");
            for (int i = 0; i < lines.Length; i++)
            {
                // Skip empty lines
                lines_correct[j] = RemoveComment(lines_correct[j]);
                while (j < lines_correct.Length && lines_correct[j].Equals(""))
                    j++;
                // Finished current state
                if (lines_correct[j].Equals("0o0o0o0o0o0o0o0o0o0o0o0o0o00o0o0o0o0o0o00o0o0o0o0o0"))
                {
                    j = j + 2;
                    run_state++;
                }
                if (run_state == (int)Cmp_mod.Reg_names || run_state == (int)Cmp_mod.Reg_entrys)
                {
                    int k;
                    string curr_group = "JACKSHIT", prev_group;
                    for (k = i + 1; k < lines.Length && !lines_correct[j - 1].Equals(lines[k]); k++)
                    {
                        // Save Groups
                        Match result = Regex.Match(lines[k], pattern);
                        if (result.Success)
                        {
                            prev_group = curr_group;
                            curr_group = Regex.Split(lines[k], pattern)[1];
                            RegisterEntry entry = RegisterEntry.RegEntryParse(curr_group, prev_group, lines[k + 1].Equals(lines_correct[j - 1]));
                            if (entry != null)
                            {
                                entry.SetIsComment(true);
                                if (entry.GetRegType() == RegisterEntry.type_field.FIELD)
                                {
                                    int address = entry.GetAddress();
                                    RegisterEntry re = FindAtAdress(address);
                                    if (re != null)
                                        re.AddField(entry);
                                }
                                else
                                    Registers.Add(entry);
                                curr_group = prev_group;
                            }
                            else if (!Groups.Contains(curr_group) && (curr_group == "" || curr_group[curr_group.Length - 1] != ','))
                                Groups.Add(curr_group);
                        }
                        // Save Names
                        else if (run_state == (int)Cmp_mod.Reg_names)
                        {
                            if (!IsValidRegName(lines[k]))
                            {
                                MessageBox.Show("COMPILATION 1: Parsing error at line " + (k + 1));
                                Console.WriteLine("COMPILATION 1: Parsing error at line " + (k + 1) + "\nFinishing compilation...");
                                return false;
                            }
                        }
                        else if (run_state == (int)Cmp_mod.Reg_entrys)
                        {
                            RegisterEntry entry = RegisterEntry.RegEntryParse(lines[k], curr_group, lines[k + 1].Equals(lines_correct[j - 1]));
                            if (entry != null)
                            {
                                //MessageBox.Show(entry.GetName() + " " + entry.GetName().Length.ToString());
                                string type = entry.GetRegType().ToString();
                                if (type.Equals("FIELD") || type.Equals("field"))
                                {
                                    int address = entry.GetAddress();
                                    RegisterEntry re = FindAtAdress(address);
                                    if (re != null)
                                        re.AddField(entry);
                                    else
                                    {
                                        MessageBox.Show("COMPILATION 2: Address " + address + " doesn't exist (" + (k + 1) + ")");
                                        Console.WriteLine("COMPILATION 2: Address " + address + " doesn't exist (" + (k + 1) + ")" + "\nFinishing compilation...");
                                        return false;
                                    }
                                }
                                else
                                    Registers.Add(entry);
                            }
                            else
                            {
                                MessageBox.Show("COMPILATION 3: Parsing error at line " + (k + 1));
                                Console.WriteLine("COMPILATION 3: Parsing error at line " + (k + 1) + "\nFinishing compilation...");
                                return false;
                            }
                        }
                    }
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
                        MessageBox.Show("COMPILATION 4: Invalid file\n" + lines[i] + "\n" + lines_correct[j]);
                        Console.WriteLine("COMPILATION 4: Invalid file\n" + lines[i] + "!=" + lines_correct[j] + "\nFinishing compilation...");
                        return false;
                    }
                    j++;
                }
            }
            Console.WriteLine("Logic analysis...");
             if (!NamesCrossValid())
               return false;
            ValidRegLogic(); // Sematic Analysis, add everything from here
            Console.WriteLine("Compilation is complete");
            return true;
        }

        private void AddressDuplicate()
        {
            for (int i = 1; i < Registers.Count; i++)
            {
                if (Registers[i].GetIsComment())
                    continue;
                for (int j = 0; j < i; j++)
                {
                    if (Registers[j].GetIsComment() || j == i)
                        continue;
                    if (Registers[i].GetAddress() == Registers[j].GetAddress())
                    {
                        Registers[i].SetReason("Register " + Registers[i].GetName() + " has the same address as register " + Registers[j].GetName());
                        Registers[i].SetValid(false);
                    }
                    if (Registers[i].GetName().Equals(Registers[j].GetName()))
                    {
                        Registers[i].SetReason("Register " + Registers[i].GetName() + "(" + Registers[i].GetAddress() + ")" + " appears at address " + Registers[j].GetAddress() + " already");
                        Registers[i].SetValid(false);
                    }
                }
            }
        }

        private bool NamesCrossValid()
        {
            string nameReg, nameField;
            foreach (RegisterEntry entry in Registers)
            {
                if (entry.GetIsComment())
                    continue;
                nameReg = entry.GetName().Trim();
                if (!names.Contains(nameReg))
                {
                    MessageBox.Show("Register " + nameReg + " doesn't appear in the first list");
                    return false;
                }
                List<RegisterEntry> fields = entry.GetFields();
                foreach (RegisterEntry field in fields)
                {
                    if (field.GetIsComment())
                        continue;
                    nameField = field.GetName().Trim();
                    if (!names.Contains(nameField))
                    {
                        MessageBox.Show("Field " + nameField + " of register " + nameReg + " doesn't appear in the first list");
                        return false;
                    }
                }
            }
            foreach (string name in names)
            {
                bool test = false;
                foreach (RegisterEntry entry in Registers)
                {
                    nameReg = entry.GetName().Trim();
                    if (nameReg.Equals(name.Trim()))
                    {
                        test = true;
                        break;
                    }
                    List<RegisterEntry> fields = entry.GetFields();
                    foreach (RegisterEntry field in fields)
                    {
                        nameField = field.GetName().Trim();
                        if (nameField.Equals(name.Trim()))
                        {
                            test = true;
                            break;
                        }
                    }  
                }
                if (!test)
                {
                    MessageBox.Show("Register " + name + " doesn't appear in the second list");
                    return false;
                }
            }
            return true;
        }

        public static bool ValidEntry(RegisterEntry entry)
        {
            if (entry.GetIsComment())
                return true;
            bool b = true;
            if (!entry.IsValidAddress())
            {
                entry.SetReason("The register " + entry.GetName() + " has invalid address: " + entry.GetAddress());
                entry.SetValid(false);
                b = false;
            }
            if (!entry.IsValidMAIS())
            {
                entry.SetReason("The register " + entry.GetName() + " has invalid MAIS field: " + entry.GetMAIS());
                entry.SetValid(false);
                b = false;
            }
            if (!entry.IsValidLSB())
            {
                entry.SetReason("The register " + entry.GetName() + "(" + entry.GetAddress() + ") has LSB out of range [0, 32)");
                entry.SetValid(false);
                b = false;
            }
            if (!entry.IsValidLSB())
            {
                entry.SetReason("The register " + entry.GetName() + "(" + entry.GetAddress() + ") has MSB out of range [0, 32)");
                entry.SetValid(false);
                b = false;
            }
            if (!entry.IsValidLsbMsb())
            {
                entry.SetReason("The register " + entry.GetName() + "(" + entry.GetAddress() + ") has MSB < LSB");
                entry.SetValid(false);
                b = false;
            }
            return b;
        }

        private void ValidRegLogic()
        {
            int n = Registers.Count;
            RegisterEntry entry;
            for (int j = 0; j < n; j++)
            {
                entry = Registers[j];
                ValidEntry(entry);
                foreach (RegisterEntry field in entry.GetFields())
                    ValidEntry(field);
                entry.FieldValidation();
            }
            AddressDuplicate();
        }
    }
}
