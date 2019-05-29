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
        public static RoutedCommand NewCommand { get; set; }
        public static RoutedCommand AddMore { get; set; }
        public static RoutedCommand Event { get; set; }
        public static RoutedCommand EventType { get; set; }
        public static RoutedCommand Label { get; set; }
        public static RoutedCommand AddLabel { get; set; }
        public static RoutedCommand RmvLabel { get; set; }
        public static RoutedCommand Demo { get; set; }
        public static RoutedCommand Mapa1 { get; set; }
        public static RoutedCommand Mapa2 { get; set; }
        public static RoutedCommand Mapa3 { get; set; }
        public static RoutedCommand Mapa4 { get; set; }
        //ovde ovako mozete dodati svoje komande i specifisati im precice ima i ovo ModifierKeys.Control da se ubaci
        static Commands()
        {
            AddLabel = new RoutedCommand();
            AddLabel.InputGestures.Add(new KeyGesture(Key.Left));

            RmvLabel = new RoutedCommand();
            RmvLabel.InputGestures.Add(new KeyGesture(Key.Right));

            Event = new RoutedCommand();
            Event.InputGestures.Add(new KeyGesture(Key.D1, ModifierKeys.Alt));

            EventType = new RoutedCommand();
            EventType.InputGestures.Add(new KeyGesture(Key.D2, ModifierKeys.Alt));

            Label = new RoutedCommand();
            Label.InputGestures.Add(new KeyGesture(Key.D3, ModifierKeys.Alt));

            AddMore = new RoutedCommand();
            AddMore.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Alt));

            NewCommand = new RoutedCommand();
            NewCommand.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));

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

            Demo = new RoutedCommand();
            Demo.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));

            Mapa1 = new RoutedCommand();
            Mapa1.InputGestures.Add(new KeyGesture(Key.D1, ModifierKeys.Control));

            Mapa2 = new RoutedCommand();
            Mapa2.InputGestures.Add(new KeyGesture(Key.D2, ModifierKeys.Control));

            Mapa3 = new RoutedCommand();
            Mapa3.InputGestures.Add(new KeyGesture(Key.D3, ModifierKeys.Control));

            Mapa4 = new RoutedCommand();
            Mapa4.InputGestures.Add(new KeyGesture(Key.D4, ModifierKeys.Control));
        }
    }

}
