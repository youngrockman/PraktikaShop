using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using PraktikaShop.Models;
using System;
using System.IO;
using System.Linq;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace PraktikaShop;

public partial class CatalogWindow : Window
{
        readonly int _currentUserId;

    public CatalogWindow()
    {
        InitializeComponent();
        
    }

    public CatalogWindow(int userId)
    {
        _currentUserId = userId;
        InitializeComponent();
        LoadProducts();
        OrderBox.SelectionChanged += OrderBox_SelectionChanged;
        SeacrhBox.KeyUp += SeacrhBox_KeyUp;
        CatalogListBox.SelectionChanged += SelectionProduct;
        ItemsBox.SelectionChanged += ItemsBox_SelectionChanged;
        LoadCombobox();
    }


    private void LoadCombobox()
    {
        using var context = new KarpovContext();
        var items = context.Products.Select(x => x.ProductName).ToList();
        items.Add("��� ��������");
        ItemsBox.ItemsSource = items.OrderByDescending(x=>x == "��� ��������");
    }
    
    private void ItemsBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {

        LoadProducts();
    }

    private async void SelectionProduct(object? sender, SelectionChangedEventArgs e)
    {
        var product = CatalogListBox.SelectedItem as Product;

        var editWindow = new EditWindow(product, _currentUserId);
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
            var searchText = SeacrhBox.Text.ToLower();
            allProducts = allProducts.Where(x=>x.ProductName.ToLower().Contains(searchText)).ToList();
        }

        if (ItemsBox.SelectionBoxItem != null && ItemsBox.SelectionBoxItem.ToString() != "��� ��������")
        {
            allProducts = allProducts.Where(x => x.ProductName == ItemsBox.SelectedItem.ToString()).ToList() ;
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



    private async void AddInBasket_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int productId)
        {
            using var context = new KarpovContext();
            var product = await context.Products.FindAsync(productId);
            if (product == null || product.Count <= 0)
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Товар недоступен", ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                await message.ShowAsync();
                return;
            }
        
            var basket = await context.Baskets.FirstOrDefaultAsync(b => b.UserId == _currentUserId);
            if (basket == null)
            {
                basket = new Basket { UserId = _currentUserId };
                context.Baskets.Add(basket);
                await context.SaveChangesAsync();
            }
        
            var basketItem = await context.BasketProducts
                .FirstOrDefaultAsync(x => x.BasketId == basket.BasketId && x.ProductId == productId);

            if (basketItem != null)
            {
                if (basketItem.ProductCount + 1 > product.Count)
                {
                    var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Нельзя добавить больше товара, чем есть в наличии", ButtonEnum.Ok,
                        MsBox.Avalonia.Enums.Icon.Error);
                    await message.ShowAsync();
                    return;
                }
            
                basketItem.ProductCount += 1;
            }
            else
            {
                basketItem = new BasketProduct
                {
                    BasketId = basket.BasketId,
                    ProductId = productId,
                    ProductCount = 1
                };
                context.BasketProducts.Add(basketItem);
            }

            await context.SaveChangesAsync();
        }
    }

    private void BasketClick(object? sender, RoutedEventArgs e)
    {

        var basketWindow = new BasketWindow(_currentUserId);
        basketWindow.Show();
        Close(this);
    }

    private void AddButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var addProductWindow = new AddProductWindow(_currentUserId);
        addProductWindow.Show();
        this.Close();
    }
}