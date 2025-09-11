using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using PraktikaShop.Models;
using System;
using System.Linq;

namespace PraktikaShop;

public partial class BasketWindow : Window
{
    private readonly int _currentUserId;

    public BasketWindow()
    {
        InitializeComponent();
    }

    public BasketWindow(int userId)
    {
        _currentUserId = userId;
        InitializeComponent();
        LoadBasket();
    }

    private void ExitClick(object? sender, RoutedEventArgs e)
    {
        var catalogWindow = new CatalogWindow(_currentUserId);
        catalogWindow.Show();
        Close(this);
    }

    private async void LoadBasket()
    {
        using var context = new KarpovContext();
        var basketItems = await context.BasketProducts
            .Include(x => x.Product)
            .Include(x => x.Basket)
            .Where(x => x.Basket.UserId == _currentUserId)
            .ToListAsync();

        BasketListBox.ItemsSource = basketItems;
    }

    private async void Quantity_ValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        if (sender is NumericUpDown numericUpDown && numericUpDown.DataContext is BasketProduct basketItem)
        {
            using var context = new KarpovContext();

            var itemToUpdate = await context.BasketProducts
                .FirstOrDefaultAsync(bp => bp.BasketProductId == basketItem.BasketProductId);

            if (itemToUpdate != null)
            {
                itemToUpdate.ProductCount = (int)e.NewValue;
                await context.SaveChangesAsync();
                LoadBasket(); 
            }
        }
    }

    private async void RemoveFromBasket_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int basketProductId)
        {
            using var context = new KarpovContext();

            var basketItem = await context.BasketProducts
                .FirstOrDefaultAsync(bp => bp.BasketProductId == basketProductId);

            if (basketItem != null)
            {
                context.BasketProducts.Remove(basketItem);
                await context.SaveChangesAsync();
                LoadBasket();
            }
        }
    }

    private void CreateOrder_Click(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("Заказ создан!");
    }
}