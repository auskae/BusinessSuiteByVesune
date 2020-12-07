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

namespace BusinessSuiteByVesune.Views.Times
{
    /// <summary>
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class Update : Window
    {
        public Update()
        {
            InitializeComponent();
        }

        public Update(WorkPeriod period)
        {
            InitializeComponent();
            this.DataContext = period;
        }

        public void ReloadTotalTime_LostFocus(object sender, RoutedEventArgs e)
        {
            int startHour = String.IsNullOrEmpty(TxtStartHour.Text) == true ? 0 : Convert.ToInt32(TxtStartHour.Text);
            int endHour = String.IsNullOrEmpty(TxtEndHour.Text) == true ? 0 : Convert.ToInt32(TxtEndHour.Text);
            int startMinute = String.IsNullOrEmpty(TxtStartMinute.Text) == true ? 0 : Convert.ToInt32(TxtStartMinute.Text);
            int endMinute = String.IsNullOrEmpty(TxtEndMinute.Text) == true ? 0 : Convert.ToInt32(TxtEndMinute.Text);

            TimeSpan start = new TimeSpan(startHour, startMinute, 0);
            TimeSpan end = new TimeSpan(endHour, endMinute, 0);

            TimeSpan total = end.Subtract(start);

            string result = total.Hours + " hours and " + total.Minutes + " minutes";

            LblTotal.Content = result;

        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            int startHour = String.IsNullOrEmpty(TxtStartHour.Text) == true ? 0 : Convert.ToInt32(TxtStartHour.Text);
            int endHour = String.IsNullOrEmpty(TxtEndHour.Text) == true ? 0 : Convert.ToInt32(TxtEndHour.Text);
            int startMinute = String.IsNullOrEmpty(TxtStartMinute.Text) == true ? 0 : Convert.ToInt32(TxtStartMinute.Text);
            int endMinute = String.IsNullOrEmpty(TxtEndMinute.Text) == true ? 0 : Convert.ToInt32(TxtEndMinute.Text);

            DateTime startDate = Convert.ToDateTime(TxtStartDate.Text);
            DateTime endDate = Convert.ToDateTime(TxtEndDate.Text);

            DateTime newStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startHour, startMinute, 0);
            DateTime newEndDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, endHour, endMinute, 0);

            WorkPeriod updatedPeriod = (WorkPeriod)this.DataContext;

            updatedPeriod.Start = newStartDate;
            updatedPeriod.End = newEndDate;

            int result = new WorkPeriodCRUD().Update(updatedPeriod);

            if (result > 0)
            {
                MessageBox.Show("Work period updated", "Success");
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to update work period", "Failure");
            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
