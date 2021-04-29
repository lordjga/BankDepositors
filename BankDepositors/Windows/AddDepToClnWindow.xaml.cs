using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BankDepositors
{
    /// <summary>
    /// Логика взаимодействия для AddDepToClnWindow.xaml
    /// </summary>
    public partial class AddDepToClnWindow : Window
    {
        private DepositClient depCln;
        private BankContext bd;
        public AddDepToClnWindow(DepositClient depCln)
        {
            InitializeComponent();
            this.depCln = depCln;
            bd = new BankContext();
            grid.DataContext = depCln;

            

            var depList = bd.Deposits.Select(p => new
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
              

            List<string> liststr = new List<string>();
            
                        foreach (var item in depList)
                        {

                            liststr.Add(item.Name.ToString());
                            CbDepToCln.ItemsSource = liststr;
                        }
        }

        private void CbDepToCln_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string select = (CbDepToCln.SelectedItem.ToString());
            var depositId = bd.Deposits.FirstOrDefault(p => p.Name == select);
            TblDepositId.Text = depositId.Id.ToString();
            TblTermDep.Text = depositId.Term.ToString();
            TblEndDate.Text =  DateTime.Today.AddMonths(depositId.Term).ToShortDateString();
            TblPercentDep.Text = depositId.Percent.ToString();
            TblSumm.Text = (double.Parse(TblEnrollment.Text) * Math.Pow((double)(1 + (depositId.Percent) / (100 * 12)), (12 * (double)depositId.Term / 12))).ToString("0.00");

        }

        private void bSaveDep_Click(object sender, RoutedEventArgs e)
        {
            if (TblEnrollment.Text != "" && CbDepToCln.SelectedItem != null)
            {
                depCln.Deposit_Id = Int32.Parse(TblDepositId.Text);
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Не все поля заполнены");
            }
            
        }

        private void bCancelDep_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
    }
}
