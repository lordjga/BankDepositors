using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

namespace BankDepositors
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        BankContext bd;
        ViewModel vm;

        public Menu()
        {
            InitializeComponent();
            bd = new BankContext();
            vm = new ViewModel();
            bd.Clients.Load();
            bd.Staffs.Load();
            bd.Deposits.Load();
            bd.Currencies.Load(); 
            //bd.DepositClients.Load();
        }
        private void dGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void bStaffTabl_Click(object sender, RoutedEventArgs e)
        {
            var staff = new Staff();
            staff = bd.Staffs.FirstOrDefault(p => p.Isenabled);
            PersonalAccountWindow win = new PersonalAccountWindow(staff);
            win.Show();
            this.Close();
        }


        #region Clients

        private void LvClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView orig = (ListView) sender;
            if (orig.SelectedItems.Count == 0) // выделения нет
                return;
            UpdatingInformationClient();
            
        }

        private void BaddClient_Click(object sender, RoutedEventArgs e)
        {
            var client = new Client();
            AddClientWindow add = new AddClientWindow(client);
            add.Owner = this;
            var result = add.ShowDialog();
            
                if (result == true)
                {
                    bd.Clients.Add(client);
                    bd.SaveChanges();
                    add.Close();
                }
                else
                {
                    add.Close();
                }

                LvClients.ItemsSource = vm.ListCln;
        }

        private void MiDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo) ==
                MessageBoxResult.Yes)
            {
                string clientS = LvClients.SelectedItem.ToString();
                int k = clientS.IndexOf('.');
                int clientId = Int32.Parse(clientS.Substring(0, k));
                Client client = bd.Clients.Find(clientId);
                bd.Clients.Remove(client);
                bd.SaveChanges();
                LvClients.ItemsSource = vm.ListCln;
            }
        }

        private void MiChange_Click(object sender, RoutedEventArgs e)
        {
            string clientS = LvClients.SelectedItem.ToString();
            int k = clientS.IndexOf('.');
            int clientId = Int32.Parse(clientS.Substring(0, k));
            Client client = bd.Clients.Find(clientId);

            AddClientWindow add = new AddClientWindow(client);
            add.Owner = this;
            var result = add.ShowDialog();
            if (result == true)
            {
                bd.SaveChanges();
                add.Close();
                UpdatingInformationClient();
                LvClients.ItemsSource = vm.ListCln;
            }
            else
            {
                add.Close();
            }
        }
        private void BaddDepCln_Click(object sender, RoutedEventArgs e)
        {
            if (LvClients.SelectedItems.Count > 0)
            {
                string client = LvClients.SelectedItem.ToString();
                int k = client.IndexOf('.');
                int clientId = Int32.Parse(client.Substring(0, k));
                var clientsSel2 = bd.Clients.FirstOrDefault(p => p.Id == clientId);

                var depCln = new DepositClient();

                depCln.Client = clientsSel2;
                depCln.DateOfEnrollment = DateTime.Today;

                AddDepToClnWindow add = new AddDepToClnWindow(depCln);
                add.Owner = this;
                var result = add.ShowDialog();
                if (result == true)
                {
                    try
                    {
                        bd.DepositClients.Add(depCln);
                        bd.SaveChanges();
                        add.Close();
                    }
                    catch (Exception h)
                    {
                        MessageBox.Show("Такой вклад у клиента уже открыт");
                    }
                }
                else
                {
                    add.Close();
                }

                UpdatingInformationClient();
            }
            else MessageBox.Show("Выделите клиента в списке");

        }
        public void UpdatingInformationClient()
        {
            InitializeInformationClient();
            string client = LvClients.SelectedItem.ToString();
            int k = client.IndexOf('.');
            int clientId = Int32.Parse(client.Substring(0, k));
            var clientsSel2 = bd.Clients.FirstOrDefault(p => p.Id==clientId);

            TblName.Text = clientsSel2.Surname + " " + clientsSel2.Name;
            TblNumber.Text = clientsSel2.PhoneNumber;
            TblPassport.Text = clientsSel2.Passport;
            TblAdress.Text = clientsSel2.City + ", " + clientsSel2.Adress;
            TblStaff.Text = clientsSel2.Staff.Surname + " " + clientsSel2.Staff.Name;

            var table = (from p in bd.DepositClients
                join d in bd.Deposits on p.Deposit_Id equals d.Id
                join c in bd.Clients on p.Client_Id equals c.Id
                where EntityFunctions.AddMonths(p.DateOfEnrollment, d.Term)>DateTime.Today
                select new
                {
                    id = c.Id,
                    name = d.Name,
                    currency = d.Currency.Name,
                    dateOfEnrollment = p.DateOfEnrollment,
                    enrollment = p.Enrollment,
                    endDate = EntityFunctions.AddMonths(p.DateOfEnrollment, d.Term),
                    summ = (double)p.Enrollment * Math.Pow((double)(1 + (d.Percent) / (100 * 12)), (12 * (double)(EntityFunctions.DiffMonths(p.DateOfEnrollment, DateTime.Today)) / 12))
                }).ToList();
            clientsGrid.ItemsSource = table.Where(p => p.id == clientId);
        }
        public void InitializeInformationClient()
        {
            depositsGrid.Visibility = Visibility.Hidden;
            clientsGrid.Visibility = Visibility.Visible;
            BaddDepCln.Visibility = Visibility.Visible;
            Tbl1.Text = "Номер телефона:";
            Tbl2.Text = "Пасспорт:";
            Tbl3.Text = "Адрес:";
            Tbl4.Text = "Отв. Сотрудник:";
        }

        #endregion

        #region Deposits
        private void LvDeposits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView orig = (ListView) sender;
            if (orig.SelectedItems.Count == 0) // выделения нет
                return;
            try
            {
                UpdatingInformationDeposit();
            }
            catch
            {
                LvClients.SelectedItem = -1;
            }
        }

        private void BaddDeposit_Click(object sender, RoutedEventArgs e)
        {
            Deposit deposit = new Deposit();
            AddDepositWindow add = new AddDepositWindow(deposit);
            add.Owner = this;
            var result = add.ShowDialog();
            if (result == true)
            {
                bd.Deposits.Add(deposit);
                bd.SaveChanges();
                add.Close();
            }
            else
            {
                add.Close();
            }
            LvDeposits.ItemsSource = vm.ListDep;
        }

        private void MiDeleteDep_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo) ==
                MessageBoxResult.Yes)
            {
                string deposit = LvDeposits.SelectedItem.ToString();
                int k = deposit.IndexOf('.');
                int depositId = Int32.Parse(deposit.Substring(0, k));
                Deposit depositSel = bd.Deposits.Find(depositId);

                bd.Deposits.Remove(depositSel);
                bd.SaveChanges();
                LvDeposits.ItemsSource = vm.ListDep;
            }
        }

        private void MiChangeDep_Click(object sender, RoutedEventArgs e)
        {
            string deposit = LvDeposits.SelectedItem.ToString();
            int k = deposit.IndexOf('.');
            int depositId = Int32.Parse(deposit.Substring(0, k));
            Deposit depositSel = bd.Deposits.Find(depositId);

            AddDepositWindow add = new AddDepositWindow(depositSel);
            add.Owner = this;
            var result = add.ShowDialog();
            if (result == true)
            {
                bd.SaveChanges();
                add.Close();
                UpdatingInformationDeposit();
                LvDeposits.ItemsSource = vm.ListDep;
            }
            else
            {
                add.Close();
            }
            
        }

        private void UpdatingInformationDeposit()
        {
            string deposit = LvDeposits.SelectedItem.ToString();
            int k = deposit.IndexOf('.');
            int depositId = Int32.Parse(deposit.Substring(0, k));
            var depositSel = bd.Deposits.Find(depositId);
            TblName.Text = depositSel.Name;
            TblNumber.Text = depositSel.Term.ToString();
            TblPassport.Text = depositSel.Percent.ToString();
            TblAdress.Text = depositSel.Currency.Name.ToString();
            TblStaff.Text = null;
            InitializeInformationDeposit();
            var table = (from p in bd.DepositClients
                join d in bd.Deposits on p.Deposit_Id equals d.Id
                join c in bd.Clients on p.Client_Id equals c.Id
                where EntityFunctions.AddMonths(p.DateOfEnrollment, d.Term) > DateTime.Today
                &&  d.Id == depositId
                select new
                {
                    id = d.Id,
                    surname = c.Surname,
                    name = c.Name,
                    dateOfEnrollment = p.DateOfEnrollment,
                    enrollment = p.Enrollment,
                    endDate = EntityFunctions.AddMonths(p.DateOfEnrollment, d.Term),
                    summ = (double)p.Enrollment * Math.Pow((double)(1 + (d.Percent) / (100 * 12)), (12 * (double)(EntityFunctions.DiffMonths(p.DateOfEnrollment, DateTime.Today)) / 12))
                }).ToList();
            depositsGrid.ItemsSource = table; //.Where(p => p.id == depositId);
        }

        public void InitializeInformationDeposit()
        {
            depositsGrid.Visibility = Visibility.Visible;
            clientsGrid.Visibility = Visibility.Hidden;
            Tbl1.Text = "Срок вклада, мес.:";
            Tbl2.Text = "Годовой процент, %:";
            Tbl3.Text = "Валюта:";
            Tbl4.Text = "";
            BaddDepCln.Visibility = Visibility.Hidden;
        }
        #endregion



        private void MenuItemStaffAcc_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
