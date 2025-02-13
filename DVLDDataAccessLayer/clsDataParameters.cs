using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace DVLDDataAccessLayer
{
    public class clsDataParameters
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;
    }
}
