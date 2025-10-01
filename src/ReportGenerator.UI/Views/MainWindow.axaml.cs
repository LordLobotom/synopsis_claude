using Avalonia.Controls;
using Avalonia.Interactivity;
using ReportGenerator.Domain.Entities;
using ReportGenerator.UI.ViewModels;

namespace ReportGenerator.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Element_Tapped(object? sender, RoutedEventArgs e)
    {
        if (sender is Border border && border.Tag is ReportElement element)
        {
            if (DataContext is MainWindowViewModel mainViewModel &&
                mainViewModel.DesignerViewModel != null)
            {
                mainViewModel.DesignerViewModel.SelectedElement = element;
            }
        }
    }
}