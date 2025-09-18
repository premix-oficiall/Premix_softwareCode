using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;

namespace Premix.ViewModels;

public partial class DesempenhoPageViewModel : ViewModelBase
{
    // Donut Charts
    public IEnumerable<ISeries> DonutSeries { get; set; } =
        new ISeries[]
        {
            new PieSeries<double> { Values = new double[] {75}, Name="Ativos", Fill = new SolidColorPaint(SKColors.Gold) },
            new PieSeries<double> { Values = new double[] {25}, Name="Inativos", Fill = new SolidColorPaint(SKColors.Gray) }
        };

    public IEnumerable<ISeries> DonutSeries2 { get; set; } =
        new ISeries[]
        {
            new PieSeries<double> { Values = new double[] {60}, Fill = new SolidColorPaint(SKColors.Gold) },
            new PieSeries<double> { Values = new double[] {40}, Fill = new SolidColorPaint(SKColors.Gray) }
        };

    // Área Chart
    public IEnumerable<ISeries> AreaSeries { get; set; } =
        new ISeries[]
        {
            new LineSeries<double>
            {
                Values = new double[] { 3, 5, 4, 7, 6, 8, 9 },
                Stroke = new SolidColorPaint(SKColors.Gold, 2),
                Fill = new SolidColorPaint(SKColors.Gold.WithAlpha(80))
            }
        };

    // Coluna Chart
    public IEnumerable<ISeries> ColumnSeries { get; set; } =
        new ISeries[]
        {
            new ColumnSeries<double>
            {
                Values = new double[] { 5, 7, 3, 6, 8 },
                Fill = new SolidColorPaint(SKColors.Gold)
            }
        };

    // Eixos
    public Axis[] XAxes { get; set; } = new Axis[] { new Axis { Labels = new[] { "Jan", "Fev", "Mar", "Abr", "Mai" } } };
    public Axis[] YAxes { get; set; } = new Axis[] { new Axis { MinLimit = 0, MaxLimit = 10 } };
}