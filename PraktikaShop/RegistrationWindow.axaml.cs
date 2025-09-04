using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
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