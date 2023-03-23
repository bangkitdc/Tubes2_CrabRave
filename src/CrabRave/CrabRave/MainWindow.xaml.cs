using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CrabRave.Core;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Threading;
using CrabRave.Core.SearchAlgorithm;

namespace CrabRave
{
    public partial class MainWindow : Window
    {
        private string? filePath;
        private int timePerStep = 0;

        public MainWindow()
        {
            InitializeComponent();
            sliderTime.ValueChanged += sliderViewChanged;
            DataContext = new MainViewModel(0, 0);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private string AlgorithmChecked(RadioButton BFSOption, RadioButton DFSOption)
        {
            if (BFSOption.IsChecked == true)
            {
                return BFSOption.Content.ToString();
            }

            if (DFSOption.IsChecked == true)
            {
                return DFSOption.Content.ToString();
            }

            return null;
        }

        private bool TSPChecked(CheckBox TSPOption)
        {
            if (TSPOption.IsChecked == true)
            {
                return true;
            }

            return false;
        }

        private async void Search(object sender, RoutedEventArgs e)
        {
            var m = (MainViewModel)DataContext;
            bool usedDFS = false;
            bool usedBFS = false;

            Stopwatch stopwatch = new Stopwatch();
            DFS dfs = new DFS();
            BFS bfs = new BFS(m._map);

            stopwatch.Start();

            if (AlgorithmChecked(BFSOption, DFSOption) == "DFS")
            {
                if (TSPChecked(TSPOption))
                {
                    dfs.StartDFS(m._map, true, timePerStep);
                }
                else
                {
                    dfs.StartDFS(m._map, false, timePerStep);
                }

                usedDFS = true;
            } 
            else if (AlgorithmChecked(BFSOption, DFSOption) == "BFS")
            {
                if (TSPChecked(TSPOption))
                {
                    bfs.Search(timePerStep, true);
                }
                else
                {
                    bfs.Search(timePerStep, false);
                }

                usedBFS = true;
            }

            stopwatch.Stop();

            await Task.Run(async () =>
            {
                while (dfs.isRunning)
                {
                    await Task.Delay(0);
                }
            });

            await Task.Run(async () =>
            {
                while (bfs.isRunning)
                {
                    await Task.Delay(0);
                }
            });

            if (usedDFS)
            {
                if (dfs.treasureFound < m._map.treasureCount)
                {
                    MessageBox.Show("No Solution!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                NodesText.Text = dfs.nodesVisited.ToString();
                StepsText.Text = dfs.stepsTaken.ToString();
                RouteText.Text = dfs.route.Length != 0 ? bfs.Route : " ";
            }

            if (usedBFS)
            {
                if (bfs.treasureFound < m._map.treasureCount)
                {
                    MessageBox.Show("No Solution!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                NodesText.Text = bfs.NodeVisited.ToString();
                StepsText.Text = bfs.Steps.ToString();
                RouteText.Text = bfs.Route.Length != 0 ? bfs.Route : " ";
            }
            
            ExecutionText.Text = stopwatch.Elapsed.TotalMilliseconds.ToString() + " ms";
            SearchBtn.Visibility = Visibility.Hidden;
        }

        private void BrowseBtnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt Files | *.txt";
            fileDialog.Title = "Please select a txt file...";
            fileDialog.Multiselect = false;

            bool? success = fileDialog.ShowDialog();
            if (success == true)
            {
                filePath = fileDialog.FileName;
                string fileName = fileDialog.SafeFileName;
                FilePathTextBox.Text = fileName;
            }
        }

        private void VisualizeClick(object sender, RoutedEventArgs e)
        {
            string fileName = FilePathTextBox.Text;
            if (fileName == "")
            {
                MessageBox.Show("Please select your file first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (!File.Exists(filePath))
            {
                MessageBox.Show("The file does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string fileContent = File.ReadAllText(filePath);

            List<List<Node>> res = GetNodeListFromString(fileContent);

            if (res == null) {
                return;
            }

            int rows = res.Count;
            int cols = res[0].Count;

            MainViewModel m = new MainViewModel(rows, cols);
            m.GenerateMatrixElements(res);
            DataContext = m;

            SearchBtn.Visibility = Visibility.Visible;
        }

        private List<List<Node>> GetNodeListFromString(string fileContent)
        {
            List<List<Node>> res = new List<List<Node>>();
            // Get Rid 
            string[] lines = fileContent.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Trim();
            }

            int countK = 0;
            int countX = 0;
            int countY = 0;

            foreach (string line in lines)
            {
                string[] values = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<Node> temp = new List<Node>();

                foreach (string value in values)
                {
                    if (value.Length != 1)
                    {
                        MessageBox.Show("The txt file is out of format", "Error", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return null;
                    }
                    else
                    {
                        if (value == "K")
                        {
                            temp.Add(new Node(countX, countY, 3, 0, Brushes.Wheat));
                            countK++;
                        }
                        else if (value == "R")
                        {
                            temp.Add(new Node(countX, countY, 1, 0, Brushes.White));
                        }
                        else if (value == "X")
                        {
                            temp.Add(new Node(countX, countY, 0, 0, Brushes.Black));
                        }
                        else if (value == "T")
                        {
                            temp.Add(new Node(countX, countY, 2, 0, Brushes.Yellow));
                        }
                        else
                        {
                            MessageBox.Show("The character in txt file is out of format", "Error", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            return null;
                        }

                        countY++;
                    }
                }
                countY = 0;
                countX++;
                res.Add(temp);
            }

            if (countK != 1)
            {
                MessageBox.Show("Krusty Krab(K) should be 1 on the map", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }

            return res;
        }
        private void sliderViewChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderText.Text = "" + Math.Round(e.NewValue) + " ms";
            timePerStep = (int)Math.Round(e.NewValue);
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        public Map _map;
        public ObservableCollection<Node> _matrixElements { get; set; }
        public Map Map
        {
            get { return _map; }
            set
            {
                if (_map != value)
                {
                    _map = value;
                    OnPropertyChanged(nameof(Map));
                    UpdateCollection();
                }
            }
        }

        public MainViewModel(int rows, int cols)
        {
            _map = new Map(rows, cols);
            _matrixElements = new ObservableCollection<Node>();
        }

        public void UpdateCollection()
        {
            Node[,] map = _map.map;
            _matrixElements.Clear();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    _matrixElements.Add(map[i, j]);
                }
            }
        }

        public int Rows
        {
            get => _map.RowEff;
            set
            {
                _map.RowEff = value;
                OnPropertyChanged(nameof(Rows));
            }
        }

        public int Columns
        {
            get => _map.ColEff;
            set
            {
                _map.ColEff = value;
                OnPropertyChanged(nameof(Columns));
            }
        }

        public ObservableCollection<Node> MatrixElements
        {
            get => _matrixElements;
            set
            {
                _matrixElements = value;
                OnPropertyChanged(nameof(MatrixElements));
            }
        }

        public void GenerateMatrixElements(List<List<Node>> listOfListNode)
        {
            _map.GenerateMap(listOfListNode);

            ObservableCollection<Node> elements = new ObservableCollection<Node>();
            int rows = listOfListNode.Count;
            int cols = listOfListNode[0].Count;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // was Node(i, j, Brushes.WhiteSmoke)
                    elements.Add(listOfListNode[i][j]);
                }
            }

            MatrixElements = elements;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
