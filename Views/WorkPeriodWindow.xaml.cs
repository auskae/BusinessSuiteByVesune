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
    /// Interaction logic for WorkPeriodWindow.xaml
    /// </summary>
    public partial class WorkPeriodWindow : Window
    {
        public WorkPeriodWindow()
        {
            InitializeComponent();
        }

        public WorkPeriodWindow(User user)
        {
            InitializeComponent();
            this.DataContext = user;
            string clockContent = user.Working ? "Clock Out" : "Clock In";
            this.BtnClock.Content = clockContent;
        }

        private void BtnClock_Click(object sender, RoutedEventArgs e)
        {
            int result = 0;
            User currentUser = (User)this.DataContext;

            if (currentUser.WorkPeriodId > 0)
            {
                // clock out
                WorkPeriodCRUD crud = new WorkPeriodCRUD();
                WorkPeriod period = crud.Read(currentUser.WorkPeriodId);
                period.End = DateTime.Now;
                int clockOutIsComplete = crud.Update(period);

                if (clockOutIsComplete > 0)
                {
                    currentUser.WorkPeriodId = 0;
                    currentUser.Working = false;
                    new UserCRUD().Update(currentUser);
                }
                this.BtnClock.Content = "Clock In";
            }
            else
            {
                // clock in
                WorkPeriod period = new WorkPeriod()
                {
                    UserId = Convert.ToInt32(TxtUserId.Text),
                    Start = DateTime.Now,
                    End = null,
                };
                result = new WorkPeriodCRUD().Create(period);
                this.TxtWorkPeriodId.Text = result.ToString();

                currentUser.WorkPeriodId = result;
                currentUser.Working = true;
                new UserCRUD().Update(currentUser);

                this.BtnClock.Content = "Clock Out";
            }

            
        }
    }
}
