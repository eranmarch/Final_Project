﻿using System;
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
        public int Chosen_address { get; set; }
        public int Index { get; set; }
        RegisterEntry[] registers;
        public ChooseAddressPrompt(RegisterEntry[] registers)
        {
            InitializeComponent();
            this.registers = registers;
            this.Chosen_address = 0;
            foreach (RegisterEntry reg in registers)
            {
                if (!reg.GetRegType().Equals(RegisterEntry.type_field.FIELD))
                    AddressOpts.Items.Add(reg.GetName());
            }
        }
        private void ChooseAddressPrompt_Load(object sender,EventArgs e)
        {
            
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            string name = (string)(AddressOpts.SelectedItem);
            int i = 0;
            for (; i < registers.Length; i++)
                if (registers[i].GetName().Equals(name))
                    break;
            //chosen_address = registers[AddressOpts.SelectedIndex].Address;
            //MessageBox.Show(registers[i].Address.ToString());
            try
            {
                //Chosen_address = registers[i].GetAddress();
                //Index = i;
                Index = AddressOpts.SelectedIndex;
                Chosen_address = registers[Index].GetAddress();
                DialogResult = DialogResult.OK;
            }
            catch (System.IndexOutOfRangeException)
            {
                MessageBox.Show("Please choose a register from the list");
            }
        }

        private void AddressOpts_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void ChooseAddressPrompt_Load_1(object sender, EventArgs e)
        {
            //DialogResult = DialogResult.Cancel;
        }
    }
}
