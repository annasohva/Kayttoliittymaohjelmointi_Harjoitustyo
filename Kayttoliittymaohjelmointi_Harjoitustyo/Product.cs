﻿using System;

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
