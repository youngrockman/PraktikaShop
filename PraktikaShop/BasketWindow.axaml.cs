using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using PraktikaShop.Models;
using System;
using System.Collections.Generic;
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
        CalculateTotalPrice(); 
    }

    private async void IncreaseQuantity_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int basketProductId)
        {
            using var context = new KarpovContext();

            var basketItem = await context.BasketProducts
                .Include(bp => bp.Product)
                .FirstOrDefaultAsync(bp => bp.BasketProductId == basketProductId);

            if (basketItem != null && basketItem.ProductCount < basketItem.Product.Count)
            {
                basketItem.ProductCount++;
                await context.SaveChangesAsync();
                LoadBasket(); 
            }
        }
    }

    private async void DecreaseQuantity_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is int basketProductId)
        {
            using var context = new KarpovContext();

            var basketItem = await context.BasketProducts
                .FirstOrDefaultAsync(bp => bp.BasketProductId == basketProductId);

            if (basketItem != null)
            {
                if (basketItem.ProductCount > 1)
                {
                    basketItem.ProductCount--;
                }
                else
                {
                    context.BasketProducts.Remove(basketItem);
                }
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

    
    private void CalculateTotalPrice()
    {
        if (BasketListBox.ItemsSource is System.Collections.IEnumerable items)
        {
            decimal totalPrice = 0;
            foreach (BasketProduct item in items)
            {
                totalPrice += (item.Product.Cost ?? 0) * item.ProductCount;
            }

            TotalPriceText.Text = $"Общая стоимость: {totalPrice} руб.";
        }
    }

    private void CreateOrder_Click(object? sender, RoutedEventArgs e)
    {
        var orderWindow = new OrderWindow(_currentUserId);
        orderWindow.Show();
        Close(this);
    }
}