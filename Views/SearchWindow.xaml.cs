using BusinessSuiteByVesune.Objects;
using BusinessSuiteByVesune.CRUD;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

namespace BusinessSuiteByVesune.Views
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow(string searchType)
        {
            InitializeComponent();

            this.TxtSearchType.Text = searchType;
            this.DataContext = new Search();

            switch (searchType)
            {
                case "1":
                    gInventory.Visibility = Visibility.Visible;
                    break;
                case "2":
                    gOrders.Visibility = Visibility.Visible;
                    break;
                case "3":
                    gJobs.Visibility = Visibility.Visible;
                    break;
                case "4":
                    gUsers.Visibility = Visibility.Visible;
                    break;
                case "5":
                    gInvoice.Visibility = Visibility.Visible;
                    break;
                case "6":
                    // this is searched on page
                    break;
                default:
                    break;
            }
        }

        private void BtnCancelSearch_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnGoSearch_Click(object sender, RoutedEventArgs e)
        {
            // handled in parent window
        }
    }
}
