using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Interaction logic for NewInvoiceWindow.xaml
    /// </summary>
    public partial class NewInvoiceWindow : Window {
        private Invoice invoiceRef = new Invoice(new Address(), DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now).AddDays(30));
        private int unsavedChanges = 1;

        /// <summary>
        /// Ikkuna uuden laskun luomista varten.
        /// </summary>
        public NewInvoiceWindow() {
            InitializeComponent();

            this.DataContext = invoiceRef;

            comCustomer.ItemsSource = DataRepository.GetCustomers();
            comCustomer.DisplayMemberPath = "Name";

            dataGridLines.ItemsSource = invoiceRef.Lines;
            dueDatePicker.SelectedDate = invoiceRef.DueDate.ToDateTime(new TimeOnly());
        }

        private void ComCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e) { // kun valitaan eri asiakas
            if (comCustomer.SelectedItem != null) {
                invoiceRef.CustomerAddress = (Address)comCustomer.SelectedItem;
            }
            else {
                invoiceRef.CustomerAddress = new Address();
            }
        }

        private void RemoveLine_Clicked(object sender, RoutedEventArgs e) { // kun laskurivin poisto -nappia painetaan
            var obj = sender as FrameworkElement;
            var invoiceLine = obj.DataContext as InvoiceLine;

            var result = MessageBox.Show($"Haluatko varmasti poistaa laskurivin \"{invoiceLine.Product.Name}\"?", "Poista laskurivi", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes) {
                invoiceRef.Lines.Remove(invoiceLine);
            }
        }

        private void Save_Btn_Clicked(object sender, RoutedEventArgs e) { // kun tallenna -nappia painetaan
            DataRepository.InsertInvoice(invoiceRef);
            MessageBox.Show("Lasku on tallennettu tietokantaan. Ikkuna sulkeutuu.", "Viesti");
            unsavedChanges = 0;
            Close();
        }

        private void AddLine_Clicked(object sender, RoutedEventArgs e) { // kun laskurivin lisäys -nappia painetaan
            var newLineWindow = new NewLineWindow(invoiceRef);
            newLineWindow.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) { // ilmoitetaan käyttäjää jos on tallentamattomia muutoksia
            if (unsavedChanges == 1) {
                var result = MessageBox.Show($"Lasku ei ole vielä tallennettu tietokantaan. Haluatko varmasti sulkea ikkunan?", "Huomio", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No) {
                    e.Cancel = true;
                }
            }
        }

        private void DueDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) { // kun eräpäivää muutetaan
            if (dueDatePicker.SelectedDate != null && invoiceRef != null) {
                invoiceRef.DueDate = DateOnly.FromDateTime((DateTime)dueDatePicker.SelectedDate); // täytyy muuntaa datetime dateonlyksi
            }
        }

        private void Products_MenuItem_Click(object sender, RoutedEventArgs e) { // kun painetaan tuotetietojen muuttamis-nappia menussa
            var productsWindow = new ProductsWindow();
            productsWindow.ShowDialog();
        }

        private void Customers_MenuItem_Click(object sender, RoutedEventArgs e) { // kun painetaan osoitetietojen muuttamis-nappia menussa
            var customersWindow = new CustomersWindow();
            customersWindow.ShowDialog();
            comCustomer.ItemsSource = DataRepository.GetCustomers();
        }
    }
}
