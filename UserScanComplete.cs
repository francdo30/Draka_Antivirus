using Draka_Antivirus.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Draka_Antivirus.Pages_Principales;
using System.IO.Abstractions;
using Microsoft.VisualBasic.FileIO;
using FileSystem = Microsoft.VisualBasic.FileIO.FileSystem;
using System.Threading;

namespace Draka_Antivirus
{
    public partial class UserScanComplete : UserControl
    {
        private System.Threading.ManualResetEvent _busy = new System.Threading.ManualResetEvent(false);
        public static string targetPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string name_db = "ScanDataBase.db";
        public static string sourceFile = "";//targetPath + name_db;
        Database db1 = new Database();
        Color[] colors = { Color.Aqua, Color.Green, Color.Blue, Color.Black, Color.DeepSkyBlue, Color.Red };

        //string path = @"C:\Users\maboa\OneDrive\Documents\Visual Studio 2019\Projects\drakashield-av\Draka Antivirus\bin\Debug\Error_Log.txt";


        int j;
        int count;
        int i;
        string path = targetPath + "Error_Log.txt";
        //string path = @"D:\job\AGMA Organization technology inc\Draka new verison\Draka Antivirus\Draka Antivirus\Draka Antivirus\bin\Debug\Error_Log.txt";
        /*string path = @"C:\Program Files (x86)\Default Company Name\Setup1\Error_Log.txt";*/

        // for test
        /*string path = @"Draka Antivirus\Draka Antivirus\Draka Antivirus\bin\Debug\Error_Log.txt";*/
        int virus;
        int files;
        String obt = "Pause";

        public UserScanComplete()
        {
            InitializeComponent();
            pictureBox1.Visible = false;
            backgroundScanComplet.WorkerReportsProgress = true;

            if (!File.Exists(sourceFile))
            {
                sourceFile = db1.createDatabase(name_db);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!backgroundScanComplet.IsBusy)
            {
                label9.Text = "Initiailisations...";
                progressBar1.Value = 0;
                pictureBox1.Visible = true;
                i = 0;
                j = 0;
                count = 0;
                label10.Text = "%";
                label8.Text = @"C:\Users\";

                //progressBar1.Update();
                progressBar1.Visible = true;
                listView1.Visible = true;
                //pictureBox11.Visible = true;

                //visibilitée des boutons
                button2.Visible = false;
                btnCclScan.Visible = true;
                button1.Visible = true;
                // appel de methode de Scan
                backgroundScanComplet.RunWorkerAsync();

                /*Program.scanRun = true;
                Program.isSp = true; */

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (backgroundScanComplet.IsBusy)
            {
                if (obt == "Continuer")
                {
                    obt = "Pause";
                    button1.Text = obt;
                    button1.Visible = true;
                    btnCclScan.Visible = true;
                    button2.Visible = true;
                }
                else
                {
                    obt = "Continuer";
                    button1.Text = obt;
                    button1.Visible = true;
                    btnCclScan.Visible = false;
                    button2.Visible = false;
                }
            }

        }

        private void btnCclScan_Click(object sender, EventArgs e)
        {
            if (backgroundScanComplet.WorkerSupportsCancellation == true)
            {
                //pictureBox11.Visible = false;
                if (MessageBox.Show(" Do you want to delete the current Scan...", " Scan stop ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    folderBrowserDialog1.SelectedPath.Trim();
                    /*listView1.Clear();*/
                    listView1.Visible = true;
                    btnCclScan.BackColor = colors[5];
                    //visibilitée des boutons
                    button2.Visible = false;
                    btnCclScan.Visible = true;
                    button1.Visible = false;
                    backgroundScanComplet.CancelAsync();
                }
            }

        }


        public UserScanComplete(Boolean flag)
        {
            if (flag == true) { this.InitializeComponent(); }
        }

        private string BytesToHex(byte[] bytes)
        {
            // write each byte as two char hex output.
            return String.Concat(Array.ConvertAll(bytes, x => x.ToString("X2")));
        }

        public List<String> fichiers(string dir)
        {
            List<String> f = new List<String>();
            List<string> dirs = Directory.GetDirectories(dir.ToString()).ToList();

            if (dirs.Count > 0)
            {
                foreach (string item in dirs)
                {
                    try
                    {
                        f.AddRange(fichiers(item));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                }
            }

            try
            {
                f.AddRange(Directory.GetFiles(dir.ToString(), "*.*", System.IO.SearchOption.TopDirectoryOnly).ToList());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Console.WriteLine(ex.ToString());
            }

            return f;
        }


        private void backgroundScanComplet_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<String> search = new List<String>();
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                //search.AddRange(fichiers(@"C:\Users\francky le boss\Downloads\le C#\code\Copie de Draka Antivirus\drakashield-av-hayyan_draka\drakashield-av-hayyan_draka\Draka Antivirus"));

                foreach (DriveInfo d in allDrives)
                {
                    if (d.IsReady == true)
                    {
                        /*search.AddRange(fichiers(d.RootDirectory.ToString() + "\\"));*/
                        search.AddRange(fichiers(@"C:\Users\"));
                        pictureBox1.Invoke(new MethodInvoker(delegate
                        {
                            pictureBox1.Visible = false;
                        }));
                    }
                }
                progressBar1.Maximum = search.Count;
                Scancp complet = new Scancp();
                /*complet.isInit = true;*/
                complet.filesCount = search.Count;
                int max = search.Count;
                int Tempmax = max;
                /*backgroundScanPersonaliser.ReportProgress(0, complet);*/
                /*complet.isInit = false;*/

                foreach (String item in search)
                {
                    try
                    {
                        files += 1;
                        string chemin = item;
                        if (backgroundScanComplet.CancellationPending == true)
                        {
                            //pictureBox11.Visible = false;
                            listView1.Visible = true;
                            e.Cancel = true;
                            break;
                        }
                        else if (button1.Text == "Continuer")
                        {
                            do
                            {
                                Thread.Sleep(500);
                            } while (button1.Text == "Continuer");
                        }
                        else
                        {
                            // read all bytes of file so we can send them to the MD5 hash algo
                            Byte[] allBytes = File.ReadAllBytes(item);
                            System.Security.Cryptography.HashAlgorithm md5Algo = null;
                            md5Algo = new System.Security.Cryptography.MD5CryptoServiceProvider();
                            // compute the Hash (MD5) on the bytes we got from the file
                            byte[] hash = md5Algo.ComputeHash(allBytes);
                            /*Console.WriteLine(BytesToHex(hash));*/

                            complet.file = files;
                            var md5signatures = File.ReadAllLines("MD5Base.txt");
                            if (md5signatures.Contains(BytesToHex(hash)))
                            {
                                //Console.WriteLine("Infected");
                                complet.statut = "Infected";
                                virus += 1;
                                complet.virus = virus;
                                string detection = BytesToHex(hash);
                                MoveItem(chemin, complet.statut, detection);
                            }
                            else
                            {
                                complet.statut = "Clean";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    progressBar1.Increment(1);
                    complet.item = item;
                    //backgroundScanComplet.ReportProgress(0, complet);                    
                    DateLine(max, complet, Tempmax);
                    Tempmax = Tempmax - 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                /* MessageBox.Show("Error : " + ex.ToString());*/
            }
        }



        private void backgroundScanComplet_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            /*int scanend, intervale, time, days, hours, minutes, seconds;
            Scancp scan = (Scancp)e.UserState;

            pictureBox11.Visible = false;
            progressBar1.Visible = true;
            listView1.Visible = true;
            progressBar1.Maximum = scan.filesCount;
            progressBar1.Increment(1);
            progressBar1.Update();
            label2.Text = scan.virus.ToString() + " virus détectées";
            label4.Text = "Fichiers scanner : " + scan.file.ToString() + " ";
            Console.WriteLine(scan.statut);
            label9.Text = scan.item;
            listView1.Items.Add(new ListViewItem(new string[] { scan.item }));
            label8.Text = @"C:\Users\";

            scanend = scan.file;
            intervale = 1;
            time = (scanend * intervale) / 4;
            days = time / 86400;
            hours = time / 3600;
            minutes = time / 60 % 60;
            seconds = time % 60;
            label7time.Text = hours + "h:" + minutes + "mm:" + seconds + "s";
            */
        }

        private void backgroundScanComplet_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Scan Cancel or paused");
                FullScan("Interrompu");
                if (button1.Text == "Continuer")
                { button1.Visible = true; btnCclScan.Visible = false; button2.Visible = false; }
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Scan Cancel or paused");
                FullScan("Interrompu");
            }
            else
            {
                // MessageBox.Show("Scan Complet");                
                MessageBox.Show("Scan Complete");
                FullScan("Terminee");
                Thread.Sleep(50);
                label9.ForeColor = colors[1];
                progressBar1.ProgressColor2 = colors[1];
                label8.Text = "Processing Completed...";
                folderBrowserDialog1.SelectedPath.Trim();

                button2.Visible = true; btnCclScan.Visible = true; button1.Visible = true;
            }
            /*button2.Enabled = true;*/
            /* btnCclScan.Visible = false;*/
            /*label7.Text = "Repertoire d\'analyse :";*/
            /*  Program.scanRun = false;
              Program.isSp = false;  */
        }
        private void ScanPersonalise_Load(object sender, EventArgs e)
        {
            /*pictureBox1.Visible = false;*/
        }
        private void DateLine(int max, Scancp scan, int Tempmax)
        {
            int scanend, intervale, time, days, hours, minutes, seconds, j;

            //scanend = scan.file; increlenter les secondes puis les minutes, puis les heures
            scanend = Tempmax;  //incrementer les secondes puis les minutes, puis les heures

            j = max / 100;

            intervale = 1;
            time = (scanend * intervale) / 4;
            days = time / 86400;
            hours = time / 3600;
            minutes = time / 60 % 60;
            seconds = time % 60;
            // **************************************************
            if (listView1.InvokeRequired && label9.InvokeRequired && label4.InvokeRequired && label7time.InvokeRequired && label10.InvokeRequired)
            {
                listView1.Invoke(new MethodInvoker(delegate
                {
                    label9.ForeColor = colors[0];
                    label9.Text = "Treatment In progress ...";
                    label4.Text = "Files : " + scan.file.ToString();
                    label2.Text = "Threat : " + scan.virus.ToString();
                    listView1.Items.Add(scan.item);
                    label7time.Text = hours + "h :" + minutes + "mm :" + seconds + "s";

                    count = count + 1;
                    // Console.WriteLine("count : " + count + " fichier : " + scan.file.ToString()+" fichier max : "+max+" j = "+j);
                    if (max < 100)
                    {
                        if (count < max - 10)
                        {
                            label10.Text = i.ToString() + "%";
                            i += 1;
                        }
                        else if (count == max - 5)
                        {
                            i = 75;
                            label10.Text = i.ToString() + "%";
                        }
                        else if (count == max - 2)
                        {
                            i = 98;
                            label10.Text = i.ToString() + "%";
                        }
                        else if (count == max)
                        {
                            i = 100;
                            label10.Text = i.ToString() + "%";
                        }
                    }
                    else
                    {
                        label10.Text = i.ToString() + "%";
                        if (count % j == 0)
                        {
                            i = i + 1;
                            if (count < j * 100)
                            {
                                label10.Text = i.ToString() + "%";
                            }
                            else
                            {
                                if (count == max - 1)
                                {
                                    i = 100;
                                    label10.Text = i.ToString() + "%";
                                }
                                else
                                {
                                    i = 99;
                                    label10.Text = i.ToString() + "%";
                                }

                            }
                        }
                    }
                    //MessageBox.Show(scan.filesCount.ToString() );                                    
                    // MessageBox.Show("aaaaaaa");
                }));
            }
            else
            {
                //listViewItem = data.value;
                listView1.Items.Add(scan.item);
            }

            // **************************************************

        }
        private void FullScan(string Etat)
        {
            string etat = Etat;
            DateTime date = DateTime.Now;
            string date1 = date.ToString("yyyy:MM:dd  hh:mm:ss");
            string duree = label7time.Text;
            string totalAnalyser = "";
            if (label4.Text.Contains("Fichiers scanner : ")) totalAnalyser = label4.Text.Replace("Fichiers scanner : ", "");
            string virus = "";
            if (label2.Text.Contains(" virus détectées")) virus = label2.Text.Replace(" virus détectées", "");
            string scantype = "Scan Complet";
            string status = "Fait";
            string sql = "insert into FullScan  (etat,date,duree,totalanalyser,virusdetect,typescan,status) values(";
            sql = sql + "'" + etat + "', ";
            sql = sql + "'" + date1 + "', ";
            sql = sql + "'" + duree + "', ";
            sql = sql + "'" + totalAnalyser + "', ";
            sql = sql + "'" + virus + "', ";
            sql = sql + "'" + scantype + "', ";
            sql = sql + "'" + status + "')";

            try
            {
                Boolean error = db1.insertData(sourceFile, sql);

                if (error == true)
                {
                    Console.WriteLine("Good Scan");
                }
                else
                {
                    Console.WriteLine("Bad no complete scan");
                }
            }
            catch (Exception ex)
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql + " " + "Error_Message:" + ex);
                    tw.Close();
                }

                else if (File.Exists(path))
                {
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql + " " + "Error_Message:" + ex);
                    tw.Close();
                }
            }
        }

        private void ScanPersonalise_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Hide();
        }

        private void ScanPersonalise_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Hide();
        }

        private void label7time_Click(object sender, EventArgs e)
        {

        }
        private void MoveItem(string directory, string etat, string hash)
        {
            //string subdir = @"D:\job\AGMA Organization technology inc\Draka new verison\Draka Antivirus\Draka Antivirus\Draka Antivirus\bin\Debug\Quarantaine\";
            //string root = @"D:\job\AGMA Organization technology inc\Draka new verison\Draka Antivirus\Draka Antivirus\Draka Antivirus\bin\Debug\Quarantaine\";
            //string subdir = @"C:\Program Files(x86)\Default Company Name\Setup1\Quarantaine\";
            string root = @"C:\Program Files(x86)\Default Company Name\Setup1\Quarantaine\";

            // test directory
            string subdir = @"Draka Antivirus\Draka Antivirus\Draka Antivirus\bin\Debug\Quarantaine\";
            //string root = @"Draka Antivirus\Draka Antivirus\Draka Antivirus\bin\Debug\Quarantaine\";*/

            try
            {
                string file = Path.GetFileName(directory);
                root = root + file;


                if (FileSystem.FileExists(directory))
                {
                    if (Directory.Exists(subdir))
                    {
                        /* MessageBox.Show("non il n'exixte pas");
                         Directory.CreateDirectory(subdir); */

                        string chemin = directory;
                        string fileName = file;
                        string NewDirectory = root;
                        DateTime date = DateTime.Now;
                        string date1 = date.ToString(" YYYY:MM:DD hh:mm:ss ");
                        string taille = " ";
                        string Etat = etat;
                        string editeur = " ";
                        string action = " Mettre en quarantaine ";
                        string detection = file + " -> " + hash;

                        string sql = "insert into Quarantaine (chemin,nomfichier,nouveldirection,date,taille,etat,editeur,action,detection) values(";
                        sql = sql + "'" + chemin + "', ";
                        sql = sql + "'" + fileName + "', ";
                        sql = sql + "'" + NewDirectory + "', ";
                        sql = sql + "'" + date1 + "', ";
                        sql = sql + "'" + taille + "', ";
                        sql = sql + "'" + Etat + "', ";
                        sql = sql + "'" + editeur + "', ";
                        sql = sql + "'" + action + "', ";
                        sql = sql + "'" + detection + "')";


                        Boolean error = db1.insertData(sourceFile, sql);

                        //MessageBox.Show("On attend encore error");
                        if (error == true)
                        {
                            // AutoClosingMessageBox.Show("Virus detectees et enregistrer en quarantaine", " Draka Quarantaine ", 3000);
                        }
                        else
                        {
                            Console.WriteLine("Virus detectees", " Draka Quarantaine ", 3000);
                            MessageBox.Show("Quarantaine échoué");
                        }

                        FileSystem.MoveFile(directory, root, true);
                        MessageBox.Show("Ménace éliminée ");

                    }
                    else
                    {
                        Directory.CreateDirectory(subdir);
                        // AutoClosingMessageBox.Show("OUI le repertoire quarantaine Existe maintenant");
                        db1.CreateTable(sourceFile, "Quarantaine");
                        FileSystem.MoveFile(directory, root, true);
                        MessageBox.Show("Quarantaine échoué et fichier supprimé");
                    }

                }
                else
                {
                    Console.WriteLine("Monexeption = " + file);
                    Console.WriteLine("Le fichier n'existe pas dans le repertoire : " + directory);
                    //AutoClosingMessageBox.Show("The file does not exist in the directory: " + directory, " Draka Quarantaine ", 3000);
                }
            }
            catch (Exception ex)
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Error_Message:" + ex);
                    tw.Close();
                }
                else if (File.Exists(path))
                {
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Error_Message:" + ex);
                    tw.Close();
                }
            }
        }

        public void CompletScan(
                Guna2Button complet_runScanBtn, Guna2Button complet_cancelScanBtn,
                Guna2Button complet_pauseScanBtn, Label complet_classificationText,
                Guna2ProgressBar complet_progressbar, Label complet_repertoireText,
                Guna2DataGridView complet_listView, Guna2ProgressIndicator complet_ProgressIndicator
            )
        {
            if (!backgroundScanComplet.IsBusy)
            {
                complet_runScanBtn.Enabled = true;
                complet_cancelScanBtn.Visible = true;
                complet_pauseScanBtn.Visible = true;
                complet_classificationText.Text = "Initiailisations...";
                complet_progressbar.Value = 0;
                complet_progressbar.Update();
                complet_repertoireText.Text = @"C:\Users\";
                backgroundScanComplet.RunWorkerAsync();

                if (complet_classificationText.Text == "Initiailisations...")
                {
                    complet_progressbar.Visible = false;
                    complet_listView.Visible = false;
                    complet_ProgressIndicator.Visible = true;
                }
                /*Program.scanRun = true;
                Program.isSp = true; */
            }
            if (!backgroundScanComplet.IsBusy)
            {
                button2.Enabled = true;
                btnCclScan.Visible = true;
                button1.Visible = true;
                label9.Text = "Initiailisations...";
                progressBar1.Value = 0;
                //progressBar1.Update();
                label8.Text = @"C:\Users\";
                backgroundScanComplet.RunWorkerAsync();

                if (label9.Text == "Initiailisations...")
                {
                    progressBar1.Visible = false;
                    listView1.Visible = false;
                    //pictureBox11.Visible = true;
                }
                /*Program.scanRun = true;
                Program.isSp = true; */
            }
        }


    }
}
