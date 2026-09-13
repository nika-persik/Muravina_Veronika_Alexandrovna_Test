using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFBankID.Infrastructure.Entities;

namespace WPFBankID
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBoxLogin.Text) || string.IsNullOrWhiteSpace(TBoxPassword.Password))
            {
                MessageBox.Show("Поля не должны быть пустыми!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Users user = App.Context.Users.FirstOrDefault(u => u.Login == TBoxLogin.Text && u.Password == TBoxPassword.Password);
            if (user != null)
            {
                NavigationService.Navigate(new PersonalAccountPage(user));
            }
            else
            {
                MessageBox.Show("Пользователь с такими данными не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRegistr_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}
