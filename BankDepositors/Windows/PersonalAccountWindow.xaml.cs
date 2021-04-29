using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
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
    /// Логика взаимодействия для Clients.xaml
    /// </summary>
    public partial class PersonalAccountWindow : Window
    {
        BankContext bd;
        private Staff staff;
        public PersonalAccountWindow(Staff staff)
        {
            InitializeComponent();

            bd = new BankContext();
            bd.Staffs.Load();
            this.staff = staff;
            clientsGrid.DataContext = staff;
            TblNameStaff.Text = staff.Surname+" "+staff.Name;
            DateTime now = DateTime.Today;
            TblExperience.Text = (now.Year - staff.StartDate.Year).ToString();
            TblAge.Text = (now.Year - staff.DateOfBirth.Year).ToString()+", День рожд.: "+staff.DateOfBirth.ToShortDateString();
        }

        private void dGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }


        private void BChangeAc_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void BChangeUsPas_Click(object sender, RoutedEventArgs e)
        {
            Staff staff2 = new Staff();
            staff2 = bd.Staffs.FirstOrDefault(p => p.Isenabled);
            ChangePassWindow cpw = new ChangePassWindow(staff2);
            cpw.Owner = this;
            var result = cpw.ShowDialog();
            if (result == true)
            {
                bd.SaveChanges();
                cpw.Close();
            }
            else
            {
                cpw.Close();
            }
        }

        private void Bcancel_Click(object sender, RoutedEventArgs e)
        {
            Menu mw = new Menu();
            mw.Show();
            this.Close();
        }
    }
}
