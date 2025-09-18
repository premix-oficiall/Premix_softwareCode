using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Premix.ViewModels
{
    public class Recompensa
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; } // caminho/URL
    }

    public partial class RecompensaPageViewModel : ViewModelBase
    {
        public ObservableCollection<Recompensa> Catalogo { get; } = new();

        // Campos do formulário
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AdicionarRecompensaCommand))]
        private string nome;

        [ObservableProperty]
        private string descricao;

        [ObservableProperty]
        private string imagem;

        public RecompensaPageViewModel()
        {
            // Catálogo inicial
            Catalogo.Add(new Recompensa { Nome = "Exemplo1", Descricao = "Conteúdo estático (exemplo)" });
        }

        // Habilita o botão Criar somente quando 'Nome' estiver preenchido
        private bool PodeAdicionarRecompensa() => !string.IsNullOrWhiteSpace(Nome);

        [RelayCommand(CanExecute = nameof(PodeAdicionarRecompensa))]
        private void AdicionarRecompensa()
        {
            Catalogo.Add(new Recompensa
            {
                Nome = Nome?.Trim(),
                Descricao = Descricao?.Trim(),
                Imagem = Imagem?.Trim()
            });

            // Limpa formulário
            Nome = string.Empty;
            Descricao = string.Empty;
            Imagem = string.Empty;
        }
    }
}