using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using PraktikaShop.Models;
using System;
using System.IO;
using System.Linq;

namespace PraktikaShop;

public partial class CatalogWindow : Window
{

    public CatalogWindow()
    {
        InitializeComponent();
        LoadProducts();
    }

    private async void LoadProducts()
    {
        using var context = new KarpovContext();
        var allProducts = await context.Products.ToListAsync();

        //сделать сортировки через свитч + if 



        CatalogListBox.ItemsSource = allProducts;
    }

     
      
}