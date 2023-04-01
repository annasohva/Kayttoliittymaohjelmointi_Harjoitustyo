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

namespace Kayttoliittymaohjelmointi_Harjoitustyo
{
    /// <summary>
    /// Interaction logic for CustomersWindow.xaml
    /// </summary>
    public partial class CustomersWindow : Window
    {
        private bool unsavedChanges = false;
        private List<int> customersToDelete = new List<int>();
        private List<Address> customersToAdd = new List<Address>();

        /// <summary>
        /// Asiakkaiden osoitetietojen tarkastelu ja muokkaus -ikkuna.
        /// </summary>
        public CustomersWindow() {
            InitializeComponent();
            UpdateDataContext();
        }

        private void UpdateDataContext() { // haetaan kaikki osoitetiedot ja asetetaan se datacontextiin
            var customers = DataRepository.GetCustomers();
            this.DataContext = customers;
        }

        private void DeleteCustomer_Clicked(object sender, RoutedEventArgs e) { // kun osoitetiedon poisto -nappia painetaan
            var obj = sender as FrameworkElement;
            var customer = obj.DataContext as Address;

            if (customer != null) {
                var result = MessageBox.Show($"Haluatko varmasti poistaa asiakkaan \"{customer.Name}\"?", "Poista asiakas", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes) {
                    var customers = this.DataContext as ObservableCollection<Address>;
                    customers.Remove(customer);

                    if (customer.ID != -1) {
                        customersToDelete.Add(customer.ID);
                        unsavedChanges = true;
                    }
                }
            }
        }

        private void SaveChanges_Clicked(object sender, RoutedEventArgs e) { // kun tallenna tiedot -nappia painetaan
            var customers = this.DataContext as ObservableCollection<Address>;

            foreach (var customer in customers) {
                if (customer.ID == -1) {
                    customersToAdd.Add(customer);
                }
            }

            foreach (var customerID in customersToDelete) {
                DataRepository.DeleteCustomer(customerID);
            }

            foreach (var customer in customersToAdd) {
                DataRepository.InsertCustomer(customer);
                var newCustomers = DataRepository.GetCustomers();
                customer.ID = newCustomers.Last().ID;
            }

            foreach (var customer in customers) {
                DataRepository.UpdateCustomer(customer);
            }

            MessageBox.Show("Tiedot on tallennettu tietokantaan.", "Viesti");
            unsavedChanges = false;

            UpdateDataContext();
        }

        // jos on ei-tallentamattomia muutoksia kysytään käyttäjältä haluaako hän varmasti sulkea ikkunan
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (unsavedChanges == true) {
                var result = MessageBox.Show($"Sinulla on tallentamattomia muutoksia. Haluatko varmasti sulkea ikkunan?", "Huomio", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No) {
                    e.Cancel = true;
                }
            }
        }

        // jos jotain solua on muokattu
        private void DataGrid_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e) {
            unsavedChanges = true;
        }
    }
}
