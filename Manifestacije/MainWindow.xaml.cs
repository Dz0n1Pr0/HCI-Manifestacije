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
        public static string PoslednjaPretraga = "";

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
            try
            {
                this.DragMove();
            }
            catch (Exception)
            {

            }
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
            var pomocna = new ObservableCollection<Manifestacija>();
            Manifestacije = new ObservableCollection<Manifestacija>(ListaManifestacija.Manifestacije.Values);
            foreach (Manifestacija m in Manifestacije)  //da se ne bi ponistavao search kada se klikne na dugme 'Nazad'
            {
                if (m.Ime.ToUpper().Contains(PoslednjaPretraga.ToUpper()))
                {
                    pomocna.Add(m);
                }
            }
            this.lista.ItemsSource = null;
            this.lista.ItemsSource = pomocna;
        }


        //Izmena vrsta
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Manifestacija selekt = (Manifestacija)lista.SelectedItem;

            if (selekt == null)
            {
                MessageBox.Show("You must choose event from the list!");
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
                MessageBox.Show("You must choose event from the list!");
                return;
            }

            ListaManifestacija.Manifestacije.Remove(selekt.ID);
            foreach (Etiketa etiketa in selekt.Etikete)
            {
                ListaEtiketa.Etikete.Remove(etiketa.ID);
            }
            this.setManifestacijeItems();
        }

        private void ListViewItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            Manifestacija selekt = (Manifestacija)lista.SelectedItem;
            var item = sender as ListViewItem;

            if (item != null && item.IsSelected)
            {
                if (ListaManifestacija.Manifestacije.ContainsKey(selekt.ID))
                {
                    ManifestacijaWindow rsv = new ManifestacijaWindow(this, true, selekt);
                    rsv.ShowDialog();
                }
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
            String message = "Are you sure?\n\n";
            MessageBoxResult mbr = MessageBox.Show(message, "Delete all events", MessageBoxButton.YesNo);

            if (mbr == MessageBoxResult.Yes)
            {
                ListaManifestacija.Manifestacije = null;
                ListaManifestacija.Manifestacije = new Dictionary<string, Manifestacija>();
                setManifestacijeItems();
            }
        }

        private void BrisanjeSvihTipovaManifestacija_Click(object sender, RoutedEventArgs e)
        {
            String message = "Are you sure?\n\n";
            MessageBoxResult mbr = MessageBox.Show(message, "Delete all event types", MessageBoxButton.YesNo);

            if (mbr == MessageBoxResult.Yes)
            {
                
                ListaTipManifestacijecs.TipoviManifestacija = null;
                ListaTipManifestacijecs.TipoviManifestacija = new Dictionary<string, TipManifestacije>();
                setManifestacijeItems();
            }
        }

        private void BrisanjeSvihEtiketa_Click(object sender, RoutedEventArgs e)
        {
            String message = "Are you sure?\n\n";
            MessageBoxResult mbr = MessageBox.Show(message, "Delete all labels", MessageBoxButton.YesNo);

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

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            this.lista.ItemsSource = null;
            string parametar = this.txtPRETRAGA.Text;
            if (this.txtPRETRAGA.FontStyle == FontStyles.Oblique)
            {
                parametar = "";
                this.txtPRETRAGA.FontStyle = FontStyles.Normal;
                this.txtPRETRAGA.Foreground = Brushes.Black;
            }
            ObservableCollection<Manifestacija> pomocna = new ObservableCollection<Manifestacija>();
            foreach (Manifestacija m in this.Manifestacije)
            {
                if (m.Ime.ToUpper().Contains(parametar.ToUpper()))
                {
                    pomocna.Add(m);
                }
            }
            PoslednjaPretraga = parametar;
            this.lista.ItemsSource = pomocna;
            this.txtPRETRAGA.Text = "";
        }

        private void txtPRETRAGA_Enter(object sender, RoutedEventArgs e)
        {
            if (this.txtPRETRAGA.FontStyle == FontStyles.Oblique)
            {
                this.txtPRETRAGA.Text = "";
                txtPRETRAGA.Foreground = Brushes.Black;
                txtPRETRAGA.FontStyle = FontStyles.Normal;
            }
        }

        private void txtPRETRAGA_Leave(object sender, RoutedEventArgs e)
        {
            if (txtPRETRAGA.Text == "" && this.Search_Button.IsMouseOver == false)
            {
                this.txtPRETRAGA.Text = "Search...";
                txtPRETRAGA.Foreground = Brushes.Silver;
                txtPRETRAGA.FontStyle = FontStyles.Oblique;
            }
        }

        private void Search_Button_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPRETRAGA.IsMouseOver == false)
            {
                this.txtPRETRAGA.Text = "Search...";
                txtPRETRAGA.Foreground = Brushes.Silver;
                txtPRETRAGA.FontStyle = FontStyles.Oblique;
            }
        }

    }
}
