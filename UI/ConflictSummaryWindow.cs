using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vatsys;

namespace AuroraLabelItemsPlugin
{
    public partial class Form1 : BaseForm
    {
        AtopPlugin.Conflict.ConflictProbe.ConflictData cpar = new AtopPlugin.Conflict.ConflictProbe.ConflictData();

        public Form1() 
        {

            InitializeComponent();
        }

        public void addRow(string cs1, string cs2)
        {
            
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
