using System.Runtime.InteropServices.JavaScript;
using Avalonia.Svg.Skia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Premix.Services;
using Premix.Views;

namespace Premix.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private const string buttonActiveClass = "active";
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SomeWidth))]
    private bool _sideMenuExpanded = true;
    public int SomeWidth => _sideMenuExpanded ? 220 : 75;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(InicioPageIsActive))]
    [NotifyPropertyChangedFor(nameof(CriarPageIsActive))]
    [NotifyPropertyChangedFor(nameof(EditarPageIsActive))]
    [NotifyPropertyChangedFor(nameof(MetaPageIsActive))]
    [NotifyPropertyChangedFor(nameof(TarefaPageIsActive))]
    [NotifyPropertyChangedFor(nameof(DesempenhoPageIsActive))]
    [NotifyPropertyChangedFor(nameof(ClassificacaoPageIsActive))]
    [NotifyPropertyChangedFor(nameof(RecompensaPageIsActive))]
    [NotifyPropertyChangedFor(nameof(ConfiguracaoPageIsActive))]
    private ViewModelBase _currentPage;
    
    public bool InicioPageIsActive => CurrentPage == _inicioPage;
    public bool CriarPageIsActive => CurrentPage == _departamentoPage;
    public bool EditarPageIsActive => CurrentPage == _editarPage;
    public bool MetaPageIsActive => CurrentPage == _metaPage;
    public bool TarefaPageIsActive => CurrentPage == _tarefaPage;
    public bool DesempenhoPageIsActive => CurrentPage == _desempenhoPage;
    public bool ClassificacaoPageIsActive => CurrentPage == _classificacaoPage;
    public bool RecompensaPageIsActive => CurrentPage == _recompensaPage;
    public bool ConfiguracaoPageIsActive => CurrentPage == _configuracaoPage;
    
 
    
    private readonly DepartamentoPageViewModel _departamentoPage = new ();
    private readonly InicioPageViewModel _inicioPage =  new ();
    private readonly EditarPageViewModel _editarPage = new ();
    private readonly MetaPageViewModel _metaPage = new ();
    private readonly TarefaPageViewModel _tarefaPage = new ();
    private readonly DesempenhoPageViewModel _desempenhoPage = new ();
    private readonly ClassificacaoPageViewModel _classificacaoPage = new ();
    private readonly RecompensaPageViewModel _recompensaPage = new ();
    private readonly ConfiguracaoPageViewModel _configuracaoPage = new ();
    private readonly PerfilPageViewModel _perfilPage = new ();
    
    
    public MainViewModel()
    {
        CurrentPage = _departamentoPage;
    }
    
    [RelayCommand]
    private void SideMenuResize()
    {
        SideMenuExpanded = !SideMenuExpanded;
    }
    
    [RelayCommand]
    private void GoToInicio() => CurrentPage = _inicioPage;
    
    [RelayCommand]
    private void GoToCriar() => CurrentPage =  _departamentoPage;
    
    [RelayCommand]
    private void GoToEditar() => CurrentPage = _editarPage;
    
    [RelayCommand]
    private void GoToMeta() => CurrentPage = _metaPage;
    
    [RelayCommand]
    private void GoToTarefa() => CurrentPage = _tarefaPage;

    [RelayCommand]
    private void GoToDesempenho() => CurrentPage = _desempenhoPage;

    [RelayCommand]
    private void GoToClassificacao() => CurrentPage = _classificacaoPage;
    
    [RelayCommand]
    private void GoToRecompensa() => CurrentPage = _recompensaPage;
    
    [RelayCommand]
    private void GoToConfiguracao() => CurrentPage = _configuracaoPage;
    
    
}