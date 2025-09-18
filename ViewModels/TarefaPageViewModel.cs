using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExCSS;
using MongoDB.Bson;
using Premix.Models;
using Premix.Services;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using SkiaSharp.HarfBuzz;

namespace Premix.ViewModels
{
    // ViewModel da página de Tarefas
    public partial class TarefaPageViewModel : ViewModelBase
    {
        
        private readonly TarefaService _tarefaService = new();
        public ObservableCollection<string> Departamentos { get; } = new ObservableCollection<string>();
        
        public ObservableCollection<string> Funcionarios { get; } = new ObservableCollection<string>();
        
        public ObservableCollection<Tarefa> Tarefa { get; } = new();
        

        // Selecionados
        private string _selectedDepartamento;
        public string SelectedDepartamento
        {
            get => _selectedDepartamento;
            set
            {
                if (_selectedDepartamento == value) return;
                _selectedDepartamento = value;
                OnPropertyChanged();
            }
        }

        private string _selectedFuncionario;
        public string SelectedFuncionario
        {
            get => _selectedFuncionario;
            set
            {
                if (_selectedFuncionario == value) return;
                _selectedFuncionario = value;
                OnPropertyChanged();
            }
        }
        
        private async Task LoadDepartamentosAsync()
        {
            var service = new TarefaService();
            var nomes = await service.GetNomesDepartamentosAsync();

            Departamentos.Clear();
            foreach (var n in nomes)
            {
                Departamentos.Add(n);
            }
        }
        
        private async Task LoadFuncionariosAsync()
        {
            var service = new TarefaService();
            var nomes = await service.GetNomesFuncionariosAsync();

            Funcionarios.Clear();
            foreach (var n in nomes)
            {
                Funcionarios.Add(n);
            }
        }
        
        
        // Comandos
        public ICommand testeCommand { get; }
        public ICommand CancelarCriarTarefaCommand { get; }
        public ICommand OpenCreateMenu { get; }
        public ICommand OpenCreateMenuCommand => OpenCreateMenu;
        
        public ICommand ContinuarCriarTarefaCommand { get; }
        public ICommand CancelarAtribuirTarefaCommand { get; }
        public ICommand SalvarAtribuicaoCommand { get; }
        public ICommand VoltarAtribuicaoCommand { get; }
        
        public ICommand OpenEditMenu { get; }
        
        public ICommand OpenEditarMenuCommand => OpenEditMenu;
        public RelayCommand<Tarefa> EditarTarefaCommand { get; }
        public IAsyncRelayCommand SalvarEdicaoTarefaCommand { get; }
        public ICommand CancelarEdicaoTarefaCommand { get; }
        public IAsyncRelayCommand ExcluirTarefaCommand { get; }
        

        // Campos privados (inicializados para evitar warnings de nullability)
        private bool _isMenuCriarVisible;
        private string _novaTarefaNome = string.Empty;
        private DateTime _novaTarefaVencimento = DateTime.Now;
        private string _novaTarefaDescricao = string.Empty;
        private bool _novaTarefaStatus = true;

        private bool _isMenuEditarVisible;
        private Tarefa? _tarefaSelecionada;
        private string _editarTarefaNome = string.Empty;
        private DateTime _editarTarefaVencimento = DateTime.Now;
        private string _editarTarefaDescricao = string.Empty;
        private bool _editarTarefaAtivo = true;
        
        private bool _isMenuAtribuirVisible;
        
        
        // Propriedades públicas para binding
        public bool IsMenuCriarVisible
        {
            get => _isMenuCriarVisible;
            set => SetProperty(ref _isMenuCriarVisible, value);
        }
        
        public bool IsMenuAtribuirVisible
        {
            get => _isMenuAtribuirVisible;
            set => SetProperty(ref _isMenuAtribuirVisible, value);
        }
        
        public bool IsMenuEditarVisible
        {
            get => _isMenuEditarVisible;
            set { _isMenuEditarVisible = value; OnPropertyChanged(nameof(IsMenuEditarVisible)); }
        }
        
        public string NovaTarefaNome
        {
            get => _novaTarefaNome;
            set { _novaTarefaNome = value; OnPropertyChanged(nameof(NovaTarefaNome)); }
        }
        
        public DateTime NovaTarefaVencimento
        {
            get => _novaTarefaVencimento;
            set { _novaTarefaVencimento = value; OnPropertyChanged(nameof(NovaTarefaVencimento)); }
        }
        
        public string NovaTarefaDescricao
        {
            get => _novaTarefaDescricao;
            set { _novaTarefaDescricao = value; OnPropertyChanged(nameof(NovaTarefaDescricao)); }
        }
        
        public bool NovaTarefaStatus
        {
            get => _novaTarefaStatus;
            set
            {
                if (_novaTarefaStatus != value)
                {
                    _novaTarefaStatus = value;
                    OnPropertyChanged(nameof(NovaTarefaStatus));
                    OnPropertyChanged(nameof(NovaTarefaStatusTexto));
                }
            }
        }
        public string NovaTarefaStatusTexto => _novaTarefaStatus ? "Em Andamento" : "Finalizado";
        
        
        public Tarefa? TarefaSelecionada
        {
            get => _tarefaSelecionada;
            set { _tarefaSelecionada = value; OnPropertyChanged(nameof(TarefaSelecionada)); }
        }
        
        public string EditarTarefaNome
        {
            get => _editarTarefaNome;
            set { _editarTarefaNome = value; OnPropertyChanged(nameof(EditarTarefaNome)); }
        }
        
        public DateTime EditarTarefaVencimento
        {
            get => _editarTarefaVencimento;
            set { _editarTarefaVencimento = value; OnPropertyChanged(nameof(EditarTarefaVencimento)); }
        }
        
        public string EditarTarefaDescricao
        {
            get => _editarTarefaDescricao;
            set { _editarTarefaDescricao = value; OnPropertyChanged(nameof(EditarTarefaDescricao)); }
        }
        
        public bool EditarTarefaAtivo
        {
            get => _editarTarefaAtivo;
            set
            {
                if (_editarTarefaAtivo != value)
                {
                    _editarTarefaAtivo = value;
                    OnPropertyChanged(nameof(EditarTarefaAtivo));
                    OnPropertyChanged(nameof(EditarTarefaStatusTexto));
                }
            }        
        }
        public string EditarTarefaStatusTexto => _editarTarefaAtivo ? "Em Andamento" : "Finalizado";
        
        
        public TarefaPageViewModel()
        {
            // Carrega os dados (pode ser assíncrono em outro lugar também)
            
            
            // itens iniciais (os mesmos que você tinha fixos no XAML)
            
            // inicializa comandos (use AsyncRelayCommand quando a ação usa await)
            CancelarCriarTarefaCommand = new RelayCommand(() => IsMenuCriarVisible = false);
            CancelarEdicaoTarefaCommand = new RelayCommand(() => IsMenuEditarVisible = false);
            OpenCreateMenu = new RelayCommand(() => IsMenuCriarVisible = true);
            
            
            
            ContinuarCriarTarefaCommand = new RelayCommand(ContinuarCriar);
            CancelarAtribuirTarefaCommand = new RelayCommand(() => IsMenuAtribuirVisible = false);
            SalvarAtribuicaoCommand = new RelayCommand(SalvarAtribuicao);
            VoltarAtribuicaoCommand = new RelayCommand(VoltarAtribuicao);
            
            EditarTarefaCommand = new RelayCommand<Tarefa>(EditarTarefa);
            SalvarEdicaoTarefaCommand = new AsyncRelayCommand(SalvarEdicaoTarefaAsync);
            ExcluirTarefaCommand = new AsyncRelayCommand(ExcluirTarefaAsync);
            testeCommand = new AsyncRelayCommand(Teste);
            OpenEditMenu = new RelayCommand(() => IsMenuEditarVisible = true);
            
            // carrega do banco (fire-and-forget; opcional: aguardar ou tratar exceção)
            _ = LoadTarefaAsync();
            _ = LoadDepartamentosAsync();
            _ = LoadFuncionariosAsync();
        }
        
        
        // Carrega lista do DB
        private async Task LoadTarefaAsync()
        {
            if (SessionService.LoggedUserId == null)
            {
                Console.WriteLine("Nenhum usuário logado encontrado.");
                return;
            }

            var usuarioId = SessionService.LoggedUserId!.Value;

            var lista = await _tarefaService.GetByUsuarioAsync(usuarioId);
            Tarefa.Clear();
            
            foreach (var t in lista)
            {
                Tarefa.Add(t);
            }
            
        }
        

        private async void SalvarAtribuicao()
        {
            var dep  = SelectedDepartamento;
            var func = SelectedFuncionario;

            if (string.IsNullOrWhiteSpace(dep) || string.IsNullOrWhiteSpace(func))
            {
                // Aqui você pode exibir um aviso de validação
                return;
            }
            
            // 🔒 Verifica duplicado no banco
            if (await _tarefaService.NomeTarefaExisteAsync(NovaTarefaNome))
            {
                await ShowErrorAsync("Já existe uma tarefa com esse nome!");

                return;
            }
            
            // Criar a tarefa com os dados selecionados
            var novaTarefa = new Tarefa
            {
                Nome = NovaTarefaNome, 
                Descricao = NovaTarefaDescricao, 
                Departamento = dep,
                Funcionario = func,
                Vencimento = DateTime.UtcNow,
                Status = NovaTarefaStatus,
                UsuarioId = SessionService.LoggedUserId!.Value
            };
            
            await _tarefaService.AddAsync(novaTarefa);

            Tarefa.Add(novaTarefa);
            
            try
            {
                var service = new TarefaService();
                await service.CriarTarefaAsync(novaTarefa);

                // ✅ aqui a tarefa já foi persistida no banco
                IsMenuAtribuirVisible = false;

                // reseta os campos
                SelectedDepartamento = null;
                SelectedFuncionario  = null;
                NovaTarefaNome = string.Empty;
                NovaTarefaDescricao =  string.Empty;
            }
            catch (Exception ex)
            {
                // logar ou mostrar erro
                Console.WriteLine($"Erro ao salvar tarefa: {ex.Message}");
            }
        }
        
        //Botões do menu de atribuição
        private void ContinuarCriar()
        {
            IsMenuCriarVisible = false;
            IsMenuAtribuirVisible = true;
        }
        
        //Botão do voltar
        private void VoltarAtribuicao()
        {
            IsMenuAtribuirVisible = false; 
            IsMenuCriarVisible = true;
        }
        
        public class OpcaoSelecionavel : ObservableObject
        {
            private string _nome = string.Empty;
            public string Nome { get => _nome; set => SetProperty(ref _nome, value); }

            private bool _selecionado;
            public bool Selecionado { get => _selecionado; set => SetProperty(ref _selecionado, value); }
        }

        
        
        // Abre menu de edição e pré-carrega campos
        private void EditarTarefa(Tarefa tarefa)
        {
            if (tarefa == null) return;

            TarefaSelecionada = tarefa;
            EditarTarefaNome = tarefa.Nome;
            EditarTarefaVencimento = tarefa.Vencimento;
            EditarTarefaDescricao = tarefa.Descricao;
            EditarTarefaAtivo = tarefa.Status;
            IsMenuEditarVisible = true;
        }
        
        private async Task SalvarEdicaoTarefaAsync()
        {
            if (TarefaSelecionada == null) return;

            TarefaSelecionada.Nome = EditarTarefaNome;
            TarefaSelecionada.Vencimento = EditarTarefaVencimento;
            TarefaSelecionada.Descricao = EditarTarefaDescricao;
            TarefaSelecionada.Status = EditarTarefaAtivo;

            await _tarefaService.UpdateAsync(TarefaSelecionada);

            IsMenuEditarVisible = false;
        }

        private async Task Teste()
        {
            await _tarefaService.TestConnectionAndListCollectionsAsync();
        }
        
        private async Task ExcluirTarefaAsync()
        {
            if (TarefaSelecionada == null) return;

            await _tarefaService.DeleteAsync(TarefaSelecionada.Id);
            Tarefa.Remove(TarefaSelecionada);
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