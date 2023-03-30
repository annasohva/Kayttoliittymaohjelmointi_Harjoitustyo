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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            DataRepository.CreateDb();

            var invoices = DataRepository.GetInvoices();
            this.DataContext = invoices;
        }

        private void View_Invoice_Clicked(object sender, RoutedEventArgs e) {
            // löysin stackoverflowista ratkaisun miten saa senderistä datacontextin, kun löysin sen debuggerissa senderin ominaisuutena
            var obj = sender as FrameworkElement; 
            var invoice = obj.DataContext as Invoice;

            var invoiceWindow = new InvoiceWindow(invoice);
            invoiceWindow.Show();
        }
    }
}
