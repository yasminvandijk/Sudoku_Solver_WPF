using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku_Solver.Solver
{
    class Sudoku
    {
        private readonly int[,] _values = new int[9, 9];

        /// <summary>
        /// prints sudoku to console
        /// </summary>
        public void PrintSudoku()
        {
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    Console.Write(_values[x, y] + " ");

                    if (x % 3 == 2) { Console.Write(" "); }
                }

                Console.WriteLine();

                if (y % 3 == 2) { Console.WriteLine(" "); }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// sets the sudoku's value at given row and column if row, column and value are valid
        /// </summary>
        /// <param name="row">value between 0 and 8</param>
        /// <param name="column">value between 0 and 8</param>
        /// <param name="value">value between 0 and 9</param>
        public void SetValue(int row, int column, int value)
        {
            if (row < 0 || row > 8 || column < 0 || column > 8 || value < 0 || value > 9)
            {
                return;
            }

            _values[row, column] = value;
        }

        /// <summary>
        /// gets value from sudoku if given row and column are valid, otherwise 0
        /// </summary>
        /// <param name="row">value between 0 and 8</param>
        /// <param name="column">value between 0 and 8</param>
        /// <returns></returns>
        public int GetValue(int row, int column)
        {
            if (row < 0 || row > 8 || column < 0 || column > 8)
            {
                return 0;
            }

            return _values[row, column];
        }

        /// <summary>
        /// for each row, checks if all non-zero values are unique
        /// </summary>
        /// <returns></returns>
        private bool AreRowsValid()
        {
            for (int y = 0; y < 9; y++)
            {
                bool[] values = new bool[9];

                for (int x = 0; x < 9; x++)
                {
                    int value = GetValue(x, y);

                    if (value >= 1 && value <= 9)
                    {
                        if (values[value - 1])
                        {
                            return false;
                        }

                        values[value - 1] = true;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// for each column, checks if all non-zero values are unique
        /// </summary>
        /// <returns></returns>
        private bool AreColumnsValid()
        {
            for (int x = 0; x < 9; x++)
            {
                bool[] values = new bool[9];

                for (int y = 0; y < 9; y++)
                {
                    int value = GetValue(x, y);

                    if (value >= 1 && value <= 9)
                    {
                        if (values[value - 1])
                        {
                            return false;
                        }

                        values[value - 1] = true;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// for each block, checks if all non-zero values are unique
        /// </summary>
        /// <returns></returns>
        private bool AreBlocksValid()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    bool[] values = new bool[9];

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            int value = GetValue(x * 3 + i, y * 3 + j);

                            if (value >= 1 && value <= 9)
                            {
                                if (values[value - 1])
                                {
                                    return false;
                                }

                                values[value - 1] = true;
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// checks if all rows, columns and blocks are valid
        /// </summary>
        /// <returns></returns>
        private bool IsSudokuValid()
        {
            return AreRowsValid() && AreColumnsValid() && AreBlocksValid();
        }

        /// <summary>
        /// removes the given value from possible values from all given cells
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private int UpdatePossibleValues(List<SudokuCell> cells, int value)
        {
            int updates = 0;

            cells.ForEach(cell =>
            {
                updates += cell.RemoveFromPossibleValues(value);
            });

            return updates;
        }

        /// <summary>
        /// fill in missing values in a row, column or block if possible
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        private int FillInMissingValues(List<SudokuCell> cells)
        {
            int updates = 0;

            // find all missing values
            List<int> missingValues = Enumerable.Range(1, 9).ToList();

            cells.ForEach(cell =>
            {
                missingValues.Remove(cell.Value);
            });

            // find possible cells for missing values
            missingValues.ForEach(missingValue =>
            {
                List<SudokuCell> possibleCells = cells.Where(c => c.PossibleValues.Contains(missingValue)).ToList();

                if (possibleCells.Count == 1)
                {
                    possibleCells.First().SetValue(missingValue);
                    SetValue(possibleCells.First().Row, possibleCells.First().Column, possibleCells.First().Value);
                    //PrintSudoku();

                    updates++;
                }
            });

            return updates;
        }

        /// <summary>
        /// attempts to solve the sudoku
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        public bool Solve()
        {
            if (!IsSudokuValid())
            {
                return false;
            }

            List<SudokuCell>[] rows = new List<SudokuCell>[9];
            List<SudokuCell>[] columns = new List<SudokuCell>[9];
            List<SudokuCell>[] blocks = new List<SudokuCell>[9];

            List<SudokuCell> fixedCells = new List<SudokuCell>();
            List<SudokuCell> emptyCells = new List<SudokuCell>();

            for (int i = 0; i < 9; i++)
            {
                rows[i] = new List<SudokuCell>();
                columns[i] = new List<SudokuCell>();
                blocks[i] = new List<SudokuCell>();
            }

            // add cells to rows, columns, blocks and to empty or fixed cells
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    SudokuCell cell = new SudokuCell(i, j, GetValue(i, j));

                    rows[cell.Row].Add(cell);
                    columns[cell.Column].Add(cell);
                    blocks[cell.Block].Add(cell);

                    if (cell.Value == 0)
                    {
                        emptyCells.Add(cell);
                    }
                    else
                    {
                        fixedCells.Add(cell);
                    }
                }
            }

            // eliminate fixed cell values from possible values in cells on same rows, columns or blocks
            fixedCells.ForEach(fixedCell =>
            {
                UpdatePossibleValues(rows[fixedCell.Row], fixedCell.Value);
                UpdatePossibleValues(columns[fixedCell.Column], fixedCell.Value);
                UpdatePossibleValues(blocks[fixedCell.Block], fixedCell.Value);
            });

            // keep updating cells untill sudoku is solved or no more updates are possible
            while (emptyCells.Count > 0)
            {
                int updates = 0;

                // get all cells from empty cells that now have a value
                emptyCells.ForEach(cell =>
                {
                    if (cell.Value != 0)
                    {
                        fixedCells.Add(cell);
                        SetValue(cell.Row, cell.Column, cell.Value);
                        updates += UpdatePossibleValues(rows[cell.Row], cell.Value);
                        updates += UpdatePossibleValues(columns[cell.Column], cell.Value);
                        updates += UpdatePossibleValues(blocks[cell.Block], cell.Value);
                    }
                });
                emptyCells.RemoveAll(c => fixedCells.Contains(c));

                // fill in missing values in rows, columns and blocks
                for (int i = 0; i < 9; i++)
                {
                    updates += FillInMissingValues(rows[i]);
                    updates += FillInMissingValues(columns[i]);
                    updates += FillInMissingValues(blocks[i]);
                }

                if (updates == 0)
                {
                    break;
                }
            }

            // check if sudoku is solved
            if (emptyCells.Count == 0)
            {
                return IsSudokuValid();
            }
            else
            {
                // find the empty cell with the least number of possible values
                emptyCells.ForEach(cell =>
                {
                    if (cell.Value != 0)
                    {
                        fixedCells.Add(cell);
                        SetValue(cell.Row, cell.Column, cell.Value);
                    }
                });
                emptyCells.RemoveAll(c => fixedCells.Contains(c));
                emptyCells.Sort((a, b) => a.PossibleValues.Count - b.PossibleValues.Count);

                SudokuCell cell = emptyCells.First();

                for (int i = 1; i <= 9; i++)
                {
                    if (cell.PossibleValues.Contains(i))
                    {
                        Sudoku sudoku = new Sudoku();
                        for (int x = 0; x < 9; x++)
                        {
                            for (int y = 0; y < 9; y++)
                            {
                                sudoku.SetValue(x, y, GetValue(x, y));
                            }
                        }
                        sudoku.SetValue(cell.Row, cell.Column, i);

                        if (sudoku.Solve())
                        {
                            for (int x = 0; x < 9; x++)
                            {
                                for (int y = 0; y < 9; y++)
                                {
                                    SetValue(x, y, sudoku.GetValue(x, y));
                                }
                            }
                            return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}
