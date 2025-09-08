using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PraktikaShop.Models;

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
        ImageBox.Text = product.Image;
    }

    private async void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        using var context = new KarpovContext();
        _product.ProductName = NameBox.Text;
        _product.Cost = int.Parse(CostBox.Text);
        _product.Image = ImageBox.Text;
        _product.Count = int.Parse(CountBox.Text);


        context.Products.Update(_product);
        await context.SaveChangesAsync();

        var catalogWindow = new CatalogWindow();
        catalogWindow.Show();
        this.Close();
        
        

    }

    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

}