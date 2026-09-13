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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private string currentCaptcha;

        public RegistrationPage()
        {
            InitializeComponent();
            GenerateNewCaptcha();
        }

        private void GenerateNewCaptcha()
        {
            string chars = "abcdefghijklmNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            currentCaptcha = "";

            for (int i = 0; i < 5; i++)
            {
                int index = random.Next(chars.Length);
                currentCaptcha += chars[index];
            }

            CaptchaTextBlock.Text = currentCaptcha;
        }

        private void RefreshCaptchaBtn_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewCaptcha();
            CaptchaInputBox.Clear();
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            string errors = CheckErrors();

            if (errors != "")
            {
                MessageBox.Show(errors);
                return;
            }

            Users newUser = new Users();
            newUser.Login = TBoxRegLog.Text;
            newUser.Password = TBoxRegPas.Text;
            newUser.Surname = DataEncryption.Encrypt(TBoxRegSurname.Text);
            newUser.Name = DataEncryption.Encrypt(TBoxRegName.Text);
            newUser.Patronymic = DataEncryption.Encrypt(TBoxRegPatronymic.Text);
            newUser.Passport_series = DataEncryption.Encrypt(TBoxRegPasSeries.Text);
            newUser.Passport_number = DataEncryption.Encrypt(TBoxRegPasNumber.Text);
            newUser.Birth_date = DataEncryption.Encrypt(TBoxRegDate.Text);

            App.Context.Users.Add(newUser);
            App.Context.SaveChanges();

            MessageBox.Show("Регистрация завершена!");
            NavigationService.GoBack();
        }

        private string CheckErrors()
        {
            string message = "";

            if (string.IsNullOrEmpty(TBoxRegLog.Text))
                message += "Логин не может быть пустым!\n";

            if (TBoxRegLog.Text.Length < 3 || TBoxRegLog.Text.Length > 30)
                message += "Логин должен быть от 3 до 30 символов!\n";

            if (string.IsNullOrEmpty(TBoxRegPas.Text))
                message += "Пароль не может быть пустым!\n";

            else if (TBoxRegPas.Text.Length < 3 || TBoxRegPas.Text.Length > 30)
                message += "Пароль должен быть от 3 до 30 символов!\n";

            if (CaptchaInputBox.Text != currentCaptcha)
                message += "Капча введена неверно!\n";

            if (string.IsNullOrEmpty(TBoxRegSurname.Text))
                message += "Фамилия не может быть пустой!\n";
            else if (TBoxRegSurname.Text.Length < 2 || TBoxRegSurname.Text.Length > 50)
                message += "Фамилия должна быть от 2 до 50 символов!\n";

            if (string.IsNullOrEmpty(TBoxRegName.Text))
                message += "Имя не может быть пустым!\n";
            else if (TBoxRegName.Text.Length < 2 || TBoxRegName.Text.Length > 50)
                message += "Имя должно быть от 2 до 50 символов!\n";

            if (!string.IsNullOrEmpty(TBoxRegPatronymic.Text) && TBoxRegPatronymic.Text.Length > 50)
                message += "Отчество не может быть более 50 символов!\n";

            if (string.IsNullOrEmpty(TBoxRegDate.Text))
                message += "Дата рождения не может быть пустой!\n";

            if (string.IsNullOrEmpty(TBoxRegPasSeries.Text))
                message += "Серия паспорта не может быть пустой!\n";
            else if (TBoxRegPasSeries.Text.Length != 4)
                message += "Серия паспорта должна содержать 4 цифры!\n";

            if (string.IsNullOrEmpty(TBoxRegPasNumber.Text))
                message += "Номер паспорта не может быть пустым!\n";
            else if (TBoxRegPasNumber.Text.Length != 6)
                message += "Номер паспорта должен содержать 6 цифр!\n";


            Users existingUser = App.Context.Users.FirstOrDefault(u => u.Login == TBoxRegLog.Text);
            if (existingUser != null)
            {
                message += "Логин уже занят!\n";
            }
            return message;
        }
    }
}
