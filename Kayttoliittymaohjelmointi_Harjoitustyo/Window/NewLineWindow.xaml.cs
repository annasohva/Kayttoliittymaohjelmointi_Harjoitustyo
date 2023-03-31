using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for NewLineWindow.xaml
    /// </summary>
    public partial class NewLineWindow : Window {
        private Invoice invoiceRef;
        public NewLineWindow(Invoice invoice) {
            InitializeComponent();

            invoiceRef = invoice;

            var products = DataRepository.GetProducts();

            comProducts.ItemsSource = products;
            comProducts.DisplayMemberPath = "Name";
            comProducts.SelectedItem = products[0];

            this.DataContext = new InvoiceLine((Product)comProducts.SelectedItem, 0);
        }

        private void ComProducts_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var line = (InvoiceLine)this.DataContext;

            if (line != null) {
                line.Product = (Product)comProducts.SelectedItem;
                line.UpdateTotal();
            }
        }

        private void TxtQuantity_TextChanged(object sender, TextChangedEventArgs e) {
            var line = (InvoiceLine)this.DataContext;

            if (double.TryParse(txtQuantity.Text, out double quantity)) {
                line.Quantity = quantity;
                line.UpdateTotal();
            }
            else {
                line.Quantity = 0;
                line.UpdateTotal();
            }
        }

        private void AddLine_Clicked(object sender, RoutedEventArgs e) {
            var line = (InvoiceLine)this.DataContext;
            invoiceRef.Lines.Add(line);
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
