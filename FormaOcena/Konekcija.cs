using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace FormaOcena
{
    internal class Konekcija
    {
        public static SqlConnection connect()
        {
            SqlConnection veza = new SqlConnection("Data Source=DESKTOP-TB5074K\\SQLEXPRESS;Initial Catalog=EsDnevnik;Integrated Security=true");
            return veza;
        }
    }
}
