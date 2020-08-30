using Sudoku_Solver.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SudokuBlock[,] blocks = new SudokuBlock[3, 3];

        public MainWindow()
        {
            InitializeComponent();

            blocks[0, 0] = block0;
            blocks[0, 1] = block1;
            blocks[0, 2] = block2;

            blocks[1, 0] = block3;
            blocks[1, 1] = block4;
            blocks[1, 2] = block5;

            blocks[2, 0] = block6;
            blocks[2, 1] = block7;
            blocks[2, 2] = block8;
        }

        private void Button_Solve_Click(object sender, EventArgs e)
        {
            int[,] sudokuValues = new int[9, 9];

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    sudokuValues[x, y] =
                        blocks[(int)Math.Floor(x / 3.0), (int)Math.Floor(y / 3.0)]
                        .GetCellValue(x % 3, y % 3);
                }
            }

            Sudoku solver = new Sudoku();

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    solver.SetValue(x, y, sudokuValues[x, y]);
                }
            }

            bool solved = solver.Solve();

            if (solved)
            {
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        blocks[(int)Math.Floor(x / 3.0), (int)Math.Floor(y / 3.0)]
                        .SetCellValue(x % 3, y % 3, solver.GetValue(x, y));
                    }
                }
            }
        }

        private void Button_Clear_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    blocks[(int)Math.Floor(x / 3.0), (int)Math.Floor(y / 3.0)]
                        .ClearCellValue(x % 3, y % 3);
                }
            }
        }
    }
}
