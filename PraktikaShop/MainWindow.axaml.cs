using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using PraktikaShop;
using PraktikaShop.Models;
using System.Linq;

namespace PraktikaShop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void LoginClick(object sender, RoutedEventArgs e)
        {
            using var context = new KarpovContext();

            var login = LoginBox.Text;
            var password = PasswordBox.Text;

            var user = context.Users.FirstOrDefault(x => x.Login == login && x.Password == password);

            if (user != null)
            {
                var catalogWindow = new CatalogWindow();
                catalogWindow.Show();
                Close();
            }

            else
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Уведомление", "Неправильный пароль", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);

                await message.ShowAsync();
            }

        }

        private void RegistrationButton(object sender, RoutedEventArgs e)
        {
            var regWindow = new RegistrationWindow();
            regWindow.Show();
            Close();
        }


    }
}