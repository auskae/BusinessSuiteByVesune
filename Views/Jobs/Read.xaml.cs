using BusinessSuiteByVesune.CRUD;
using BusinessSuiteByVesune.Objects;
using System.Windows;

namespace BusinessSuiteByVesune.Views.Jobs
{
    /// <summary>
    /// Interaction logic for Read.xaml
    /// </summary>
    public partial class Read : Window
    {
        public int JobId;

        public Read()
        {
            InitializeComponent();
        }

        public Read(Job job)
        {
            InitializeComponent();
            this.DataContext = job;
            this.JobId = job.JobId;
            BindGridData(job.JobId);

            cbJobType.ItemsSource = new JobTypeCRUD().GetJobTypes();
            cbJobType.SelectedValuePath = "JobTypeId";
            cbJobType.DisplayMemberPath = "JobTypeName";

            cbJobStatusType.ItemsSource = new JobTypeCRUD().GetJobStatusTypes();
            cbJobStatusType.SelectedValuePath = "JobStatusTypeId";
            cbJobStatusType.DisplayMemberPath = "JobStatusTypeName";
        }

        public void BindGridData(int jobId)
        {
            this.dgChargesData.ItemsSource = new ChargeCRUD().GetChargesByJobId(jobId);
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            Charges.CreateChargeWindow window = new Charges.CreateChargeWindow(this.JobId);
            window.Show();
            window.Closing += (s, ev) =>
            {
                BindGridData(this.JobId);
            };
        }
    }
}
