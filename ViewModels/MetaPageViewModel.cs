using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Premix.Models;
using Premix.Services;

namespace Premix.ViewModels
{
    // ViewModel da página de Metas
    public partial class MetaPageViewModel : ViewModelBase
    {
        private readonly MetaService _metaService = new();
        public ObservableCollection<Meta> Meta { get; } = new();
        
        
        // Comandos
        public IAsyncRelayCommand CriarMetaCommand { get; }
        public ICommand CancelarCriarMetaCommand { get; }
        public ICommand OpenCreateMenu { get; }
        public ICommand OpenCreateMenuCommand => OpenCreateMenu;
        
        public ICommand OpenEditMenu { get; }
        public ICommand OpenEditarMenuCommand => OpenEditMenu;
        public RelayCommand<Meta> EditarMetaCommand { get; }
        public IAsyncRelayCommand SalvarEdicaoMetaCommand { get; }
        public ICommand CancelarEdicaoMetaCommand { get; }
        public IAsyncRelayCommand ExcluirMetaCommand { get; }
        
        // Campos privados (inicializados para evitar warnings de nullability)
        private bool _isMenuCriarVisible;
        private string _novaMetaNome = string.Empty;
        private DateTime _novaMetaVencimento = DateTime.Now;
        private string _novaMetaDescricao = string.Empty;
        private int _novaMetaProgresso = int.MaxValue;
        private string _novaMetaRecompensa = string.Empty;

        private bool _isMenuEditarVisible;
        private Meta? _metaSelecionada;
        private string _editarMetaNome = string.Empty;
        private DateTime _editarMetaVencimento = DateTime.Now;
        private string _editarMetaDescricao = string.Empty;
        private int _editarMetaProgresso = int.MaxValue;
        private string _editarMetaRecompensa = string.Empty;
        
        
        // Propriedades públicas para binding
        public bool IsMenuCriarVisible
        {
            get => _isMenuCriarVisible;
            set => SetProperty(ref _isMenuCriarVisible, value);
        }
        
        public bool IsMenuEditarVisible
        {
            get => _isMenuEditarVisible;
            set { _isMenuEditarVisible = value; OnPropertyChanged(nameof(IsMenuEditarVisible)); }
        }
        
        public string NovaMetaNome
        {
            get => _novaMetaNome;
            set { _novaMetaNome = value; OnPropertyChanged(nameof(NovaMetaNome)); }
        }
        
        public DateTime NovaMetaVencimento
        {
            get => _novaMetaVencimento;
            set { _novaMetaVencimento = value; OnPropertyChanged(nameof(NovaMetaVencimento)); }
        }
        
        public string NovaMetaDescricao
        {
            get => _novaMetaDescricao;
            set { _novaMetaDescricao = value; OnPropertyChanged(nameof(NovaMetaDescricao)); }
        }

        public int NovaMetaProgresso
        {
            get => _novaMetaProgresso;
            set { _novaMetaProgresso = value; OnPropertyChanged(nameof(NovaMetaProgresso)); }
        }

        public string NovaMetaRecompensa
        {
            get => _novaMetaRecompensa;
            set { _novaMetaRecompensa = value; OnPropertyChanged(nameof(NovaMetaRecompensa)); }
        }
        
        public Meta? MetaSelecionada
        {
            get => _metaSelecionada;
            set { _metaSelecionada = value; OnPropertyChanged(nameof(MetaSelecionada)); }
        }
        
        public string EditarMetaNome
        {
            get => _editarMetaNome;
            set { _editarMetaNome = value; OnPropertyChanged(nameof(EditarMetaNome)); }
        }
        
        public DateTime EditarMetaVencimento
        {
            get => _editarMetaVencimento;
            set { _editarMetaVencimento = value; OnPropertyChanged(nameof(EditarMetaVencimento)); }
        }
        
        public string EditarMetaDescricao
        {
            get => _editarMetaDescricao;
            set { _editarMetaDescricao = value; OnPropertyChanged(nameof(EditarMetaDescricao)); }
        }

        public int EditarMetaProgresso
        {
            get => _editarMetaProgresso;
            set { _editarMetaProgresso = value; OnPropertyChanged(nameof(EditarMetaProgresso)); }
        }

        public string EditarMetaRecompensa
        {
            get => _editarMetaRecompensa;
            set { _editarMetaRecompensa = value; OnPropertyChanged(nameof(EditarMetaRecompensa)); }
        }

        
        public MetaPageViewModel()
        {
            
            // inicializa comandos
            CriarMetaCommand = new AsyncRelayCommand(AddMetaAsync);
            CancelarCriarMetaCommand = new RelayCommand(() => IsMenuCriarVisible = false);
            CancelarEdicaoMetaCommand = new RelayCommand(() => IsMenuEditarVisible = false);
            OpenCreateMenu = new RelayCommand(() => IsMenuCriarVisible = true);
            
            EditarMetaCommand = new RelayCommand<Meta>(EditarMeta);
            SalvarEdicaoMetaCommand = new AsyncRelayCommand(SalvarEdicaoMetaAsync);
            ExcluirMetaCommand = new AsyncRelayCommand(ExcluirMetaAsync);
            OpenEditMenu = new RelayCommand(() => IsMenuEditarVisible = true);
            
            // carrega do banco
            _ = LoadMetaAsync();
        }
        
        // Carrega lista do DB
        private async Task LoadMetaAsync()
        {
            if (SessionService.LoggedUserId == null)
            {
                Console.WriteLine("Nenhum usuário logado encontrado.");
                return;
            }

            var lista = await _metaService.GetByUsuarioAsync(SessionService.LoggedUserId.ToString());
            Meta.Clear();

            foreach (var m in lista)
            {
                Meta.Add(m);
                m.PropertyChanged += Meta_PropertyChanged;
            }
        }
        private void Meta_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsActive") ;
        }
        
        // Cria novo departamento
        private async Task AddMetaAsync()
        {
            // validações
            if (string.IsNullOrWhiteSpace(NovaMetaNome) || 
                string.IsNullOrWhiteSpace(NovaMetaDescricao) || 
                NovaMetaVencimento == default) // verifica se a data não foi definida
            {
                return;
            }

            // 🔒 Verifica duplicado no banco
            if (await _metaService.NomeMetaExisteAsync(NovaMetaNome))
            {
                await ShowErrorAsync("Já existe uma meta com esse nome!");
                return;
            }

            var novo = new Meta
            {
                Nome = NovaMetaNome,
                Vencimento = NovaMetaVencimento,
                Descricao = NovaMetaDescricao,
                Progresso = NovaMetaProgresso,
                Recompensa = NovaMetaRecompensa,
                UsuarioId = SessionService.LoggedUserId.ToString()
            };

            await _metaService.AddAsync(novo);

            Meta.Add(novo);

            // reseta os campos
            NovaMetaNome = string.Empty;
            NovaMetaVencimento = DateTime.Now;
            NovaMetaDescricao = string.Empty;
            NovaMetaProgresso = 0;
            NovaMetaRecompensa = string.Empty;
            IsMenuCriarVisible = false;
        }
        
        // Abre menu de edição e pré-carrega campos
        private void EditarMeta(Meta meta)
        {
            if (meta == null) return;

            MetaSelecionada = meta;
            EditarMetaNome = meta.Nome;
            EditarMetaVencimento = meta.Vencimento;
            EditarMetaDescricao = meta.Descricao;
            EditarMetaProgresso = meta.Progresso;
            EditarMetaRecompensa = meta.Recompensa;
            IsMenuEditarVisible = true;
        }
        
        private async Task SalvarEdicaoMetaAsync()
        {
            if (MetaSelecionada == null) return;

            MetaSelecionada.Nome = EditarMetaNome;
            MetaSelecionada.Vencimento = EditarMetaVencimento;
            MetaSelecionada.Progresso = EditarMetaProgresso;
            MetaSelecionada.Recompensa = EditarMetaRecompensa;
            MetaSelecionada.Descricao = EditarMetaDescricao;

            await _metaService.UpdateAsync(MetaSelecionada);

            IsMenuEditarVisible = false;
        }
        
        private async Task ExcluirMetaAsync()
        {
            if (MetaSelecionada == null) return;

            await _metaService.DeleteAsync(MetaSelecionada.Id);
            Meta.Remove(MetaSelecionada);
            IsMenuEditarVisible = false;
        }
        
        public async Task ShowErrorAsync(string message)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Erro", message, ButtonEnum.Ok, Icon.Error);

            await box.ShowAsync();
        }
    }
}