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
        private int initialised = 0; // kun tämä on 1 lasketaan tallentamattomia muutoksia tekstikentistä

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

        private void RemoveLine_Clicked(object sender, RoutedEventArgs e) { // kun laskurivin poisto -nappia painetaan
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
                else {
                    linesToAdd.Remove(invoiceLine);
                }
            }
        }

        private void Save_Btn_Clicked(object sender, RoutedEventArgs e) { // kun tallenna-nappia painetaan tallennetaan muutokset
            Invoice invoice = this.DataContext as Invoice;

            foreach (var line in linesToDelete) {
                DataRepository.DeleteInvoiceLine(line);
            }

            foreach (var line in linesToAdd) {
                DataRepository.InsertInvoiceLine(line, invoice.ID);
            }

            DataRepository.UpdateInvoice(invoice);

            MessageBox.Show("Tiedot on tallennettu tietokantaan.", "Viesti");

            unsavedChanges = false;
        }

        private void DeleteInvoice_MenuItem_Click(object sender, RoutedEventArgs e) { // kun menusta valitaan laskun poisto
            Invoice invoice = this.DataContext as Invoice;
            var result = MessageBox.Show($"Haluatko varmasti poistaa laskun {invoice.ID}?", "Poista lasku", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes) {
                DataRepository.DeleteInvoice(invoice.ID);
                MessageBox.Show("Lasku on poistettu tietokannasta.", "Viesti");
                this.Close();
            }
        }

        // datepicker hyväksyy minulla vain datetimen niin dateonly täytyy muuntaa
        private void DueDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) { 
            Invoice invoice = this.DataContext as Invoice;

            if (dueDatePicker.SelectedDate != null && invoice != null) {
                invoice.DueDate = DateOnly.FromDateTime((DateTime)dueDatePicker.SelectedDate);
                unsavedChanges = true;
            }
        }

        private void AddLine_Clicked(object sender, RoutedEventArgs e) { // kun lisää laskurivi-nappia painetaan
            Invoice invoice = this.DataContext as Invoice;

            var newLineWindow = new NewLineWindow(invoice);
            newLineWindow.ShowDialog();

            linesToAdd.Add(invoice.Lines.Last());
            unsavedChanges = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) { // kun ikkunaa yritetään sulkea
            if (unsavedChanges == true) {
                var result = MessageBox.Show($"Sinulla on tallentamattomia muutoksia. Haluatko varmasti sulkea ikkunan?", "Huomio", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No) {
                    e.Cancel = true;
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) { // kun tekstilaatikoissa muutetaan tekstiä
            if (initialised == 1) {
                TextBox textBox = (TextBox)e.Source;

                if (textBox.Text != null && textBox.Text != string.Empty) {
                    unsavedChanges = true;
                }
            }
        }

        // kun tallenna-nappi mikä on xaml tiedoston perällä on latautunut voidaan alkaa laskemaan onko tallentamattomia muutoksia
        private void SaveButton_Loaded(object sender, RoutedEventArgs e) { 
            initialised = 1;
        }
    }
}
