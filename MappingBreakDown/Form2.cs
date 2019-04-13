using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MappingBreakDown
{
    public partial class ChooseAddressPrompt : Form
    {
        //string[] reg_name_list;
        public int chosen_address { get; set; }
        RegisterEntry[] registers;
        public ChooseAddressPrompt(RegisterEntry[] registers)
        {
            InitializeComponent();
            this.registers = registers;
            //reg_name_list = registers.Select(x => x.GetName()).ToArray();
            this.chosen_address = 0;
            foreach (RegisterEntry reg in registers)
            {
                if (!reg.Type.Equals(RegisterEntry.type_field.FIELD))
                    AddressOpts.Items.Add(reg.Name);
            }
        }
        private void ChooseAddressPrompt_Load(object sender,EventArgs e)
        {
            
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            chosen_address = registers[AddressOpts.SelectedIndex].Address;
            DialogResult = DialogResult.OK;
        }

        private void AddressOpts_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void ChooseAddressPrompt_Load_1(object sender, EventArgs e)
        {

        }
    }
}
