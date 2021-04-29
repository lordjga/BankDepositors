using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для ChangePassWindow.xaml
    /// </summary>
    public partial class ChangePassWindow : Window
    {
        private Staff staff;
        private BankContext bd;
        public ChangePassWindow(Staff staff)
        {
            InitializeComponent();
            this.staff = staff;
            bd = new BankContext();
            grid.DataContext = staff;
            TblName.Text = staff.Surname + " " + staff.Name;
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
