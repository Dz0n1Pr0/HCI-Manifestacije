using Manifestacije.Modeli;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Manifestacije.Validation
{
    class IDValidation : ValidationRule
    {
        private int _min;
        private int _max;


        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }


        public IDValidation() { }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int num = 0;

            try
            {
                if (((string)value).Length > 0)
                    num = Int32.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Please enter a non-negative number with no more than 5 digits.");
            }

            if ((num < Min) || (num > Max))
            {
                return new ValidationResult(false,
                  "Please enter a non-negative number with no more than 5 digits.");
            }


            bool exists = false;
            foreach (KeyValuePair<string, Manifestacija> m in ListaManifestacija.Manifestacije)
            {
                if (m.Key.Equals((string)value))
                    exists = true;
            }


            if (exists)
                return new ValidationResult(false, "ID already exists!");
            else
                return ValidationResult.ValidResult;
        }
    }
}
