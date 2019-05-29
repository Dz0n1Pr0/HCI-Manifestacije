using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Manifestacije.Modeli
{
    [Serializable]
    class ListaTipManifestacijecs
    { 


    private static Dictionary<string, TipManifestacije> tipManifestacije = null;
    public static Dictionary<string, TipManifestacije> TipoviManifestacija
    {
        get
        {
            if (tipManifestacije == null)
            {
               /* tipManifestacije = new Dictionary<string, TipManifestacije>();
                  tipManifestacije.Add("1", new TipManifestacije("1", "Tip1", "Opis1",
                    new BitmapImage(new Uri("images/tip1.png", UriKind.Relative))));
                 tipManifestacije.Add("2", new TipManifestacije("2", "Tip2", "Opis2",
                     new BitmapImage(new Uri("images/tip1.png", UriKind.Relative))));
                  tipManifestacije.Add("3", new TipManifestacije("3", "Tip3", "Opis3",

*/            }
            return tipManifestacije;
        }
        set
        {
            if (value != tipManifestacije)
                 tipManifestacije = value;
        }
    }

}
}

