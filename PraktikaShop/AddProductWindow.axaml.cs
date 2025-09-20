using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using PraktikaShop.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace PraktikaShop;

public partial class AddProductWindow : Window
{
    private readonly int _currentUserId;

    public AddProductWindow()
    {
        InitializeComponent();
    }


    public AddProductWindow(int currentUserId)
    {
        InitializeComponent();
        _currentUserId = currentUserId;
    }

    string imageName = Guid.NewGuid().ToString("N");

    private async void AddImage_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Сохранить изображение",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("Изображения")
                    {
                        Patterns = new[] { "*.jpg", "*.jpeg", "*.png", "*.bmp" }
                    }
                }
            });

            if (file != null)
            {
                string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shop",
                    imageName + Path.GetExtension(file.Name));
                File.Copy(file.Path.LocalPath, targetPath);
                imageName = targetPath;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении изображения: {ex.Message}");
        }
    }

    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var catalogWindow = new CatalogWindow(_currentUserId);
        catalogWindow.Show();
        Close(this);

    }

    private async void AddProduct_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new KarpovContext();

        if (Validation() == true)
        {
            var newProduct = new Product
            {
                ProductName = NameBox.Text,
                Cost = int.Parse(CostBox.Text),
                Count = int.Parse(CountBox.Text),
                Image = imageName
            };

            context.Products.Add(newProduct);
            await context.SaveChangesAsync();

            var catalogWindow = new CatalogWindow(_currentUserId);
            catalogWindow.Show();
            Close();
        }
    }

    private bool Validation()
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text) || 
            string.IsNullOrWhiteSpace(CostBox.Text) || 
            string.IsNullOrWhiteSpace(CountBox.Text))
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Все поля должны быть заполнены",
                ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            message.ShowAsync();
            return false;
        }
        
        if (!int.TryParse(CostBox.Text, out int cost) || !int.TryParse(CountBox.Text, out int count))
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Цена и количество должны быть числами",
                ButtonEnum.Ok,  MsBox.Avalonia.Enums.Icon.Error);
            message.ShowAsync();
            return false;
        }

        if (cost > 700000 || cost <= 0)
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Некорректная стоимость товара",
                ButtonEnum.Ok,  MsBox.Avalonia.Enums.Icon.Error);
            message.ShowAsync();
            return false;
        }

        if (count > 100000 || count <= 0)
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Некорректное количество товара",
                ButtonEnum.Ok,  MsBox.Avalonia.Enums.Icon.Error);
            message.ShowAsync();
            return false;
        }
    
        return true;
    }

}