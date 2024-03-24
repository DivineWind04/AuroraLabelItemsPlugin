using System;
using System.Windows.Forms;
using AtopPlugin.Conflict;
using vatsys;

namespace AtopPlugin.UI
{
    public partial class ConflictSummaryTable : BaseForm
    {
        Conflict.ConflictProbe.ConflictData cpar = new global::AtopPlugin.Conflict.ConflictProbe.ConflictData();

        public ConflictSummaryTable()
        {
            InitializeComponent();
        }

        public void addRow(string cs1, string cs2)
        {
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ConflictSummaryTable_Load(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}