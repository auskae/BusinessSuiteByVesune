using BusinessSuiteByVesune.CRUD;
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
    /// Interaction logic for InvoicesWindow.xaml
    /// </summary>
    public partial class InvoicesWindow : Window
    {
        public InvoicesWindow()
        {
            InitializeComponent();
            BindDataGrid();
        }

        public void BindDataGrid()
        {
            this.dgData.ItemsSource = new InvoiceCRUD().GetInvoices();
        }
        
        public void BindDataGrid(List<Objects.Invoice> invoices)
        {
            this.dgData.ItemsSource = invoices;
        }

        public Objects.Invoice GetSelectedInvoice()
        {
            Objects.Invoice selectedInvoice = new Objects.Invoice();

            if (dgData.SelectedItems.Count > 0)
            {
                selectedInvoice = (Objects.Invoice)dgData.SelectedItems[0];
            }
            else
            {
                MessageBox.Show("Select an invoice from the invoice grid to work with", "Information");
            }
            return selectedInvoice;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            Invoice.Create window = new Invoice.Create();
            window.Show();
            window.Closing += (s, ev) =>
            {
                BindDataGrid();
            };
        }

        public void InvoiceSearch(SearchWindow window)
        {
            string name = window.TxtSearchInvoiceName.Text;
            string statusId = window.CbSearchInvoiceStatus.Text;

            List<Objects.Invoice> invoices = new InvoiceCRUD().GetInvoices();

            if (!String.IsNullOrEmpty(name))
            {
                invoices.RemoveAll(x => x.InvoiceName != name);
            }

            if (!String.IsNullOrEmpty(statusId))
            {
                invoices.RemoveAll(x => x.InvoiceTypeId != Convert.ToInt32(statusId));
            }

            if (invoices.Count > 0)
            {
                BindDataGrid(invoices);
                window.Close();
            }
            else
            {
                MessageBox.Show("There are no results", "Information");
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            Views.SearchWindow window = new SearchWindow("5");
            window.Show();
            window.BtnGoSearch.Click += (s, ev) =>
            {
                InvoiceSearch(window);
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

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedInvoice().InvoiceId > 0)
            {
                Objects.Invoice selectedItem = (Objects.Invoice)dgData.SelectedItems[0];
                new Invoice.Read(selectedItem).Show();
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedInvoice().InvoiceId > 0)
            {
                Objects.Invoice selectedItem = (Objects.Invoice)dgData.SelectedItems[0];
                Invoice.Update window = new Invoice.Update(selectedItem);
                window.Show();
                window.Closing += (s, ev) =>
                {
                    this.BindDataGrid();
                };
            }
        }

        private void BtnDestroy_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedInvoice().InvoiceId > 0)
            {
                Objects.Invoice selectedItem = (Objects.Invoice)dgData.SelectedItems[0];
                new InvoiceCRUD().Destroy(selectedItem.InvoiceId);
                this.BindDataGrid();
            }
        }
    }
}
