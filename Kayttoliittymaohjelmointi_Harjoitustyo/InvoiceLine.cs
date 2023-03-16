using System;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Luokka laskuriviä varten.
    /// </summary>
    internal class InvoiceLine {
        public Product Product { get; private set; }
        public double Quantity { get; private set; }
        public double Total { get; private set; }

        /// <summary>
        /// Luo uuden laskurivin tuoteobjektin ja määrätiedon avulla. Laskee tuotteiden kokonaishinnan.
        /// </summary>
        /// <param name="product">Tuote-objekti.</param>
        /// <param name="quantity">Tuotteiden määrä.</param>
        public InvoiceLine(Product product, double quantity) {
            Product = product;
            Quantity = quantity;
            Total = product.PricePerUnit * quantity;
        }
    }
}
