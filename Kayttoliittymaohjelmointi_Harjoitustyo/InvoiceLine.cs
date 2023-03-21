using System;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Luokka laskuriviä varten.
    /// </summary>
    public class InvoiceLine {
        public int ID { get; private set; } = -1;
        public Product Product { get; private set; }
        public double Quantity { get; private set; }
        public double Total { get; private set; }

        /// <summary>
        /// Luo uuden laskurivin.
        /// </summary>
        /// <param name="product">Tuote-objekti.</param>
        /// <param name="quantity">Tuotteiden määrä.</param>
        public InvoiceLine(Product product, double quantity, int id = -1) {
            ID = id;
            Product = product;
            Quantity = quantity;
            Total = product.PricePerUnit * quantity;
        }
    }
}
