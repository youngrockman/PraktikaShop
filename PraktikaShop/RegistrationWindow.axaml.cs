using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using PraktikaShop.Models;
using System;
using System.Linq;
using MsBox.Avalonia.Enums;

namespace PraktikaShop;

public partial class RegistrationWindow : Window
{
    public RegistrationWindow()
    {
        InitializeComponent();

    }

    private void BackButton(object sender, RoutedEventArgs e)
    {
        var backButton = new MainWindow();
        backButton.Show();
        Close();
    }

    private async void RegistrationButton(object sender, RoutedEventArgs e)
    {
        using var context = new KarpovContext();
        var maxUserId = context.Users.Count();

        if (Validation() == true)
        {

            var newUser = new User
            {
                /*     UserId = context.Users.OrderBy(x=>x.UserId).LastOrDefault().UserId + 1,*/
                Fullname = NameBox.Text,
                Phone = PhoneBox.Text,
                RoleId = 2,
                Passport = PasswordBox.Text,
                Birthday = new DateOnly(BirthdayBox.SelectedDate.Value.Year, BirthdayBox.SelectedDate.Value.Month, BirthdayBox.SelectedDate.Value.Day),
                Login = LoginBox.Text,
                Password = PasswordBox.Text
            };


            context.Users.Add(newUser);
            await context.SaveChangesAsync();


            var maxUserid = newUser.UserId;


            var newBasket = new Basket
            {
                UserId = maxUserid
            };

            context.Baskets.Add(newBasket);
            await context.SaveChangesAsync();

            Close(this);
            var goBackWindow = new MainWindow();
            goBackWindow.Show();
        }
    }


    private bool Validation()
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text) || 
            string.IsNullOrWhiteSpace(PhoneBox.Text) ||
            string.IsNullOrWhiteSpace(PassportBox.Text) ||
            string.IsNullOrWhiteSpace(LoginBox.Text) ||
            string.IsNullOrWhiteSpace(PasswordBox.Text) ||
            !BirthdayBox.SelectedDate.HasValue)
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Все поля должны быть заполнены",
                ButtonEnum.Ok,  MsBox.Avalonia.Enums.Icon.Error);
            message.ShowAsync();
            return false;
        }
        if (PhoneBox.Text.Length != 10 || !PhoneBox.Text.All(char.IsDigit))
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Номер телефона должен содержать 10 цифр",
                ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            message.ShowAsync();
            return false;
        }
        
        if (PassportBox.Text.Length != 10 || !PassportBox.Text.All(char.IsDigit))
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Серия и номер паспорта должны содержать 10 цифр",
                ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            message.ShowAsync();
            return false;
        }
        
        using var context = new KarpovContext();
        if (context.Users.Any(u => u.Login == LoginBox.Text))
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Пользователь с таким логином уже существует",
                ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            message.ShowAsync();
            return false;
        }

        return true;
    }
}