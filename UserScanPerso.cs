using Draka_Antivirus.DAO;
using Draka_Antivirus.Pages_Principales;
using Microsoft.VisualBasic.FileIO;
using FileSystem = Microsoft.VisualBasic.FileIO.FileSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Draka_Antivirus
{
    public partial class UserScanPerso : UserControl
    {
        private System.Threading.ManualResetEvent _busy = new System.Threading.ManualResetEvent(false);
        public static string targetPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string name_db = "ScanDataBase.db";
        public static string sourceFile = ""; //targetPath + name_db;
        Database db1 = new Database();
        Color[] colors = { Color.Aqua, Color.Green, Color.Blue, Color.Black, Color.DeepSkyBlue, Color.Red };

        //string path = @"C:\Users\maboa\OneDrive\Documents\Visual Studio 2019\Projects\drakashield-av\Draka Antivirus\bin\Debug\Error_Log.txt";
        string path = targetPath + "Error_Log.txt";

        String obt = "Pause";
        int j;
        int count;
        int i;
        int virus;
        int files;
        public UserScanPerso()
        {
            InitializeComponent();
            pictureBox1.Visible = false;
            backgroundWorker1.WorkerReportsProgress = true;

            if (!File.Exists(sourceFile))
            {
                sourceFile = db1.createDatabase(name_db);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            labelDirectory.Text = folderBrowserDialog1.SelectedPath;
            i = 0;
            j = 0;
            count = 0;
            progressBar1.Value = 0;

            label9.Text = " % ";

            label2.Text = "0";
            label4.Text = "0";
            label6.Text = "00h:00mm:00s";
            label8.ForeColor = colors[3];
            listView1.Items.Clear();


            virus = 0;
            files = 0;
            //label4.Text = virus.ToString();            
            Thread.Sleep(100);
            label8.Text = "initiailisations... ";
            button3.Visible = false; btnCclScan.Visible = false; button2.Visible = true;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                pictureBox1.Visible = true;
                btnCclScan.BackColor = colors[2];
                backgroundWorker1.RunWorkerAsync();
                button3.Visible = true; btnCclScan.Visible = true; button2.Visible = false;
                AutoClosingMessageBox.Show("Loading files in progress");

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                if (obt == "Continuer")
                {
                    obt = "Pause";
                    button3.Text = obt;
                    button3.Visible = true;
                    btnCclScan.Visible = true;
                    button2.Visible = true;
                }
                else
                {
                    obt = "Continuer";
                    button3.Text = obt;
                    button3.Visible = true;
                    btnCclScan.Visible = false;
                    button2.Visible = false;
                }
            }

        }

        private void btnCclScan_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                button3.Visible = false; btnCclScan.Visible = true; button2.Visible = false;
                //btnCclScan.BackColor = colors[4];
                this.backgroundWorker1.CancelAsync();
            }

        }

        // nos fonction utiles
        private string BytesToHex(byte[] bytes)
        {
            // write each byte as two char hex output.
            return String.Concat(Array.ConvertAll(bytes, x => x.ToString("X2")));
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Do not access the form's BackgroundWorker reference directly.
            // Instead, use the reference provided by the sender parameter.
            BackgroundWorker bw = sender as BackgroundWorker;

            // Extract the argument.
            //int arg = (int)e.Argument;

            // Start the time-consuming operation.
            //e.Result = 

            // If the operation was canceled by the user,
            // set the DoWorkEventArgs.Cancel property to true.        

            TimeConsumingOperation(bw, e);

        }

        private void TimeConsumingOperation(BackgroundWorker bw, DoWorkEventArgs e)
        {

            List<String> search = Directory.GetFiles(@folderBrowserDialog1.SelectedPath, "*.*", System.IO.SearchOption.AllDirectories).ToList();  //, System.IO.SearchOption.AllDirectories
            MessageBox.Show("Click on ok to start the Scan");

            progressBar1.Maximum = search.Count;
            Scancp perso = new Scancp();
            perso.filesCount = search.Count;

            int max = search.Count;
            int Tempmax = max;

            pictureBox1.Invoke(new MethodInvoker(delegate
            {
                pictureBox1.Visible = false;
            }));

            foreach (String item in search)
            {
                try
                {
                    this.files += 1;
                    string chemin = item;

                    if (bw.CancellationPending == true)
                    {
                        listView1.Visible = true;
                        e.Cancel = true;
                        break;
                    }
                    else if (button3.Text == "Continuer")
                    {
                        do
                        {
                            Thread.Sleep(500);
                        } while (button3.Text == "Continuer");
                    }
                    else
                    {
                        // read all bytes of file so we can send them to the MD5 hash algo

                        Byte[] allBytes = File.ReadAllBytes(item);
                        System.Security.Cryptography.HashAlgorithm md5Algo = null;
                        md5Algo = new System.Security.Cryptography.MD5CryptoServiceProvider();

                        // compute the Hash (MD5) on the bytes we got from the file
                        // compute the Hash (MD5) on the bytes we got from the file

                        byte[] hash = md5Algo.ComputeHash(allBytes);
                        Console.WriteLine(BytesToHex(hash));

                        perso.file = files;
                        var md5signatures = File.ReadAllLines("MD5Base.txt");
                        if (md5signatures.Contains(BytesToHex(hash)))
                        {

                            perso.statut = "Infected";
                            virus += 1;
                            perso.virus = virus;
                            string detection = BytesToHex(hash);
                            //MessageBox.Show("virus : " + detection);
                            MoveItem(chemin, perso.statut, detection);
                        }
                        else
                        {
                            perso.statut = "Clean";
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    // AutoClosingMessageBox.Show("Error : " + ex.ToString());
                    AutoClosingMessageBox.Show("Execption généré ");
                }
                progressBar1.Increment(1);
                perso.item = item;
                DateLine(max, perso, Tempmax);
                Tempmax = Tempmax - 1;
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
            MessageBox.Show("appel :");

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Scan Cancel or paused");
                PartialScan("Interrompu");
                if (button3.Text == "Continuer")
                { button3.Visible = true; btnCclScan.Visible = true; button2.Visible = false; }
            }
            else if (e.Error != null)
            {
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine(DateTime.Now.ToString() + " " + "Error_Message:" + e.Error.ToString());
                tw.Close();
                progressBar1.Value = 0;
                label9.Text = "";
                label6.Text = "00:00:00";
                MessageBox.Show("Scan Cancel or paused");
                labelDirectory.Text = "Repertoire d\'analyse";
                PartialScan("Interrompu");
            }
            else
            {
                MessageBox.Show("Scan Complete");
                Thread.Sleep(50);
                label8.ForeColor = colors[1];
                progressBar1.ProgressColor2 = colors[1];
                label8.Text = "Processing Completed...";
                folderBrowserDialog1.SelectedPath.Trim();
                PartialScan("Terminee");
                labelDirectory.Text = "Repertoire d\'analyse";

                button3.Visible = true; btnCclScan.Visible = true; button2.Visible = true;
            }
            /*button2.Enabled = true;
            btnCclScan.Visible = false; */
            labelDirectory.Text = "Repertoire d\'analyse";
            //Program.scanRun = false;
            //Program.isSp = false;
        }

        private void DateLine(int max, Scancp scan, int Tempmax)
        {
            int scanend, intervale, time, days, hours, minutes, seconds;

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
            if (listView1.InvokeRequired && label8.InvokeRequired && label4.InvokeRequired && label6.InvokeRequired && label9.InvokeRequired)
            {
                listView1.Invoke(new MethodInvoker(delegate
                {
                    label8.ForeColor = colors[0];
                    label8.Text = "Processing Completed...";
                    label4.Text = "Files : " + scan.file.ToString();
                    label2.Text = "Threat : " + scan.virus.ToString();
                    listView1.Items.Add(scan.item);
                    label6.Text = hours + "h :" + minutes + "mm :" + seconds + "s";
                    count = count + 1;
                    Console.WriteLine(count);
                    if (max < 100)
                    {
                        if (count < max - 10)
                        {
                            label9.Text = i.ToString() + "%";
                            i += 1;
                        }
                        else if (count == max - 5)
                        {
                            i = 75;
                            label9.Text = i.ToString() + "%";
                        }
                        else if (count == max - 2)
                        {
                            i = 98;
                            label9.Text = i.ToString() + "%";
                        }
                        else if (count == max)
                        {
                            i = 100;
                            label9.Text = i.ToString() + "%";
                        }
                    }
                    else
                    {
                        label9.Text = i.ToString() + "%";
                        if (count % j == 0)
                        {
                            i = i + 1;
                            if (count < j * 100)
                            {
                                label9.Text = i.ToString() + "%";
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
        private void PartialScan(string Etat)
        {
            string etat = Etat;
            DateTime date = DateTime.Now;
            string date1 = date.ToString("yyyy:MM:dd  hh:mm:ss");
            string duree = label6.Text;
            string totalAnalyser = "";
            if (label4.Text.Contains("Fichiers scanner : ")) totalAnalyser = label4.Text.Replace("Fichiers scanner : ", "");
            string virus = "";
            if (label2.Text.Contains(" virus détectées")) virus = label2.Text.Replace(" virus détectées", "");
            string scantype = "Scan Partiel";
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
                            AutoClosingMessageBox.Show("Virus detectees et enregistrer en quarantaine", " Draka Quarantaine ", 3000);
                        }
                        else
                        {
                            Console.WriteLine("Virus detectees", " Draka Quarantaine ", 3000);
                            AutoClosingMessageBox.Show("Quarantaine échoué");
                        }

                        FileSystem.MoveFile(directory, root, true);
                        AutoClosingMessageBox.Show("Ménace éliminée ");

                    }
                    else
                    {
                        Directory.CreateDirectory(subdir);
                        AutoClosingMessageBox.Show("OUI le repertoire quarantaine Existe maintenant");
                        db1.CreateTable(sourceFile, "Quarantaine");
                        FileSystem.MoveFile(directory, root, true);
                        AutoClosingMessageBox.Show("Quarantaine échoué et fichier supprimé");
                    }

                }
                else
                {
                    Console.WriteLine("Monexeption = " + file);
                    Console.WriteLine("Le fichier n'existe pas dans le repertoire : " + directory);
                    AutoClosingMessageBox.Show("The file does not exist in the directory: " + directory, " Draka Quarantaine ", 3000);
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

        
    }
}
