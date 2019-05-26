using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Manifestacije.Modeli
{
    [Serializable]
    public class Manifestacija : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


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
        private Point _tacka;

        private List<Etiketa> _etikete { get; set; }

        public List<Etiketa> Etikete
        {
            get
            {
                return _etikete;
            }
            set
            {
                _etikete = value;
            }
        }


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
        public String Tip
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
        public ImageSource Ikonica
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
        public Point Tacka
        {
            get
            {
                return _tacka;
            }
            set
            {
                if (value != _tacka)
                {
                    _tacka = value;
                    OnPropertyChanged("Pozicija");
                }
            }
        }

        /*
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
         
             */

        public Manifestacija(string id,
                     string ime,
                     string opis,
                     string statusSluzenjaAlkohola,
                     string kategorijaCene,
                     bool hendikepirani,
                     bool pusenje,
                     bool napolju,
                     int ocekivanaPublika,
                     string datum,
                     ImageSource ikonica,
                     string tip,
                     Point tacka)
        {
            ID = id;
            Ime = ime;
            Opis = opis;
            StatusSluzenjaAlkohola = statusSluzenjaAlkohola;
            KategorijaCene = kategorijaCene;
            Hendikepirani = hendikepirani;
            Pusenje = pusenje;
            Napolju = napolju;
            OcekivanaPublika = ocekivanaPublika;
            Datum = datum;
            Ikonica = ikonica;
            Tip = tip;
            Tacka = tacka;
            Etikete = new List<Etiketa>();
        }

        public Manifestacija(string id, string ime)
        {
            Etikete = new List<Etiketa>();
            ID = id;
            Ime = ime;
        }

        public Manifestacija()
        {
        }

        public override string ToString()
        {
            //(Ikonica as BitmapImage).UriSource
            return this.Ime + ";" +
                   this.Opis + ";" +
                   this.StatusSluzenjaAlkohola + ";" +
                   this.KategorijaCene + ";" +
                   this.Hendikepirani + ";" +
                   this.Pusenje + ";" +
                   this.Napolju + ";" +
                   this.OcekivanaPublika + ";" +
                   this.Datum + ";" +
                   this.Ikonica + ";" +
                   this.Tip + ";" + 
                   this.Tacka;
        }


    }


}


