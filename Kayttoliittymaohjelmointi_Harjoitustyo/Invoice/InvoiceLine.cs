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
                OnPropertyChanged("Product");
            }
        }
        public double Quantity {
            get {
                return quantity;
            }
            set {
                quantity = value;
                OnPropertyChanged("Quantity");
                UpdateTotal();
            }
        }
        public double Total {
            get {
                return total;
            }
            private set {
                total = value;
                OnPropertyChanged("Total");
            }
        }
        public double RoundedTotal {
            get {
                return roundedTotal;
            }
            private set {
                roundedTotal = value;
                OnPropertyChanged("RoundedTotal");
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

            Product.PropertyChanged += Product_PropertyChanged;
        }

        /// <summary>
        /// Päivittää laskurivin kokonaishinnan.
        /// </summary>
        public void UpdateTotal() {
            Total = Product.PricePerUnit * Quantity;
            RoundedTotal = Math.Round(Total, 2);
        }

        // kokonaissumman päivittymiseen liittyviä metodeita

        private void OnPropertyChanged(string callerName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
        }

        private void Product_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
            UpdateTotal();
        }
    }
}
