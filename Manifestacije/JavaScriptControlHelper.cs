using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Manifestacije
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class JavaScriptControlHelper
    {
        Window prozor;
        public JavaScriptControlHelper(MainWindow w)
        {
            prozor = w;
        }
        public JavaScriptControlHelper(ViewWindow w)
        {
            prozor = w;
        }
        public JavaScriptControlHelper(ManifestacijaWindow w)
        {
            prozor = w;
        }
        public JavaScriptControlHelper(TipManifestacijeWindow w)
        {
            prozor = w;
        }
        public JavaScriptControlHelper(EtiketaWindow w)
        {
            prozor = w;
        }

        public void RunFromJavascript(string param)
        {
            if(prozor is MainWindow)
            {
                ((MainWindow)prozor).doThings();
            }
            else if (prozor is ViewWindow)
            {
                ((ViewWindow)prozor).doThings();
            }
            else if (prozor is ManifestacijaWindow)
            {
                ((ManifestacijaWindow)prozor).doThings();
            }
            else if (prozor is EtiketaWindow)
            {
                ((EtiketaWindow)prozor).doThings();
            }
            else if (prozor is TipManifestacijeWindow)
            {
                ((TipManifestacijeWindow)prozor).doThings();
            }
        }
    }
}
