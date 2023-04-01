using System;
using System.ComponentModel;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Luokka laskutettavaa tuotetta varten.
    /// </summary>
    public class Product : INotifyPropertyChanged {
        private string name = string.Empty;
        private string unit = string.Empty;
        private double pricePerUnit;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int ID { get; set; } = -1;
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public string Unit {
            get {
                return unit;
            }
            set {
                unit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Unit"));
            }
        }
        public double PricePerUnit {
            get {
                return pricePerUnit;
            }
            set {
                pricePerUnit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PricePerUnit"));
            }
        }

        /// <summary>
        /// Luo uuden tuote-olion.
        /// </summary>
        public Product() { }

        /// <summary>
        /// Luo uuden tuote-olion.
        /// </summary>
        /// <param name="name">Tuotteen nimi</param>
        /// <param name="unit">Yksikkö (kpl, m)</param>
        /// <param name="pricePerUnit">A-hinta</param>
        /// <param name="id">Tuotteen ID. Jos uusi tuote niin ei tarvitse asettaa.</param>
        public Product(string name, string unit, double pricePerUnit, int id = -1) {
            ID = id;
            Name= name;
            Unit = unit;
            PricePerUnit = pricePerUnit;
        }
    }
}
