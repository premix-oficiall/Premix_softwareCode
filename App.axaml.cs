using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Premix.Services;
using Premix.ViewModels;
using Premix.Views;

namespace Premix;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
            DataTemplates.Add(new ViewLocator());
            
        Console.WriteLine(Application.Current.Styles.Count);
        foreach (var style in Application.Current.Styles)
        {
            Console.WriteLine(style);
        }

    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new LoginGestor();
        }
        
        
        
        
        base.OnFrameworkInitializationCompleted();
    }
    

    
}