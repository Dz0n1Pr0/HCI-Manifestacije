using Manifestacije.Modeli;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
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

        public ObservableCollection<Manifestacija> Manifestacije { get; set; }
        public ObservableCollection<TipManifestacije> TipoviManifestacija { get; set; }
        public ObservableCollection<Etiketa> Etikete { get; set; }

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
            trenutniPanel = pnlManif;

        }


        public void popuniManifestacije()
        {
            this.Manifestacije = null;
            ObservableCollection<Manifestacija> manif = new ObservableCollection<Manifestacija>(ListaManifestacija.Manifestacije.Values);
            this.Manifestacije = manif;
            tabelaManif.ItemsSource = Manifestacije;

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
            tabelaTipoviManif.ItemsSource = TipoviManifestacija;
        }

        public void dodajManifestaciju(Manifestacija v)
        {
            Manifestacije.Add(v);
        }

        public void dodajEtiketu(Etiketa e)
        {
            Etikete.Add(e);
        }

        public void dodajTipManifestacije(TipManifestacije tm)
        {
            TipoviManifestacija.Add(tm);
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

        private void Manifestacije_Click(object sender, RoutedEventArgs e)
        {
            trenutniPanel.Visibility = Visibility.Hidden;
            trenutniPanel = pnlManif;
            pnlManif.Visibility = Visibility.Visible;
            if (tabelaManif.SelectedItems.Count > 1)
            {
                Edit_btn.IsEnabled = false;
            }
            else if (tabelaManif.SelectedItems.Count == 0)
            {
                Edit_btn.IsEnabled = false;
                Del_btn.IsEnabled = false;
            }
            else
            {
                Edit_btn.IsEnabled = true;
                Del_btn.IsEnabled = true;
            }

        }

        private void TipoviManif_Click(object sender, RoutedEventArgs e)
        {
            trenutniPanel.Visibility = Visibility.Hidden;
            trenutniPanel = pnlTipoviManif;
            pnlTipoviManif.Visibility = Visibility.Visible;
            if (tabelaTipoviManif.SelectedItems.Count > 1)
            {
                Edit_btn.IsEnabled = false;
            }
            else if (tabelaTipoviManif.SelectedItems.Count == 0)
            {
                Edit_btn.IsEnabled = false;
                Del_btn.IsEnabled = false;
            }
            else
            {
                Edit_btn.IsEnabled = true;
                Del_btn.IsEnabled = true;
            }
        }

        private void Etikete_Click(object sender, RoutedEventArgs e)
        {
            trenutniPanel.Visibility = Visibility.Hidden;
            trenutniPanel = pnlEtikete;
            pnlEtikete.Visibility = Visibility.Visible;
            if (tabelaEtikete.SelectedItems.Count > 1)
            {
                Edit_btn.IsEnabled = false;
            }else if (tabelaEtikete.SelectedItems.Count == 0)
            {
                Edit_btn.IsEnabled = false;
                Del_btn.IsEnabled = false;
            }
            else
            {
                Edit_btn.IsEnabled = true;
                Del_btn.IsEnabled = true;
            }
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (trenutniPanel == pnlManif)
            {
                if (ListaTipManifestacijecs.TipoviManifestacija.Count == 0)
                {
                    MessageBoxResult mbr = MessageBox.Show("There is no event type. Add event type for enabling this option.", "No event type");
                }
                else
                {
                    ManifestacijaWindow rsv = new ManifestacijaWindow(this, false, null);
                    rsv.ShowDialog();
                }
            }
            else if (trenutniPanel == pnlEtikete)
            {
                EtiketaWindow rse = new EtiketaWindow(this, false, null);
                rse.ShowDialog();
            }
            else if (trenutniPanel == pnlTipoviManif)
            {
                TipManifestacijeWindow rstv = new TipManifestacijeWindow(this, false, null);
                rstv.ShowDialog();
            }
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {


            if (trenutniPanel == pnlManif)
            {
                if (tabelaManif.SelectedItems.Count > 1)
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to delete all the selected events?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        foreach (Manifestacija m in tabelaManif.SelectedItems)
                        {
                            ListaManifestacija.Manifestacije.Remove(m.ID);
                        }
                        popuniManifestacije();
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
                    String selektovanID = ((Manifestacija)tabelaManif.SelectedItem).ID;
                    ListaManifestacija.Manifestacije.Remove(selektovanID);
                    popuniManifestacije();
                }
            }
            else if (trenutniPanel == pnlEtikete)
            {
                if (tabelaEtikete.SelectedItems.Count > 1)
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to delete all the selected labels?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        foreach (Etiketa et in tabelaEtikete.SelectedItems)
                        {
                            ListaEtiketa.Etikete.Remove(et.ID);
                        }
                        popuniEtikete();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                
                String selektovanaID = ((Etiketa)tabelaEtikete.SelectedItem).ID;
                String message = "Are you sure you want to delete selected label?";
                MessageBoxResult mbr = MessageBox.Show(message, "Delete Confirmation", MessageBoxButton.YesNo);

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
            else if (trenutniPanel == pnlTipoviManif)
            {
                if (tabelaTipoviManif.SelectedItems.Count > 1)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete all the selected types?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        foreach (TipManifestacije tip in tabelaTipoviManif.SelectedItems)
                        {
                            foreach (Manifestacija m in ListaManifestacija.Manifestacije.Values)
                            {

                                DateTime d = DateTime.ParseExact(m.Datum, "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                                if (m.Tip.Equals(tip) && d.Ticks > DateTime.Now.Ticks)
                                {
                                    MessageBox.Show("Error: There are future events with type " + tip.Ime, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                                    return;
                                }
                            }
                            ListaTipManifestacijecs.TipoviManifestacija.Remove(tip.ID);
                        }
                        popuniTipoveManifestacija();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                String message = "Are you sure you want to delete selected type?";
                MessageBoxResult mbr = MessageBox.Show(message, "Delete Confirmation", MessageBoxButton.YesNo);

                if (mbr == MessageBoxResult.Yes)
                {
                    String selektovanID = ((TipManifestacije)tabelaTipoviManif.SelectedItem).ID;
                    String selektovanIme = ((TipManifestacije)tabelaTipoviManif.SelectedItem).Ime;
                    foreach (Manifestacija m in ListaManifestacija.Manifestacije.Values)
                    {
                        DateTime d = DateTime.ParseExact(m.Datum, "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                        if (m.Tip.Equals(selektovanIme) && d.Ticks > DateTime.Now.Ticks)
                        {
                            MessageBox.Show("Error: There are future events with type " + selektovanIme, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                    ListaTipManifestacijecs.TipoviManifestacija.Remove(selektovanID);
                    popuniTipoveManifestacija();
                }
            }

        }


        private void Refresh()
        {
            popuniTipoveManifestacija();
            popuniEtikete();
            popuniManifestacije();
        }

        private void Izmeni_Click(object sender, RoutedEventArgs e)
        {
            if (trenutniPanel == pnlManif)
            {
                ManifestacijaWindow rsv = new ManifestacijaWindow(this, true, (Manifestacija)tabelaManif.SelectedItem);
                rsv.ShowDialog();
                Refresh();
            }
            else if (trenutniPanel == pnlEtikete)
            {
                EtiketaWindow rse = new EtiketaWindow(this, true, (Etiketa)tabelaEtikete.SelectedItem);
                rse.ShowDialog();
                Refresh();
            }
            else if (trenutniPanel == pnlTipoviManif)
            {
                TipManifestacijeWindow rstv = new TipManifestacijeWindow(this, true, (TipManifestacije)tabelaTipoviManif.SelectedItem);
                rstv.ShowDialog();
                Refresh();
            }
        }


        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

            if (e.PropertyName == "Etikete")
            {
                e.Column = null;
            }
            else if (e.PropertyName == "BojaBrush")
            {
                e.Column = null;
            }
            else if (e.PropertyName == "Tip")
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
            else if(e.PropertyName == "Tacka")
            {
                e.Column = null;
            }

            string displayName = GetPropertyDisplayName(e.PropertyDescriptor);
            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }
        }

        public static string GetPropertyDisplayName(object descriptor)
        {

            PropertyDescriptor pd = descriptor as PropertyDescriptor;
            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                DisplayNameAttribute displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }

            }
            else
            {
                PropertyInfo pi = descriptor as PropertyInfo;
                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        DisplayNameAttribute displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }
            return null;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (trenutniPanel == pnlManif)
            {
                Manifestacija selekt = (Manifestacija)tabelaManif.SelectedItem;
                var item = sender as DataGridRow;

                if (item != null && item.IsSelected)
                {
                    if (ListaManifestacija.Manifestacije.ContainsKey(selekt.ID))
                    {
                        ManifestacijaWindow rsv = new ManifestacijaWindow(this, true, selekt);
                        rsv.ShowDialog();
                        return;
                    }
                }
            }
            else if (trenutniPanel == pnlTipoviManif)
            {
                TipManifestacije selekt = (TipManifestacije)tabelaTipoviManif.SelectedItem;
                var item = sender as DataGridRow;

                if (item != null && item.IsSelected)
                {
                    if (ListaTipManifestacijecs.TipoviManifestacija.ContainsKey(selekt.ID))
                    {
                        TipManifestacijeWindow rsv = new TipManifestacijeWindow(this, true, selekt);
                        rsv.ShowDialog();
                        return;
                    }
                }
            }
            else if (trenutniPanel == pnlEtikete)
            {
                Etiketa selekt = (Etiketa)tabelaEtikete.SelectedItem;
                var item = sender as DataGridRow;

                if (item != null && item.IsSelected)
                {
                    if (ListaEtiketa.Etikete.ContainsKey(selekt.ID))
                    {
                        EtiketaWindow rsv = new EtiketaWindow(this, true, selekt);
                        rsv.ShowDialog();
                        return;
                    }
                }
            }

        }

        private void tabelaManif_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (tabelaManif.SelectedItems.Count > 1)
            {
                Edit_btn.IsEnabled = false;
            }
            else if (tabelaManif.SelectedItems.Count == 0)
            {
                Edit_btn.IsEnabled = false;
                Del_btn.IsEnabled = false;
            }
            else
            {
                Edit_btn.IsEnabled = true;
                Del_btn.IsEnabled = true;
            }
        }

        private void tabelaTipoviManif_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (tabelaTipoviManif.SelectedItems.Count > 1)
            {
                Edit_btn.IsEnabled = false;
            }
            else if (tabelaTipoviManif.SelectedItems.Count == 0)
            {
                Edit_btn.IsEnabled = false;
                Del_btn.IsEnabled = false;
            }
            else
            {
                Edit_btn.IsEnabled = true;
                Del_btn.IsEnabled = true;
            }
        }

        private void tabelaEtikete_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (tabelaEtikete.SelectedItems.Count > 1)
            {
                Edit_btn.IsEnabled = false;
            }
            else if (tabelaEtikete.SelectedItems.Count == 0)
            {
                Edit_btn.IsEnabled = false;
                Del_btn.IsEnabled = false;
            }
            else
            {
                Edit_btn.IsEnabled = true;
                Del_btn.IsEnabled = true;
            }
        }
    }
}
