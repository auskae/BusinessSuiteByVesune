using BusinessSuiteByVesune.CRUD;
using BusinessSuiteByVesune.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BusinessSuiteByVesune.Views
{
    /// <summary>
    /// Interaction logic for JobsWindow.xaml
    /// </summary>
    public partial class JobsWindow : Window
    {
        public JobsWindow()
        {
            InitializeComponent();
            BindDataGrid();
        }

        public void BindDataGrid()
        {
            dgData.ItemsSource = new JobCRUD().GetJobs();
        }

        public void BindDataGrid(List<Job> jobs)
        {
            dgData.ItemsSource = jobs;
        }

        public Job GetSelectedJob()
        {
            Job selectedJob = new Job();
            if (dgData.SelectedItems.Count > 0)
            {
                selectedJob = (Job)dgData.SelectedItems[0];
            }
            else
            {
                MessageBox.Show("Select a job from the Job Grid to work with", "Information");
            }

            return selectedJob;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            Jobs.Create window = new Jobs.Create();
            window.Show();
            window.Closing += (s, ev) =>
            {
                this.BindDataGrid();
            };
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedJob().JobId > 0)
            {
                Jobs.Read window = new Jobs.Read(GetSelectedJob());
                window.Show();
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedJob().JobId > 0)
            {
                Jobs.Update window = new Jobs.Update();
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
            if (GetSelectedJob().JobId > 0)
            {
                MessageBoxResult mbResult = MessageBox.Show("Deleting a job will also delete any charges for this job. Are you sure you want to delete this job and it's charges?", "Warning", MessageBoxButton.YesNo);
                if (mbResult == MessageBoxResult.Yes)
                {
                    result = new JobCRUD().Destroy(GetSelectedJob().JobId);

                    if (result > 0)
                    {
                        BindDataGrid();
                        MessageBox.Show("The job and it's charges were deleted", "Success");
                    }
                    else
                    {
                        MessageBox.Show("Unable to delete job and it's charges", "Failure");
                    }

                }
            }
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

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            Views.SearchWindow window = new SearchWindow("3");
            window.Show();
            window.BtnGoSearch.Click += (s, ev) =>
            {
                JobSearch(window);
            };
        }

        public void JobSearch(SearchWindow window)
        {
            string name = window.TxtSearchJobName.Text;
            string balanceDue = window.TxtSearchJobBalanceDue.Text;
            string city = window.TxtSearchJobCity.Text;
            bool? mobile = window.ChkSearchJobMobile.IsChecked;
            string statusId = window.CbSearchJobJobStatusId.Text;
            string scheduledDate = window.DpSearchJobScheduledDate.Text;

            List<Job> jobs = new JobCRUD().GetJobs();

            if (!String.IsNullOrEmpty(name))
            {
                jobs.RemoveAll(x => x.Name != name);
            }

            if (!String.IsNullOrEmpty(balanceDue))
            {
                jobs.RemoveAll(x => x.BalanceDue != Convert.ToDecimal(balanceDue));
            }

            if (!String.IsNullOrEmpty(city))
            {
                jobs.RemoveAll(x => x.City != city);
            }

            if (mobile != null)
            {
                jobs.RemoveAll(x => x.Mobile != mobile);
            }

            if (!String.IsNullOrEmpty(statusId))
            {
                jobs.RemoveAll(x => x.JobStatusId != Convert.ToInt32(statusId));
            }

            if (scheduledDate.Length > 0)
            {
                jobs.RemoveAll(x => x.ScheduledDate != Convert.ToDateTime(scheduledDate));
            }

            if (jobs.Count > 0)
            {
                BindDataGrid(jobs);
                window.Close();
            }
            else
            {
                MessageBox.Show("There are no results", "Information");
            }
        }
    }
}
