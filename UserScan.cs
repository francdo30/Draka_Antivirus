using Draka_Antivirus.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Draka_Antivirus
{
    public partial class UserScan : UserControl
    {
        public UserScan()
        {
            InitializeComponent();
        }

        private void panelBodyScan_Paint(object sender, PaintEventArgs e)
        {
        }

        UserScanPerso personnalScan1 = new UserScanPerso();
        UserScanComplete completeScan1 = new UserScanComplete();
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            personnalScan1.Hide();
            completeScan1.Show();
            completeScan1.BringToFront();

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            completeScan1.Hide();
            personnalScan1.Show();
            personnalScan1.BringToFront();

        }
    }
}
