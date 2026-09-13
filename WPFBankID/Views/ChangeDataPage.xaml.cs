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
    /// Логика взаимодействия для ChangeDataPage.xaml
    /// </summary>
    public partial class ChangeDataPage : Page
    {
        private Users _user;

        public ChangeDataPage(Users user)
        {
            InitializeComponent();
            _user = user;
            FillCurrentData();
        }

        private void FillCurrentData()
        {
            TBoxSurname.Text = DataEncryption.Decrypt(_user.Surname);
            TBoxName.Text = DataEncryption.Decrypt(_user.Name);
            TBoxPatronymic.Text = DataEncryption.Decrypt(_user.Patronymic);
            TBoxPassSeries.Text = DataEncryption.Decrypt(_user.Passport_series);
            TBoxPassNumber.Text = DataEncryption.Decrypt(_user.Passport_number);
            TBoxBirthDate.Text = DataEncryption.Decrypt(_user.Birth_date);
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            bool isChanged = TBoxSurname.Text != DataEncryption.Decrypt(_user.Surname) ||
                             TBoxName.Text != DataEncryption.Decrypt(_user.Name) ||
                             TBoxPatronymic.Text != DataEncryption.Decrypt(_user.Patronymic) ||
                             TBoxPassSeries.Text != DataEncryption.Decrypt(_user.Passport_series) ||
                             TBoxPassNumber.Text != DataEncryption.Decrypt(_user.Passport_number) ||
                             TBoxBirthDate.Text != DataEncryption.Decrypt(_user.Birth_date);

            if (!isChanged)
            {
                MessageBox.Show("Данные не были изменены");
                return;
            }

            Requests changeReq = new Requests();
            changeReq.Id_user = _user.Id_user;
            changeReq.Id_request_type = 4;
            changeReq.Request_date = DateTime.Now;
            changeReq.Request_status = "В обработке";

            App.Context.Requests.Add(changeReq);
            App.Context.SaveChanges();

            MessageBox.Show("Отправить заявку на изменение персональных данных");
            NavigationService.GoBack();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
