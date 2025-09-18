using System;
using Avalonia;
using Avalonia.Controls;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Premix.Services;

namespace Premix.Views;

public partial class FuncCodigo : Window
{
    private readonly CodigoService _codigoService;

    public FuncCodigo()
    {
        InitializeComponent();
        _codigoService = new CodigoService();
    }

    private async void OnLoginClick(object? sender, RoutedEventArgs e)
    {
        string codigo = txt_codigo.Text ?? "";

        var departamentos = await _codigoService.ValidarCodigoAsync(codigo);

        if (departamentos != null)
        {
            // ✅ Código válido → abre tela do departamento
            var main = new FuncMainView();
            main.Show(); // abre como uma nova janela
            
            this.Close();
        }
        else 
        {
            // ❌ Código inválido → mostra popup simples
            var mainWindow = (Window)this.VisualRoot!;

            if (departamentos != null)
            {
                mainWindow.Content = new FuncMainView();
            }
            else
            {
                var dlg = new Window
                {
                    Content = new TextBlock { Text = "Código inválido!", Margin = new Avalonia.Thickness(20) },
                    Width = 250,
                    Height = 100
                };
                await dlg.ShowDialog(mainWindow);
            }


        }
    }
}