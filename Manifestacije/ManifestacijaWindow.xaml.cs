using Manifestacije.Modeli;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for ManifestacijaWindow.xaml
    /// </summary>
    public partial class ManifestacijaWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        public bool Editing { get; set; }
        public Manifestacija Selektovan { get; set; }

        public ObservableCollection<String> StatusAlkohola { get; set; }
        public ObservableCollection<String> StatusKategorije { get; set; }

        internal void doThings()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<String> TipoviManifestacije { get; set; }
        public ObservableCollection<Etiketa> SveEtikete { get; set; }
        public ObservableCollection<Etiketa> IzabraneEtikete { get; set; }

        public Window ParWindow { get; set; }


        private string _id;
        private string _ime;
        private string _opis;
        private string _statusSluzenjaAlkohola;
        private string _kategorijaCene;
        private ImageSource _ikonica;
        private bool _hendikepirani;
        private bool _pusenje;
        private bool _napolju;
        private int _ocekivanaPublika;
        private string _tip { get; set; }
        private string _datum { get; set; }


        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("ID");
                }
            }
        }
        public string Ime
        {
            get
            {
                return _ime;
            }
            set
            {
                if (value != _ime)
                {
                    _ime = value;
                    OnPropertyChanged("Ime");
                }
            }
        }
        public string Tip
        {
            get
            {
                return _tip;
            }
            set
            {
                if (value != _tip)
                {
                    _tip = value;
                    OnPropertyChanged("Tip");
                }
            }
        }
        public bool Hendikepirani
        {
            get
            {
                return _hendikepirani;
            }
            set
            {
                if (value != _hendikepirani)
                {
                    _hendikepirani = value;
                    OnPropertyChanged("Pogodno za hendikepirane");
                }
            }
        }
        public bool Pusenje
        {
            get
            {
                return _pusenje;
            }
            set
            {
                if (value != _pusenje)
                {
                    _pusenje = value;
                    OnPropertyChanged("Dozvoljeno je pušenje cigara");
                }
            }
        }
        public bool Napolju
        {
            get
            {
                return _napolju;
            }
            set
            {
                if (value != _napolju)
                {
                    _napolju = value;
                    OnPropertyChanged("Održava se napolju");
                }
            }
        }
        public string Opis
        {
            get
            {
                return _opis;
            }
            set
            {
                if (value != _opis)
                {
                    _opis = value;
                    OnPropertyChanged("Opis");
                }
            }
        }
        public int OcekivanaPublika
        {
            get
            {
                return _ocekivanaPublika;
            }
            set
            {
                if (value != _ocekivanaPublika)
                {
                    _ocekivanaPublika = value;
                    OnPropertyChanged("Prihod");
                }
            }
        }
        public string Datum
        {
            get
            {
                return _datum;
            }
            set
            {
                if (value != _datum)
                {
                    _datum = value;
                    OnPropertyChanged("Datum");
                }
            }
        }
        public string StatusSluzenjaAlkohola
        {
            get
            {
                return _statusSluzenjaAlkohola;
            }
            set
            {
                if (value != _statusSluzenjaAlkohola)
                {
                    _statusSluzenjaAlkohola = value;
                    OnPropertyChanged("Status služenja alkohola");
                }
            }
        }
        public string KategorijaCene
        {
            get
            {
                return _kategorijaCene;
            }
            set
            {
                if (value != _kategorijaCene)
                {
                    _kategorijaCene = value;
                    OnPropertyChanged("Turistički status");
                }
            }
        }
        public ImageSource IkonicaP
        {
            get
            {
                return _ikonica;
            }
            set
            {
                if (value != _ikonica)
                {
                    _ikonica = value;
                    OnPropertyChanged("Ikonica");
                }
            }
        }


        public ManifestacijaWindow(Window parent, bool edit, Manifestacija sel)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Editing = edit;

            ParWindow = parent;
            this.Owner = parent;

            if (sel == null)
            {
                Selektovan = new Manifestacija();
                Selektovan.Etikete = new List<Etiketa>();
            }
            else
            {
                Selektovan = sel;
                if (sel.Etikete == null)
                {
                    Selektovan.Etikete = new List<Etiketa>();
                }
            }


            TipoviManifestacije = new ObservableCollection<String>();
            foreach (TipManifestacije tip in ListaTipManifestacijecs.TipoviManifestacija.Values)
            {
                TipoviManifestacije.Add(tip.Ime);
            }

            if (TipoviManifestacije.Count > 0)
            {
                Tip = TipoviManifestacije[0];
            }


            StatusAlkohola = new ObservableCollection<string>();
            StatusAlkohola.Add("No alcohol");
            StatusAlkohola.Add("You can bring alcohol");
            StatusAlkohola.Add("You can buy alcohol");
            StatusSluzenjaAlkohola = StatusAlkohola[0];

            StatusKategorije = new ObservableCollection<string>();
            StatusKategorije.Add("Free");
            StatusKategorije.Add("Low prices");
            StatusKategorije.Add("Meduim prices");
            StatusKategorije.Add("High prices");
            KategorijaCene = StatusKategorije[0];

            CalendarDateRange cdr = new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1));
            dDATUM.BlackoutDates.Add(cdr);

            AddLbl.IsEnabled = false;
            RmvLbl.IsEnabled = false;

            SveEtikete = new ObservableCollection<Etiketa>();
            IzabraneEtikete = new ObservableCollection<Etiketa>();

            //napunimo listu svim etiketama koje imamo 
            foreach (Etiketa e in ListaEtiketa.Etikete.Values)
            {
                SveEtikete.Add(e);
            }

            listaPostojecihEtiketa.ItemsSource = SveEtikete;
            listaEtiketa.ItemsSource = IzabraneEtikete;

            if (Editing)
            {
                Title = "Edit Event";
                popuniPolja();
                txtID.IsEnabled = false;        //ID se ne moze menjati
                NapuniEtikete();
                AddMore.Visibility = Visibility.Collapsed;
                
            }


        }


        private void NapuniEtikete()
        {
            foreach (Etiketa etiketa in Selektovan.Etikete)
            {
                IzabraneEtikete.Add(etiketa);
                SveEtikete.Remove(etiketa);
            }
            listaEtiketa.ItemsSource = IzabraneEtikete;
            listaPostojecihEtiketa.ItemsSource = SveEtikete;

        }


        private void popuniPolja()
        {
            foreach (string s in StatusAlkohola)
            {
                if (s.Equals(Selektovan.StatusSluzenjaAlkohola))
                {
                    StatusSluzenjaAlkohola = s;
                    break;
                }
            }

            foreach (string s in StatusKategorije)
            {
                if (s.Equals(Selektovan.KategorijaCene))
                {
                    KategorijaCene = s;
                    break;
                }
            }

            foreach (string s in TipoviManifestacije)
            {
                if (Selektovan.Tip == null)
                {
                    Tip = TipoviManifestacije[0];
                    break;
                }
                else if (s.Equals(Selektovan.Tip.Ime))
                {
                    Tip = s;
                    break;

                }
            }

            ID = Selektovan.ID;
            Ime = Selektovan.Ime;
            Opis = Selektovan.Opis;
            StatusSluzenjaAlkohola = Selektovan.StatusSluzenjaAlkohola;
            KategorijaCene = Selektovan.KategorijaCene;
            Hendikepirani = Selektovan.Hendikepirani;
            Pusenje = Selektovan.Pusenje;
            Napolju = Selektovan.Napolju;
            OcekivanaPublika = Selektovan.OcekivanaPublika;
            Opis = Selektovan.Opis;
            IkonicaP = Selektovan.Ikonica;
            Datum = Selektovan.Datum;



        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            // Begin dragging the window
            this.DragMove();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            String message = "Data will be lost. Are you sure?";
            MessageBoxResult mbr = MessageBox.Show(message, "Unfinished", MessageBoxButton.YesNo);

            if (mbr == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            TipManifestacije tip = null;
            foreach (TipManifestacije tm in ListaTipManifestacijecs.TipoviManifestacija.Values)
            {
                if (tm.Ime.Equals(Tip))
                {
                    tip = tm;

                }
            }

            if (Editing == true)
            {

                ListaManifestacija.Manifestacije[Selektovan.ID].Tip = tip;
                ListaManifestacija.Manifestacije[Selektovan.ID].Ime = Ime;
                ListaManifestacija.Manifestacije[Selektovan.ID].StatusSluzenjaAlkohola = StatusSluzenjaAlkohola;
                ListaManifestacija.Manifestacije[Selektovan.ID].KategorijaCene = KategorijaCene;
                ListaManifestacija.Manifestacije[Selektovan.ID].Hendikepirani = Hendikepirani;
                ListaManifestacija.Manifestacije[Selektovan.ID].Pusenje = Pusenje;
                ListaManifestacija.Manifestacije[Selektovan.ID].Napolju = Napolju;
                ListaManifestacija.Manifestacije[Selektovan.ID].OcekivanaPublika = OcekivanaPublika;
                ListaManifestacija.Manifestacije[Selektovan.ID].Opis = Opis;
                ListaManifestacija.Manifestacije[Selektovan.ID].Datum = Datum;
                ListaManifestacija.Manifestacije[Selektovan.ID].Ikonica = IkonicaP;
                ListaManifestacija.Manifestacije[Selektovan.ID].Etikete = null;
                ListaManifestacija.Manifestacije[Selektovan.ID].Etikete = new List<Etiketa>();
                foreach (Etiketa etiketa in this.IzabraneEtikete)
                {
                    ListaManifestacija.Manifestacije[Selektovan.ID].Etikete.Add(etiketa);
                }

            }
            else
            {

                if (IkonicaP == null)
                {
                    string idTipa = "";
                    foreach (KeyValuePair<string, TipManifestacije> pair in ListaTipManifestacijecs.TipoviManifestacija)
                    {
                        if (pair.Value.Ime.Equals(Tip))
                        {
                            idTipa = pair.Key;
                            break;
                        }
                    }
                    IkonicaP = ListaTipManifestacijecs.TipoviManifestacija[idTipa].Ikonica;
                }

                if (Opis == null)
                {
                    Opis = "";
                }


                Manifestacija novaManifestacija = new Manifestacija(ID, Ime, Opis, StatusSluzenjaAlkohola, KategorijaCene, Hendikepirani, Pusenje, Napolju, OcekivanaPublika, Datum, IkonicaP, tip, new Point());
                novaManifestacija.Etikete = new List<Etiketa>();
                if (this.IzabraneEtikete != null)
                {
                    if (this.IzabraneEtikete.Count != 0)
                    {
                        foreach (Etiketa etiketa in this.IzabraneEtikete)
                        {
                            novaManifestacija.Etikete.Add(etiketa);
                        }
                    }
                }
                
                ListaManifestacija.Manifestacije.Add(ID, novaManifestacija);
            }



            //Refresh liste u parent prozoru
            if (ParWindow is MainWindow)
            {
                MainWindow pw = (MainWindow)Owner;
                pw.setManifestacijeItems();
            }
            else if (ParWindow is ViewWindow)
            {
                ViewWindow parentWindow = (ViewWindow)Owner;
                parentWindow.dodajManifestaciju(new Manifestacija(ID, Ime, Opis, StatusSluzenjaAlkohola, KategorijaCene, Hendikepirani, Pusenje, Napolju, OcekivanaPublika, Datum, IkonicaP, tip, new Point()));
                MainWindow main = parentWindow.ParentWindow;
                main.setManifestacijeItems();
            }

            Selektovan = null;
            IzabraneEtikete = null;

            if (((Button)sender).Name.Equals("AddMore"))
            {
                Selektovan = new Manifestacija();
                Selektovan.Etikete = new List<Etiketa>();

                popuniPolja();
                NapuniEtikete();

            }
            else
            {
                Close();
            }

        }

        private void Ikonica_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                FileName = "File",
                DefaultExt = ".png",
                Filter = "All Images Files (*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif)|*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif" +
                "|PNG Portable Network Graphics (*.png)|*.png" +
                "|JPEG File Interchange Format (*.jpg *.jpeg *jfif)|*.jpg;*.jpeg;*.jfif" +
                "|BMP Windows Bitmap (*.bmp)|*.bmp" +
                "|TIF Tagged Imaged File Format (*.tif *.tiff)|*.tif;*.tiff" +
                "|GIF Graphics Interchange Format (*.gif)|*.gif"
            };

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {

                string url = dlg.FileName;
                Ikonica.Source = new BitmapImage(new Uri(url, UriKind.Absolute));   // za prikaz
                IkonicaP = new BitmapImage(new Uri(url, UriKind.Absolute));
            }
        }

        private void Ikonica_ClickRmv(object sender, RoutedEventArgs e)
        {

            Ikonica.Source = null;   // za prikaz
            IkonicaP = null;
        }



        private void DodajEtiketu_Click(object sender, RoutedEventArgs e)
        {

            if (listaPostojecihEtiketa.SelectedItem != null)
            {

                Selektovan.Etikete.Add((Etiketa)listaPostojecihEtiketa.SelectedItem);
                IzabraneEtikete.Add((Etiketa)listaPostojecihEtiketa.SelectedItem);
                SveEtikete.Remove((Etiketa)listaPostojecihEtiketa.SelectedItem);
                AddLbl.IsEnabled = false;
                RmvLbl.IsEnabled = false;

            }
        }

        private void ObrisiEtiketu_Click(object sender, RoutedEventArgs e)
        {
            if (listaEtiketa.SelectedItem != null)
            {
                SveEtikete.Add((Etiketa)listaEtiketa.SelectedItem);
                Selektovan.Etikete.Remove((Etiketa)listaEtiketa.SelectedItem);
                IzabraneEtikete.Remove((Etiketa)listaEtiketa.SelectedItem);
                AddLbl.IsEnabled = false;
                RmvLbl.IsEnabled = false;
            }
        }

        private void listaPostojecihEtiketa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddLbl.IsEnabled = true;
            RmvLbl.IsEnabled = false;
        }

        private void listaEtiketa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RmvLbl.IsEnabled = true;
            AddLbl.IsEnabled = false;
        }
        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HelpProvider.ShowHelp(this);
        }
    }
}
