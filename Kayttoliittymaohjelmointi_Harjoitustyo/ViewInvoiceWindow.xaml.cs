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
    public partial class ViewInvoiceWindow : Window
    {
        public ViewInvoiceWindow(Invoice invoice)
        {
            InitializeComponent();

            this.DataContext = invoice;
        }
    }
}
