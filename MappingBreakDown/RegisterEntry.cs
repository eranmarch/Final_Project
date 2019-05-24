using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace MappingBreakDown
{
    public class RegisterEntry //: IComparable<RegisterEntry>
    {
        /* Globals */
        public enum type_field { RD, WR, RD_WR, FIELD };
        public enum fpga_field { G, D, A, B, C, ABC, ABCG };
        public enum Reg_entry_field { Name, Address, MAIS, LSB, MSB, Type, FPGA, INIT, comment };

        public static string[] valid_type = { "RD", "WR", "RD_WR", "FIELD" };
        public static string[] valid_fpga = { "G", "D", "A", "B", "C", "ABC", "ABCG" };
        public static string[] valid_type_lower = { "rd", "wr", "rd_wr", "field" };
        public static string[] valid_fpga_lower = { "g", "d", "a", "b", "c", "abc", "abcg" };

        /* Class fields */
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
        public List<RegisterEntry> Fields { get; set; }
        public bool IsValid { get; set; }
        public bool IsComment { get; set; }
        public string Reason { get; set; }
        public int Index { get; set; }
        public int SecondaryIndex { get; set; }

        public static string pattern = @"^[ \t]*\(([a-zA-Z][a-zA-Z0-9_ ]*)[ \t]*,[ \t]*(\d+)[ \t]*,[ \t]*([0124])[ \t]*,[ \t]*(\d+)[ \t]*,[ \t]*(\d+)[ \t]*,[ \t]*([a-zA-Z_]+)[ \t]*,[ \t]*([a-zA-Z_]+)[ \t]*,[ \t]*(\w+)[ \t]*\)[ \t]*,[ \t]*(--[ \t]*(.*)[ \t]*)*";
        public static string final_pattern = @"^[ \t]*\(([a-zA-Z][a-zA-Z0-9_ ]*)[ \t]*,[ \t]*(\d+)[ \t]*,[ \t]*([0124])[ \t]*,[ \t]*(\d+)[ \t]*,[ \t]*(\d+)[ \t]*,[ \t]*([a-zA-Z_]+)[ \t]*,[ \t]*([a-zA-Z_]+)[ \t]*,[ \t]*(\w+)[ \t]*\)[ \t]*(--[ \t]*(.*)[ \t]*)*";

        /* Constructors */
        public RegisterEntry() : this("", -1, 0, 0, 31, type_field.RD, fpga_field.G, "", "", "") { }

        public RegisterEntry(string Name, int Address, int MAIS, int LSB, int MSB,
            type_field Type, fpga_field FPGA, string Init, string Comment, string Group)
        {
            this.Name = Name;
            this.Address = Address;
            this.MAIS = MAIS;
            this.LSB = LSB;
            this.MSB = MSB;
            this.Type = Type;
            this.FPGA = FPGA;
            this.Init = Init;
            this.Comment = Comment;
            this.Group = Group;
            Fields = new List<RegisterEntry>();
            IsValid = true;
            IsComment = false;
            Reason = "";
            Index = -1;
            SecondaryIndex = -1;
        }

        public RegisterEntry(string Name, int Address, string MAIS, string LSB, string MSB,
            string type, string FPGA, string Init, string Comment, string Group) :
            this(Name, Address, int.Parse(MAIS), int.Parse(LSB), int.Parse(MSB),
                (type_field)Enum.Parse(typeof(type_field), type, true),
                (fpga_field)Enum.Parse(typeof(fpga_field), FPGA, true),
                Init, Comment, Group)
        { }

        /* Get and Set functions */
        public String GetName()
        {
            return Name;
        }

        public void SetName(string Name)
        {
            this.Name = Name;
        }

        public int GetAddress()
        {
            return Address;
        }

        public void SetAddress(int Address)
        {
            this.Address = Address;
        }

        public int GetMAIS()
        {
            return MAIS;
        }

        public void SetMAIS(int Mais)
        {
            MAIS = Mais;
        }

        public int GetLSB()
        {
            return LSB;
        }

        public void SetLSB(int LSB)
        {
            this.LSB = LSB;
        }

        public int GetMSB()
        {
            return MSB;
        }

        public void SetMSB(int MSB)
        {
            this.MSB = MSB;
        }

        public type_field GetRegType()
        {
            return Type;
        }

        public void SetRegType(type_field Type)
        {
            this.Type = Type;
        }

        public void SetRegType(string Type)
        {
            SetRegType((type_field)Enum.Parse(typeof(type_field), Type, true));
        }

        public fpga_field GetFPGA()
        {
            return FPGA;
        }

        public void SetFPGA(fpga_field FPGA)
        {
            this.FPGA = FPGA;
        }

        public void SetFPGA(string FPGA)
        {
            SetFPGA((fpga_field)Enum.Parse(typeof(fpga_field), FPGA, true));
        }

        public String GetInit()
        {
            return Init;
        }

        public void SetInit(string Init)
        {
            this.Init = Init;
        }

        public String GetComment()
        {
            return Comment;
        }

        public void SetComment(string Comment)
        {
            this.Comment = Comment;
        }

        public String GetGroup()
        {
            return Group;
        }

        public void SetGroup(string Group)
        {
            this.Group = Group;
        }

        public List<RegisterEntry> GetFields()
        {
            return Fields;
        }

        public string GetFormat()
        {
            return pattern;
        }

        public void AddField(RegisterEntry Field)
        {
            Field.SetGroup(Group);
            Field.SetSecondaryIndex(Fields.Count);
            Fields.Add(Field);
        }

        public string GetReason()
        {
            return Reason;
        }

        public void SetReason(string reason)
        {
            Reason = reason;
        }

        public bool GetValid()
        {
            return IsValid;
        }

        public void SetValid(bool valid)
        {
            IsValid = valid;
        }

        public bool GetIsComment()
        {
            return IsComment;
        }

        public void SetIsComment(bool IsComment)
        {
            this.IsComment = IsComment;
        }

        public int GetIndex()
        {
            return Index;
        }

        public void SetIndex(int Index)
        {
            this.Index = Index;
        }

        public int GetSecondaryIndex()
        {
            return SecondaryIndex;
        }

        public void SetSecondaryIndex(int index)
        {
            SecondaryIndex = index;
        }

        /* Validation Functions */
        public bool IsValidLsbMsb()
        {
            return MSB >= LSB;
        }

        public static bool IsValidLsbMsb(string msb, string lsb)
        {
            int lsbInt = Int32.Parse(lsb);
            int msbInt = Int32.Parse(msb);
            return msbInt >= lsbInt;
        }

        public bool IsValidMAIS()
        {
            return MAIS == 0 || MAIS == 1 || MAIS == 2 || MAIS == 4;
        }

        public bool IsValidAddress()
        {
            return Address >= 0 && Address < 1024;
        }

        // Check fields don't intersect
        public bool FieldValidation()
        {
            if (!IsComment && Fields.Count > 0)
            {
                List<Interval> fieldsIntervals = new List<Interval>();
                foreach (RegisterEntry item in Fields)
                    fieldsIntervals.Add(new Interval(item.Name, item.LSB, item.MSB));
                Tuple<string, string> inter = Interval.IsIntersectList(fieldsIntervals);
                string field1 = inter.Item1, field2 = inter.Item2;
                if (!(field1.Equals("") && field2.Equals("")))
                {
                    Reason = "Field bits " + field1 + " and " + field2 + " of register " + GetName() + " (" + Address + ") intersect";
                    IsValid = false;
                    return false;
                }
            }
            return true;
        }

        public bool IsValidLSB()
        {
            return LSB >= 0 && LSB < 32;
        }

        public bool IsValidMSB()
        {
            return MSB >= 0 && MSB < 32;
        }

        public static bool IsValidType(string Type)
        {
            try
            {
                type_field t = (type_field)Enum.Parse(typeof(type_field), Type, true);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        public static bool IsValidFPGA(string fpga)
        {
            try
            {
                fpga_field t = (fpga_field)Enum.Parse(typeof(fpga_field), fpga, true);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }
        
        /* Output Functions */
        // Returns x spaces
        public static string getSpaces(int x)
        {
            return string.Concat(Enumerable.Repeat(" ", x));
        }

        public static RegisterEntry RegEntryParse(String str_entry, String group, bool last)
        {
            string actual = pattern;
            if (last)
                actual = final_pattern;
            string[] fields = Regex.Split(str_entry, actual);
            if (fields.Length > 1)
            {
                string comment = "";
                if (fields.Length == 12)
                    comment = fields[10];
                if (!IsValidType(fields[6]) || !IsValidFPGA(fields[7]))
                    return null;
                return new RegisterEntry(fields[1], Int32.Parse(fields[2]), fields[3], fields[4], fields[5], fields[6], fields[7], fields[8], comment, group);
            }
            return null;
        }

        public void EditRegister(string mais, string lsb, string msb, type_field t, fpga_field r, string init, string comment, string group)
        {
            MAIS = Int32.Parse(mais);
            LSB = Int32.Parse(lsb);
            MSB = Int32.Parse(msb);
            Type = t;
            FPGA = r;
            Init = init;
            Comment = comment;
            Group = group;
        }

        override
        public string ToString()
        {
            string addr = GetAddress().ToString();
            string mais = MAIS.ToString();
            string lsb = LSB.ToString();
            string msb = MSB.ToString();
            string type = valid_type[(int)Type];
            string fpga = valid_fpga[(int)FPGA];
            int spaces;
            if (Type.Equals(type_field.FIELD))
                spaces = 4;
            else
                spaces = 0;

            string ___reg_name___ = getSpaces(16) + "(" + Name + getSpaces((56 - spaces - ((17 + Name.Length))));
            string __address = getSpaces(8 - addr.Length) + addr;
            string __mais = getSpaces(3 - mais.Length) + mais;
            string __lsb__msb = getSpaces(3 - lsb.Length) + lsb + "," + getSpaces(3 - msb.Length) + msb;
            string _type__ = " " + type + getSpaces(5 - type.Length);
            string _fpga__ = " " + fpga + getSpaces(4 - fpga.Length);
            string __init;
            if (int.TryParse(Init, out int x))
                __init = getSpaces(5 - Init.Length) + Init;
            else
                __init = Init;
            return ___reg_name___ + "," + __address + "," + __mais + "," + __lsb__msb + "," + _type__ + "," + _fpga__ + "," + __init + ")";
        }

        public object[] GetTableEntry()
        {
            return new object[] { Name, Address, MAIS, LSB, MSB, Type, FPGA, Init, Comment, Index, SecondaryIndex };
        }

        /*public int CompareTo(RegisterEntry other)
        {
            int comp = Name.CompareTo(other.GetName());
            if (comp < 0)
                return -1;
            else if (comp > 0)
                return 1;
            comp = Address.CompareTo(other.GetAddress());
            if (comp < 0)
                return -1;
            else if (comp > 0)
                return 1;
            comp = MAIS.CompareTo(other.GetMAIS());
            if (comp < 0)
                return -1;
            else if (comp > 0)
                return 1;
            comp = LSB.CompareTo(other.GetLSB());
            if (comp < 0)
                return -1;
            else if (comp > 0)
                return 1;
            comp = MSB.CompareTo(other.GetMSB());
            if (comp < 0)
                return -1;
            else if (comp > 0)
                return 1;
            comp = Type.CompareTo(other.GetRegType());
            if (comp < 0)
                return -1;
            else if (comp > 0)
                return 1;
            comp = FPGA.CompareTo(other.GetFPGA());
            if (comp < 0)
                return -1;
            else if (comp > 0)
                return 1;
            comp = Init.CompareTo(other.GetInit());
            if (comp < 0)
                return -1;
            else if (comp > 0)
                return 1;
            comp = Group.CompareTo(other.GetGroup());
            if (comp < 0)
                return -1;
            else if (comp > 0)
                return 1;
            return 0;
        }*/
    }
}
