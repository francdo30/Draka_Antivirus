using System;
using System.Windows.Forms;

namespace Draka_Antivirus
{
    internal static class Program
    {
        /*public static ScanPersonalise sp;
        public static bool isSp;
        public static ScanComplete sc;*/
        public static bool isSc;
        /*public static Scan s;
        public static bool scanRun;
        public static Home home;*/

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Index());
        }
    }
}
