using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Premix.Services;

namespace Premix.Views;

public partial class LoginGestor : Window
{
    
    private object _initialContent;
    private readonly GestorService _gestorService;
    public LoginGestor()
    {
        InitializeComponent();
        _initialContent = MainContent.Content;
        _gestorService = new GestorService();
    }


    private void GoToLoginFunc(object?sender,RoutedEventArgs e)
    {
       MainContent.Content = new LoginFunc();
    }

    
  
    public void GobackGestor()
    {
        MainContent.Content = _initialContent;
    }

    private async void OnLoginClick(object? sender, RoutedEventArgs e)
    {
        string username = txt_usuario.Text ?? "";
        string password = txt_senha.Text ?? "";
        
        bool ok = await _gestorService.LoginAsync(username, password);

        if (ok)
        {
            await ShowDialog("Login realizado com sucesso!");
        }
        else
        {
            await ShowError("Login não realizado credenciaias invalidas!");
        }
    }
    
    private async Task ShowDialog(string message)
    {
        var mainWindow = new MainView();
        await mainWindow.ShowDialog((Window)this.VisualRoot!);

        // Fecha a tela de login
        (this.VisualRoot as Window)?.Close();
        
    }

    private async Task ShowError(string message)
    {
        var dlg = new Window
        {
            Content = new TextBlock { Text = message, Margin = new Avalonia.Thickness(20) },
            Width = 300,
            Height = 100
        };
        await dlg.ShowDialog((Window)this.VisualRoot!);
            
    }
    
}
    

    
    
