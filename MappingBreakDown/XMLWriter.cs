using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MappingBreakDown
{
    class XMLWriter
    {
        XElement result { get; set; }
        RegisterEntry[] reg_list { get; set; }

        private struct RegTree
        {
            public RegisterEntry entry;
            public List<RegisterEntry> fields;
            public RegTree(List<RegisterEntry> regs_to_tree)
            {
                if (regs_to_tree.Count() == 1)
                {
                    entry = regs_to_tree.First();
                    fields = null;
                }
                else
                {
                    entry = regs_to_tree.First();
                    regs_to_tree.Remove(regs_to_tree.OrderBy(x => x.Address).First());
                    fields = regs_to_tree;
                }
            }
        }

        private struct RegisterGroup
        {
            public string group_name;
            public List <RegTree> registers;
            //public RegisterGroup(string name, List<RegisterEntry> reg_list)
            //{
            //    group_name = name;
            //    //Array.FindAll<RegisterEntry>(reg_list, x => x.Group.Equals(g)).ToArray<RegisterEntry>()
            //    adrs_arr = 

            //}
        }

        public XMLWriter(RegisterEntry[] input_list)
        {
            reg_list = input_list;
            result = CreateXMLStream();
            result.Save("XMLRes.txt");
        }

        private XElement CreateXMLStream()
        {
            reg_list = reg_list.OrderBy(x => x.Type).ToArray();
            reg_list = reg_list.OrderBy(x => x.Address).ToArray();
            reg_list = reg_list.OrderBy(x => x.Group).ToArray();
            List <string> group_list = reg_list.Select(x => x.Group).ToList();
            return new XElement("Registers",
                                    from g in group_list
                                    select new XElement(g,
                   from s in
                       Array.FindAll<RegisterEntry>(reg_list, x => x.Group.Equals(g)).ToArray<RegisterEntry>()
                   select CreateRegXElem(s)));
        }


        private XElement CreateRegXElem(RegisterEntry element)
        {
            if (element.Comment.Equals(""))
                return new XElement("Register",
                            new XAttribute("Name", element.Name),
                            new XAttribute("Address", element.Address),
                            new XAttribute("MAIS", element.MAIS),
                            new XAttribute("LSB", element.LSB),
                            new XAttribute("MSB", element.MSB),
                            new XAttribute("Type", element.Type.ToString("G")),
                            new XAttribute("FPGA", element.FPGA.ToString("G")));
            else
                return new XElement("Register",
                            new XAttribute("Name", element.Name),
                            new XAttribute("Address", element.Address),
                            new XAttribute("MAIS", element.MAIS),
                            new XAttribute("LSB", element.LSB),
                            new XAttribute("MSB", element.MSB),
                            new XAttribute("Type", element.Type.ToString("G")),
                            new XAttribute("FPGA", element.FPGA.ToString("G")),
                            new XAttribute("Comment", element.Comment));
        }
    }
}
