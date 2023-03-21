using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Luokka tietokantaan tietojen tallennusta ja hakua varten.
    /// </summary>
    public static class DataRepository {
        private const string local = @"Server=127.0.0.1; Port=3306; User ID=opiskelija; Pwd=opiskelija1;";
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=opiskelija; Pwd=opiskelija1; Database=laskutus;";
        
        /// <summary>
        /// Luo sovelluksen tietokannan käyttäen SQL-skriptitiedostoa.
        /// </summary>
        public static void CreateDb() {
            string script = File.ReadAllText("laskutussovellus_tietokannanluonti.sql"); // sql-tiedosto tulisi sijoittaa samaan kansioon kuin exe, kopio löytyy kansiosta missä on .cs-tiedostot

            using (MySqlConnection conn = new MySqlConnection(local)) {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(script, conn);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Lisää uuden laskun, asiakkaan osoitteen ja laskurivit tietokantaan.
        /// </summary>
        /// <param name="invoice">Lasku mikä lisätään tietokantaan.</param>
        public static void InsertInvoice(Invoice invoice) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                // lisätään lasku tietokantaan
                MySqlCommand cmd1 = new MySqlCommand("INSERT INTO laskut(Paivays, Erapaiva, Lisatiedot) VALUES(@date, @duedate, @details);", conn);
                cmd1.Parameters.AddWithValue("@date", invoice.Date);
                cmd1.Parameters.AddWithValue("@duedate", invoice.DueDate);
                cmd1.Parameters.AddWithValue("@details", invoice.Details);
                cmd1.ExecuteNonQuery();

                // lisätään laskun asiakas tietokantaan
                MySqlCommand cmd2 = new MySqlCommand("INSERT INTO laskujenasiakkaat(LaskuID,Nimi,Katuosoite,Postinumero,Kaupunki) VALUES(@id, @name, @streetAddress, @postalCode, @city);", conn);
                cmd2.Parameters.AddWithValue("@id", invoice.ID);
                cmd2.Parameters.AddWithValue("@name", invoice.CustomerAddress.Name);
                cmd2.Parameters.AddWithValue("@streetAddress", invoice.CustomerAddress.StreetAddress);
                cmd2.Parameters.AddWithValue("@postalCode", invoice.CustomerAddress.PostalCode);
                cmd2.Parameters.AddWithValue("@city", invoice.CustomerAddress.City);
                cmd2.ExecuteNonQuery();

                // lisätään laskurivit tietokantaan
                foreach (var line in invoice.Lines) {
                    MySqlCommand cmd3 = new MySqlCommand("INSERT INTO laskurivit(LaskuID, Tuotenimi, TuotteidenMaara, Yksikko, Yksikkohinta) VALUES(@invoiceID, @productName, @quantity, @unit, @pricePerUnit);", conn);
                    cmd3.Parameters.AddWithValue("@LaskuID", invoice.ID);
                    cmd3.Parameters.AddWithValue("@productName", line.Product.Name);
                    cmd3.Parameters.AddWithValue("@quantity", line.Quantity);
                    cmd3.Parameters.AddWithValue("@unit", line.Product.Unit);
                    cmd3.Parameters.AddWithValue("@pricePerUnit", line.Product.PricePerUnit);
                    cmd3.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Päivittää laskun tiedot tietokantaan.
        /// </summary>
        /// <param name="invoice">Lasku jota on päivitetty.</param>
        public static void UpdateInvoice(Invoice invoice) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                // päivitetään laskun tiedot, laskun päiväys ei kuitenkaan muutu koska se on automaattinen
                MySqlCommand cmd1 = new MySqlCommand("UPDATE laskut SET Erapaiva=@duedate, Lisatiedot=@details WHERE LaskuID=@id;", conn);
                cmd1.Parameters.AddWithValue("@duedate", invoice.DueDate);
                cmd1.Parameters.AddWithValue("@details", invoice.Details);
                cmd1.Parameters.AddWithValue("@id", invoice.ID);
                cmd1.ExecuteNonQuery();

                // päivitetään laskun asiakkaan osoitetiedot
                MySqlCommand cmd2 = new MySqlCommand("UPDATE laskujenasiakkaat SET Nimi=@name, Katuosoite=@streetAddress, Postinumero=@postalCode, Kaupunki=@city WHERE LaskuID=@id;", conn);
                cmd2.Parameters.AddWithValue("@name", invoice.CustomerAddress.Name);
                cmd2.Parameters.AddWithValue("@streetAddress", invoice.CustomerAddress.StreetAddress);
                cmd2.Parameters.AddWithValue("@postalCode", invoice.CustomerAddress.PostalCode);
                cmd2.Parameters.AddWithValue("@city", invoice.CustomerAddress.City);
                cmd2.Parameters.AddWithValue("@id", invoice.ID);
                cmd2.ExecuteNonQuery();

                // päivitetään laskurivin tiedot
                foreach (var line in invoice.Lines) {
                    MySqlCommand cmd3 = new MySqlCommand("UPDATE laskurivit SET Tuotenimi=@productName, TuotteidenMaara=@quantity, Yksikko=@unit, Yksikkohinta=@pricePerUnit WHERE LaskuriviID=@id;", conn);
                    cmd3.Parameters.AddWithValue("@productName", line.Product.Name);
                    cmd3.Parameters.AddWithValue("@quantity", line.Quantity);
                    cmd3.Parameters.AddWithValue("@unit", line.Product.Unit);
                    cmd3.Parameters.AddWithValue("@pricePerUnit", line.Product.PricePerUnit);
                    cmd3.Parameters.AddWithValue("@id", line.ID);
                    cmd3.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Poistaa laskun, laskun asiakkaan ja laskurivit tietokannasta.
        /// </summary>
        /// <param name="id">Laskun ID.</param>
        public static void DeleteInvoice(int id) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd1 = new MySqlCommand("DELETE FROM laskut WHERE LaskuID=@id;", conn);
                cmd1.Parameters.AddWithValue("@id", id);
                cmd1.ExecuteNonQuery();

                MySqlCommand cmd2 = new MySqlCommand("DELETE FROM laskujenasiakkaat WHERE LaskuID=@id;", conn);
                cmd2.Parameters.AddWithValue("@id", id);
                cmd2.ExecuteNonQuery();

                MySqlCommand cmd3 = new MySqlCommand("DELETE FROM laskurivit WHERE LaskuID=@id;", conn);
                cmd3.Parameters.AddWithValue("@id", id);
                cmd3.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Poistaa yksittäisen laskurivin.
        /// </summary>
        /// <param name="id">Laskurivin ID.</param>
        public static void DeleteInvoiceLine(int id) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd1 = new MySqlCommand("DELETE FROM laskurivit WHERE LaskuriviID=@id;", conn);
                cmd1.Parameters.AddWithValue("@id", id);
                cmd1.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Hakee kaikki laskut tietokannasta.
        /// </summary>
        /// <returns>Laskukokoelma ObservableCollection</returns>
        public static ObservableCollection<Invoice> GetInvoices() {
            var invoices = new ObservableCollection<Invoice>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmdInvoices = new MySqlCommand("SELECT * FROM laskut;", conn);
                var dr = cmdInvoices.ExecuteReader();

                while (dr.Read()) { 
                    int invoiceId = dr.GetInt32("LaskuID");
                    DateOnly date = dr.GetDateOnly("Paivays");
                    DateOnly duedate = dr.GetDateOnly("Erapaiva");
                    string details = dr.GetString("Lisatiedot");

                    // haetaan laskun asiakkaan osoite
                    MySqlCommand cmdCustomer = new MySqlCommand("SELECT * FROM laskujenasiakkaat WHERE LaskuID = @id;", conn);
                    cmdCustomer.Parameters.AddWithValue("@id", invoiceId);
                    var customerDr = cmdInvoices.ExecuteReader();

                    Address address = null;
                    while (customerDr.Read()) {
                        string name = dr.GetString("Nimi");
                        string streetAddress = dr.GetString("Katuosoite");
                        string postalCode = dr.GetString("Postinumero");
                        string city = dr.GetString("Kaupunki");

                        address = new Address(name, streetAddress, postalCode, city);
                    }

                    // luodaan itse lasku
                    Invoice invoice = new Invoice(address, date, duedate, invoiceId, details);
                    
                    // haetaan laskuun laskurivit
                    MySqlCommand cmdInvoiceLines = new MySqlCommand("SELECT * FROM laskurivit WHERE LaskuID = @id;", conn);
                    cmdInvoiceLines.Parameters.AddWithValue("@id", invoiceId);
                    var linesDr = cmdInvoices.ExecuteReader();

                    while (linesDr.Read()) {
                        int lineId = linesDr.GetInt32("LaskuriviID");
                        string productName = linesDr.GetString("Tuotenimi");
                        double quantity = linesDr.GetDouble("TuotteidenMaara");
                        string unit = linesDr.GetString("Yksikko");
                        double pricePerUnit = linesDr.GetDouble("Yksikkohinta");

                        Product product = new Product(productName, unit, pricePerUnit);
                        invoice.AddLine(new InvoiceLine(product, quantity, lineId));
                    }

                    invoices.Add(invoice);
                }
            }

            return invoices;
        }

        /// <summary>
        /// Lisää uuden tuotteen tietokantaan.
        /// </summary>
        /// <param name="product">Lisättävä tuote.</param>
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

        /// <summary>
        /// Päivittää tuotteen tiedot tietokannassa
        /// </summary>
        /// <param name="product">Tuote minkä tiedot päivitetään.</param>
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

        /// <summary>
        /// Poistaa tuotteen tietokannasta.
        /// </summary>
        /// <param name="id">Poistettavan tuotteen ID.</param>
        public static void DeleteProduct(int id) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM tuotteet WHERE TuoteID=@id;", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Hakee kaikki tuotteet tietokannasta.
        /// </summary>
        /// <returns>Product-oliot ObservableCollection</returns>
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

        /// <summary>
        /// Lisää uuden asiakkaan tietokantaan.
        /// </summary>
        /// <param name="address">Asiakkaan osoite.</param>
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

        /// <summary>
        /// Päivittää asiakastiedot tietokantaan.
        /// </summary>
        /// <param name="address">Päivitettävän asiakkaan osoite.</param>
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

        /// <summary>
        /// Poistaa asiakkaan osoitteen tietokannasta.
        /// </summary>
        /// <param name="id">Poistettavan asiakkaan osoitetiedon ID.</param>
        public static void DeleteCustomer(int id) {
            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM asiakkaat WHERE AsiakasID=@id;", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Hakee kaikki asiakkaiden osoitetiedot.
        /// </summary>
        /// <returns>Asiakkaiden osoitetiedot ObservableCollection</returns>
        public static ObservableCollection<Address> GetCustomers() {
            var customers = new ObservableCollection<Address>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb)) {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM asiakkaat;", conn);

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
