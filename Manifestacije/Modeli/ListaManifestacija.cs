using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Manifestacije.Modeli
{
    class ListaManifestacija
    {
        //Singleton
        private static Dictionary<string, Manifestacija> manifestacije = null;
        public static Dictionary<string, Manifestacija> Manifestacije
        {
            get
            {
                if (manifestacije == null)
                {
                    manifestacije = new Dictionary<string, Manifestacija>();
                    //new Manifestacija(ID, Ime, Opis, StatusSluzenjaAlkohola, KategorijaCene, Hendikepirani, Pusenje, Napolju, OcekivanaPublika, Datum, IkonicaP, Tip);
                    manifestacije.Add("1", new Manifestacija("1", "Primer1", "Opis1", "Alkohol se moze doneti", "Besplatno", true, true, false, 5000, "5/25/2019 12:00:00 AM",
                        new BitmapImage(new Uri("images/slanina.png", UriKind.Relative)), "Tip1", new Point()));
                    manifestacije.Add("2", new Manifestacija("2", "Primer2", "Opis2", "Nema alkohola", "Visoke cene", false, true, false, 100, "5/25/2019 12:00:00 AM",
                         new BitmapImage(new Uri("images/robot.png", UriKind.Relative)), "Tip2", new Point()));

                }
                return manifestacije;
            }
            set
            {
                if (value != manifestacije)
                {
                    manifestacije = value;
                }
            }
        }

        //Konstruktor
        public ListaManifestacija() { }

    }
}

