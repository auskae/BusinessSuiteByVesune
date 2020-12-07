using BusinessSuiteByVesune.CRUD;
using BusinessSuiteByVesune.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace BusinessSuiteByVesune.Views
{
    /// <summary>
    /// Interaction logic for OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        public OrdersWindow()
        {
            InitializeComponent();
            BindDataGrid();
        }

        public void BindDataGrid()
        {
            dgData.ItemsSource = new OrderCRUD().GetOrders();
        }

        public void BindDataGrid(List<Order> orders)
        {
            dgData.ItemsSource = orders;
        }

        public Order GetSelectedOrder()
        {
            Order order = new Order();

            if (dgData.SelectedItems.Count > 0)
            {
                order = (Order)dgData.SelectedItems[0];
            }
            else
            {
                MessageBox.Show("Please select an order from the Order Grid to work with", "Information");
            }
            return order;
        }

        public void OrdersSearch(SearchWindow window)
        {
            string name = window.TxtSearchOrderName.Text;
            string price = window.TxtSearchOrderPrice.Text;
            string quantity = window.TxtSearchOrderQuantity.Text;
            string statusId = window.CbOrderStatusId.Text;

            List<Order> order = new OrderCRUD().GetOrders();

            if (!String.IsNullOrEmpty(name))
            {
                order.RemoveAll(x => x.Name != name);
            }

            if (!String.IsNullOrEmpty(price))
            {
                order.RemoveAll(x => x.Price != Convert.ToDecimal(price));
            }

            if (!String.IsNullOrEmpty(quantity))
            {
                order.RemoveAll(x => x.Quantity != Convert.ToInt32(quantity));
            }

            if (!String.IsNullOrEmpty(statusId))
            {
                order.RemoveAll(x => x.OrderStatusId != Convert.ToInt32(statusId));
            }

            if (order.Count > 0)
            {
                BindDataGrid(order);
                window.Close();
            }
            else
            {
                MessageBox.Show("There are no results", "Information");
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            Views.SearchWindow window = new SearchWindow("2");
            window.Show();
            window.BtnGoSearch.Click += (s, ev) =>
            {
                OrdersSearch(window);
            };
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = dgData;
            dg.SelectAllCells();
            dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dg);
            dg.UnselectAllCells();
            String Clipboardresult = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            StreamWriter swObj = new StreamWriter("exportToExcel.csv");
            swObj.WriteLine(Clipboardresult);
            swObj.Close();
            Process.Start("exportToExcel.csv");
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            Orders.Create window = new Orders.Create();
            window.Show();
            window.Closing += (s, ev) =>
            {
                BindDataGrid();
            };
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedOrder().OrderId > 0)
            {
                Orders.Read window = new Orders.Read(GetSelectedOrder());
                window.Show();
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedOrder().OrderId > 0)
            {
                Orders.Update window = new Orders.Update(GetSelectedOrder());
                window.Show();
                window.Closing += (s, ev) =>
                {
                    BindDataGrid();
                };
            }
        }

        private void BtnDestroy_Click(object sender, RoutedEventArgs e)
        {
            int result = 0;

            if (GetSelectedOrder().OrderId > 0)
            {
                result = new OrderCRUD().Destroy(GetSelectedOrder().OrderId);
            }

            if (result > 0)
            {
                BindDataGrid();
            }
            else
            {
                MessageBox.Show("Order was deleted", "Success");
            }
        }
    }
}
