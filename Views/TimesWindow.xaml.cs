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

namespace BusinessSuiteByVesune.Views
{
    /// <summary>
    /// Interaction logic for TimesWindow.xaml
    /// </summary>
    public partial class TimesWindow : Window
    {
        public TimesWindow()
        {
            InitializeComponent();
            LbUsers.ItemsSource = new UserCRUD().GetUsers();
            LbUsers.DisplayMemberPath = "Name";
            LbUsers.SelectedValuePath = "UserId";
        }

        public bool DataSelected()
        {
            if (dgData.SelectedItems.Count > 0)
            {
                WorkPeriod currentItem = (WorkPeriod)dgData.SelectedItems[0];

                return currentItem.WorkPeriodId > 0;
            }
            else
            {
                MessageBox.Show("Select an item from the grid to work with", "Notice");
                return false;
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<WorkPeriod> periods = new List<WorkPeriod>();

            if (LbUsers.SelectedItem != null)
            {
                User user = (User)LbUsers.SelectedItem;

                DateTime startDate = (DateTime)dpStartDate.SelectedDate;
                DateTime endDate = (DateTime)dpEndDate.SelectedDate;

                periods = new WorkPeriodCRUD().GetWorkPeriodsByUserId(user.UserId, startDate, endDate);

                for (int i = 0; i < periods.Count; i++)
                {
                    if (periods[i].End != null)
                    {
                        DateTime periodStartDate = periods[i].Start;
                        DateTime periodEndDate = (DateTime)periods[i].End;
                        int hours = periods[i].WorkPeriodTime.Value.Hours;
                        int minutes = periods[i].WorkPeriodTime.Value.Minutes;
                        
                        // set fields
                        periods[i].WorkPeriodTime = periodEndDate.Subtract(periods[i].Start);
                        periods[i].WorkPeriodReadable = hours + "Hours and " + minutes + " minutes";
                        periods[i].StartHour = periods[i].Start.Hour;
                        periods[i].StartMinute = periods[i].Start.Minute;
                        periods[i].EndHour = periodEndDate.Hour;
                        periods[i].EndMinute = periodEndDate.Minute;
                    }
                }
            }

            dgData.ItemsSource = periods;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (DataSelected())
            {
                new Times.Update((WorkPeriod)dgData.SelectedItems[0]).Show();
            }
        }
    }
}
