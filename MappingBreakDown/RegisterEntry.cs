using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingBreakDown
{
    public class RegisterEntry
    {
        public enum type_field { RD, WR, RD_WR, FIELD };
        public enum fpga_field { G, D, A, B, C, ABC, ABCG };
        public enum Reg_entry_field { Name, Address, MAIS, LSB, MSB, Type, FPGA, INIT, comment };

        public static string[] valid_type = { "RD", "WR", "RD_WR", "FIELD" };
        public static string[] valid_fpga = { "G", "D", "A", "B", "C", "ABC", "ABCG" };
        public static string[] valid_type_lower = { "rd", "wr", "rd_wr", "field" };
        public static string[] valid_fpga_lower = { "g", "d", "a", "b", "c", "abc", "abcg" };

        public string Name { get; set; }
        public int Address { get; set; }
        public int MAIS { get; set; }
        public int LSB { get; set; }
        public int MSB { get; set; }
        public type_field Type { get; set; }
        public fpga_field FPGA { get; set; }
        public string Init { get; set; }
        public string Comment { get; set; }
        public string Group { get; set; }
        public bool isValid = true;

        public RegisterEntry()
        {
            this.MAIS = 0;
            this.LSB = 0;
            this.MSB = 31;
            this.Type = type_field.RD;
            this.FPGA = fpga_field.G;
            this.Init = "";
        }

        public RegisterEntry(string Name, int Address, int MAIS, int LSB, int MSB,
            type_field type, fpga_field FPGA, string Init, string Comment, string Group)
        {
            this.Name = Name;
            this.Address = Address;
            this.MAIS = MAIS;
            this.LSB = LSB;
            this.MSB = MSB;
            this.Type = type;
            this.FPGA = FPGA;
            this.Init = Init;
            this.Comment = Comment;
            this.Group = Group;
        }
        public RegisterEntry(string Name, int Address, string MAIS, string LSB, string MSB,
            string type, string FPGA, string Init, string Comment, string Group)
        {
            this.Name = Name;
            this.Address = Address;
            this.MAIS = int.Parse(MAIS);
            this.LSB = int.Parse(LSB);
            this.MSB = int.Parse(MSB);
            this.Type = FindTypeFromString(type);
            this.Init = Init;
            this.Comment = Comment;
            this.Group = Group;
        }

        private type_field FindTypeFromString(string type)
        {
            for (int i = 0; i < valid_type.Length; i++)
            {
                if (valid_type[i].Equals(type))
                {
                    return (type_field)i;
                }
            }
            return type_field.RD;   // default value - should't get here
        }

        private fpga_field FindFPGAFromString(string fpga)
        {
            for (int i = 0; i < valid_type.Length; i++)
            {
                if (valid_fpga[i].Equals(fpga))
                {
                    return (fpga_field)i;
                }
            }
            return fpga_field.G;   // default value - should't get here
        }

        public bool IsValidLsbMsb()
        {
            if (Type == type_field.FIELD)
                return true;
            return MSB >= LSB;
        }

        private bool IsValidMAIS()
        {
            return MAIS == 0 || MAIS == 1 || MAIS == 2 || MAIS == 4;
        }


        public bool IsValidAddress()
        {
            return Address >= 0 && Address < 1024;
        }

        private bool EntryIsInteger(int entry_index)
        {
            return (entry_index == (int)Reg_entry_field.MSB ||
                     entry_index == (int)Reg_entry_field.LSB ||
                     entry_index == (int)Reg_entry_field.MAIS);
        }

        private string getSpaces(int x)
        {
            return string.Concat(Enumerable.Repeat(" ", x));
        }

        public String GetName()
        {
            return Name;
        }

        public int GetAddress()
        {
            return Address;
        }

        public int GetMAIS()
        {
            return MAIS;
        }

        public bool IsValiMAIS()
        {
            return MAIS == 0 || MAIS == 1 || MAIS == 2 || MAIS == 4;
        }

        public int GetLSB()
        {
            return LSB;
        }

        public int GetMSB()
        {
            return MSB;
        }

        public type_field GetRegType()
        {
            return this.Type;
        }

        public fpga_field GetFPGA()
        {
            return FPGA;
        }

        public String GetInit()
        {
            return this.Init;
        }

        public String GetComment()
        {
            return Comment;
        }

        public String GetGroup()
        {
            return Group;
        }


        private bool isNum(String s)
        {
            double num;
            return double.TryParse(s, out num);
        }

        public string EntryToString()
        {
            string ___reg_name___ = getSpaces(16) + "(" + Name.ToString() + getSpaces((56 - ((17 + Name.ToString().Length))));
            string __address = getSpaces(8 - Address.ToString().Length) + Address.ToString();
            string __mais = getSpaces(3 - MAIS.ToString().Length) + MAIS.ToString();
            string __lsb__msb = getSpaces(3 - LSB.ToString().Length) + LSB.ToString() + "," + getSpaces(3 - MSB.ToString().Length) + MSB.ToString();
            string _type__ = " " + valid_type[(int)Type] + getSpaces(5 - Type.ToString("G").Length);
            string _fpga__ = " " + FPGA.ToString("G") + getSpaces(4 - FPGA.ToString("G").Length);
            string __init;
            if (int.TryParse(Init, out int x))
                __init = getSpaces(5 - Init.Length) + Init;
            else
                __init = Init;
            return ___reg_name___ + "," + __address + "," + __mais + "," + __lsb__msb + "," + _type__ + "," + _fpga__ + "," + __init + ")";
        }

        public static RegisterEntry RegEntryParse(String str_entry, String group)
        {
            int comment_index = -1;
            String[] fields = str_entry.Split(',');
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].Trim('(', ')', ' ', '\t');
            }
            if (fields.Length == 8) // last line doesn't contain ',' before comment
            {
                comment_index = str_entry.IndexOf("--");
                if (-1 == comment_index)   // no comment in last line
                {
                    return new RegisterEntry(
                                            fields[0],    // name
                                            Int32.Parse(fields[1]), // address
                                            Int32.Parse(fields[2]), // MAIS
                                            Int32.Parse(fields[3]), // LSB
                                            Int32.Parse(fields[4]), // MSB
                                            (type_field)Array.IndexOf(valid_type_lower, fields[5]),
                                            (fpga_field)Array.IndexOf(valid_fpga_lower, fields[6]),
                                            fields[7],    // init
                                            "",                     // no comment
                                            group);                 // group
                }
                else                       // comment in last line
                {
                    return new RegisterEntry(
                        fields[0],             // name
                        Int32.Parse(fields[1]),                     // address
                        Int32.Parse(fields[2]),                     // MAIS
                        Int32.Parse(fields[3]),                     // LSB
                        Int32.Parse(fields[4]),                     // MSB
                        (type_field)Array.IndexOf(valid_type_lower, fields[5]),
                        (fpga_field)Array.IndexOf(valid_fpga_lower, fields[6]),
                        fields[7].Substring(0, comment_index).Trim(')'),    // init
                        fields[7].Substring(comment_index + 2).Trim(' '),    //comment
                        group);                                     // group
                }
            }
            else
            {
                return new RegisterEntry(
                                        fields[0],        // name
                                        Int32.Parse(fields[1]),     // address
                                        Int32.Parse(fields[2]),     // MAIS
                                        Int32.Parse(fields[3]),     // LSB
                                        Int32.Parse(fields[4]),     // MSB
                                        (type_field)Array.IndexOf(valid_type_lower, fields[5]),
                                        (fpga_field)Array.IndexOf(valid_fpga_lower, fields[6]),
                                        fields[7].Trim(')'),        // init
                                        fields[8].Trim('-', ' '),    // comment
                                        group);                     // group
            }
        }
    }
}
