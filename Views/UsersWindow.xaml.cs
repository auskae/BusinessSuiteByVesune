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
    /// Interaction logic for UsersWindow.xaml
    /// </summary>
    public partial class UsersWindow : Window
    {
        public UsersWindow()
        {
            InitializeComponent();
            BindDataGrid();
        }

        public void BindDataGrid()
        {
            dgData.ItemsSource = new UserCRUD().GetUsers();
        }
        
        public void BindDataGrid(List<User> users)
        {
            dgData.ItemsSource = users;
        }

        public User GetSelectedUser()
        {
            User user = new User();
            if (dgData.SelectedItems.Count > 0)
            {
                user = (User)dgData.SelectedItems[0];
            }
            else
            {
                MessageBox.Show("Select a user from the user grid to work with", "Information");
            }
            return user;
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
            Users.Create window = new Users.Create();
            window.Show();
            window.Closing += (s, ev) =>
            {
                BindDataGrid();
            };
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedUser().UserId > 0)
            {
                Users.Read window = new Users.Read(GetSelectedUser());
                window.Show();
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedUser().UserId > 0)
            {
                Users.Update window = new Users.Update(GetSelectedUser());
                window.Show();
                window.Closing += (s, ev) =>
                {
                    BindDataGrid();
                };
            }
        }

        private void BtnDestroy_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete this user?", "Warning", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                int result = new UserCRUD().Destroy(GetSelectedUser().UserId);

                if (result > 0)
                {
                    MessageBox.Show("User was deleted", "Success");
                    BindDataGrid();
                }
                else
                {
                    MessageBox.Show("Unable to delete user", "Failure");
                }

            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            Views.SearchWindow window = new SearchWindow("4");
            window.Show();
            window.BtnGoSearch.Click += (s, ev) =>
            {
                UserSearch(window);
            };
        }

        public void UserSearch(SearchWindow window)
        {
            string name = window.TxtSearchUserName.Text;
            string pin = window.TxtSearchUserPin.Text;
            bool? working = window.ChkSearchUserWorking.IsChecked;

            List<User> users = new UserCRUD().GetUsers();

            if (!String.IsNullOrEmpty(name))
            {
                users.RemoveAll(x => x.Name != name);
            }

            if (!String.IsNullOrEmpty(pin))
            {
                users.RemoveAll(x => x.Pin != pin);
            }

            if (working != null)
            {
                users.RemoveAll(x => x.Working != working);
            }

            if (users.Count > 0)
            {
                BindDataGrid(users);
                window.Close();
            }
            else
            {
                MessageBox.Show("There are no results", "Information");
            }
        }
    }
}
