using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Manifestacije
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        private Window _par;
        private JavaScriptControlHelper ch;
        public Window Par
        {
            get { return _par; }
            set { _par = value; }
        }

        public HelpWindow(Window parent)
        {
            InitializeComponent();
            Par = parent;
            string curDir = Directory.GetCurrentDirectory();
            string key = "";
            
            if (Par is MainWindow)
            {
                ch = new JavaScriptControlHelper((MainWindow)Par);
                key = "main";

            }
            else if(Par is ViewWindow)
            {
                key = "view";
                ch = new JavaScriptControlHelper((ViewWindow)Par);
            }
            else if(Par is EtiketaWindow)
            {
                key= "etiketa";
                ch = new JavaScriptControlHelper((EtiketaWindow)Par);
            }
            else if(Par is ManifestacijaWindow)
            {
                key = "manif";
                ch = new JavaScriptControlHelper((ManifestacijaWindow)Par);
            }
            else if(Par is TipManifestacijeWindow)
            {
                key = "tipmanif";
                ch = new JavaScriptControlHelper((TipManifestacijeWindow)Par);
            }
            else
            {
                key = "error";
            }

            string path = String.Format(@"{0}/Help/{1}.htm", curDir, key);
            if (!File.Exists(path))
            {
                key = "error";
            }
            Console.WriteLine(String.Format(@"file:///{0}/Help/{1}.htm", curDir, key));
            Uri uri = new Uri(String.Format(@"file:{0}/Help/{1}.htm", curDir, key));

            wbHelp.Source = uri;
            wbHelp.ObjectForScripting = ch;
            wbHelp.Navigate(uri);
            
        }
        private void wbHelp_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }
    }
}
