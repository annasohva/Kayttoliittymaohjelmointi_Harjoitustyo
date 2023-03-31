using System;
using System.ComponentModel;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Luokka laskuriviä varten.
    /// </summary>
    public class InvoiceLine : INotifyPropertyChanged {
        private Product product;
        private double quantity;
        private double roundedTotal;
        private double total;
        public int ID { get; private set; } = -1;
        public Product Product {
            get {
                return product;
            }
            set {
                product = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Product"));
            }
        }
        public double Quantity {
            get {
                return quantity;
            }
            set {
                quantity = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Quantity"));
            }
        }
        public double Total {
            get {
                return total;
            }
            private set {
                total = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Total"));
            }
        }
        public double RoundedTotal {
            get {
                return roundedTotal;
            }
            private set {
                roundedTotal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RoundedTotal"));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Luo uuden laskurivin.
        /// </summary>
        /// <param name="product">Tuote-objekti.</param>
        /// <param name="quantity">Tuotteiden määrä.</param>
        public InvoiceLine(Product product, double quantity, int id = -1) {
            ID = id;
            Product = product;
            Quantity = quantity;
            UpdateTotal();
        }

        /// <summary>
        /// Päivittää laskurivin kokonaishinnan.
        /// </summary>
        public void UpdateTotal() {
            Total = Product.PricePerUnit * Quantity;
            RoundedTotal = Math.Round(Total, 2);
        }
    }
}
