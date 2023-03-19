using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CrabCrave
{
    public partial class MainWindow : Window
    {
        private string filePath;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(3, 3);
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
            if (fileName == "Your File Here")
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

            GenerateMatrixElements();
        }

        public int Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                GenerateMatrixElements();
                OnPropertyChanged(nameof(Rows));
            }
        }

        public int Columns
        {
            get => _columns;
            set
            {
                _columns = value;
                GenerateMatrixElements();
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

        private void GenerateMatrixElements()
        {
            var elements = new ObservableCollection<Node>();

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    // was Node(i, j, Brushes.WhiteSmoke)
                    elements.Add(new Node(i, j, false, false, false, true, false, Brushes.WhiteSmoke));
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
