using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PraktikaShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PraktikaShop;

public partial class OrderWindow : Window
{

    private readonly int _currentUserId;
    private List<BasketProduct> _basketItems;
    public OrderWindow()
    {
        InitializeComponent();
    }


    public OrderWindow(int currentUserId)
    {
        InitializeComponent();
        _currentUserId = currentUserId;
        GetOrder();

    }


    private void GetOrder()
    {
        using var context = new KarpovContext();    

        _basketItems = context.BasketProducts.Include(x=>x.Product).Include(x=>x.Basket).Where(x=>x.Basket.UserId == _currentUserId).ToList();

        OrderBox.ItemsSource = _basketItems;

        CalculateTotalPrice();

    }


    private void CalculateTotalPrice()
    {
        decimal totalPrice = 0;
        foreach (var item in _basketItems)
        {
            totalPrice += (item.Product.Cost ?? 0) * item.ProductCount;
        }
        TotalPriceText.Text = $"Общая стоимость: {totalPrice} руб.";
    }

    private void BackButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var basketWindow = new BasketWindow();
        basketWindow.Show();
        Close(this);
    }

    private async void CreateOrderClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new KarpovContext();

        var Neworder = new Order()
        {
            StartDate =DateOnly.FromDateTime(DateTime.Now),
            EndDate = new DateOnly(2026,12,12),
            Status = "Новый"
        };

        context.Orders.Add(Neworder);
        await context.SaveChangesAsync();


        foreach (var basketItem in _basketItems)
        {
            var orderProduct = new OrderProduct
            {
                OrderId = Neworder.OrderId,
                ProductId = basketItem.ProductId
            };
            context.OrderProducts.Add(orderProduct);
        }


       await context.SaveChangesAsync();

    }
}