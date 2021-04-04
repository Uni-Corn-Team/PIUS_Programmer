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
        public MainWindow()
        {
            InitializeComponent();
            GuiHandler.Init(() => this.Dispatcher.Invoke(() => { DrawDetail(State.Detail); UpdateKnifePosition(); }));
            //TimerCallback timerCB = new TimerCallback((object state) => DrawDetail(State.Detail));
            //Timer t = new Timer(timerCB, null, 0, 1000);
            

        }

        public void AutomaticRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            GuiHandler.SetAutoWork();
        }
        public void ManualRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            GuiHandler.SetManualWork();
        }

        public void StartButtonClicked(object sender, RoutedEventArgs e)
        {
            GuiHandler.Start();
        }

        public void StopButtonClicked(object sender, RoutedEventArgs e)
        {
            GuiHandler.Stop();
        }

        public void SettingsButtonClicked(object sender, RoutedEventArgs e)
        {
            //RandomInit(out int[][][] detailPoints);
            GuiHandler.SetSettings(new Settings(
                (WorkMode)Convert.ToInt32(AutomaticMode.IsChecked ?? false),
                Convert.ToInt32(XMax.Text), Convert.
                ToInt32(YMax.Text),
                Convert.ToInt32(ZMax.Text),
                Convert.ToInt32(TZad.Text)));
            DrawDetail(State.Detail);
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
            GuiHandler.DoStep(this);
        }

        public void FinishButtonClicked(object sender, RoutedEventArgs e)
        {
            GuiHandler.End();
            RemoveDetailPoints(Isometry);
            RemoveDetailPoints(Top);
            RemoveDetailPoints(Left);
            RemoveDetailPoints(Front);
            XMax.Text = ""; YMax.Text = ""; ZMax.Text = ""; TZad.Text = "";
            
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
                for(int j = 0; j < detailPoints[i].Length; j++)
                {
                    for (int k = 0; k < detailPoints[i][j].Length; k++)
                    {
                        
                        if (detailPoints[i][j][k] != 0)
                        {

                            DrawPoint(i,  k,j);
                        }
                    }
                }
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
    }
}
