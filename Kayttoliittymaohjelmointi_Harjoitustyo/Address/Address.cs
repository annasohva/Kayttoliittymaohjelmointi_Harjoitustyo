using System.ComponentModel;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Luokka osoitetietoja varten.
    /// </summary>
    public class Address : INotifyPropertyChanged {
        private string name = string.Empty;
        private string streetAddress = string.Empty;
        private string postalCode = string.Empty;
        private string city = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int ID { get; private set; } = -1;
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string StreetAddress {
            get {
                return streetAddress;
            }
            set {
                streetAddress = value;
                OnPropertyChanged("StreetAddress");
            }
        }
        public string PostalCode {
            get {
                return postalCode;
            }
            set {
                postalCode = value;
                OnPropertyChanged("PostalCode");
            }
        }
        public string City {
            get {
                return city;
            }
            set {
                city = value;
                OnPropertyChanged("City");
            }
        }

        /// <summary>
        /// Luo uuden osoitetiedon.
        /// </summary>
        public Address() { }

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

        private void OnPropertyChanged(string callerName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
        }
    }
}
