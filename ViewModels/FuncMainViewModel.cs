using System.Runtime.InteropServices.JavaScript;
using Avalonia.Svg.Skia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Premix.Views;
using SkiaSharp;


namespace Premix.ViewModels;


public partial class FuncMainViewModel : ViewModelBase
{
   private const string buttonActiveClass = "active";


   [ObservableProperty] [NotifyPropertyChangedFor(nameof(SomeWidth))]
   private bool _sideMenuExpanded = true;


   public int SomeWidth => _sideMenuExpanded ? 220 : 75;


   [ObservableProperty]
   [NotifyPropertyChangedFor(nameof(FuncInicioPageIsActive))]
   [NotifyPropertyChangedFor(nameof(FuncDesempenhoPageIsActive))]
   [NotifyPropertyChangedFor(nameof(FuncPreparacaoPageIsActive))]
   [NotifyPropertyChangedFor(nameof(FuncConfiguracaoPageIsActive))]
   [NotifyPropertyChangedFor(nameof(FuncEntregasPageIsActive))]
   [NotifyPropertyChangedFor(nameof(FuncPerfilPageIsActive))]
   private ViewModelBase _currentPage;


   public bool FuncInicioPageIsActive => CurrentPage == _funcInicioPage;
   public bool FuncDesempenhoPageIsActive => CurrentPage == _funcDesempenhoPage;
   public bool FuncPreparacaoPageIsActive => CurrentPage == _funcPreparacaoPage;
   public bool FuncConfiguracaoPageIsActive => CurrentPage == _funcConfiguracaoPage;
   public bool FuncEntregasPageIsActive => CurrentPage == _funcEntregasPage;
   public bool FuncPerfilPageIsActive => CurrentPage == _funcPerfilPage;


   private readonly FuncDesempenhoPageViewModel _funcDesempenhoPage = new();
   private readonly FuncInicioPageViewModel _funcInicioPage = new();
   private readonly FuncPreparacaoPageViewModel _funcPreparacaoPage = new();
   private readonly FuncConfiguracaoPageViewModel _funcConfiguracaoPage = new();
   private readonly FuncEntregasPageViewModel _funcEntregasPage = new();
   private readonly FuncPerfilPageViewModel _funcPerfilPage = new();


   public FuncMainViewModel()
   {
       CurrentPage = _funcInicioPage;
   }


   [RelayCommand]
   private void SideMenuResize()
   {
       SideMenuExpanded = !SideMenuExpanded;
   }


   [RelayCommand]
   private void GoToFuncInicio() => CurrentPage = _funcInicioPage;


   [RelayCommand]
   private void GoToFuncDesempenho() => CurrentPage = _funcDesempenhoPage;


   [RelayCommand]
   private void GoToFuncPreparacao() => CurrentPage = _funcPreparacaoPage;


   [RelayCommand]
   private void GoToFuncConfiguracao() => CurrentPage = _funcConfiguracaoPage;
  
   [RelayCommand]
   private void GoToFuncEntregas() => CurrentPage = _funcEntregasPage;


   [RelayCommand]
   private void GoToFuncPerfil() => CurrentPage = _funcPerfilPage;
   
   
}
