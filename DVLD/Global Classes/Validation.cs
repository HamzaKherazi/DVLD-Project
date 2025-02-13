using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DVLD
{
    public class clsValidation
    {
        static public bool ValidateEmail(string Email)

        {
            return Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
