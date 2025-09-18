using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using Premix.Services;
using Premix.ViewModels;
using Premix.Views;

namespace Premix;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.ClickCount != 2)
            return;
        
        (DataContext as MainViewModel)?.SideMenuResizeCommand?.Execute(null);
    }
    
    private async void BtnSair_Click(object? sender, RoutedEventArgs e)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ContentTitle = "Confirmação",
            ContentMessage = "Deseja realmente sair?",
            ButtonDefinitions = ButtonEnum.YesNo,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        });

        var result = await box.ShowAsync();

        if (result == ButtonResult.Yes)
        {
            var login = new LoginGestor(); // 👈 sua tela de login (Window)

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Define a nova janela principal
                desktop.MainWindow = login;

                login.Show();
                SessionService.Clear();
                Console.WriteLine($"Usuário deslogado: {SessionService.Username}, ID: {SessionService.LoggedUserId}");
                // Fecha a janela atual (a que contém o botão Sair)
                this.Close();
            }
            else
            {
                // fallback (se não for DesktopLifetime, mas em geral sempre é)
                login.Show();
                this.Close();
            }
        }
    }
}