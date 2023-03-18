using MySqlConnector;
using System.Diagnostics;
using System.IO;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    public static class DataRepository {
        private const string local = @"Server=127.0.0.1; Port=3306; User ID=opiskelija; Pwd=opiskelija1;";
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=opiskelija; Pwd=opiskelija1; Database=laskutus;";
        
        public static void CreateDb() {
            string script = File.ReadAllText("laskutussovellus_tietokannanluonti.sql"); // sql-tiedoston tulisi olla samassa kansiossa kun exe

            using (MySqlConnection conn = new MySqlConnection(local)) {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(script, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void InsertCustomer(Address address) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("", conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
