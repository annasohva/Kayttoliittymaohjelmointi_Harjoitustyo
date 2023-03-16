using System;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Luokka laskutettavaa tuotetta varten.
    /// </summary>
    internal class Product {
        public string Code { get; private set; } = string.Empty;
        public string Unit { get; private set; } = string.Empty;
        public double PricePerUnit { get; private set; }

        /// <summary>
        /// Luo uuden tuotteen tuotekoodilla, yksiköllä ja a-hinnalla.
        /// </summary>
        /// <param name="code">Tuotekoodi</param>
        /// <param name="unit">Yksikkö (kpl, m)</param>
        /// <param name="pricePerUnit">A-hinta</param>
        public Product(string code, string unit, double pricePerUnit) {
            Code = code;
            Unit = unit;
            PricePerUnit = pricePerUnit;
        }
    }
}
