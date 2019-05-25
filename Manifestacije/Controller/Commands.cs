using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Manifestacije.Controller
{
    public class Commands
    {
        public static RoutedCommand DeleteCommand { get; set; }
        public static RoutedCommand NewEventType { get; set; }

        public static RoutedCommand NewEvent { get; set; }
        public static RoutedCommand NewLabel { get; set; }
        public static RoutedCommand SaveCommand { get; set; }
        public static RoutedCommand Edit { get; set; }
        public static RoutedCommand ViewAll { get; set; }
        public static RoutedCommand HelpCommand { get; set; }
        public static RoutedCommand Exit { get; set; }
        public static RoutedCommand Accept { get; set; }
        public static RoutedCommand Return { get; set; }

        //ovde ovako mozete dodati svoje komande i specifisati im precice ima i ovo ModifierKeys.Control da se ubaci
        static Commands()
        {
            DeleteCommand = new RoutedCommand();
            DeleteCommand.InputGestures.Add(new KeyGesture(Key.Delete));

            NewEventType = new RoutedCommand();
            NewEventType.InputGestures.Add(new KeyGesture(Key.T, ModifierKeys.Control));

            NewEvent = new RoutedCommand();
            NewEvent.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));

            NewLabel = new RoutedCommand();
            NewLabel.InputGestures.Add(new KeyGesture(Key.L, ModifierKeys.Control));

            SaveCommand = new RoutedCommand();
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));

            Edit = new RoutedCommand();
            Edit.InputGestures.Add(new KeyGesture(Key.M, ModifierKeys.Control));

            ViewAll = new RoutedCommand();
            ViewAll.InputGestures.Add(new KeyGesture(Key.F3));

            HelpCommand = new RoutedCommand();
            HelpCommand.InputGestures.Add(new KeyGesture(Key.F1));

            Exit = new RoutedCommand();
            Exit.InputGestures.Add(new KeyGesture(Key.Escape));

            Accept = new RoutedCommand();
            Accept.InputGestures.Add(new KeyGesture(Key.Enter));

            Return = new RoutedCommand();
            Return.InputGestures.Add(new KeyGesture(Key.Back));
        }
    }

}
