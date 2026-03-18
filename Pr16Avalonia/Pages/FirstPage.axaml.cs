using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using WpfLikeAvaloniaNavigation;

namespace Pr16Avalonia.Pages;

public partial class FirstPage : Page
{
    public FirstPage()
    {
        InitializeComponent();
    }

    private void NewGameButton_Click(object? sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new GamePage());
    }

    private void ContinueButton_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void SettingsButton_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void AboutButton_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void ExitButton_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}