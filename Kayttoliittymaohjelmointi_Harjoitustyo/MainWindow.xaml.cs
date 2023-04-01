using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        /// <summary>
        /// Laskutussovelluksen pääikkuna.
        /// </summary>
        public MainWindow() {
            InitializeComponent();

            DataRepository.CreateDb();
            UpdateDatacontext();
        }

        private void UpdateDatacontext() { // hakee laskut tietokannasta ja asettaa ne datacontextiin
            var invoices = DataRepository.GetInvoices();
            this.DataContext = invoices;
        }

        private void View_Invoice_Clicked(object sender, RoutedEventArgs e) {
            // löysin stackoverflowista ratkaisun miten saa senderistä datacontextin
            var obj = sender as FrameworkElement;
            var invoice = obj.DataContext as Invoice;

            var invoiceWindow = new ViewInvoiceWindow(invoice);
            invoiceWindow.ShowDialog();

            UpdateDatacontext(); // päivitetään datacontexti, että käyttäjä näkee esim. laskun poistuneen
        }

        private void NewInvoice_MenuItem_Click(object sender, RoutedEventArgs e) { // kun uuden laskun lisäyspainiketta painetaan menussa
            var newInvoiceWindow = new NewInvoiceWindow();
            newInvoiceWindow.ShowDialog();
            UpdateDatacontext();
        }

        private void Products_MenuItem_Click(object sender, RoutedEventArgs e) { // kun tuotetietojen nappia painetaan menussa
            var productsWindow = new ProductsWindow();
            productsWindow.ShowDialog();
        }

        private void Customers_MenuItem_Click(object sender, RoutedEventArgs e) { // kun osoitetietojen nappia painetaan menussa
            var customersWindow = new CustomersWindow();
            customersWindow.ShowDialog();
        }
    }
}
