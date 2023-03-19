using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CrabCrave.Core;
using System;
using System.Threading.Tasks;

namespace CrabCrave
{
    public partial class MainWindow : Window
    {
        private string? filePath;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(0, 0);
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

        private bool TSPChecked(object sender, RoutedEventArgs e)
        {
            if (TSPOption.IsChecked == true)
            {
                return true;
            }

            return false;
        }

        private async void TESTBTN(object sender, RoutedEventArgs e)
        {
            var m = (MainViewModel)DataContext;
            m._map.map[0, 0].Color = (Brushes.Yellow);

            await Task.Delay(1000); // wait for 1 second

            m._map.map[0, 0].Color = (Brushes.Black);
            
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
            string algorithmUsed = AlgorithmChecked(BFSOption, DFSOption);
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

            int rows = res.Count;
            int cols = res[0].Count;

            MainViewModel m = new MainViewModel(rows, cols);
            m.GenerateMatrixElements(res);
            DataContext = m;
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
                            temp.Add(new Node(countX, countY, 3, 0, Brushes.Orange));
                        }
                        else if (value == "R")
                        {
                            temp.Add(new Node(countX, countY, 1, 0, Brushes.LightGreen));
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

            return res;
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

        public void ChangeNodeColor(int x, int y, Brush color)
        {
            int index = x * Columns + y;
            if (index >= 0 && index < MatrixElements.Count)
            {
                MatrixElements[index].setColor(color);
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
