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
    /// Interaction logic for ViewInvoiceWindow.xaml
    /// </summary>
    public partial class ViewInvoiceWindow : Window {
        private bool unsavedChanges = false;
        private List<int> linesToDelete = new List<int>();
        private List<InvoiceLine> linesToAdd = new List<InvoiceLine>();

        /// <summary>
        /// Luo uuden ikkunan laskun tarkastelemista varten.
        /// </summary>
        /// <param name="invoice">Lasku jota tarkastellaan.</param>
        public ViewInvoiceWindow(Invoice invoice) {
            InitializeComponent();

            dataGridLines.ItemsSource = invoice.Lines;
            dueDatePicker.SelectedDate = invoice.DueDate.ToDateTime(new TimeOnly());
            this.DataContext = invoice;
        }

        private void Remove_Line_Clicked(object sender, RoutedEventArgs e) {
            var obj = sender as FrameworkElement;
            var invoiceLine = obj.DataContext as InvoiceLine;

            var result = MessageBox.Show($"Haluatko varmasti poistaa laskurivin \"{invoiceLine.Product.Name}\"?", "Poista laskurivi", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes) {
                Invoice invoice = this.DataContext as Invoice;
                invoice.Lines.Remove(invoiceLine);

                if (invoiceLine.ID != -1) {
                    linesToDelete.Add(invoiceLine.ID);
                    unsavedChanges = true;
                }
            }
        }

        private void Save_Btn_Clicked(object sender, RoutedEventArgs e) {
            SaveChanges();
        }

        private void Save_MenuItem_Click(object sender, RoutedEventArgs e) {
            SaveChanges();
        }

        private void SaveChanges() {
            Invoice invoice = this.DataContext as Invoice;

            foreach (var line in linesToDelete) {
                DataRepository.DeleteInvoiceLine(line);
            }

            foreach (var line in linesToAdd) {
                DataRepository.InsertInvoiceLine(line, invoice.ID);
            }

            DataRepository.UpdateInvoice(invoice);

            MessageBox.Show("Tiedot on tallennettu tietokantaan.", "Viesti");
        }

        private void DeleteInvoice_MenuItem_Click(object sender, RoutedEventArgs e) {
            Invoice invoice = this.DataContext as Invoice;
            var result = MessageBox.Show($"Haluatko varmasti poistaa laskun {invoice.ID}?", "Poista lasku", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes) {
                DataRepository.DeleteInvoice(invoice.ID);
                MessageBox.Show("Lasku on poistettu tietokannasta.", "Viesti");
                this.Close();
            }
        }

        private void DueDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
            Invoice invoice = this.DataContext as Invoice;

            if (dueDatePicker.SelectedDate != null && invoice != null) {
                invoice.DueDate = DateOnly.FromDateTime((DateTime)dueDatePicker.SelectedDate);
            }
        }

        private void AddLine_Clicked(object sender, RoutedEventArgs e) {
            Invoice invoice = this.DataContext as Invoice;
            var newLineWindow = new NewLineWindow(invoice);
            newLineWindow.ShowDialog();
        }
    }
}
