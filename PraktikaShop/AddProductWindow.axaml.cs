using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using PraktikaShop.Models;
using System;
using System.IO;
using System.Threading.Tasks;

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

    private async void AddImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Text File",
            FileTypeChoices = new[]
        { new FilePickerFileType("Images") { Patterns = new[] { "*.jpg" } } }
        });

        File.Copy(file.Path.LocalPath, AppDomain.CurrentDomain.BaseDirectory + "/shop/" + imageName);


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
        var newProduct = new Product
        {
            ProductName = NameBox.Text,
            Cost =  int.Parse(CostBox.Text),
            Count = int.Parse(CountBox.Text),
            Image = "shop/" + imageName
        };
         
        context.Products.Add(newProduct);
        await context.SaveChangesAsync();

        var catalogWindow = new CatalogWindow(_currentUserId);
        catalogWindow.Show();
        Close();
        
    }
}