using System;
using System.Collections.Generic;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Luokka laskun tietoja ja rivejä varten.
    /// </summary>
    internal class Invoice {
        private readonly List<InvoiceLine> lines = new List<InvoiceLine>();

        public int ID { get; private set; } = -1;
        public DateOnly Date { get; private set; }
        public DateOnly DueDate { get; private set; }
        public Address BillerAddress { get; private set; }
        public Address CustomerAddress { get; private set; }
        public string Details { get; set; } = string.Empty;
        public double Total { get; private set; } = -1;

        /// <summary>
        /// Luo uuden laskun parametreinaan asiakkaan osoite ja vaihtoehtoiset lisätiedot.
        /// Asettaa automaattisesti nykyisen päivämäärän, eräpäivän 30 päivän päähän ja laskuttajan osoitteen.
        /// </summary>
        /// <param name="customerAddress">Asiakkaan osoite</param>
        /// <param name="details">Vapaaehtoiset lisätiedot</param>
        public Invoice(Address customerAddress, string details = "") {
            Date = DateOnly.FromDateTime(DateTime.Now); // laskun päiväys on nykyinen päivä
            DueDate = DateOnly.FromDateTime(DateTime.Now).AddDays(30); // laskun eräpäivä on 30 päivän päästä

            CustomerAddress = customerAddress;
            BillerAddress = Biller.Address;

            Details = details;
        }

        /// <summary>
        /// Luo uuden laskun parametreinaan työn hinta ja kesto, asiakkaan osoite ja mahdolliset lisätiedot.
        /// Asettaa automaattisesti nykyisen päivämäärän, eräpäivän 30 päivän päähän ja laskuttajan osoitteen.
        /// </summary>
        /// <param name="workQuantity">Työn kesto-</param>
        /// <param name="workPricePerHour">Työn hinta per tunti</param>
        /// <param name="customerAddress">Asiakkaan osoite</param>
        /// <param name="details">Vapaaehtoiset lisätiedot</param>
        public Invoice(double workQuantity, double workPricePerHour, Address customerAddress, string details = "") : this(customerAddress, details) {
            Product work = new Product("Työ", "t", workPricePerHour);
            var line = new InvoiceLine(work, workQuantity);
            AddLine(line);
        }

        /// <summary>
        /// Lisää laskuun uuden laskurivin ja päivittää laskun kokonaissumman.
        /// </summary>
        /// <param name="line">Uusi laskurivi mikä lisätään laskuun.</param>
        public void AddLine(InvoiceLine line) {
            lines.Add(line);
            this.Total += line.Total;
        }

        /// <summary>
        /// Muuttaa laskun eräpäivää.
        /// </summary>
        /// <param name="date">Laskun uusi eräpäivä.</param>
        public void ChangeDueDate(DateOnly date) {
            DueDate = date; // toiminnallisuutta ei ole toteutettu, sillä sitä ei vaadittu
        }

        /// <summary>
        /// Asettaa laskun yksilöivän numeron. Toimii vain jos ID on oletusarvossa eli -1.
        /// </summary>
        /// <param name="id">Yksilöivä numero.</param>
        public void SetID(int id) {
            if (ID == -1) {
                ID = id;
            }
        }
    }
}
