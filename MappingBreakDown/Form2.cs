using System;
using System.Data;
using System.Windows.Forms;

namespace MappingBreakDown
{
    public partial class ChooseAddressPrompt : Form
    {
        public int Chosen_address { get; set; }
        public int Index { get; set; }

        DataRowCollection regs;

        public ChooseAddressPrompt(DataRowCollection registers)
        {
            InitializeComponent();

            Chosen_address = 0;

            regs = registers;

            foreach (DataRow reg in registers)
                    AddressOpts.Items.Add(reg.Field<string>("Name"));

            AddressOpts.SelectedIndex = 0;
        }
        

        private void OKButton_Click(object sender, EventArgs e)
        {
            string name = (string)(AddressOpts.SelectedItem);

            foreach (DataRow r in regs)
            {
                if (r.Field<string>("Name").Equals(name))
                {
                    Chosen_address = int.Parse(r.Field<string>("Address"));
                    Index = r.Field<int>("Index");
                    break;
                }
            }

            DialogResult = DialogResult.OK;
        }
        
    }
}
