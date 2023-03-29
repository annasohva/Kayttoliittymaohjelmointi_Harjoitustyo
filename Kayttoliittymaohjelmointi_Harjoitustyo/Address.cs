namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Luokka osoitetietoja varten.
    /// </summary>
    public class Address {
        public int ID { get; private set; } = -1;
        public string Name { get; set; } = string.Empty; // ominaisuudet on private set, koska niitä muutetaan metodien avulla
        public string StreetAddress { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Luo uuden osoitetiedon.
        /// </summary>
        /// <param name="name">Yrityksen tai henkilön nimi</param>
        /// <param name="streetAddress">Katuosoite</param>
        /// <param name="postalCode">Postinumero</param>
        /// <param name="city">Kaupunki / Postitoimipaikka</param>
        /// <param name="id">Osoitetiedon ID. Jos uusi niin ei tarvitse asettaa.</param>
        public Address(string name, string streetAddress, string postalCode, string city, int id = -1) {
            ID = id;
            Name = name;
            StreetAddress= streetAddress;
            PostalCode = postalCode;
            City = city;
        }
    }
}
