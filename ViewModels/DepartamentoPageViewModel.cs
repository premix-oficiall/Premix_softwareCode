using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExCSS;
using Premix.Models;
using Premix.Services;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;





namespace Premix.ViewModels
{
    public class DepartamentoPageViewModel : ViewModelBase
    {
        private readonly DepartamentoService _departamentoService = new();

        public ObservableCollection<Departamentos> Departamentos { get; } = new();

        public int DepartamentosAtivos   => Departamentos.Count(d => d.IsActive);
        public int DepartamentosInativos => Departamentos.Count(d => !d.IsActive);
        public int FuncionariosSemDepartamento => 3;

        // Comandos
        public IAsyncRelayCommand CriarDepartamentoCommand { get; }
        public ICommand CancelarCriarDepartamentoCommand { get; }
        public ICommand OpenCreateMenu { get; }
        public ICommand OpenCreateMenuCommand => OpenCreateMenu;

        public ICommand OpenEditMenu { get; }
        public ICommand OpenEditarMenuCommand => OpenEditMenu;
        public RelayCommand<Departamentos> EditarDepartamentoCommand { get; }
        public IAsyncRelayCommand SalvarEdicaoDepartamentoCommand { get; }
        public ICommand CancelarEdicaoDepartamentoCommand { get; }
        public IAsyncRelayCommand ExcluirDepartamentoCommand { get; }

        // Campos privados (inicializados para evitar warnings de nullability)
        private bool _isMenuCriarVisible;
        private string _novoDepartamentoNome = string.Empty;
        private string _novoDepartamentoCodigo = string.Empty;
        private bool _novoDepartamentoAtivo = true;

        private bool _isMenuEditarVisible;
        private Departamentos? _departamentoSelecionado;
        private string _editarDepartamentoNome = string.Empty;
        private string _editarDepartamentoCodigo = string.Empty;
        private bool _editarDepartamentoAtivo = true;

        // Propriedades públicas para binding
        public bool IsMenuCriarVisible
        {
            get => _isMenuCriarVisible;
            set { _isMenuCriarVisible = value; OnPropertyChanged(nameof(IsMenuCriarVisible)); }
        }

        public bool IsMenuEditarVisible
        {
            get => _isMenuEditarVisible;
            set { _isMenuEditarVisible = value; OnPropertyChanged(nameof(IsMenuEditarVisible)); }
        }

        public string NovoDepartamentoNome
        {
            get => _novoDepartamentoNome;
            set { _novoDepartamentoNome = value; OnPropertyChanged(nameof(NovoDepartamentoNome)); }
        }

        public string NovoDepartamentoCodigo
        {
            get => _novoDepartamentoCodigo;
            set { _novoDepartamentoCodigo = value; OnPropertyChanged(nameof(NovoDepartamentoCodigo)); }
        }

        public bool NovoDepartamentoAtivo
        {
            get => _novoDepartamentoAtivo;
            set { _novoDepartamentoAtivo = value; OnPropertyChanged(nameof(NovoDepartamentoAtivo)); }
        }

        public Departamentos? DepartamentoSelecionado
        {
            get => _departamentoSelecionado;
            set { _departamentoSelecionado = value; OnPropertyChanged(nameof(DepartamentoSelecionado)); }
        }

        public string EditarDepartamentoNome
        {
            get => _editarDepartamentoNome;
            set { _editarDepartamentoNome = value; OnPropertyChanged(nameof(EditarDepartamentoNome)); }
        }

        public string EditarDepartamentoCodigo
        {
            get => _editarDepartamentoCodigo;
            set { _editarDepartamentoCodigo = value; OnPropertyChanged(nameof(EditarDepartamentoCodigo)); }
        }

        public bool EditarDepartamentoAtivo
        {
            get => _editarDepartamentoAtivo;
            set { _editarDepartamentoAtivo = value; OnPropertyChanged(nameof(EditarDepartamentoAtivo)); }
        }

        // ctor
        public DepartamentoPageViewModel()
        {
            // dados de exemplo (design-time / runtime rápido)
            Departamentos.Add(new Departamentos { Nome = "Tecnologia da Informação", Codigo = "xxxxxxxx", IsActive = true });
            Departamentos.Add(new Departamentos { Nome = "Comercial/Vendas",        Codigo = "xxxxxxxx", IsActive = true });
            Departamentos.Add(new Departamentos { Nome = "Recursos Humanos",        Codigo = "xxxxxxxx", IsActive = false });
            Departamentos.Add(new Departamentos { Nome = "Marketing",               Codigo = "xxxxxxxx", IsActive = true });

            Departamentos.CollectionChanged += Departamentos_CollectionChanged;
            foreach (var d in Departamentos) d.PropertyChanged += Departamento_PropertyChanged;

            // inicializa comandos (use AsyncRelayCommand quando a ação usa await)
            CriarDepartamentoCommand = new AsyncRelayCommand(AddDepartmentAsync);
            CancelarCriarDepartamentoCommand = new RelayCommand(() => IsMenuCriarVisible = false);
            CancelarEdicaoDepartamentoCommand = new RelayCommand(() => IsMenuEditarVisible = false);
            OpenCreateMenu = new RelayCommand(() => IsMenuCriarVisible = true);

            EditarDepartamentoCommand = new RelayCommand<Departamentos>(EditarDepartamento);
            SalvarEdicaoDepartamentoCommand = new AsyncRelayCommand(SalvarEdicaoDepartamentoAsync);
            ExcluirDepartamentoCommand = new AsyncRelayCommand(ExcluirDepartamentoAsync);
            OpenEditMenu = new RelayCommand(() => IsMenuEditarVisible = true);

            // carrega do banco (fire-and-forget; opcional: aguardar ou tratar exceção)
            _ = LoadDepartamentosAsync();
        }

        // Carrega lista do DB
        private async Task LoadDepartamentosAsync()
        {
            if (SessionService.LoggedUserId == null)
            {
                Console.WriteLine("Nenhum usuário logado encontrado.");
                return;
            }

            var lista = await _departamentoService.GetByUsuarioAsync(SessionService.LoggedUserId.ToString());
            Departamentos.Clear();

            foreach (var d in lista)
            {
                Departamentos.Add(d);
                d.PropertyChanged += Departamento_PropertyChanged;
            }

            RaiseCounters();
        }

        // Cria novo departamento
        private async Task AddDepartmentAsync()
        {
            if (string.IsNullOrWhiteSpace(NovoDepartamentoNome) || string.IsNullOrWhiteSpace(NovoDepartamentoCodigo))
                return;

            // 🔒 Verifica duplicado no banco (global)
            if (await _departamentoService.CodigoExisteAsync(NovoDepartamentoCodigo))
            {
                if (await _departamentoService.CodigoExisteAsync(NovoDepartamentoCodigo))
                {
                    await ShowErrorAsync("Já existe um departamento com esse código!");
                    return;
                }
            } else if (await _departamentoService.NomeExisteAsync(NovoDepartamentoNome))
            {
                await ShowErrorAsync("Já existe um departamento com esse nome!");
                return;
            }
            
            

            var novo = new Departamentos
            {
                Nome = NovoDepartamentoNome,
                Codigo = NovoDepartamentoCodigo,
                IsActive = NovoDepartamentoAtivo,
                UsuarioId = SessionService.LoggedUserId.ToString()
            };

            await _departamentoService.AddAsync(novo);

            Departamentos.Add(novo);

            NovoDepartamentoNome = string.Empty;
            NovoDepartamentoCodigo = string.Empty;
            NovoDepartamentoAtivo = true;
            IsMenuCriarVisible = false;

            RaiseCounters();
        }
        

        // Abre menu de edição e pré-carrega campos
        private void EditarDepartamento(Departamentos departamento)
        {
            if (departamento == null) return;

            DepartamentoSelecionado = departamento;
            EditarDepartamentoNome = departamento.Nome;
            EditarDepartamentoCodigo = departamento.Codigo;
            EditarDepartamentoAtivo = departamento.IsActive;
            IsMenuEditarVisible = true;
        }

        private async Task SalvarEdicaoDepartamentoAsync()
        {
            if (DepartamentoSelecionado == null) return;

            DepartamentoSelecionado.Nome = EditarDepartamentoNome;
            DepartamentoSelecionado.Codigo = EditarDepartamentoCodigo;
            DepartamentoSelecionado.IsActive = EditarDepartamentoAtivo;

            await _departamentoService.UpdateAsync(DepartamentoSelecionado);

            IsMenuEditarVisible = false;
            RaiseCounters();
        }

        private async Task ExcluirDepartamentoAsync()
        {
            if (DepartamentoSelecionado == null) return;

            await _departamentoService.DeleteAsync(DepartamentoSelecionado.Id);
            Departamentos.Remove(DepartamentoSelecionado);
            IsMenuEditarVisible = false;
            RaiseCounters();
        }

        // Coleção -> atualizar contadores
        private void Departamentos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (Departamentos d in e.NewItems) d.PropertyChanged += Departamento_PropertyChanged;

            if (e.OldItems != null)
                foreach (Departamentos d in e.OldItems) d.PropertyChanged -= Departamento_PropertyChanged;

            RaiseCounters();
        }

        private void Departamento_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsActive")
                RaiseCounters();
        }

        private void RaiseCounters()
        {
            OnPropertyChanged(nameof(DepartamentosAtivos));
            OnPropertyChanged(nameof(DepartamentosInativos));
        }
        
        public async Task ShowErrorAsync(string message)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Erro", message, ButtonEnum.Ok, Icon.Error);

            await box.ShowAsync();
        }

        
    }
}