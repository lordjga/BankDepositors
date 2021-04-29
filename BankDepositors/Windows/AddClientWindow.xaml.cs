using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BankDepositors
{

    /// <summary>
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        private Client client;
        private BankContext bd;
        public AddClientWindow(Client client)
        {
            InitializeComponent();
            this.client = client;
            bd = new BankContext();
            grid.DataContext = client;

            
            var staff = bd.Staffs.Select(p => new
            {
                Id = p.Id,
                Surname = p.Surname
            }).ToList();

            List<string> liststr = new List<string>();
            
            foreach (var item in staff)
            {
                liststr.Add(item.Surname.ToString());
                CbStaffs.ItemsSource = liststr;
            }
            
            if (client.StaffId>0)
            {
                CbStaffs.SelectedItem = client.Staff.Surname;
            }
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            if (TbName.Text!=""&& TbSurname.Text != "" && TbPhoneNumber.Text != "" &&
                TbPassport.Text != "" && TbCity.Text != "" && TbAdress.Text != "" && CbStaffs.SelectedItem!=null)
            {
                client.StaffId = Int32.Parse(TblStaffId.Text);
                this.DialogResult = true;
            }else
            {
                MessageBox.Show("Не все поля заполнены");
            }

        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        private void CbStaffs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string select = (CbStaffs.SelectedItem.ToString());
            var staffId = bd.Staffs.FirstOrDefault(p => p.Surname == select);
            TblStaffId.Text = staffId.Id.ToString();
        }
    }
}