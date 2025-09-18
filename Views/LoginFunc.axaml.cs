using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Premix.Services;


namespace Premix.Views;


public partial class LoginFunc : UserControl
{
  
    private object _initialContent;
    private readonly FuncService _funcService;
    
    public LoginFunc()
    {
        InitializeComponent();
        _initialContent = MainContent.Content;
        _funcService = new FuncService();
    }




    private void Comeback(object? sender, RoutedEventArgs e)
    {
        if (this.VisualRoot is LoginGestor mainWindow)
        {
            // Volta para o conteúdo inicial
            mainWindow.GobackGestor();
        }
    }
    

    private void GoToCadastroFunc(object? sender, RoutedEventArgs e)
    {
        MainContent.Content = new CadastroFunc();
    }
    
    

    private async void OnLoginClick(object? sender, RoutedEventArgs e)
    {
        string username = txt_usuario.Text ?? "";
        string password = txt_senha.Text ?? "";

        bool ok = await _funcService.LoginAsync(username, password);

        if (ok)
        {
            await ShowDialog("Login realizado com sucesso!");
        }
        else
        {
            await ShowError("Usuário ou senha inválidos!");
        }
    }

    private async Task ShowDialog(string message)
    {
        var mainWindow = new FuncCodigo();
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