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
using MsBox.Avalonia.Enums;

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

        if (Validation() == true)
        {

            context.Products.Update(_product);
            await context.SaveChangesAsync();

            var catalogWindow = new CatalogWindow(currentUserId);
            catalogWindow.Show();
            this.Close();
        }
    }

    private bool Validation()
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text) || 
            string.IsNullOrWhiteSpace(CostBox.Text) || 
            string.IsNullOrWhiteSpace(CountBox.Text))
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Все поля должны быть заполнены",
                ButtonEnum.Ok,MsBox.Avalonia.Enums.Icon.Error);
            message.ShowWindowDialogAsync(this);
            return false;
        }
        
        if (!int.TryParse(CostBox.Text, out int cost) || !int.TryParse(CountBox.Text, out int count))
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Цена и количество должны быть числами",
                ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            message.ShowWindowDialogAsync(this);
            return false;
        }

        if (cost > 300000)
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Слишком высокая цена",
                ButtonEnum.Ok,MsBox.Avalonia.Enums.Icon.Error);
            message.ShowWindowDialogAsync(this);
            return false;
        }

        if (count > 10000)
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Слишком большое количество",
                ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
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
        try
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Выберите изображение",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Изображения")
                    {
                        Patterns = new[] { "*.jpg", "*.jpeg", "*.png", "*.bmp" }
                    }
                }
            });

           
            if (files != null && files.Count > 0)
            {
                MainImage.Source = new Bitmap(files[0].Path.LocalPath);

                string nameImage = Guid.NewGuid().ToString("N");
                string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shop",
                    nameImage + Path.GetExtension(files[0].Name));

                

                File.Copy(files[0].Path.LocalPath, targetPath);
                _product.Image = targetPath;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при изменении изображения: {ex.Message}");
        }
    }
}
    





