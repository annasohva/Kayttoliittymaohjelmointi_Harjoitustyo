using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Kayttoliittymaohjelmointi_Harjoitustyo {
    /// <summary>
    /// Interaction logic for ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        private bool unsavedChanges = false;
        private List<int> productsToDelete = new List<int>();
        private List<Product> productsToAdd = new List<Product>();

        /// <summary>
        /// Tuotetietojen tarkastelu ja muokkaus -ikkuna.
        /// </summary>
        public ProductsWindow()
        {
            InitializeComponent();
            UpdateDataContext();
        }

        private void UpdateDataContext() { // haetaan kaikki tuotetiedot ja asetetaan se datacontextiin
            var products = DataRepository.GetProducts();
            this.DataContext = products;
        }

        private void DeleteProduct_Clicked(object sender, RoutedEventArgs e) { // kun tuotten poisto -nappia painetaan
            var obj = sender as FrameworkElement;
            var product = obj.DataContext as Product;

            if (product != null) {
                var result = MessageBox.Show($"Haluatko varmasti poistaa tuotteen \"{product.Name}\"?", "Poista tuote", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes) {
                    var products = this.DataContext as ObservableCollection<Product>;
                    products.Remove(product);

                    if (product.ID != -1) {
                        productsToDelete.Add(product.ID);
                        unsavedChanges = true;
                    }
                }
            }
        }

        private void SaveChanges_Clicked(object sender, RoutedEventArgs e) { // kun tallenna tiedot -nappia painetaan
            var products = this.DataContext as ObservableCollection<Product>;

            foreach (var product in products) {
                if (product.ID == -1) {
                    productsToAdd.Add(product);
                }
            }

            foreach (var productID in productsToDelete) {
                DataRepository.DeleteProduct(productID);
            }

            foreach (var product in productsToAdd) {
                DataRepository.InsertProduct(product);
                var newProducts = DataRepository.GetProducts();
                product.ID = newProducts.Last().ID;
            }

            foreach (var product in products) {
                DataRepository.UpdateProduct(product);
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
