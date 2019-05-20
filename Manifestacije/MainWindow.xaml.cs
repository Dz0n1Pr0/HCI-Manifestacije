using Manifestacije.Modeli;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Manifestacije
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public static MainWindow instance = null;
        public static MainWindow Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainWindow();
                }
                return instance;
            }
        }

        public string FileName { get; set; }
        public Random rnd;

        private ObservableCollection<Manifestacija> Manifestacije { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;


            //punjenje liste
            setManifestacijeItems();
            rnd = new Random();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            // Begin dragging the window
            this.DragMove();
        }

        private void SveManifestacije_Click(object sender, RoutedEventArgs e)
        {
            
            ViewWindow view = new ViewWindow(this);
            view.ShowDialog();
        }

        private void DodajManifestaciju_Click(object sender, RoutedEventArgs e)
        {
            ManifestacijaWindow manif = new ManifestacijaWindow(this, false, null);
            manif.ShowDialog();
        }

        private void DodajEtiketu_Click(object sender, RoutedEventArgs e)
        {
            EtiketaWindow etiketa = new EtiketaWindow(this, false, null);
            etiketa.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DodajTipManifestacije_Click(object sender, RoutedEventArgs e)
        {
            TipManifestacijeWindow manif = new TipManifestacijeWindow(this, false, null);
            manif.ShowDialog();
        }

        //Metoda koja puni listu stringovima sa imenima vrsta
        public void setManifestacijeItems()
        {
            this.lista.ItemsSource = null;
            Manifestacije = new ObservableCollection<Manifestacija>(ListaManifestacija.Manifestacije.Values);
            this.lista.ItemsSource = Manifestacije;
        }


        //Izmena vrsta
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Manifestacija selekt = (Manifestacija)lista.SelectedItem;

            if (selekt == null)
            {
                MessageBox.Show("Označite iz liste manifestaciju za izmenu izmenite!");
                return;
            }

            ManifestacijaWindow dv = new ManifestacijaWindow(this, true, selekt);     //true jer se edituje
            dv.ShowDialog();
        }


        //Brisanje vrsta
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Manifestacija selekt = (Manifestacija)lista.SelectedItem;

            if (selekt == null)
            {
                MessageBox.Show("Označite iz liste manifestaciju za brisanje!");
                return;
            }

            ListaManifestacija.Manifestacije.Remove(selekt.ID);
            foreach (Etiketa etiketa in selekt.Etikete)
            {
                ListaEtiketa.Etikete.Remove(etiketa.ID);
            }
            this.setManifestacijeItems();
        }

        private void Lista_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Manifestacija selekt = (Manifestacija)lista.SelectedItem;
            if (ListaManifestacija.Manifestacije.ContainsKey(selekt.ID))
            {
                ManifestacijaWindow rsv = new ManifestacijaWindow(this, true, selekt);
                rsv.ShowDialog();
            }

        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "unknown.txt";
            savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";



            if (savefile.ShowDialog() == true)
            {
                FileName = savefile.FileName;
                FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate);
                using (fs)
                {
                    using (TextWriter tw = new StreamWriter(fs))
                    {
                        export_Manifestacije(tw, FileName);
                        export_TipoviManifestacija(tw, FileName);
                        export_Etikete(tw, FileName);
                    }
                }
            }

        }

        private void export_Etikete(TextWriter tw, string file)
        {
            foreach (KeyValuePair<String, Etiketa> kvp in ListaEtiketa.Etikete)
            {
                
                tw.WriteLine(string.Format("{0};{1}", "ETIKETA" + kvp.Key, kvp.Value));
            }
        }

        private void export_TipoviManifestacija(TextWriter tw, string file)
        {
            foreach (KeyValuePair<string, TipManifestacije> kvp in ListaTipManifestacijecs.TipoviManifestacija)
            {
                tw.WriteLine(string.Format("{0};{1}", "TIPMANIF" + kvp.Key, kvp.Value));          
            }
        }

        private void export_Manifestacije(TextWriter tw, string file)
        {
            foreach (KeyValuePair<string, Manifestacija> kvp in ListaManifestacija.Manifestacije)
            {
                tw.WriteLine(string.Format("{0};{1}", "MANIF" + kvp.Key, kvp.Value)); 
                foreach (Etiketa etiketa in kvp.Value.Etikete)
                {
                    tw.WriteLine(string.Format("{0};{1}", kvp.Key + ",ETIKETA" + etiketa.ID, etiketa));
                    ListaEtiketa.Etikete.Remove(etiketa.ID);
                }
            }
        }


        private void BrisanjeSvihManifestacija_Click(object sender, RoutedEventArgs e)
        {
            String message = "Da li ste sigurni da želite da obrišete sve manifestacije?\n\n";
            MessageBoxResult mbr = MessageBox.Show(message, "Brisanje svih manifestacija", MessageBoxButton.YesNo);

            if (mbr == MessageBoxResult.Yes)
            {
                ListaManifestacija.Manifestacije = null;
                ListaManifestacija.Manifestacije = new Dictionary<string, Manifestacija>();
                setManifestacijeItems();
            }
        }

        private void BrisanjeSvihTipovaManifestacija_Click(object sender, RoutedEventArgs e)
        {
            String message = "Da li ste sigurni da želite da obrišete sve unete tipove manifestacija?\n\n";
            MessageBoxResult mbr = MessageBox.Show(message, "Brisanje svih tipova manifestacija", MessageBoxButton.YesNo);

            if (mbr == MessageBoxResult.Yes)
            {
                
                ListaTipManifestacijecs.TipoviManifestacija = null;
                ListaTipManifestacijecs.TipoviManifestacija = new Dictionary<string, TipManifestacije>();
                setManifestacijeItems();
            }
        }

        private void BrisanjeSvihEtiketa_Click(object sender, RoutedEventArgs e)
        {
            String message = "Da li ste sigurni da želite da obrišete sve unete etikete?\n\n";
            MessageBoxResult mbr = MessageBox.Show(message, "Brisanje svih etiketa", MessageBoxButton.YesNo);

            if (mbr == MessageBoxResult.Yes)
            {
                ListaEtiketa.Etikete = null;
                ListaEtiketa.Etikete = new Dictionary<string, Etiketa>();

                foreach (KeyValuePair<string, Manifestacija> pair in ListaManifestacija.Manifestacije)
                {
                    pair.Value.Etikete = null;
                    pair.Value.Etikete = new List<Etiketa>();
                }
            }
        }
    }
}
