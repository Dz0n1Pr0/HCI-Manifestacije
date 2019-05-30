using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Manifestacije
{
    public class HelpProvider
    {
        public static string GetHelpKey(DependencyObject obj)   //prosledjuje se elem u fokusu
        {
            return obj.GetValue(HelpKeyProperty) as string;     //vraca naziv(Ime, Index, Prezime) objekta u fokusu
        }

        public static void SetHelpKey(DependencyObject obj, string value)
        {
            obj.SetValue(HelpKeyProperty, value);
        }

        public static readonly DependencyProperty HelpKeyProperty =
            DependencyProperty.RegisterAttached("HelpKey", typeof(string), typeof(HelpProvider), new PropertyMetadata("index", HelpKey));
        private static void HelpKey(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //NOOP
        }

        public static void ShowHelp(Window parent)
        {
            HelpWindow hh = new HelpWindow(parent);
            hh.Show();
        }
    }
}
