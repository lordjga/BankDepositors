using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddDepositWindow.xaml
    /// </summary>
    public partial class AddDepositWindow : Window
    {
        private Deposit deposit;
        private BankContext bd;
        public AddDepositWindow(Deposit deposit)
        {
            InitializeComponent();
            this.deposit = deposit;
            bd = new BankContext();
            grid.DataContext = deposit;

            var currency = bd.Currencies.Select(p => new
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();

            List<string> liststr = new List<string>();

            foreach (var item in currency)
            {
                liststr.Add(item.Name.ToString());
                CbCurrencies.ItemsSource = liststr;
            }

            if (deposit.CurrencyId > 0)
            {
                CbCurrencies.SelectedItem = deposit.Currency.Name;
            }
        }

        private void bSaveDep_Click(object sender, RoutedEventArgs e)
        {
            if (TbNameDep.Text != "" && TbPercentDep.Text != "" && TbTermDep.Text != "" &&
                CbCurrencies.SelectedItem != null)
            {
                deposit.CurrencyId = Int32.Parse(TblCurrencyId.Text);
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

        private void CbCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string select = (CbCurrencies.SelectedItem.ToString());
            var currencyId = bd.Currencies.FirstOrDefault(p => p.Name == select);
            TblCurrencyId.Text = currencyId.Id.ToString();
        }
    }
}
