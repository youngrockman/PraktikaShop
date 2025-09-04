using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using PraktikaShop.Models;
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
        CatalogListBox.ItemsSource = allProducts;
    }
}