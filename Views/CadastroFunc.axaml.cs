using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using Premix.Services;

namespace Premix.Views;

public partial class CadastroFunc : UserControl
{
    private object _initialContent;
    public CadastroFunc()
    {
        InitializeComponent();
        _initialContent = MainContent.Content;
        _cadastroService = new CadastroService();
    }


    public void Goback()
    {
        MainContent.Content = _initialContent;
    }


    private void Comeback(object? sender, RoutedEventArgs e)
    {
        MainContent.Content = new  LoginFunc();
    }
    
    private readonly CadastroService _cadastroService;

    private async void OnCadastroClick(object? sender, RoutedEventArgs e)
    {
        string usuario = txt_usuario.Text ?? "";
        string senha = txt_senha.Text ?? "";
        string cpfText = txt_cpf.Text ?? "";

        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(senha) || string.IsNullOrWhiteSpace(cpfText))
        {
            await ShowMessageAsync("Preencha todos os campos!");
            return;
        }
        
        if (!long.TryParse(cpfText, out long cpf))
        {
            await ShowMessageAsync("CPF inválido, use apenas números!");
            return;
        }

        var cadastro = await _cadastroService.CadastrarFuncionarioAsync(usuario, senha, cpf);

        if (cadastro == null)
        {
            await ShowMessageAsync("Usuário já cadastrado com esses dados!");
        }
        else
        {
            await ShowMessageAsync("Cadastro realizado com sucesso!");
            MainContent.Content = new LoginFunc(); // redireciona após cadastro
        }
    }

    
    private async Task ShowMessageAsync(string message)
    {
        var dlg = new Window
        {
            Width = 300,
            Height = 150,
            Content = new TextBlock
            {
                Text = message,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            }
        };

        await dlg.ShowDialog((Window)this.VisualRoot!);
    }
}