using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.IO;
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
    /// Логика взаимодействия для PersonalAccountPage.xaml
    /// </summary>
    public partial class PersonalAccountPage : Page
    {
        private Users _user;

        public PersonalAccountPage(Users user)
        {
            InitializeComponent();
            _user = user;
            ShowData();
            LoadAccounts();
        }

        private void ShowData()
        {
            if (_user == null) 
                return;
                TxtFio.Text = $"ФИО: {DataEncryption.Decrypt(_user.Surname)} {DataEncryption.Decrypt(_user.Name)} {DataEncryption.Decrypt(_user.Patronymic)}";
                TxtPassport.Text = $"Паспорт: {DataEncryption.Decrypt(_user.Passport_series)} {DataEncryption.Decrypt(_user.Passport_number)}";
                TxtDate.Text = $"Дата рождения: {DataEncryption.Decrypt(_user.Birth_date)}";
        }

        private void LoadAccounts()
        {
            List<BankAccounts> accounts = App.Context.BankAccounts.Where(a => a.Id_user == _user.Id_user).ToList();
            ListAccounts.ItemsSource = accounts;
        }

        private void BtnChangeData_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ChangeDataPage(_user));
        }

        private void BtnRequestsHistory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RequestHistoryPage(_user));
        }

        private void BtnOpenProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ComboNewProduct.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите тип заявки из списка.");
                return;
            }

            Requests newRequest = new Requests();
            newRequest.Id_user = _user.Id_user;
            newRequest.Request_date = DateTime.Now;
            newRequest.Request_status = "В обработке";

            string message = "";

            if (ComboNewProduct.SelectedIndex == 1)
            {
                newRequest.Id_request_type = 2;
                message = "Заявка на открытие счёта отправлена";
            }
            else if (ComboNewProduct.SelectedIndex == 0)
            {
                newRequest.Id_request_type = 1;
                message = "Заявка на открытие вклада отправлена";
            }


            App.Context.Requests.Add(newRequest);
            App.Context.SaveChanges();

            MessageBox.Show(message);
        }

        private void BtnCloseProduct_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            BankAccounts account = btn.DataContext as BankAccounts;

            if (account == null) return;

            string typeTitle = "Закрытие счета";
            if (account.AccountTypes != null && account.AccountTypes.Title.Contains("Вклад"))
            {
                typeTitle = "Закрытие вклада";
            }

            RequestTypes reqType = App.Context.RequestTypes.FirstOrDefault(t => t.Title == typeTitle);

            if (reqType != null)
            {
                Requests closeReq = new Requests();
                closeReq.Id_user = _user.Id_user;
                closeReq.Id_request_type = reqType.Id;
                closeReq.Request_date = DateTime.Now;
                closeReq.Request_status = "В обработке";

                App.Context.Requests.Add(closeReq);
                App.Context.SaveChanges();

                MessageBox.Show("Заявка на закрытие принята.");
            }
        }

        private void BtnExtract_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            BankAccounts account = btn.DataContext as BankAccounts;
            if (account == null) return;

            string report = "ВЫПИСКА ПО БАНКОВСКОМУ СЧЕТУ\n\n";
            report += "Владелец: " + TxtFio.Text.Replace("ФИО: ", "") + "\n";
            report += "Дата выписки: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + "\n\n";

            report += "Тип продукта: " + account.AccountTypes.Title + "\n";
            report += "Номер счета: " + account.Id_account + "\n";
            report += "Текущий баланс: " + account.Balance + " " + account.Currency + "\n";
            report += "Процентная ставка: " + account.Interest_rate + "%\n";
            report += "Дата открытия вклада: " + account.Date_opened + "\n";
            report += "Дата закрытия вклада: " + account.Closing_date + "\n";

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt";
            sfd.FileName = "Выписка_" + account.Id_account;

            if (sfd.ShowDialog() == true)
            {
                File.WriteAllText(sfd.FileName, report);
                MessageBox.Show("Выписка сохранена в файл.");
            }
        }
    }
}