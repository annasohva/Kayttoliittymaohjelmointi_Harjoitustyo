using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Luokka laskun tietoja ja rivejä varten.
    /// </summary>
    public class Invoice {
        public readonly ObservableCollection<InvoiceLine> Lines = new ObservableCollection<InvoiceLine>();
        public int ID { get; private set; } = -1;
        public DateOnly Date { get; private set; }
        public DateOnly DueDate { get; set; }
        public Address BillerAddress { get; private set; }
        public Address CustomerAddress { get; private set; }
        public string Details { get; set; } = string.Empty;
        public double RoundedTotal {
            get {
                return Math.Round(total,2);
            }
        }
        private double total = -1;

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
            AddLine(line);
        }

        /// <summary>
        /// Lisää laskuun uuden laskurivin ja päivittää laskun kokonaissumman.
        /// </summary>
        /// <param name="line">Uusi laskurivi mikä lisätään laskuun.</param>
        public void AddLine(InvoiceLine line) {
            if (total == -1) {
                total = 0;
            }
            Lines.Add(line);
            this.total += line.Total;
        }

        /// <summary>
        /// Päivittää laskurivin kokonaissumman.
        /// </summary>
        public void UpdateTotal() {
            total = 0;

            foreach (var line in Lines) {
                this.total += line.Total;
            }
        }
    }
}
