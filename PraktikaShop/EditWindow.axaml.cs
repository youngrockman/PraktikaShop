using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using PraktikaShop.Models;
using System;
using System.IO;

namespace PraktikaShop;

public partial class EditWindow : Window
{

    private readonly Product _product;
    private readonly int currentUserId;

    public EditWindow()
    {
        InitializeComponent();
    }

    public EditWindow(Models.Product? product, int _currentUserId)
    {
        InitializeComponent();
        _currentUserId = currentUserId;
        _product = product;
        NameBox.Text = product.ProductName;
        CountBox.Text = product.Count.ToString();
        CostBox.Text = product.Cost.ToString();
        MainImage.Source = product.ParseImage;
    }

    private async void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        using var context = new KarpovContext();
        _product.ProductName = NameBox.Text;
        _product.Cost = int.Parse(CostBox.Text);
        _product.Count = int.Parse(CountBox.Text);

        if (Validation() == true) { 

        context.Products.Update(_product);
        await context.SaveChangesAsync();

        var catalogWindow = new CatalogWindow(currentUserId);
        catalogWindow.Show();
        this.Close();
        }
    }

    private bool Validation()
    {
        var costBox = int.Parse(CostBox.Text);
        var countBox = int.Parse(CountBox.Text);


        if (NameBox.Text == null || string.IsNullOrEmpty(NameBox.Text) || CostBox.Text == null || string.IsNullOrEmpty(CostBox.Text) || CountBox.Text == null || string.IsNullOrEmpty(CountBox.Text))
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Alarm", "Поля не должны быть пустыми", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            message.ShowWindowDialogAsync(this);
            return false;
        }

        if (costBox > 300000)
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Alarm", "Слишком большая цена", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            message.ShowWindowDialogAsync(this);
            return false;
        }

        if(countBox > 10000)
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Alarm", "Слишком много товара", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            message.ShowWindowDialogAsync(this);
            return false;
        }



        return true;
    }

    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        var catalogWinodow = new CatalogWindow(currentUserId);
        catalogWinodow.Show();
        Close(this);
    }

    private async void ChangeImage(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);


        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Text File",
            AllowMultiple = false
        });

        MainImage.Source = new Bitmap(files[0].Path.LocalPath);

        string nameImage = Guid.NewGuid().ToString("N");
        File.Copy(files[0].Path.LocalPath, AppDomain.CurrentDomain.BaseDirectory + "/shop/" + nameImage);

        _product.Image = "shop/" + nameImage;
    }


}


