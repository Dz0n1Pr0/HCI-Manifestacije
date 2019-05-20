using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Manifestacije.Modeli
{
    [Serializable]
    class ListaEtiketa
    {
        private static Dictionary<string, Etiketa> etikete = null;
        public static Dictionary<string, Etiketa> Etikete
        {
            get
            {
                if (etikete == null)
                {
                    //string _id, string _boja, string _opis
                    etikete = new Dictionary<string, Etiketa>();
                    Etikete.Add("1", new Etiketa("1", Colors.Red, "...", new SolidColorBrush(Colors.Red)));
                }
                return etikete;
            }
            set
            {
                if (value != etikete)
                    etikete = value;
            }
        }
    }
}
