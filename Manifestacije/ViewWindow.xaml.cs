using Manifestacije.Modeli;
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
    /// Interaction logic for ViewWindow.xaml
    /// </summary>
    public partial class ViewWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public ObservableCollection<Manifestacija> Manifestacije
        {
            get;
            set;
        }
        public ObservableCollection<TipManifestacije> TipoviManifestacija
        {
            get;
            set;
        }
        public ObservableCollection<Etiketa> Etikete
        {
            get;
            set;
        }

        public StackPanel trenutniPanel;

        public MainWindow ParentWindow;

        public ViewWindow(MainWindow par)
        {
            InitializeComponent();
            this.DataContext = this;

            ParentWindow = par;

            Manifestacije = new ObservableCollection<Manifestacija>(ListaManifestacija.Manifestacije.Values);
            TipoviManifestacija = new ObservableCollection<TipManifestacije>(ListaTipManifestacijecs.TipoviManifestacija.Values);
            Etikete = new ObservableCollection<Etiketa>(ListaEtiketa.Etikete.Values);
            trenutniPanel = pnlVrste;
        }

        public void popuniManifestacije()
        {
            this.Manifestacije = null;
            ObservableCollection<Manifestacija> manif = new ObservableCollection<Manifestacija>(ListaManifestacija.Manifestacije.Values);
            this.Manifestacije = manif;
            tabelaVrste.ItemsSource = Manifestacije;
        }

        public void popuniEtikete()
        {
            this.Etikete = null;
            ObservableCollection<Etiketa> etikete = new ObservableCollection<Etiketa>(ListaEtiketa.Etikete.Values);
            this.Etikete = etikete;
            tabelaEtikete.ItemsSource = Etikete;
        }

        public void popuniTipoveManifestacija()
        {
            this.TipoviManifestacija = null;
            ObservableCollection<TipManifestacije> tipoviManif = new ObservableCollection<TipManifestacije>(ListaTipManifestacijecs.TipoviManifestacija.Values);
            this.TipoviManifestacija = tipoviManif;
            tabelaTipoviVrsta.ItemsSource = TipoviManifestacija;
        }

        public void dodajManifestaciju(Manifestacija v)
        {
            Manifestacije.Add(v);
        }

        public void dodajEtiketu(Etiketa e)
        {
            Etikete.Add(e);
        }

        public void dodajTipManifestacije(TipManifestacije tv)
        {
            TipoviManifestacija.Add(tv);
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.setManifestacijeItems();
            this.Close();
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            // Begin dragging the window
            this.DragMove();
        }

        private void Vrste_Click(object sender, RoutedEventArgs e)
        {
            trenutniPanel.Visibility = Visibility.Hidden;
            trenutniPanel = pnlVrste;
            pnlVrste.Visibility = Visibility.Visible;

        }

        private void TipoviVrsta_Click(object sender, RoutedEventArgs e)
        {
            trenutniPanel.Visibility = Visibility.Hidden;
            trenutniPanel = pnlTipoviVrsta;
            pnlTipoviVrsta.Visibility = Visibility.Visible;
        }

        private void Etikete_Click(object sender, RoutedEventArgs e)
        {
            trenutniPanel.Visibility = Visibility.Hidden;
            trenutniPanel = pnlEtikete;
            pnlEtikete.Visibility = Visibility.Visible;
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (trenutniPanel == pnlVrste)
            {
                ManifestacijaWindow rsv = new ManifestacijaWindow(this, false, null);
                rsv.ShowDialog();
            }
            else if (trenutniPanel == pnlEtikete)
            {
                EtiketaWindow rse = new EtiketaWindow(this, false, null);
                rse.ShowDialog();
            }
            else if (trenutniPanel == pnlTipoviVrsta)
            {
                TipManifestacijeWindow rstv = new TipManifestacijeWindow(this, false, null);
                rstv.ShowDialog();
            }
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {


            if (trenutniPanel == pnlVrste)
            {
                if ((Manifestacija)tabelaVrste.SelectedItem == null)
                {
                    MessageBox.Show("You must choose event from the list!");
                    return;
                }
                String selektovanID = ((Manifestacija)tabelaVrste.SelectedItem).ID;
                ListaManifestacija.Manifestacije.Remove(selektovanID);
                popuniManifestacije();
            }
            else if (trenutniPanel == pnlEtikete)
            {
                if ((Etiketa)tabelaEtikete.SelectedItem == null)
                {
                    MessageBox.Show("You must choose label from the list!");
                    return;
                }
                String selektovanaID = ((Etiketa)tabelaEtikete.SelectedItem).ID;
                String message = "Are you sure?";
                MessageBoxResult mbr = MessageBox.Show(message, "Delete", MessageBoxButton.YesNo);

                if (mbr == MessageBoxResult.Yes)
                {
                    ListaEtiketa.Etikete.Remove(selektovanaID);


                    //kad se obrise iz tabele, brise se i iz liste etiketa kod manif
                    foreach (KeyValuePair<String, Manifestacija> pair in ListaManifestacija.Manifestacije)
                    {
                        foreach (Etiketa etiketa in pair.Value.Etikete)
                        {
                            if (etiketa.ID == selektovanaID)
                            {
                                pair.Value.Etikete.Remove(etiketa);
                                break;
                            }
                        }
                    }

                    popuniEtikete();
                }
                else
                {
                    return;
                }

            }
            else if (trenutniPanel == pnlTipoviVrsta)
            {
                if ((TipManifestacije)tabelaTipoviVrsta.SelectedItem == null)
                {
                    MessageBox.Show("You must choose event type from the list!");
                    return;
                }
                String selektovanID = ((TipManifestacije)tabelaTipoviVrsta.SelectedItem).ID;
                ListaTipManifestacijecs.TipoviManifestacija.Remove(selektovanID);
                popuniTipoveManifestacija();
            }
            else if (trenutniPanel == pnlTipoviVrsta)
            {
                String selektovanID = ((TipManifestacije)tabelaTipoviVrsta.SelectedItem).ID;
                ListaTipManifestacijecs.TipoviManifestacija.Remove(selektovanID);
                popuniTipoveManifestacija();
            }
        }

        private void Izmeni_Click(object sender, RoutedEventArgs e)
        {
            if (trenutniPanel == pnlVrste)
            {
                if ((Manifestacija)tabelaVrste.SelectedItem == null)
                {
                    MessageBox.Show("You must choose event from the list!");
                    return;
                }
                ManifestacijaWindow rsv = new ManifestacijaWindow(this, true, (Manifestacija)tabelaVrste.SelectedItem);
                rsv.ShowDialog();
            }
            else if (trenutniPanel == pnlEtikete)
            {
                if ((Etiketa)tabelaEtikete.SelectedItem == null)
                {
                    MessageBox.Show("You must choose label from the list!");
                    return;
                }
                EtiketaWindow rse = new EtiketaWindow(this, true, (Etiketa)tabelaEtikete.SelectedItem);
                rse.ShowDialog();
            }
            else if (trenutniPanel == pnlTipoviVrsta)
            {
                if ((TipManifestacije)tabelaTipoviVrsta.SelectedItem == null)
                {
                    MessageBox.Show("You must choose event type from the list!");
                    return;
                }
                TipManifestacijeWindow rstv = new TipManifestacijeWindow(this, true, (TipManifestacije)tabelaTipoviVrsta.SelectedItem);
                rstv.ShowDialog();
            }
        }

        private void TabelaVrste_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Etikete")
            {
                e.Column = null;
            }
        }

        private void TabelaEtikete_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "BojaBrush")
            {
                e.Column = null;
            }
            else if (e.PropertyName == "Boja")
            {
                Style cellStyle = new Style(typeof(DataGridCell));
                cellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new Binding("BojaBrush")));
                cellStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, new SolidColorBrush(Colors.Transparent)));
                e.Column.CellStyle = cellStyle;
            }

        }
    }
}
