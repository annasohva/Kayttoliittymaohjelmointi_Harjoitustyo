using System;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Luokka laskutettavaa tuotetta varten.
    /// </summary>
    public class Product {
        public int ID { get; private set; } = -1;
        public string Name { get; private set; } = string.Empty;
        public string Unit { get; private set; } = string.Empty;
        public double PricePerUnit { get; private set; }

        /// <summary>
        /// Luo uuden tuotteen tuotteen nimellä, yksiköllä ja a-hinnalla.
        /// </summary>
        /// <param name="name">Tuotteen nimi</param>
        /// <param name="unit">Yksikkö (kpl, m)</param>
        /// <param name="pricePerUnit">A-hinta</param>
        public Product(string name, string unit, double pricePerUnit) {
            Name= name;
            Unit = unit;
            PricePerUnit = pricePerUnit;
        }

        /// <summary>
        /// Luo uuden tuotteen id:llä tuotteen nimellä, yksiköllä ja a-hinnalla.
        /// </summary>
        /// <param name="id">Tuotteen ID</param>
        /// <param name="name">Tuotteen nimi</param>
        /// <param name="unit">Yksikkö (kpl, m)</param>
        /// <param name="pricePerUnit">A-hinta</param>
        public Product(int id, string name, string unit, double pricePerUnit) {
            ID = id;
            Name = name;
            Unit = unit;
            PricePerUnit = pricePerUnit;
        }
    }
}
