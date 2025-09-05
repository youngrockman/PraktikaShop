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
        OrderBox.SelectionChanged += OrderBox_SelectionChanged;
    }

    private void OrderBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        LoadProducts();
    }

    private async void LoadProducts()
    {
        using var context = new KarpovContext();
        var allProducts = await context.Products.ToListAsync();

 

        switch (OrderBox.SelectedIndex)
        {
            case 0:
                allProducts = allProducts.OrderByDescending(x=>x.Count).ToList();
                break;
            case 1:
                allProducts = allProducts.OrderBy(x=>x.Count).ToList();
                break;
        }

        
        


        CatalogListBox.ItemsSource = allProducts;
    }

     
      
}