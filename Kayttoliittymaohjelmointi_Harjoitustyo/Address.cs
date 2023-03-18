namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Luokka osoitetietoja varten.
    /// </summary>
    public class Address {
        public int ID { get; private set; } = -1;
        public string Name { get; private set; } = string.Empty; // ominaisuudet on private set, koska niitä muutetaan metodien avulla
        public string StreetAddress { get; private set; } = string.Empty;
        public string PostalCode { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;

        /// <summary>
        /// Luo uuden osoitetiedon nimellä, katuosoitteella, postinumerolla ja postitoimipaikalla.
        /// </summary>
        /// <param name="name">Yrityksen tai henkilön nimi</param>
        /// <param name="streetAddress">Katuosoite</param>
        /// <param name="postalCode">Postinumero</param>
        /// <param name="city">Kaupunki / Postitoimipaikka</param>
        public Address(string name, string streetAddress, string postalCode, string city) {
            Name = name;
            ChangeAddress(streetAddress, postalCode, city);
        }

        /// <summary>
        /// Muuttaa osoitetiedoista katuosoitteen, postinumeron ja postitoimipaikan.
        /// </summary>
        /// <param name="streetAddress">Katuosoite</param>
        /// <param name="postalCode">Postinumero</param>
        /// <param name="city">Kaupunki / Postitoimipaikka</param>
        public void ChangeAddress(string streetAddress, string postalCode, string city) {
            StreetAddress = streetAddress;
            PostalCode = postalCode;
            City = city.ToUpper();
        }

        /// <summary>
        /// Muuttaa osoitetiedoista nimen.
        /// </summary>
        /// <param name="name">Yrityksen tai henkilön nimi.</param>
        public void ChangeName(string name) {
            Name = name;
        }
    }
}
