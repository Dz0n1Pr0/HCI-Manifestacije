﻿using Manifestacije.Modeli;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security;
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
using System.Windows.Threading;

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

        internal void doThings()
        {
            throw new NotImplementedException();
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
        private Point startPoint = new Point();
        public DispatcherTimer timer;
        public int tajmer;
        public int aktivnaMapa { get; set; }

        private ObservableCollection<Manifestacija> Manifestacije { get; set; }
        private ObservableCollection<Manifestacija> ManifestacijeNaMapi { get; set; }
        

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;


            //punjenje liste
            //setManifestacijeItems();
            rnd = new Random();
            
            initMapLists(); //SAMO ZA TESTIRANJE, INACE SE PUNI PODACIMA PRILIKOM LOAD

            Load();
            aktivnaMapa = 1;
            loadMapEvents(aktivnaMapa);
            setManifestacijeItems();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //brise selekciju
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                lista.UnselectAll();

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
            if (ListaTipManifestacijecs.TipoviManifestacija.Count == 0)
            {
                MessageBoxResult mbr = MessageBox.Show("There is no event type. Add event type for enabling this option.", "No event type");
            }
            else
            {
                ManifestacijaWindow manif = new ManifestacijaWindow(this, false, null);
                manif.ShowDialog();
            }

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

       
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Manifestacija selekt1 = (Manifestacija)lista.SelectedItem;
            Manifestacija selekt2 = (Manifestacija)MapaGrada.SelectedItem;

            if ((selekt1 != null && selekt2 != null) || lista.SelectedItems.Count > 1 || MapaGrada.SelectedItems.Count > 1)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Cannot edit multiple items. Please select only one.", "Error", System.Windows.MessageBoxButton.OK);
            }
            else
            {
                if (selekt1 != null)
                {
                    ManifestacijaWindow dv = new ManifestacijaWindow(this, true, selekt1);     //true jer se edituje
                    dv.ShowDialog();
                }
                else if (selekt2 != null)
                {
                    ManifestacijaWindow dv = new ManifestacijaWindow(this, true, selekt2);     //true jer se edituje
                    dv.ShowDialog();
                }
            }

            saveMapEvents(aktivnaMapa);
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if ((lista.SelectedItems.Count + MapaGrada.SelectedItems.Count) > 1)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to delete all the selected events?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Manifestacija selekt = (Manifestacija)MapaGrada.SelectedItem;
                    if (selekt != null)
                        ManifestacijeNaMapi.Remove(selekt);
                    foreach (Manifestacija m in lista.SelectedItems)
                    {
                        ListaManifestacija.Manifestacije.Remove(m.ID);
                    }
                    this.setManifestacijeItems();
                    saveMapEvents(aktivnaMapa);
                    return;
                }
                else
                {
                    return;
                }
            }
            MessageBoxResult messageBoxResult1 = System.Windows.MessageBox.Show("Are you sure you want to delete selected event?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult1 == MessageBoxResult.Yes)
            {
                if (lista.SelectedItems.Count > 0)
                {
                    Manifestacija selekt = (Manifestacija)lista.SelectedItem;
                    ListaManifestacija.Manifestacije.Remove(selekt.ID);
                    foreach (Etiketa etiketa in selekt.Etikete)
                    {
                        ListaEtiketa.Etikete.Remove(etiketa.ID);
                    }
                    this.setManifestacijeItems();
                }
                if (MapaGrada.SelectedItems.Count > 0)
                {
                    Manifestacija selekt = (Manifestacija)MapaGrada.SelectedItem;
                    ManifestacijeNaMapi.Remove(selekt);
                    foreach (Etiketa etiketa in selekt.Etikete)
                    {
                        ListaEtiketa.Etikete.Remove(etiketa.ID);
                    }
                    saveMapEvents(aktivnaMapa);
                }
            }
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

        private void Load()
        {
            bool nadjeno = true;
            StreamReader sr = null;
            OpenFileDialog openFile = null;
            string line;

            while (nadjeno)
            {
                openFile = new OpenFileDialog();
                openFile.Title = "Open Text File";
                openFile.Filter = "TXT files|*.txt";
                if (openFile.ShowDialog() == true)
                {
                    try
                    {
                        sr = new StreamReader(openFile.FileName);

                    }
                    catch (SecurityException ex)
                    {
                        MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                        $"Details:\n\n{ex.StackTrace}");
                    }
                }


                string line1 = File.ReadLines(openFile.FileName).First();
                if (!(line1.Equals("MAP-EVENTS")))
                {
                MessageBoxResult result = MessageBox.Show("Not Supported File");
                }else
                {
                nadjeno = false;
                }

            }

            sr.ReadLine();
            int i = 0;
            ListaEtiketa.Etikete = new Dictionary<string, Etiketa>();
            ListaTipManifestacijecs.TipoviManifestacija = new Dictionary<string, TipManifestacije>();
            ListaManifestacija.Manifestacije = new Dictionary<string, Manifestacija>();
            initMapLists();
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine("\n"+line+"\n");
                while (line == "")
                {
                    i++;
                    if (sr.ReadLine() != null && i < 7)
                    {
                        line = sr.ReadLine();
                    }
                    else if (sr.EndOfStream)
                        return;
                }

                Console.WriteLine("LINE\n" + line + "\nWITH I " + i);

                if (i == 0)
                {
                    string[] et = line.Split('|');
                    Color color = (Color)ColorConverter.ConvertFromString(et[2]);
                    Etiketa novaE = new Etiketa(et[0], color, et[1], new SolidColorBrush(color));
                    ListaEtiketa.Etikete.Add(et[0], novaE);

                }
                else if (i == 1)
                {
                    string[] et = line.Split('|');
                    ImageSource slika = null;
                    slika = new BitmapImage(new Uri(et[3]));
                    TipManifestacije tipM = new TipManifestacije(et[0], et[1], et[2], slika);
                    ListaTipManifestacijecs.TipoviManifestacija.Add(et[0], tipM);
                }
                else if (i == 2)
                {
                    bool a = true;
                    Manifestacija m = null;
                    //line = sr.ReadLine();
                    while (a)
                    {
                        if (line == null)
                        {
                            ListaManifestacija.Manifestacije.Add(m.ID, m);
                            return;
                        }
                        string[] manif = line.Split('|');

                        ImageSource slika = null;
                        slika = new BitmapImage(new Uri(manif[10]));
                        TipManifestacije tipM = ListaTipManifestacijecs.TipoviManifestacija[manif[11]];
                        string[] koord = manif[15].Split(',');
                        Point tacka = new Point(Double.Parse(koord[0]), Double.Parse(koord[1]));
                        m = new Manifestacija(manif[0], manif[1], manif[2], manif[3], manif[4], Boolean.Parse(manif[5]), Boolean.Parse(manif[6]), Boolean.Parse(manif[7]), int.Parse(manif[8]), manif[9], slika,
                            tipM, tacka);
                        m.Etikete = new List<Etiketa>();
                        while ((line = sr.ReadLine())!=null)
                        {
                            if ((line.StartsWith(manif[0] + "#E")))
                            {
                                string[] l1 = line.Split('|');
                                Etiketa et1 = ListaEtiketa.Etikete[l1[1]];
                                m.Etikete.Add(et1);
                            }
                            else
                            {
                                ListaManifestacija.Manifestacije.Add(m.ID, m);
                                break;
                            }
                        }
                    }
                }
                else if (i == 3)
                {
                    bool a = true;
                    Manifestacija m = null;
                    //line = sr.ReadLine();
                    while (a)
                    {
                        if (line == null)
                        {
                            ListaManifestacija.SacuvaneNaMapi1.Add(m);
                            return;
                        }
                        string[] manif = line.Split('|');
                        Console.WriteLine("FIRST MAP W STRING\n" + line);

                        ImageSource slika = null;
                        slika = new BitmapImage(new Uri(manif[10]));
                        TipManifestacije tipM = ListaTipManifestacijecs.TipoviManifestacija[manif[11]];
                        string[] koord = manif[15].Split(',');
                        Point tacka = new Point(Double.Parse(koord[0]), Double.Parse(koord[1]));
                        m = new Manifestacija(manif[0], manif[1], manif[2], manif[3], manif[4], Boolean.Parse(manif[5]), Boolean.Parse(manif[6]), Boolean.Parse(manif[7]), int.Parse(manif[8]), manif[9], slika,
                            tipM, tacka);
                        m.Etikete = new List<Etiketa>();
                        while ((line = sr.ReadLine()) != "")
                        {
                            Console.WriteLine("FIRST MAP W ETIKETE\n" + line);
                            if ((line.StartsWith(manif[0] + "#E")))
                            {
                                Console.WriteLine("FIRST MAP AT ETIKETA");
                                string[] l1 = line.Split('|');
                                Etiketa et1 = ListaEtiketa.Etikete[l1[1]];
                                m.Etikete.Add(et1);
                            }
                            else
                            {
                                Console.WriteLine("FIRST MAP NO ETIKETA");
                                ListaManifestacija.SacuvaneNaMapi1.Add(m);
                                break;
                            }
                        }
                        if (line == "")
                        {
                            i++;
                            line = sr.ReadLine();
                            break;
                        }
                    }
                }
                else if (i == 4)
                {
                    bool a = true;
                    Manifestacija m = null;
                    //line = sr.ReadLine();
                    while (a)
                    {
                        if (line == null)
                        {
                            ListaManifestacija.SacuvaneNaMapi2.Add(m);
                            return;
                        }
                        string[] manif = line.Split('|');
                        Console.WriteLine("SECOND MAP W STRING\n" + line);

                        ImageSource slika = null;
                        slika = new BitmapImage(new Uri(manif[10]));
                        TipManifestacije tipM = ListaTipManifestacijecs.TipoviManifestacija[manif[11]];
                        string[] koord = manif[15].Split(',');
                        Point tacka = new Point(Double.Parse(koord[0]), Double.Parse(koord[1]));
                        m = new Manifestacija(manif[0], manif[1], manif[2], manif[3], manif[4], Boolean.Parse(manif[5]), Boolean.Parse(manif[6]), Boolean.Parse(manif[7]), int.Parse(manif[8]), manif[9], slika,
                            tipM, tacka);
                        m.Etikete = new List<Etiketa>();
                        while ((line = sr.ReadLine()) != "")
                        {
                            Console.WriteLine("SECOND MAP W ETIKETE\n" + line);
                            if ((line.StartsWith(manif[0] + "#E")))
                            {
                                Console.WriteLine("SECOND MAP AT ETIKETA");
                                string[] l1 = line.Split('|');
                                Etiketa et1 = ListaEtiketa.Etikete[l1[1]];
                                m.Etikete.Add(et1);
                            }
                            else
                            {
                                Console.WriteLine("SECOND MAP NO ETIKETA");
                                ListaManifestacija.SacuvaneNaMapi2.Add(m);
                                break;
                            }
                        }
                        if (line == "")
                        {
                            i++;
                            line = sr.ReadLine();
                            break;
                        }
                    }
                }
                else if (i == 5)
                {
                    bool a = true;
                    Manifestacija m = null;
                    //line = sr.ReadLine();
                    while (a)
                    {
                        if (line == null)
                        {
                            ListaManifestacija.SacuvaneNaMapi3.Add(m);
                            return;
                        }
                        string[] manif = line.Split('|');
                        Console.WriteLine("THIRD MAP W STRING\n" + line);

                        ImageSource slika = null;
                        slika = new BitmapImage(new Uri(manif[10]));
                        TipManifestacije tipM = ListaTipManifestacijecs.TipoviManifestacija[manif[11]];
                        string[] koord = manif[15].Split(',');
                        Point tacka = new Point(Double.Parse(koord[0]), Double.Parse(koord[1]));
                        m = new Manifestacija(manif[0], manif[1], manif[2], manif[3], manif[4], Boolean.Parse(manif[5]), Boolean.Parse(manif[6]), Boolean.Parse(manif[7]), int.Parse(manif[8]), manif[9], slika,
                            tipM, tacka);
                        m.Etikete = new List<Etiketa>();
                        while ((line = sr.ReadLine()) != "")
                        {
                            Console.WriteLine("THIRD MAP W ETIKETE\n" + line);
                            if ((line.StartsWith(manif[0] + "#E")))
                            {
                                Console.WriteLine("THIRD MAP AT ETIKETA");
                                string[] l1 = line.Split('|');
                                Etiketa et1 = ListaEtiketa.Etikete[l1[1]];
                                m.Etikete.Add(et1);
                            }
                            else
                            {
                                Console.WriteLine("THIRD MAP NO ETIKETA");
                                ListaManifestacija.SacuvaneNaMapi3.Add(m);
                                break;
                            }
                        }
                        if (line == "")
                        {
                            i++;
                            line = sr.ReadLine();
                            break;
                        }
                    }
                }
                else if (i == 6)
                {
                    bool a = true;
                    Manifestacija m = null;
                    //line = sr.ReadLine();
                    while (a)
                    {
                        if (line == null)
                        {
                            ListaManifestacija.SacuvaneNaMapi4.Add(m);
                            return;
                        }
                        string[] manif = line.Split('|');

                        ImageSource slika = null;
                        slika = new BitmapImage(new Uri(manif[10]));
                        TipManifestacije tipM = ListaTipManifestacijecs.TipoviManifestacija[manif[11]];
                        string[] koord = manif[15].Split(',');
                        Point tacka = new Point(Double.Parse(koord[0]), Double.Parse(koord[1]));
                        m = new Manifestacija(manif[0], manif[1], manif[2], manif[3], manif[4], Boolean.Parse(manif[5]), Boolean.Parse(manif[6]), Boolean.Parse(manif[7]), int.Parse(manif[8]), manif[9], slika,
                            tipM, tacka);
                        m.Etikete = new List<Etiketa>();
                        while ((line = sr.ReadLine()) != "")
                        {
                            if ((line.StartsWith(manif[0] + "#E")))
                            {
                                string[] l1 = line.Split('|');
                                Etiketa et1 = ListaEtiketa.Etikete[l1[1]];
                                m.Etikete.Add(et1);
                            }
                            else
                            {
                                ListaManifestacija.SacuvaneNaMapi4.Add(m);
                                break;
                            }
                        }
                        if (line == "")
                        {
                            i++;
                            line = sr.ReadLine();
                            break;
                        }
                    }
                }



            }

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "unknown.txt";
            savefile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            savefile.DefaultExt = "txt";



            if (savefile.ShowDialog() == true)
            {
                FileName = savefile.FileName;
                FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate);
                using (fs)
                {
                    using (TextWriter tw = new StreamWriter(fs))
                    {
                        tw.WriteLine("MAP-EVENTS");
                        Save_etikete(tw, FileName);
                        tw.WriteLine("\n");
                        Save_TipoviManifestacija(tw, FileName);
                        tw.WriteLine("\n");
                        Save_Manifestacije(tw, FileName);
                        
                    }
                }
            }

        }

        private void Save_etikete(TextWriter tw, string file)
        {
            foreach (KeyValuePair<String, Etiketa> kvp in ListaEtiketa.Etikete)
            {
                
                tw.WriteLine(string.Format("{0}|{1}", kvp.Key, kvp.Value));
            }
        }

        private void Save_TipoviManifestacija(TextWriter tw, string file)
        {
            foreach (KeyValuePair<string, TipManifestacije> kvp in ListaTipManifestacijecs.TipoviManifestacija)
            {
                tw.WriteLine(string.Format("{0}", kvp.Value));          
            }
        }
        
        private void Save_Manifestacije(TextWriter tw, string file)
        {
            foreach (KeyValuePair<string, Manifestacija> kvp in ListaManifestacija.Manifestacije)
            {
                tw.WriteLine(string.Format("{0}|{1}",kvp.Key, kvp.Value)); 
                foreach (Etiketa etiketa in kvp.Value.Etikete)
                {
                    tw.WriteLine(string.Format("{0}", kvp.Key + "#E|" + etiketa.ID));
                }
            }
            tw.WriteLine("\n");
            foreach (Manifestacija m in ListaManifestacija.SacuvaneNaMapi1)
            {
                tw.WriteLine(string.Format("{0}|{1}", m.ID, m));
                foreach (Etiketa etiketa in m.Etikete)
                {
                    tw.WriteLine(string.Format("{0}", m.ID + "#E|" + etiketa.ID));
                }
            }
            tw.WriteLine("\n");
            foreach (Manifestacija m in ListaManifestacija.SacuvaneNaMapi2)
            {
                tw.WriteLine(string.Format("{0}|{1}", m.ID, m));
                foreach (Etiketa etiketa in m.Etikete)
                {
                    tw.WriteLine(string.Format("{0}", m.ID + "#E|" + etiketa.ID));
                }
            }
            tw.WriteLine("\n");
            foreach (Manifestacija m in ListaManifestacija.SacuvaneNaMapi3)
            {
                tw.WriteLine(string.Format("{0}|{1}", m.ID, m));
                foreach (Etiketa etiketa in m.Etikete)
                {
                    tw.WriteLine(string.Format("{0}", m.ID + "#E|" + etiketa.ID));
                }
            }
            tw.WriteLine("\n");
            foreach (Manifestacija m in ListaManifestacija.SacuvaneNaMapi4)
            {
                tw.WriteLine(string.Format("{0}|{1}", m.ID, m));
                foreach (Etiketa etiketa in m.Etikete)
                {
                    tw.WriteLine(string.Format("{0}", m.ID + "#E|" + etiketa.ID));
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
                SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#F1DED7"));
                this.txtPRETRAGA.Foreground = color;
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
                SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#F1DED7"));
                this.txtPRETRAGA.Foreground = color;
                txtPRETRAGA.FontStyle = FontStyles.Normal;
            }
        }

        private void txtPRETRAGA_Leave(object sender, RoutedEventArgs e)
        {
            if (txtPRETRAGA.Text == "" && this.Search_Button.IsMouseOver == false)
            {
                this.txtPRETRAGA.Text = "Search...";
                SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#F1DED7"));
                this.txtPRETRAGA.Foreground = color;
                txtPRETRAGA.FontStyle = FontStyles.Oblique;
            }
        }

        private void Search_Button_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPRETRAGA.IsMouseOver == false)
            {
                this.txtPRETRAGA.Text = "Search...";
                SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#F1DED7"));
                this.txtPRETRAGA.Foreground = color;
                txtPRETRAGA.FontStyle = FontStyles.Oblique;
            }
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<ObservableCollection<Manifestacija>> manifNaMapi = new ObservableCollection<ObservableCollection<Manifestacija>>();
            Filter filter = new Filter(this);
            filter.ShowDialog();
            //this.lista.ItemsSource = filter.getFiltrirano();

        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            setManifestacijeItems();
            if (aktivnaMapa == 1)
            {
                MapaGrada.ItemsSource = ListaManifestacija.SacuvaneNaMapi1;
            } else if (aktivnaMapa == 2)
            {
                MapaGrada.ItemsSource = ListaManifestacija.SacuvaneNaMapi2;
            }else if (aktivnaMapa == 3)
            {
                MapaGrada.ItemsSource = ListaManifestacija.SacuvaneNaMapi3;
            }
            else if (aktivnaMapa == 4)
            {
                MapaGrada.ItemsSource = ListaManifestacija.SacuvaneNaMapi4;
            }
        }

        private void DemoStart_Click(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
           
            if (mePlayer.Source != null)
            {
                if (mePlayer.Visibility.Equals(Visibility.Collapsed))
                {
                    mePlayer.Visibility = Visibility.Visible;

                    mePlayer.Play();
                    Cursor = Cursors.None;
                    timer.Start();
                }
                else
                {
                    mePlayer.Visibility = Visibility.Collapsed;
                    mePlayer.Stop();
                    Cursor = Cursors.Arrow;
                    timer.Stop();
                }
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (mePlayer.Source != null)
            {
                Console.WriteLine("{0}", mePlayer.Position.ToString(@"mm\:ss"));
                if (mePlayer.Position.ToString(@"mm\:ss").Equals(mePlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss")))
                {
                    mePlayer.Visibility = Visibility.Collapsed;
                    mePlayer.Stop();
                    Cursor = Cursors.Arrow;
                    timer.Stop();
                }
            }
        }

        #region Drag & Drop

        private void Mapa_Grada_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void Lista_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void Lista_OnItemSelected(object sender, RoutedEventArgs e)
        {

            lista.Tag = e.OriginalSource;
        }

        private void Lista_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("manifestacijaMapa"))
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Lista_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("manifestacijaMapa"))
            {
                Manifestacija man = e.Data.GetData("manifestacijaMapa") as Manifestacija;
                Console.WriteLine(man.Tacka);
                man.Tacka = e.GetPosition(MapaGrada);
               

                ListaManifestacija.Manifestacije.Add(man.ID, man);
                this.setManifestacijeItems();
                ManifestacijeNaMapi.Remove(man);
            }
        }

        private void Lista_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(null);
            Vector diff = startPoint - position;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance) &&
                lista.SelectedItem is Manifestacija)
            {
                Manifestacija selectedMan = (Manifestacija)lista.SelectedItem;
                ListViewItem tvi = lista.Tag as ListViewItem;

                DataObject dragData = new DataObject("manifestacija", selectedMan);
                DragDrop.DoDragDrop(tvi, dragData, DragDropEffects.Move);
            }
        }

        private void Mapa_Grada_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("manifestacija") && !e.Data.GetDataPresent("manifestacijaMapa"))
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Mapa_Grada_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("manifestacija") || e.Data.GetDataPresent("manifestacijaMapa"))
            {
                Manifestacija man = e.Data.GetDataPresent("manifestacija") ? e.Data.GetData("manifestacija") as Manifestacija :
                    e.Data.GetData("manifestacijaMapa") as Manifestacija;
                Console.WriteLine(man.Tacka);
                
                man.Tacka = e.GetPosition(MapaGrada);
                Canvas canvas = (Canvas)MapaGrada.Template.FindName("CanvasPanel", MapaGrada);
                if (canvas != null)
                    man.Tacka = e.GetPosition(canvas);
                
                if (!ManifestacijeNaMapi.Contains(man))
                {
                    ListaManifestacija.Manifestacije.Remove(man.ID);
                    this.setManifestacijeItems();
                    ManifestacijeNaMapi.Add(man);
                }
                else
                {
                    Console.WriteLine(man.Tacka);
                    ManifestacijeNaMapi.Remove(man);
                    ManifestacijeNaMapi.Add(man);
                }
            }
        }

        private void Mapa_Grada_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(null);
            Vector diff = startPoint - position;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance) &&
                MapaGrada.SelectedItem is Manifestacija)    
            {
                Manifestacija selectedItem = (Manifestacija)MapaGrada.SelectedItem;
                ListBoxItem listBoxItem = (ListBoxItem)MapaGrada.ItemContainerGenerator.ContainerFromItem(selectedItem);
                
                DataObject dragData = new DataObject("manifestacijaMapa", selectedItem);
                DragDrop.DoDragDrop(listBoxItem, dragData, DragDropEffects.Move);
            }
        }
        

        private void Mapa1_Click(object sender, RoutedEventArgs e)
        {
            Canvas canvas = (Canvas)MapaGrada.Template.FindName("CanvasPanel", MapaGrada);
            var uriSource = new Uri("../../images/GlavniProzor/MapaNS.png", UriKind.Relative);
            canvas.Background = new ImageBrush(new BitmapImage(uriSource));
            Refresh_Click(sender, e);
            saveMapEvents(aktivnaMapa);
            aktivnaMapa = 1;
            loadMapEvents(aktivnaMapa);
        }

        private void Mapa2_Click(object sender, RoutedEventArgs e)
        {
            Canvas canvas = (Canvas)MapaGrada.Template.FindName("CanvasPanel", MapaGrada);
            var uriSource = new Uri("../../images/GlavniProzor/MapaUE.png", UriKind.Relative);
            canvas.Background = new ImageBrush(new BitmapImage(uriSource));
            Refresh_Click(sender, e);
            saveMapEvents(aktivnaMapa);
            aktivnaMapa = 2;
            loadMapEvents(aktivnaMapa);
        }

        private void Mapa3_Click(object sender, RoutedEventArgs e)
        {
            Canvas canvas = (Canvas)MapaGrada.Template.FindName("CanvasPanel", MapaGrada);
            var uriSource = new Uri("../../images/GlavniProzor/MapaSU.png", UriKind.Relative);
            canvas.Background = new ImageBrush(new BitmapImage(uriSource));
            Refresh_Click(sender, e);
            saveMapEvents(aktivnaMapa);
            aktivnaMapa = 3;
            loadMapEvents(aktivnaMapa);
        }

        private void Mapa4_Click(object sender, RoutedEventArgs e)
        {
            Canvas canvas = (Canvas)MapaGrada.Template.FindName("CanvasPanel", MapaGrada);
            var uriSource = new Uri("../../images/GlavniProzor/MapaBG.png", UriKind.Relative);
            canvas.Background = new ImageBrush(new BitmapImage(uriSource));
            Refresh_Click(sender, e);
            saveMapEvents(aktivnaMapa);
            aktivnaMapa = 4;
            loadMapEvents(aktivnaMapa);
        }

        private void saveMapEvents(int map)
        {
            switch (map)
            {
                case 1:
                    ListaManifestacija.SacuvaneNaMapi1 = ManifestacijeNaMapi;
                    break;
                case 2:
                    ListaManifestacija.SacuvaneNaMapi2 = ManifestacijeNaMapi;
                    break;
                case 3:
                    ListaManifestacija.SacuvaneNaMapi3 = ManifestacijeNaMapi;
                    break;
                case 4:
                    ListaManifestacija.SacuvaneNaMapi4 = ManifestacijeNaMapi;
                    break;
                default:
                    throw new Exception("Trying to acces an out of bounds map. Only 4 exist.");
            }
        }

        private void loadMapEvents(int map)
        {
            switch (map)
            {
                case 1:
                    ManifestacijeNaMapi = ListaManifestacija.SacuvaneNaMapi1;
                    break;
                case 2:
                    ManifestacijeNaMapi = ListaManifestacija.SacuvaneNaMapi2;
                    break;
                case 3:
                    ManifestacijeNaMapi = ListaManifestacija.SacuvaneNaMapi3;
                    break;
                case 4:
                    ManifestacijeNaMapi = ListaManifestacija.SacuvaneNaMapi4;
                    break;
                default:
                    throw new Exception("Trying to acces an out of bounds map. Only 4 exist.");
            }
            MapaGrada.ItemsSource = ManifestacijeNaMapi;
        }

        private void initMapLists()
        {
            ManifestacijeNaMapi = new ObservableCollection<Manifestacija>();
            ListaManifestacija.SacuvaneNaMapi1 = new ObservableCollection<Manifestacija>();
            ListaManifestacija.SacuvaneNaMapi2 = new ObservableCollection<Manifestacija>();
            ListaManifestacija.SacuvaneNaMapi3 = new ObservableCollection<Manifestacija>();
            ListaManifestacija.SacuvaneNaMapi4 = new ObservableCollection<Manifestacija>();
            ListaManifestacija.FilterSacuvaneNaMapi1 = new ObservableCollection<Manifestacija>();
            ListaManifestacija.FilterSacuvaneNaMapi2 = new ObservableCollection<Manifestacija>();
            ListaManifestacija.FilterSacuvaneNaMapi3 = new ObservableCollection<Manifestacija>();
            ListaManifestacija.FilterSacuvaneNaMapi4 = new ObservableCollection<Manifestacija>();
        }


        #endregion

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            HelpProvider.ShowHelp(this);
        }
    }
}
