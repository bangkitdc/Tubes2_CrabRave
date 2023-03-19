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
            popupText.Text = fileContent;
            myPopup.IsOpen = true;

            List<List<Node>> res = GetNodeListFromString(fileContent);

            int rows = res.Count;
            int cols = res[0].Count;

            MainViewModel m = new MainViewModel(rows, cols);
            m.GenerateMatrixElements(res);
            DataContext = m;

            var mainViewModel = (MainViewModel)DataContext;
            mainViewModel.ChangeNodeColor(0, 0, Brushes.Red);
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
                            temp.Add(new Node(countX, countY, true, true, false, false, false, Brushes.Orange));
                        }
                        else if (value == "R")
                        {
                            temp.Add(new Node(countX, countY, false, true, false, false, false, Brushes.LightGreen));
                        }
                        else if (value == "X")
                        {
                            temp.Add(new Node(countX, countY, false, false, false, false, false, Brushes.Black));
                        }
                        else if (value == "T")
                        {
                            temp.Add(new Node(countX, countY, false, true, true, false, false, Brushes.Yellow));
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
        private int _rows;
        private int _columns;
        private ObservableCollection<Node> _matrixElements;

        public MainViewModel(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
        }

        public int Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                OnPropertyChanged(nameof(Rows));
            }
        }

        public int Columns
        {
            get => _columns;
            set
            {
                _columns = value;
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
            //ObservableCollection<List<Node>> nodesCollection = new ObservableCollection<List<Node>>(listOfListNode);

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
