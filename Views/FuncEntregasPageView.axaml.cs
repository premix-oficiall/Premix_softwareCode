using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Premix.Views;

public partial class FuncEntregasPageView : UserControl
{
    
    public FuncEntregasPageView()
    {
        InitializeComponent();
        // Registra eventos de Drag & Drop no DropZone
        DropZone.AddHandler(DragDrop.DragOverEvent, OnDropZoneDragOver, handledEventsToo: true);
        DropZone.AddHandler(DragDrop.DropEvent, OnDropZoneDrop, handledEventsToo: true);
    }


    private void OnSearchKeyUp(object? sender, KeyEventArgs e)
    {
        StatusText.Text = $"Buscando por: {SearchBox.Text}";
    }


    private void OnTodasClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        StatusText.Text = "Filtro aplicado: Todas";
    }


    private void OnMinhasClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        StatusText.Text = "Filtro aplicado: Minhas Tarefas";
    }


    private void OnEntregarClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        StatusText.Text = "Clique em Entregar Tarefa";
    }


    private void OnDropZoneDragOver(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.FileNames))
            e.DragEffects = DragDropEffects.Copy;
        else
            e.DragEffects = DragDropEffects.None;


        e.Handled = true;
    }


    private void OnDropZoneDrop(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.FileNames))
        {
            var files = e.Data.GetFileNames();
            foreach (var file in files)
            {
                Console.WriteLine($"Arquivo solto: {file}");
            }
        }
    }


}