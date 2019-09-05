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
    public class RegisterEntry 
    {
        /* Globals */
        public static string[] valid_type = { "RD", "WR", "RD_WR", "FIELD" } ;
        public static string[] valid_fpga = { "G", "D", "A", "B", "C", "ABC", "ABCG" };

        /* Class fields */
        public string Name { get; set; }
        public int Address { get; set; }
        public int MAIS { get; set; }
        public int LSB { get; set; }
        public int MSB { get; set; }
        public string Type;
        public string FPGA;
        public string Init { get; set; }
        public string Comment { get; set; }
        public string Group { get; set; }
        public List<RegisterEntry> Fields { get; set; }
        public bool IsValid { get; set; }
        public bool IsReserved { get; set; }
        public bool IsComment { get; set; }
        public string Reason { get; set; }
        public int Index { get; set; }
        public int SecondaryIndex { get; set; }

        public static string pattern = @"^[ \t]*\(([a-zA-Z][a-zA-Z0-9_]*)[ \t]*,[ \t]*(\d+)[ \t]*,[ \t]*([0124])[ \t]*,[ \t]*(\d+)[ \t]*,[ \t]*(\d+)[ \t]*,[ \t]*([a-zA-Z_]+)[ \t]*,[ \t]*([a-zA-Z_]+)[ \t]*,[ \t]*(\w+)[ \t]*\)[ \t]*,[ \t]*(--[ \t]*(.*)[ \t]*)*";
        public static string final_pattern = @"^[ \t]*\(([a-zA-Z][a-zA-Z0-9_]*)[ \t]*,[ \t]*(\d+)[ \t]*,[ \t]*([0124])[ \t]*,[ \t]*(\d+)[ \t]*,[ \t]*(\d+)[ \t]*,[ \t]*([a-zA-Z_]+)[ \t]*,[ \t]*([a-zA-Z_]+)[ \t]*,[ \t]*(\w+)[ \t]*\)[ \t]*(--[ \t]*(.*)[ \t]*)*";
        public static bool last_flag = false;

        /* Constructors */
        public RegisterEntry() : this("", -1, 0, 0, 31, "RD", "G", "", "", "") { }

        public RegisterEntry(string comment, string group) : this("", -1, 0, 0, 31, "", "", "", comment, group) { }

        public RegisterEntry(string Name, int Address, int MAIS, int LSB, int MSB,
            string Type, string FPGA, string Init, string Comment, string Group)
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
                this(Name,
                    Address,
                    int.Parse(MAIS),
                    int.Parse(LSB),
                    int.Parse(MSB),
                    type,
                    FPGA,
                    Init,
                    Comment,
                    Group) { }

        /* Get and Set functions */
        public string GetName()
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

        public string GetRegType()
        {
            return Type;
        }

        public void SetRegType(string Type)
        {
            this.Type = Type;
        }

        public string GetFPGA()
        {
            return FPGA;
        }

        public void SetFPGA(string FPGA)
        {
            this.FPGA = FPGA;
        }

        public string GetInit()
        {
            return Init;
        }

        public void SetInit(string Init)
        {
            this.Init = Init;
        }

        public string GetComment()
        {
            return Comment;
        }

        public void SetComment(string Comment)
        {
            this.Comment = Comment;
        }

        public string GetGroup()
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

        public bool GetIsReserved()
        {
            return IsReserved;
        }

        public void SetIsReserved(bool IsReserved)
        {
            this.IsReserved = IsReserved;
            foreach (RegisterEntry field in Fields)
            {
                field.SetIsReserved(IsReserved);
            }
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

        public static void ResetLastFlag()
        {
            last_flag = false;
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
            return valid_type.Contains(Type.ToUpper());
        }

        public static bool IsValidFPGA(string fpga)
        {
            return valid_fpga.Contains(fpga.ToUpper());
        }
        
        /* Output Functions */
        // Returns x spaces
        public static string getSpaces(int x)
        {
            return string.Concat(Enumerable.Repeat(" ", x));
        }

        /*public static RegisterEntry RegEntryParse(string str_entry, string group, bool last)
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
        }*/

        public static RegisterEntry RegEntryParse(string str_entry)
        {
            //string actual = @"\s*\(\s*([A-Za-z][A-Za-z0-9_]*)\s*,\s*(\d+)\s*,\s*([0-4])\s*\s*,\s*(\d+)\s*,\s*(\d+)\s*,\s*([a-zA-Z0-9_]*)\s*,\s*(\w+)\s*,\s*(.+)\s*\),?\s*(--\s*(.*))*";
            string entry_pattern = @"\s*(--)?([Rr])?\s*\(";        // is reserved
            entry_pattern += @"([A-Za-z][A-Za-z0-9_]*)\s*,";    // name
            entry_pattern += @"\s*(\d+)\s*,";                   // addres
            entry_pattern += @"\s*([0-4])\s*,";                 // MAIS
            entry_pattern += @"\s*(\d+)\s*,";                   // LSB 
            entry_pattern += @"\s*(\d+)\s*,";                   // MSB
            entry_pattern += @"\s*([A-Z_]*)\s*,";               // Type
            entry_pattern += @"\s*([A-Z_]*)\s*,";               // FPGA
            entry_pattern += @"\s*(.+)\)";                      // init

            entry_pattern += @"(,)?(?:[ \t]*--\s*(.*)[ \t]*)?";   // possible comment

            // Split by regex
            Match match = Regex.Match(str_entry, entry_pattern);

            if (!match.Success)
                return null;

            GroupCollection fields = match.Groups;
            string comment = "";

            if (!IsValidType(fields[8].ToString()) || !IsValidFPGA(fields[9].ToString()))
                return null;

            if (!last_flag && !fields[11].Success)
                last_flag = true;

            if (fields[12].Success)
                comment = fields[12].ToString();

            RegisterEntry res =  new RegisterEntry(
                fields[3].ToString(),
                Int32.Parse(fields[4].ToString()),
                fields[5].ToString(),
                fields[6].ToString(),
                fields[7].ToString(),
                fields[8].ToString(),
                fields[9].ToString(),
                fields[10].ToString(),
                comment,
                "");

            if (fields[1].Success && !fields[2].Success)
                return null;

            if (fields[1].Success)
            {
                res.IsReserved = true;
                last_flag = false;
            }
            else
                res.IsReserved = false;

            return res;

        }
        public void EditRegister(string mais, string lsb, string msb, string t, string r, string init, string comment, string group)
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

        //override
        public string toName()
        {
            string res = "";

            if (IsComment)
                return "\t\t-- " + Comment + "\n";

            if (IsReserved)
                res += "--R";

            if (!Type.ToUpper().Equals("FIELD"))
                res += "\t\t\t\t" + Name + ",\n";
            else
                res += "\t\t\t\t\t" + Name + ",\n";
            return res;
        }

        public string ToXMLstring()
        {
            string res = "";

            if (IsComment)
                res += "<tr bgcolor = 'green'>";

            else if (IsReserved)
                res += "<tr bgcolor = 'blue'>";

            else if (!IsValid)
                res += "<tr bgcolor = 'red'>";

            else
                res += "<tr>";
            res += "<td>" + Name;
            res += "</td><td>" + Group;
            res += "</td><td>" + Address.ToString();
            res += "</td><td>" + MAIS.ToString();
            res += "</td><td>" + LSB.ToString();
            res += "</td><td>" + MSB.ToString();
            res += "</td><td>" + Type;
            res += "</td><td>" + FPGA;
            res += "</td><td>" + Init;
            res += "</td><td>" + Comment + "</td></tr>";
            return res; 
        }

        public string ToEntry(bool last = false)
        {
            string res = "";

            if (IsComment)
                return "\t\t-- " + Comment + "\n";

            if (IsReserved)
                res += "--R";

            if (Type.ToUpper().Equals("FIELD"))
                res += "\t\t\t\t\t" + "(" + Name + getSpaces(35 - Name.Length) + ",";

            else
                res += "\t\t\t\t" + "(" + Name + getSpaces(39 - Name.Length) + ",";

            string adrs = Address.ToString();
            res += getSpaces(8 - adrs.Length) + adrs + ",";
            res += "  " + MAIS.ToString() + ",";
            string lsb = LSB.ToString();
            string msb = MSB.ToString();
            res += getSpaces(3 - lsb.Length) + lsb + "," + getSpaces(3 - msb.Length) + msb + ",";
            res += " " + Type + getSpaces(5 - Type.Length) + ",";
            res += " " + FPGA + getSpaces(4 - FPGA.Length) + ",";
            
            if (int.TryParse(Init, out int x))
                res += getSpaces(Math.Max(4-Init.Length, 0)) + Init + ")";
            
            else
                res += Init + ")";

            if (!last)
                res += ",";

            if (Comment != "")
                res += "\t-- " + Comment;
            res += "\n";
            return res;
        }

        public object[] GetTableEntry()
        {
            if (!IsComment)
                return new object[] {
                    Group,
                    Name,
                    Address.ToString(),
                    MAIS.ToString(),
                    LSB.ToString(),
                    MSB.ToString(),
                    Type,
                    FPGA,
                    Init,
                    Comment,
                    IsComment,
                    IsReserved,
                    IsValid,
                    Reason,
                    Index,
                    SecondaryIndex
            };
            else
                return new object[] {
                    Group,
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    Comment,
                    true,
                    false,
                    true,
                    "Comment line",
                    Index,
                    SecondaryIndex };
        }
    }
}
