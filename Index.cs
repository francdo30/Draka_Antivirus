using Draka_Antivirus.DAO;
using Draka_Antivirus.Windows;
using Draka_Antivirus.Pages_Principales;
using System;
/*using System.Management;*/
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
    public partial class Index : Form
    {
        InfoSystem perf = new InfoSystem();
        List<String> perfo = new List<string>();
        ControleNav nav = new ControleNav();
        public Index()
        {
            InitializeComponent();            
            //nav.TesteChrome();
        }
        private Form activeForm = null;
        
        // méthode d'appel des fenêtres du menu principale
        private void openChildrenForm2(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.home.Controls.Add(childForm);
            this.home.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            //labelInfosbtn.Text = childForm.Text;

        }

        // les méthodes pour chaque boutton du menu principale

        private void guna2ImageButton10_Click(object sender, EventArgs e)
        {
            // parametre de l'application
            openChildrenForm2(new Windows.Parametres());
        }

        private void guna2ImageButton11_Click(object sender, EventArgs e)
        {
            // Nous contacter
            openChildrenForm2(new Windows.Contacts());
        }

        private void buyNowBtn_Click(object sender, EventArgs e)
        {
            // activation du produit
        }

        private void title3_Click(object sender, EventArgs e)
        {
            // période d"essai du produit
        }

        private void homeBtn_Click(object sender, EventArgs e)
        {
            // page d'acceuil
            openChildrenForm2(new Windows.Home());
        }

        private void scanBtn_Click(object sender, EventArgs e)
        {
            // pages des scans de l'application
            openChildrenForm2(new Windows.Scan());
        }
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            // button scan nown du home page
            openChildrenForm2(new Windows.Scan());
           /* UserScan.Invoke(new MethodInvoker(delegate
            {
                UserScan.Show();
            }));*/
        }

        private void performanceBtn_Click(object sender, EventArgs e)
        {
            // les performance de ma machine
            openChildrenForm2(new Windows.Performance());
        }

        private void stabilityBtn_Click(object sender, EventArgs e)
        {
            // les stabilités de la machine
            openChildrenForm2(new Windows.Stability());
        }

        private void maintainBtn_Click(object sender, EventArgs e)
        {
            // la maintenance de notre machine
            openChildrenForm2(new Windows.Maintenance());
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            // les mise à jours disponibles
            openChildrenForm2(new Windows.Update());
        }

        private void historyBtn_Click(object sender, EventArgs e)
        {
            // historique des recherches
            openChildrenForm2(new Windows.Historics());
        }

        private void QuarantBtn_Click(object sender, EventArgs e)
        {
            // page de la quarantaine
            openChildrenForm2(new Windows.Quarantaine());
        }

        private void securityBtn_Click(object sender, EventArgs e)
        {
            // la page sécurity
            openChildrenForm2(new Windows.Securite());
        }
        
        // méthode d'appel des fenêtres du menu principale






    }
}
