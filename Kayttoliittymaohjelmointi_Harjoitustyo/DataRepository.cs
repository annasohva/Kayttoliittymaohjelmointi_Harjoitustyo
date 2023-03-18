using MySqlConnector;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;

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

        public static void InsertInvoice(Invoice invoice) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("", conn);
                cmd.ExecuteNonQuery();
            }
        }
        public static void UpdateInvoice(Invoice invoice) { }
        public static void DeleteInvoice(Invoice invoice) { }
        public static ObservableCollection<Invoice> GetInvoices() {
            var invoices = new ObservableCollection<Invoice>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT TuoteID, Tuotenimi, Yksikko, Yksikkohinta FROM tuotteet;", conn);

                var dr = cmd.ExecuteReader();

                while (dr.Read()) {
                    int id = dr.GetInt32("TuoteID");
                    string name = dr.GetString("Tuotenimi");
                    string unit = dr.GetString("Yksikko");
                    double pricePerUnit = dr.GetDouble("Yksikkohinta");

                    invoices.Add(new Invoice(name, unit, pricePerUnit, id));
                }
            }

            return invoices;
        }

        public static void InsertProduct(Product product) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO tuotteet(Tuotenimi, Yksikko, Yksikkohinta) VALUES(@name, @unit, @pricePerUnit);", conn);
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@unit", product.Unit);
                cmd.Parameters.AddWithValue("@pricePerUnit", product.PricePerUnit);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateProduct(Product product) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE tuotteet SET Tuotenimi=@name, Yksikko=@unit, Yksikkohinta=@pricePerUnit WHERE TuoteID=@id;", conn);
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@unit", product.Unit);
                cmd.Parameters.AddWithValue("@pricePerUnit", product.PricePerUnit);
                cmd.Parameters.AddWithValue("@id", product.ID);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteProduct(Product product) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM tuotteet WHERE TuoteID=@id;", conn);
                cmd.Parameters.AddWithValue("@id", product.ID);
                cmd.ExecuteNonQuery();
            }
        }

        public static ObservableCollection<Product> GetProducts() {
            var products = new ObservableCollection<Product>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT TuoteID, Tuotenimi, Yksikko, Yksikkohinta FROM tuotteet;", conn);

                var dr = cmd.ExecuteReader();

                while (dr.Read()) {
                    int id = dr.GetInt32("TuoteID");
                    string name = dr.GetString("Tuotenimi");
                    string unit = dr.GetString("Yksikko");
                    double pricePerUnit = dr.GetDouble("Yksikkohinta");

                    products.Add(new Product(name, unit, pricePerUnit, id));
                }
            }

            return products;
        }

        public static void InsertCustomer(Address address) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO asiakkaat(Nimi,Katuosoite,Postinumero,Kaupunki) VALUES(@name, @streetAddress, @postalCode, @city);", conn);
                cmd.Parameters.AddWithValue("@name", address.Name);
                cmd.Parameters.AddWithValue("@streetAddress", address.StreetAddress);
                cmd.Parameters.AddWithValue("@postalCode", address.PostalCode);
                cmd.Parameters.AddWithValue("@city", address.City);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateCustomer(Address address) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE asiakkaat SET Nimi=@name, Katuosoite=@streetAddress, Postinumero=@postalCode, Kaupunki=@city WHERE AsiakasID=@id;", conn);
                cmd.Parameters.AddWithValue("@name", address.Name);
                cmd.Parameters.AddWithValue("@streetAddress", address.StreetAddress);
                cmd.Parameters.AddWithValue("@postalCode", address.PostalCode);
                cmd.Parameters.AddWithValue("@city", address.City);
                cmd.Parameters.AddWithValue("@id", address.ID);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteCustomer(Address address) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM asiakkaat WHERE AsiakasID=@id;", conn);
                cmd.Parameters.AddWithValue("@id", address.ID);
                cmd.ExecuteNonQuery();
            }
        }

        public static ObservableCollection<Address> GetCustomers() {
            var customers = new ObservableCollection<Address>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT AsiakasID, Nimi, Katuosoite, Postinumero, Kaupunki FROM asiakkaat;", conn);

                var dr = cmd.ExecuteReader();

                while (dr.Read()) {
                    int id = dr.GetInt32("AsiakasID");
                    string name = dr.GetString("Nimi");
                    string streetAddress = dr.GetString("Katuosoite");
                    string postalCode = dr.GetString("Postinumero");
                    string city = dr.GetString("Kaupunki");

                    customers.Add(new Address(name, streetAddress, postalCode, city, id));
                }
            }

            return customers;
        }
    }
}
