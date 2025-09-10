using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using PraktikaShop.Models;
using System;
using System.IO;

namespace PraktikaShop;

public partial class EditWindow : Window
{

    private readonly Product _product;


    public EditWindow()
    {
        InitializeComponent();
    }

    public EditWindow(Models.Product? product)
    {
        InitializeComponent();
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


        context.Products.Update(_product);
        await context.SaveChangesAsync();

        var catalogWindow = new CatalogWindow();
        catalogWindow.Show();
        this.Close();



    }

    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        var catalogWinodow = new CatalogWindow();
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


