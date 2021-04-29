using System.Linq;
using System.Windows;

namespace BankDepositors
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BankContext bd;
        public MainWindow()
        {
            InitializeComponent();
            bd = new BankContext();
        }
        private void BLogIn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in bd.Staffs)
            {
                item.Isenabled = false;
            }
            if (TbUsername.Text.Length > 0) // проверяем введён ли логин     
            {
                if (PbPassword.Password.Length > 0) // проверяем введён ли пароль         
                {             // ищем в базе данных пользователя с такими данными         
                     var dtUser =
                        bd.Staffs.FirstOrDefault(s => s.Username.Equals(TbUsername.Text)  & s.Password.Equals(PbPassword.Password));
                    if (dtUser != null) // если такая запись существует       
                    {
                        MessageBox.Show("Пользователь авторизовался"); // говорим, что авторизовался  
                        Menu menu = new Menu();
                        menu.Show();
                        dtUser.Isenabled = true;
                        Staff staff = new Staff();
                        staff = dtUser;
                        bd.SaveChanges();
                        this.Close();
                    }
                    else MessageBox.Show("Пользователь не найден"); // выводим ошибку  
                }
                else MessageBox.Show("Введите пароль"); // выводим ошибку    
            }
            else MessageBox.Show("Введите логин"); // выводим ошибку 
        }
        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    
}
