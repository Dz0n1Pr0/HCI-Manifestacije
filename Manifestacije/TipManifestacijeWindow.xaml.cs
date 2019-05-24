using Manifestacije.Modeli;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for TipManifestacijeWindow.xaml
    /// </summary>
    public partial class TipManifestacijeWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        private string id;
        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged("ID");
                }
            }
        }
        private string ime;
        public string Ime
        {
            get
            {
                return ime;
            }
            set
            {
                if (value != ime)
                {
                    ime = value;
                    OnPropertyChanged("Ime");
                }
            }
        }
        private string opis;
        public string Opis
        {
            get
            {
                return opis;
            }
            set
            {
                if (value != opis)
                {
                    opis = value;
                    OnPropertyChanged("Opis");
                }
            }
        }
        private ImageSource ikonica;
        public ImageSource IkonicaP
        {
            get
            {
                return ikonica;
            }
            set
            {
                if (value != ikonica)
                {
                    ikonica = value;
                    ikonica = value;
                    OnPropertyChanged("Ikonica");
                }
            }
        }

        public bool Editing { get; set; }
        public TipManifestacije Selektovan { get; set; }
        public Window ParentWindow { get; set; }

        public TipManifestacijeWindow(Window parent, bool edit, TipManifestacije sel)
        {
            InitializeComponent();
            this.DataContext = this;

            ParentWindow = parent;
            this.Owner = parent;
            Editing = edit;
            Selektovan = sel;

            if (Editing)
            {
                lblNaslov.Text = "Izmena tipa vrste";
                popuniPolja();
                txtID.IsEnabled = false;
            }
        }

        public void popuniPolja()
        {
            this.ID = Selektovan.ID;
            this.Opis = Selektovan.Opis;
            this.Ime = Selektovan.Ime;
            this.IkonicaP = Selektovan.Ikonica;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            // Begin dragging the window
            this.DragMove();
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            String message = "Data will be lost. Are you sure?";
            MessageBoxResult mbr = MessageBox.Show(message, "Unfinished", MessageBoxButton.YesNo);

            if (mbr == MessageBoxResult.Yes)
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
                txtIKONICA.Text = dlg.FileName;
                string url = dlg.FileName;
                Ikonica.Source = new BitmapImage(new Uri(url, UriKind.Absolute));   // za prikaz
            }
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (ID == null || ID.Equals("") || Ime == null || Ime.Equals("") || Opis == null || Opis.Equals("") || IkonicaP == null)
            {
                MessageBox.Show("You must fill all fields", "Error");
                return;
            }
            else if (ListaTipManifestacijecs.TipoviManifestacija.ContainsKey(ID) && Editing == false)
            {
                MessageBox.Show("ID already exists!", "Wrong ID");
                return;
            }


            if (Editing == true)
            {
                //SpisakVrsta.Vrste[stariID].Ime = Ime;
                ListaTipManifestacijecs.TipoviManifestacija[ID].Ime = Ime;
                ListaTipManifestacijecs.TipoviManifestacija[ID].Opis = Opis;
                ListaTipManifestacijecs.TipoviManifestacija[ID].Ikonica = IkonicaP;

            }
            else
            {
                ListaTipManifestacijecs.TipoviManifestacija.Add(ID, new TipManifestacije(ID, Ime, Opis, IkonicaP));
            }

            if (ParentWindow is ViewWindow)
            {
                ViewWindow parentWindow = (ViewWindow)Owner;
                parentWindow.dodajTipManifestacije(new TipManifestacije(ID, Ime, Opis, IkonicaP));
            }

            this.Close();
        }
    }
}

