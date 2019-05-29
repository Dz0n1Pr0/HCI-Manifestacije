using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Manifestacije.Validation
{
    class Min : ValidationRule
    {
        private int _min;


        public int MinValue
        {
            get { return _min; }
            set { _min = value; }
        }

        public Min() { }

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
                return new ValidationResult(false, "Please enter non-negative number.");
            }
            if (((string)value).Length == 0)
                doNothing();

            if ((num < MinValue))
                return new ValidationResult(false, "Please enter non-negative number.");
            else
                return ValidationResult.ValidResult;
        }

        private void doNothing() { }
    }
}
