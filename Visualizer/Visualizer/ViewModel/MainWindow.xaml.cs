using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Manager;


namespace Visualizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool isFinished;
        public MainWindow()
        {
            InitializeComponent();
            GuiHandler.Init(() => this.Dispatcher.Invoke(() =>
            {
                if (GuiHandler.isChanged)
                {
                    DrawDetail(State.Detail);
                    UpdateKnifePosition();
                }
                isFinished = GuiHandler.isFinished;
            }));

            BlockAll();
            SettingsButton.IsEnabled = true;
        }

        public void StartButtonClicked(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(XMax.Text, out int xmax) && (xmax <= 20) && (xmax >= 3)
                && Int32.TryParse(YMax.Text, out int ymax) && (ymax <= 4) && (ymax >= 1)
                && Int32.TryParse(ZMax.Text, out int zmax) && (zmax <= 2) && (zmax >= 1)
                && Int32.TryParse(TZad.Text, out int tzad) && (tzad <= 3000) && (tzad >= 100))
            {
                if (ManualMode.IsChecked == true)
                {
                    GuiHandler.SetManualWork();
                }
                else
                {
                    GuiHandler.SetAutoWork();
                }

                BlockAll();
                StopButton.IsEnabled = true;

                GuiHandler.Start();
            }
        }

        public void StopButtonClicked(object sender, RoutedEventArgs e)
        {
            BlockAll();

            AutomaticMode.IsEnabled = true;
            ManualMode.IsEnabled = true;


            if (AutomaticMode.IsChecked ?? false)
            {
                StartButton.IsEnabled = true;
            }

            StepButton.IsEnabled = true;
            EndButton.IsEnabled = true;

            GuiHandler.Stop();
        }

        public void SettingsButtonClicked(object sender, RoutedEventArgs e)
        {
            BlockAll();
            XMax.IsEnabled = true;
            YMax.IsEnabled = true;
            ZMax.IsEnabled = true;
            TZad.IsEnabled = true;

            AutomaticMode.IsEnabled = true;
            ManualMode.IsEnabled = true;

            SaveSettingsButton.IsEnabled = true;
        }

        public void SaveSettingsButtonClicked(object sender, RoutedEventArgs e)
        {
            GuiHandler.SetSettings(new Settings(
                (WorkMode)Convert.ToInt32(AutomaticMode.IsChecked ?? false),
                Convert.ToInt32(XMax.Text), Convert.
                ToInt32(YMax.Text),
                Convert.ToInt32(ZMax.Text),
                Convert.ToInt32(TZad.Text)));
            DrawDetail(State.Detail);

            BlockAll();

            SettingsButton.IsEnabled = true;
            if (AutomaticMode.IsChecked ?? false)
            {
                StartButton.IsEnabled = true;
            }
            StepButton.IsEnabled = true;
        }

        private void RandomInit(out int[][][] detailPoints)
        {
            Random random = new Random();
            detailPoints = new int[20][][];
            for (int i = 0; i < detailPoints.Length; i++)
            {
                detailPoints[i] = new int[4][];
                for (int j = 0; j < detailPoints[i].Length; j++)
                {
                    detailPoints[i][j] = new int[2];
                    for (int k = 0; k < detailPoints[i][j].Length; k++)
                    {
                        detailPoints[i][j][k] = random.Next(0, 2);
                        if (i == 19 && j == 3 && k == 1) detailPoints[i][j][k] = 0;
                    }
                }
            }
        }

        public void StepButtonClicked(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(XMax.Text, out int xmax) && (xmax <= 20) && (xmax >= 3)
                   && Int32.TryParse(YMax.Text, out int ymax) && (ymax <= 4) && (ymax >= 1)
                   && Int32.TryParse(ZMax.Text, out int zmax) && (zmax <= 2) && (zmax >= 1)
                   && Int32.TryParse(TZad.Text, out int tzad) && (tzad <= 3000) && (tzad >= 100))
            {
                BlockAll();
                StepButton.IsEnabled = true;
                if (AutomaticMode.IsChecked ?? false)
                {
                    StartButton.IsEnabled = true;
                }
                EndButton.IsEnabled = true;

                GuiHandler.DoStep(this);
            }
        }

        public void FinishButtonClicked(object sender, RoutedEventArgs e)
        {
            BlockAll();
            SettingsButton.IsEnabled = true;

            GuiHandler.End();
            RemoveDetailPoints(Isometry);
            RemoveDetailPoints(Top);
            RemoveDetailPoints(Left);
            RemoveDetailPoints(Front);
            XMax.Text = "0"; YMax.Text = "0"; ZMax.Text = "0"; TZad.Text = "100";
            currentX.Content = "0"; currentY.Content = "0"; currentZ.Content = "0";
        }

        private void RemoveDetailPoints(Viewport3D vp)
        {
            List<Visual3D> toRemove = new List<Visual3D>();
            foreach (var child in vp.Children)
            {
                if (((ModelVisual3D)child).Content.GetType() == typeof(GeometryModel3D))
                {
                    toRemove.Add(child);
                }
            }
            foreach (var child in toRemove)
            {
                vp.Children.Remove(child);
            }
            toRemove.Clear();
        }

        public void DrawDetail(int[][][] detailPoints)
        {
            RemoveDetailPoints(Isometry);
            RemoveDetailPoints(Top);
            RemoveDetailPoints(Left);
            RemoveDetailPoints(Front);

            for (int i = 0; i < detailPoints.Length; i++)
            {
                for (int j = 0; j < detailPoints[i].Length; j++)
                {
                    for (int k = 0; k < detailPoints[i][j].Length; k++)
                    {
                        if (detailPoints[i][j][k] != 0)
                        {
                            DrawPoint(i, k, j);
                        }
                    }
                }
            }
            
            EndButton.IsEnabled = true;

            if ((AutomaticMode.IsChecked ?? false) && !StartButton.IsEnabled)
            {
                StopButton.IsEnabled = !isFinished;
            }

            if (AutomaticMode.IsChecked ?? false)
            {
                StartButton.IsEnabled = true;
            }
            else
            {
                StepButton.IsEnabled = !isFinished;
            }
        }

        public void DrawPoint(int pointX, int pointY, int pointZ)
        {
            Isometry.Children.Add(GetModelVisual3D(pointX, pointY, pointZ));
            Top.Children.Add(GetModelVisual3D(pointX, pointY, pointZ));
            Left.Children.Add(GetModelVisual3D(pointX, pointY, pointZ));
            Front.Children.Add(GetModelVisual3D(pointX, pointY, pointZ));
        }

        private ModelVisual3D GetModelVisual3D(int pointX, int pointY, int pointZ)
        {
            ModelVisual3D mv = new ModelVisual3D()
            {
                Content = new GeometryModel3D()
                {
                    Geometry = new MeshGeometry3D()
                    {
                        Positions = new Point3DCollection(new List<Point3D>()
                        {   new Point3D(pointX, pointY, pointZ), new Point3D(pointX+1, pointY, pointZ),
                            new Point3D(pointX, pointY+1, pointZ), new Point3D(pointX+1, pointY+1, pointZ),
                            new Point3D(pointX, pointY, pointZ+1), new Point3D(pointX+1, pointY, pointZ+1),
                            new Point3D(pointX, pointY+1, pointZ+1), new Point3D(pointX+1, pointY+1, pointZ+1)
                        }),
                        TriangleIndices = new Int32Collection(new List<int>()
                        {
                            0, 2, 1, 1, 2, 3, 0, 4, 2, 2, 4, 6, 0, 1, 4, 1, 5, 4, 1, 7, 5, 1, 3, 7, 4, 5, 6, 7, 6, 5, 2, 6, 3, 3, 6, 7
                        })
                    },
                    Material = new DiffuseMaterial()
                    {
                        Brush = new SolidColorBrush()
                        {
                            Color = Color.FromRgb(127, 255, 212)
                        }
                    }
                }
            };
            return mv;
        }

        public void UpdateKnifePosition()
        {
            currentX.Content = State.CoordinateX.ToString();
            currentY.Content = State.CoordinateY.ToString();
            currentZ.Content = State.CoordinateZ.ToString();
        }

        private void BlockAll()
        {
            XMax.IsEnabled = false;
            YMax.IsEnabled = false;
            ZMax.IsEnabled = false;
            TZad.IsEnabled = false;

            AutomaticMode.IsEnabled = false;
            ManualMode.IsEnabled = false;

            SettingsButton.IsEnabled = false;
            SaveSettingsButton.IsEnabled = false;

            StartButton.IsEnabled = false;
            StepButton.IsEnabled = false;
            StopButton.IsEnabled = false;
            EndButton.IsEnabled = false;
        }
    }
}
