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
    /// Логика взаимодействия для RequestHistoryPage.xaml
    /// </summary>
    public partial class RequestHistoryPage : Page
    {
        public RequestHistoryPage()
        {
            InitializeComponent();
        }

        private Users _user;

        public RequestHistoryPage(Users user)
        {
            InitializeComponent();
            _user = user;
            ListRequests.ItemsSource = App.Context.Requests.Where(r => r.Id_user == _user.Id_user).ToList();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
