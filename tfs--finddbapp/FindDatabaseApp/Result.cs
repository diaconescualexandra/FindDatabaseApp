using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindDatabaseApp
{
    public partial class Result : Form
    {
        public Result()
        {
            InitializeComponent();
        }

        public void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void DisplayResults(string valueLookedAfter)
        {

            if (DatabaseManagementMethods.ds != null && DatabaseManagementMethods.ds.Tables.Count > 0)
            {
                dataGridView1.ReadOnly = true;
                dataGridView1.DataSource = DatabaseManagementMethods.ds.Tables[0];
            }
        }
    }
}
