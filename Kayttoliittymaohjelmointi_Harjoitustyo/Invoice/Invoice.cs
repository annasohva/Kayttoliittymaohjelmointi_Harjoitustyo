using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Luokka laskun tietoja ja rivejä varten.
    /// </summary>
    public class Invoice : INotifyPropertyChanged {
        public readonly ObservableCollection<InvoiceLine> Lines = new ObservableCollection<InvoiceLine>();

        public event PropertyChangedEventHandler? PropertyChanged;

        public int ID { get; private set; } = -1;
        public DateOnly Date { get; private set; }
        public DateOnly DueDate { get; set; }
        public Address BillerAddress { get; private set; }
        private Address customerAddress;
        public Address CustomerAddress {
            get {
                return customerAddress;
            }
            set {
                customerAddress = value;
                OnPropertyChanged("CustomerAddress");
            }
        }
        public string Details { get; set; } = string.Empty;
        public double Total {
            get {
                double total = 0;

                foreach (var line in Lines) {
                    total += line.Total;
                }

                return Math.Round(total,2);
            }
        }
        
        /// <summary>
        /// Luo uuden laskun ilman työn määrää ja hintaa.
        /// </summary>
        /// <param name="customerAddress">Asiakkaan osoite</param>
        /// <param name="date">Laskun päiväys</param>
        /// <param name="duedate">Laskun eräpäivä</param>
        /// <param name="id">Laskun ID. Jos uusi lasku niin ei tarvitse asettaa.</param>
        /// <param name="details">Laskun lisätiedot</param>
        public Invoice(Address customerAddress, DateOnly date, DateOnly duedate, int id = -1, string details = "") {
            ID = id;
            Date = date;
            DueDate = duedate;

            CustomerAddress = customerAddress;
            BillerAddress = Biller.Address;

            Details = details;

            Lines.CollectionChanged += Lines_CollectionChanged;
            
        }

        /// <summary>
        /// Luo uuden laskun työn määrällä ja hinnalla.
        /// </summary>
        /// <param name="workQuantity">Työn määrä tunneissa</param>
        /// <param name="workPricePerHour">Työn tuntihinta</param>
        /// <param name="customerAddress">Asiakkaan osoite</param>
        /// <param name="date">Laskun päiväys</param>
        /// <param name="duedate">Laskun eräpäivä</param>
        /// <param name="id">Laskun ID. Jos uusi lasku niin ei tarvitse asettaa.</param>
        /// <param name="details">Laskun lisätiedot</param>
        public Invoice(double workQuantity, double workPricePerHour, Address customerAddress, DateOnly date, DateOnly duedate, int id = -1, string details = "") : this(customerAddress, date, duedate, id, details) {
            Product work = new Product("Työ", "t", workPricePerHour);
            var line = new InvoiceLine(work, workQuantity);
            Lines.Add(line);
        }

        // seuraavat metodit on sovellettu stackoverflowsta, ja auttavat laskun kokonaissumman päivityksessä
        private void OnPropertyChanged(string callerName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
        }

        private void Lines_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            OnPropertyChanged("Total");

            if (e.OldItems != null) {
                foreach (INotifyPropertyChanged line in e.OldItems)
                    line.PropertyChanged -= Line_PropertyChanged;
            }
            if (e.NewItems != null) {
                foreach (INotifyPropertyChanged line in e.NewItems)
                    line.PropertyChanged += Line_PropertyChanged;
            }
        }

        private void Line_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            OnPropertyChanged("Total");
        }
    }
}
