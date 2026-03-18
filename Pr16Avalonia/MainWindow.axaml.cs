using System;
using _3ISIP323_Zevakin.Новая_папка;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Pr16Avalonia.Pages;

namespace Pr16Avalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainContent.Navigate(new FirstPage());
    }
    }

    

