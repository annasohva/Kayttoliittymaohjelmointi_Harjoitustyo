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

namespace Kayttoliittymaohjelmointi_Harjoitustyo
{
    /// <summary>
    /// Interaction logic for ViewInvoiceWindow.xaml
    /// </summary>
    public partial class InvoiceWindow : Window
    {
        /// <summary>
        /// Luo uuden ikkunan uuden laskun luomista varten.
        /// </summary>
        public InvoiceWindow() {
            InitializeComponent();

            Invoice invoice = new Invoice(0, 0, new Address(), DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(30)));

            dataGridLines.ItemsSource = invoice.Lines;
            this.DataContext = invoice;
        }

        /// <summary>
        /// Luo uuden ikkunan laskun tarkastelemista varten.
        /// </summary>
        /// <param name="invoice">Lasku jota tarkastellaan.</param>
        public InvoiceWindow(Invoice invoice)
        {
            InitializeComponent();

            dataGridLines.ItemsSource = invoice.Lines;
            this.DataContext = invoice;
        }
    }
}
