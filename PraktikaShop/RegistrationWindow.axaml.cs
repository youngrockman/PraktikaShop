using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using PraktikaShop.Models;
using System;
using System.Linq;

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


    private  bool Validation()
    {
        if (PhoneBox.Text == null || PhoneBox.Text.Length != 10)
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Alarm", "Phone number has 10 symbols", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
             message.ShowAsync();
            return false;
        }

        if (PassportBox.Text == null || PassportBox.Text.Length != 10)
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Alarm", "Passport has 10 symbols", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
             message.ShowAsync();
            return false;
        }   

        if(string.IsNullOrWhiteSpace(PassportBox.Text) || string.IsNullOrEmpty(NameBox.Text) || string.IsNullOrEmpty(PhoneBox.Text) || BirthdayBox.SelectedDate.HasValue != false || string.IsNullOrEmpty(LoginBox.Text) || string.IsNullOrEmpty(PasswordBox.Text))
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Alarm", "Заполните все поля", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            message.ShowAsync();
            return false;
        }
       
        return true;

    }
}