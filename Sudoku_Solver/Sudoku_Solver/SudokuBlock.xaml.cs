using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku_Solver
{
    /// <summary>
    /// Interaction logic for SudokuBlock.xaml
    /// </summary>
    public partial class SudokuBlock : UserControl
    {
        private TextBox[,] textBoxes = new TextBox[3, 3];

        public SudokuBlock()
        {
            InitializeComponent();

            textBoxes[0, 0] = cell0;
            textBoxes[0, 1] = cell1;
            textBoxes[0, 2] = cell2;

            textBoxes[1, 0] = cell3;
            textBoxes[1, 1] = cell4;
            textBoxes[1, 2] = cell5;

            textBoxes[2, 0] = cell6;
            textBoxes[2, 1] = cell7;
            textBoxes[2, 2] = cell8;

            foreach(TextBox textBox in textBoxes)
            {
                textBox.TextChanged += (e, o) => { OnCellTextChanged(textBox); };
            }
        }

        private void OnCellTextChanged(TextBox textBox)
        {
            // check if cell is empty
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                return;
            }

            // parse string value
            bool valid = int.TryParse(textBox.Text, out int value);

            if (valid && value >= 1 && value <= 9)
            {
                textBox.Foreground = Brushes.Black;
            }
            else
            {
                textBox.Foreground = Brushes.Red;
            }
        }

        public int GetCellValue(int row, int column)
        {
            // check row and column bounds
            if (row < 0 || row > 2 || column < 0 || column > 2)
            {
                return 0;
            }

            // check if cell is empty
            if (string.IsNullOrWhiteSpace(textBoxes[row, column].Text))
            {
                return 0;
            }

            // parse string value
            bool valid = int.TryParse(textBoxes[row, column].Text, out int value);

            if (valid && value >= 1 && value <= 9)
            {
                return value;
            }
            else
            {
                return 0;
            }
        }

        public void SetCellValue(int row, int column, int value)
        {
            // check row and column bounds and value
            if (row < 0 || row > 2 || column < 0 || column > 2)
            {
                return;
            }

            // check if value is valid
            if (value < 1 || value > 9)
            {
                return;
            }

            textBoxes[row, column].Text = value.ToString();
        }

        public void ClearCellValue(int row, int column)
        {
            // check row and column bounds and value
            if (row < 0 || row > 2 || column < 0 || column > 2)
            {
                return;
            }

            textBoxes[row, column].Text = string.Empty;
        }
    }
}
