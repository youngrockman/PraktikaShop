using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using PraktikaShop.Models;
using System;
using System.IO;
using System.Linq;
using Avalonia.Input;

namespace PraktikaShop;

public partial class CatalogWindow : Window
{

    public CatalogWindow()
    {
        InitializeComponent();
        LoadProducts();
        OrderBox.SelectionChanged += OrderBox_SelectionChanged;
        SeacrhBox.KeyUp += SeacrhBox_KeyUp;
        CatalogListBox.SelectionChanged += SelectionProduct;

    }

    private async void SelectionProduct(object? sender, SelectionChangedEventArgs e)
    {
        var product = CatalogListBox.SelectedItem as Product;

        var editWindow = new EditWindow(product);
        editWindow.Show();
        this.Close();
        
    }

    private void OrderBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        LoadProducts();
    }

    private void SeacrhBox_KeyUp(object sender, EventArgs e) 
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


        if (SeacrhBox.Text != null && SeacrhBox.Text != "")
        {
            allProducts = allProducts.Where(x=>x.ProductName.Contains(SeacrhBox.Text)).ToList();
        }





        CatalogListBox.ItemsSource = allProducts;
    }

    private async void DeleteProduct_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new KarpovContext();
        var idProduct = (int)(sender as Button)!.Tag!;

        var people = context.Products.FirstOrDefault(x => x.ProductId == idProduct)!;

        if (people != null)
        {
            context.Products.Remove(people);
            await context.SaveChangesAsync();
        }

        LoadProducts();
    }

    private void Exit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        Close(this);

       

    }


    



    //private void AddInBasket_Click(object? sender, EventArgs e)
    //{

    //}



}