using BusinessSuiteByVesune.CRUD;
using BusinessSuiteByVesune.Objects;
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

namespace BusinessSuiteByVesune.Views.Orders
{
    /// <summary>
    /// Interaction logic for Create.xaml
    /// </summary>
    public partial class Create : Window
    {
        public Create()
        {
            InitializeComponent();
            this.DataContext = new Order();

            CbOrderStatus.ItemsSource = new OrderCRUD().GetOrderStatuses();
            CbOrderStatus.SelectedValuePath = "OrderStatusId";
            CbOrderStatus.DisplayMemberPath = "OrderStatusName";
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Order currentOrder = (Order)this.DataContext;
            int result = new OrderCRUD().Create(currentOrder);
            if (result > 0)
            {
                this.Close();
                MessageBox.Show("Order was created", "Success");
            }
            else
            {
                MessageBox.Show("Failed to create order", "Failure");
            }
        }
    }
}
