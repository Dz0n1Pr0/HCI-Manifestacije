using Manifestacije.Modeli;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;

namespace Manifestacije
{
    /// <summary>
    /// Interaction logic for Filter.xaml
    /// </summary>
    public partial class Filter : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Window parentWindow;

        public ObservableCollection<TipManifestacije> TipoviManifestacije { get; set; }
        public ObservableCollection<Etiketa> Etikete { get; set; }

      
        public ObservableCollection<Manifestacija> filtrirano { get; set; }
        public ObservableCollection<TipManifestacije> filterTipovi { get; set; }
        public ObservableCollection<Etiketa> filterEtikete { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public bool NoAlcohol;
        public bool BringAlcohol;
        public bool BuyAlcohol;
        public bool Free;
        public bool Low;
        public bool Medium;
        public bool High;
        public bool YesHandic;
        public bool NoHandic;
        public bool YesSmoking;
        public bool NoSmoking;
        public bool YesOut;
        public bool NoOut;


        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public Filter()
        {
            InitializeComponent();
        }

        public Filter(Window parent)
        {
            InitializeComponent();
            CreateCheckBoxes();
            CalendarDateRange cdr = new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1));
            DateFrom.BlackoutDates.Add(cdr);
            filtrirano = new ObservableCollection<Manifestacija>();
            filterTipovi = new ObservableCollection<TipManifestacije>();
            filterEtikete = new ObservableCollection<Etiketa>();
            NoAlcohol = false;
            BringAlcohol = false;
            BuyAlcohol = false;
            Free = false;
            Low = false;
            Medium = false;
            High = false;
            YesHandic = false;
            NoHandic = false;
            YesSmoking = false;
            NoSmoking = false;
            YesOut = false;
            NoOut = false;
            FromDate = "";
            ToDate ="";

            this.DataContext = this;
            parentWindow = parent;
            this.Owner = parent;
  
        }

        public ObservableCollection<Manifestacija> getFiltrirano()
        {
            return filtrirano;
        }
        public void CreateCheckBoxes()
        {
            TipoviManifestacije= new ObservableCollection<TipManifestacije>();
            Etikete = new ObservableCollection<Etiketa>();
            
            foreach(TipManifestacije tip in ListaTipManifestacijecs.TipoviManifestacija.Values)
            {
                TipoviManifestacije.Add(tip);
            }
            foreach(Etiketa e in ListaEtiketa.Etikete.Values)
            {
                Etikete.Add(e);
            }
            this.DataContext = this;
        }

       
        private void CheckBoxZoneTypes_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chkZone = (CheckBox)sender;
            TipManifestacije tip = ListaTipManifestacijecs.TipoviManifestacija[chkZone.Tag.ToString()];
            filterTipovi.Add(tip);          
        }
        private void CheckBoxZoneTypes_unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chkZone = (CheckBox)sender;
            TipManifestacije tip = ListaTipManifestacijecs.TipoviManifestacija[chkZone.Tag.ToString()];
            filterTipovi.Remove(tip);
        }

        private void CheckBoxZoneLabels_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chkZone = (CheckBox)sender;
            Etiketa et = ListaEtiketa.Etikete[chkZone.Tag.ToString()];
            filterEtikete.Add(et);
        }
        private void CheckBoxZoneLabels_unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chkZone = (CheckBox)sender;
            Etiketa et = ListaEtiketa.Etikete[chkZone.Tag.ToString()];
            filterEtikete.Remove(et);
        }

        private void DateFromChanged(object sender, RoutedEventArgs e)
        {
            var picker = sender as DatePicker;

            DateTime? date = picker.SelectedDate;
            if (date == null)
            {
                this.Title = "No date";
                DateTo.IsEnabled = false;
                filterBtn.IsEnabled = true;
            }
            else
            {
                FromDate = picker.SelectedDate.Value.ToShortDateString();
                DateTo.IsEnabled = true;
                filterBtn.IsEnabled = false;
                CalendarDateRange cdr = new CalendarDateRange(DateTime.MinValue, ((DateTime)date).AddDays(-1));
                DateTo.BlackoutDates.Add(cdr);
            }
       
        }

        private void DateToChanged(object sender, RoutedEventArgs e)
        {
           
            var picker = sender as DatePicker;

            DateTime? date = picker.SelectedDate;
            if (date == null)
            {
                this.Title = "No date";
                filterBtn.IsEnabled = false;
               
            }
            else
            {
                ToDate = picker.SelectedDate.Value.ToShortDateString();
                filterBtn.IsEnabled = true;
                CalendarDateRange cdr = new CalendarDateRange(DateTime.MinValue, ((DateTime)date).AddDays(-1));
                DateTo.BlackoutDates.Add(cdr);
            }

        }

        private void CbxChecked(object sender, RoutedEventArgs e)
        {
            CheckBox cbx = (CheckBox)sender;
            String name = cbx.Name;
            switch (name)
            {
                case "chcNo":
                    NoAlcohol = true;
                    break;
                case "chcBring":
                    BringAlcohol = true;
                    break;
                case "chcBuy":
                    BuyAlcohol = true;
                    break;
                case "chcFree":
                    Free = true;
                    break;
                case "chcLow":
                    Low = true;
                    break;
                case "chcMedium":
                    Medium = true;
                    break;
                case "chcHigh":
                    High = true;
                    break;
                case "HandYes":
                    YesHandic = true;
                    break;
                case "HandNo":
                    NoHandic = true;
                    break;
                case "SmokingYes":
                    YesSmoking = true;
                    break;
                case "SmokingNo":
                    NoSmoking = true;
                    break;
                case "OutdoorsYes":
                    YesOut = true;
                    break;
                case "OutdoorsNo":
                    NoOut = true;
                    break;

                default:
                    break;

            }
        }
        private void CbxUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cbx = (CheckBox)sender;
            String name = cbx.Name;
            switch (name)
            {
                case "chcNo":
                    NoAlcohol = false;
                    break;
                case "chcBring":
                    BringAlcohol = false;
                    break;
                case "chcBuy":
                    BuyAlcohol = false;
                    break;
                case "chcFree":
                    Free = false;
                    break;
                case "chcLow":
                    Low = false;
                    break;
                case "chcMedium":
                    Medium = false;
                    break;
                case "chcHigh":
                    High = false;
                    break;
                case "HandYes":
                    YesHandic = false;
                    break;
                case "HandNo":
                    NoHandic = false;
                    break;
                case "SmokingYes":
                    YesSmoking = false;
                    break;
                case "SmokingNo":
                    NoSmoking = false;
                    break;
                case "OutdoorsYes":
                    YesOut = false;
                    break;
                case "OutdoorsNo":
                    NoOut = false;
                    break;
                default:
                    break;

            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            String message = "Are you sure?";
            MessageBoxResult mbr = MessageBox.Show(message, "Unfinished", MessageBoxButton.YesNo);

            if (mbr == MessageBoxResult.Yes)
            {
                MainWindow pw = (MainWindow)Owner;
                pw.filtriranje = false;
                Close();
            }
        }

        private void DoFilter(object sender, RoutedEventArgs e)
        {
            ListaManifestacija.FilterManifestacije = DoFilterList();
            ListaManifestacija.FilterSacuvaneNaMapi1 = DoFilterMape(ListaManifestacija.SacuvaneNaMapi1);
            ListaManifestacija.FilterSacuvaneNaMapi2 = DoFilterMape(ListaManifestacija.SacuvaneNaMapi2);
            ListaManifestacija.FilterSacuvaneNaMapi3 = DoFilterMape(ListaManifestacija.SacuvaneNaMapi3);
            ListaManifestacija.FilterSacuvaneNaMapi4 = DoFilterMape(ListaManifestacija.SacuvaneNaMapi4);

            MainWindow pw = (MainWindow)Owner;
            pw.lista.ItemsSource = ListaManifestacija.FilterManifestacije;
            if(pw.aktivnaMapa == 1)
            {
                pw.MapaGrada.ItemsSource = ListaManifestacija.FilterSacuvaneNaMapi1;
            }
            else if(pw.aktivnaMapa == 2)
            {
                pw.MapaGrada.ItemsSource = ListaManifestacija.FilterSacuvaneNaMapi2;
            }
            else if (pw.aktivnaMapa == 3)
            {
                pw.MapaGrada.ItemsSource = ListaManifestacija.FilterSacuvaneNaMapi3;
            }
            else if (pw.aktivnaMapa == 4)
            {
                pw.MapaGrada.ItemsSource = ListaManifestacija.FilterSacuvaneNaMapi4;
            }


            Close();

        }

        private ObservableCollection<Manifestacija> DoFilterMape(ObservableCollection<Manifestacija> manifZaFilter)
        {
            var pomocna = new ObservableCollection<Manifestacija>();
            var pomocna2 = new ObservableCollection<Manifestacija>();
            //prvo filtriramo po tipu ako ima neki cekiran
            if (filterTipovi.Count > 0)
            {
                foreach (TipManifestacije tip in filterTipovi)
                {
                    foreach (Manifestacija m in manifZaFilter)
                    {
                        if (m.Tip == null)
                        {
                            continue;
                        }
                        if (m.Tip.Equals(tip))
                        {
                            pomocna.Add(m);
                        }
                    }
                }
            }
            else
            {
                pomocna = new ObservableCollection<Manifestacija>(manifZaFilter);
            }
            //filtriramo novu listu prema etiketama
            if (filterEtikete.Count > 0)
            {
                foreach (Etiketa et in filterEtikete)
                {
                    foreach (Manifestacija m in pomocna)
                    {
                        foreach (Etiketa et2 in m.Etikete)
                        {
                            if (et.Equals(et2))
                            {
                                pomocna2.Add(m);
                                break;
                            }
                        }
                    }
                }
                pomocna = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna2 = pomocna;
                pomocna = new ObservableCollection<Manifestacija>();
            }
            //filtriranje po datumu
            if (!(FromDate.Equals("")))
            {
                Console.WriteLine(FromDate + "aa" + ToDate);
                DateTime fDate = DateTime.ParseExact(FromDate, "dd-MMM-yy", null);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd-MMM-yy", null);
                Console.WriteLine(fDate + "eeeeee" + tDate);
                foreach (Manifestacija m in pomocna2)
                {
                    DateTime mDate = DateTime.ParseExact(m.Datum, "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                    if (mDate.Ticks >= fDate.Ticks && mDate.Ticks <= tDate.Ticks)
                    {
                        pomocna.Add(m);
                    }
                }
                pomocna2 = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna = pomocna2;
                pomocna2 = new ObservableCollection<Manifestacija>();
            }
            //alkohol filter 
            if (NoAlcohol || BringAlcohol || BuyAlcohol)
            {
                foreach (Manifestacija m in pomocna)
                {
                    if (NoAlcohol && m.StatusSluzenjaAlkohola.Equals("No alcohol"))
                    {
                        pomocna2.Add(m);
                    }
                    if (BringAlcohol && m.StatusSluzenjaAlkohola.Equals("You can bring alcohol"))
                    {
                        pomocna2.Add(m);
                    }
                    if (BuyAlcohol && m.StatusSluzenjaAlkohola.Equals("You can buy alcohol"))
                    {
                        pomocna2.Add(m);
                    }
                }
                pomocna = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna2 = pomocna;
                pomocna = new ObservableCollection<Manifestacija>();
            }

            //cene filter
            if (Free || Low || Medium || High)
            {
                foreach (Manifestacija m in pomocna2)
                {
                    if (Free && m.KategorijaCene.Equals("Free"))
                    {
                        pomocna.Add(m);
                    }
                    if (Low && m.KategorijaCene.Equals("Low prices"))
                    {
                        pomocna.Add(m);
                    }
                    if (Medium && m.KategorijaCene.Equals("Meduim prices"))
                    {
                        pomocna.Add(m);
                    }
                    if (High && m.KategorijaCene.Equals("High prices"))
                    {
                        pomocna.Add(m);
                    }
                }
                pomocna2 = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna = pomocna2;
                pomocna2 = new ObservableCollection<Manifestacija>();
            }
            //hendikepirani filter
            if (YesHandic || NoHandic)
            {
                foreach (Manifestacija m in pomocna)
                {
                    if (YesHandic && m.Hendikepirani)
                    {
                        pomocna2.Add(m);
                    }
                    if (NoHandic && !(m.Hendikepirani))
                    {
                        pomocna2.Add(m);
                    }
                }
                pomocna = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna2 = pomocna;
                pomocna = new ObservableCollection<Manifestacija>();
            }
            //pusenje filter
            if (YesSmoking || NoSmoking)
            {
                foreach (Manifestacija m in pomocna2)
                {
                    if (YesSmoking && m.Pusenje)
                    {
                        pomocna.Add(m);
                    }
                    if (NoSmoking && !(m.Pusenje))
                    {
                        pomocna2.Add(m);
                    }
                }
                pomocna2 = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna = pomocna2;
                pomocna2 = new ObservableCollection<Manifestacija>();
            }
            //napolju filter
            if (YesOut || NoOut)
            {
                foreach (Manifestacija m in pomocna)
                {
                    if (YesOut && m.Hendikepirani)
                    {
                        pomocna2.Add(m);
                    }
                    if (NoOut && !(m.Hendikepirani))
                    {
                        pomocna2.Add(m);
                    }
                }
                pomocna = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna2 = pomocna;
                pomocna = new ObservableCollection<Manifestacija>();
            }

            filtrirano = pomocna2;

            return filtrirano;
            
        }


     private ObservableCollection<Manifestacija> DoFilterList()
        {
            var pomocna = new ObservableCollection<Manifestacija>();
            var pomocna2 = new ObservableCollection<Manifestacija>();
            //prvo filtriramo po tipu ako ima neki cekiran
            if (filterTipovi.Count > 0)
            {
                foreach(TipManifestacije tip in filterTipovi)
                {
                    foreach(Manifestacija m in ListaManifestacija.Manifestacije.Values)
                    {
                        if (m.Tip == null)
                        {
                            continue;
                        }
                        if (m.Tip.Equals(tip))
                        {
                            pomocna.Add(m);
                        }
                    }
                }
            }else
            {
                pomocna = new ObservableCollection<Manifestacija>(ListaManifestacija.Manifestacije.Values);
            }
            //filtriramo novu listu prema etiketama
            if (filterEtikete.Count > 0)
            {
                foreach (Etiketa et in filterEtikete)
                {
                    foreach (Manifestacija m in pomocna)
                    {
                       foreach(Etiketa et2 in m.Etikete)
                        {
                            if (et.Equals(et2))
                            {
                                pomocna2.Add(m);
                                break;
                            }
                        }
                    }
                }
                pomocna = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna2 = pomocna;
                pomocna = new ObservableCollection<Manifestacija>();
            }
            //filtriranje po datumu
            if (!(FromDate.Equals("")))
            {
                Console.WriteLine(FromDate + "aa" + ToDate);
                DateTime fDate = DateTime.ParseExact(FromDate, "dd-MMM-yy", null);
                DateTime tDate = DateTime.ParseExact(ToDate, "dd-MMM-yy", null);
                Console.WriteLine(fDate + "eeeeee" + tDate);
                foreach (Manifestacija m in pomocna2)
                {
                    DateTime mDate = DateTime.ParseExact(m.Datum, "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                    if (mDate.Ticks>=fDate.Ticks && mDate.Ticks <= tDate.Ticks)
                    {
                        pomocna.Add(m);
                    }
                }
                pomocna2 = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna = pomocna2;
                pomocna2 = new ObservableCollection<Manifestacija>();
            }
            //alkohol filter 
            if (NoAlcohol || BringAlcohol || BuyAlcohol)
            {
                foreach(Manifestacija m in pomocna)
                {
                    if (NoAlcohol && m.StatusSluzenjaAlkohola.Equals("No alcohol"))
                    {
                        pomocna2.Add(m);
                    }
                    if(BringAlcohol && m.StatusSluzenjaAlkohola.Equals("You can bring alcohol"))
                    {
                        pomocna2.Add(m);
                    }
                    if (BuyAlcohol && m.StatusSluzenjaAlkohola.Equals("You can buy alcohol"))
                    {
                        pomocna2.Add(m);
                    }
                }
                pomocna = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna2 = pomocna;
                pomocna = new ObservableCollection<Manifestacija>();
            }

            //cene filter
            if (Free || Low || Medium || High)
            {
                foreach (Manifestacija m in pomocna2)
                {
                    if (Free && m.KategorijaCene.Equals("Free"))
                    {
                        pomocna.Add(m);
                    }
                    if (Low && m.KategorijaCene.Equals("Low prices"))
                    {
                        pomocna.Add(m);
                    }
                    if (Medium && m.KategorijaCene.Equals("Meduim prices"))
                    {
                        pomocna.Add(m);
                    }
                    if (High && m.KategorijaCene.Equals("High prices"))
                    {
                        pomocna.Add(m);
                    }
                }
                pomocna2 = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna = pomocna2;
                pomocna2 = new ObservableCollection<Manifestacija>();
            }
            //hendikepirani filter
            if (YesHandic || NoHandic)
            {
                foreach (Manifestacija m in pomocna)
                {
                    if (YesHandic && m.Hendikepirani)
                    {
                        pomocna2.Add(m);
                    }
                    if (NoHandic && !(m.Hendikepirani))
                    {
                        pomocna2.Add(m);
                    }
                }
                pomocna = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna2 = pomocna;
                pomocna = new ObservableCollection<Manifestacija>();
            }
            //pusenje filter
            if (YesSmoking || NoSmoking)
            {
                foreach (Manifestacija m in pomocna2)
                {
                    if (YesSmoking && m.Pusenje)
                    {
                        pomocna.Add(m);
                    }
                    if (NoSmoking && !(m.Pusenje))
                    {
                        pomocna2.Add(m);
                    }
                }
                pomocna2 = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna = pomocna2;
                pomocna2 = new ObservableCollection<Manifestacija>();
            }
            //napolju filter
            if (YesOut || NoOut)
            {
                foreach (Manifestacija m in pomocna)
                {
                    if (YesOut && m.Hendikepirani)
                    {
                        pomocna2.Add(m);
                    }
                    if (NoOut && !(m.Hendikepirani))
                    {
                        pomocna2.Add(m);
                    }
                }
                pomocna = new ObservableCollection<Manifestacija>();
            }
            else
            {
                pomocna2 = pomocna;
                pomocna = new ObservableCollection<Manifestacija>();
            }

            filtrirano = pomocna2;
            return filtrirano;

            
           
            
           
        }

    }
}
