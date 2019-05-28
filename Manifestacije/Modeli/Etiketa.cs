using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Manifestacije.Modeli
{
    [Serializable]
    public class Etiketa : INotifyPropertyChanged
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
        [DisplayName("ID")]
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
        private Color boja;
        [DisplayName("Color")]
        public Color Boja
        {
            get
            {
                return boja;
            }
            set
            {
                if (value != boja)
                {
                    boja = value;
                    OnPropertyChanged("Boja");
                }
            }
        }
        private string opis;
        [DisplayName("Description")]
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
        private Brush bojaBrush;
        public Brush BojaBrush
        {
            get
            {
                return bojaBrush;
            }
            set
            {
                if (value != bojaBrush)
                {
                    bojaBrush = value;
                    OnPropertyChanged("Boja Brush");
                }
            }
        }


        public Etiketa(string _id, Color _boja, string _opis, Brush _brush)
        {
            ID = _id;
            Boja = _boja;
            Opis = _opis;
            BojaBrush = _brush;
        }

        public override string ToString()
        {
            return this.Opis + "|" +
                   this.BojaBrush;
        }
    }
}

