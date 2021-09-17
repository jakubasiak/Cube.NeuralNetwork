using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cube.NeuralNetwork.Visualization
{
    public partial class ChartForm : Form
    {
        public Perceptron Perceptron { get; set; } = new Perceptron(2);
        public List<PredictionPoint> TrainingSet { get; set; } = new List<PredictionPoint>();
        public List<PredictionPoint> TestSet { get; set; } = new List<PredictionPoint>();
        int trainIndex = 0;
        public ChartForm()
        {
            this.InitializeComponent();
            this.InitializeTraningAndTestSet();
            this.InitializeChart();
            this.DrawResults();
            cartesianChart1.LegendLocation = LegendLocation.Right;
        }

        private void InitializeTraningAndTestSet()
        {
            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                var x = random.Next(-100, 100);
                var y = random.Next(-100, 100);
                this.TrainingSet.Add(new PredictionPoint()
                {
                    X = x,
                    Y = y,
                    Label = x > y ? -1.0 : 1.0
                });
            }

            for (int i = 0; i < 500; i++)
            {
                var x = random.Next(-100, 100);
                var y = random.Next(-100, 100);
                this.TestSet.Add(new PredictionPoint()
                {
                    X = x,
                    Y = y,
                });
            }
        }

        private void InitializeChart()
        {
            var xAxis = new Axis()
            {
                Title = "x",
                Separator = new LiveCharts.Wpf.Separator()
                {
                    Step = 10.0,
                    IsEnabled = false,
                },

            };

            var yAxis = new Axis()
            {
                Title = "y",
                Separator = new LiveCharts.Wpf.Separator()
                {
                    Step = 10.0,
                    IsEnabled = false
                }
            };

            this.cartesianChart1.AxisX.Add(xAxis);
            this.cartesianChart1.AxisY.Add(yAxis);
        }

        private void DrawChartSeries(List<PredictionPoint> correctPredictionPoints, List<PredictionPoint> wrongPredictionPoints)
        {
            var line = new List<ObservablePoint>();
            for (int i = -100; i < 100; i++)
            {
                line.Add(new ObservablePoint(i, i));
            }


            var correctPoints = new List<ObservablePoint>();
            foreach (var item in correctPredictionPoints)
            {
                correctPoints.Add(new ObservablePoint(item.X, item.Y));
            }

            var wrongPoints = new List<ObservablePoint>();
            foreach (var item in wrongPredictionPoints)
            {
                wrongPoints.Add(new ObservablePoint(item.X, item.Y));
            }

            var series = new SeriesCollection()
            {
                new ScatterSeries()
                {
                    Values = new ChartValues<ObservablePoint>(correctPoints),
                    Title = "Correct points",
                },
                new ScatterSeries()
                {
                    Values = new ChartValues<ObservablePoint>(wrongPoints),
                    Title = "Wrong points",
                },
                new ScatterSeries()
                {
                    Title = "Line",
                    Values = new ChartValues<ObservablePoint>(line),
                }
            };

            this.cartesianChart1.Series = series;
        }

        private void DrawResults()
        {
            List<PredictionPoint> correctPredictionPoints = new List<PredictionPoint>();
            List<PredictionPoint> wrongPredictionPoints = new List<PredictionPoint>();
            // First guess
            for (int i = 0; i < this.TestSet.Count; i++)
            {
                var point = this.TestSet[i];
                point.Label = this.Perceptron.ComputeOutput(new double[] { point.X, point.Y });
                if (point.X > point.Y)
                {
                    if(point.Label == -1)
                    {
                        correctPredictionPoints.Add(point);
                    } else
                    {
                        wrongPredictionPoints.Add(point);
                    }
                }
                if (point.X < point.Y)
                {
                    if (point.Label == 1)
                    {
                        correctPredictionPoints.Add(point);
                    }
                    else
                    {
                        wrongPredictionPoints.Add(point);
                    }
                }
            }

            this.DrawChartSeries(correctPredictionPoints, wrongPredictionPoints);
            this.label2.Text = $"{string.Join(" - ", this.Perceptron.Weights)}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var item in this.TrainingSet)
            {
                this.Perceptron.Train(new double[] { item.X, item.Y }, item.Label);
                this.DrawResults();
            }
        }
    }
}
