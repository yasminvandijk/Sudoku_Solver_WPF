using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Sudoku_Solver.Solver
{
    class SudokuCell
    {
        public readonly int Row;
        public readonly int Column;
        public readonly int Block;

        public int Value { get; private set; }
        public List<int> PossibleValues { get; private set; } = new List<int>();

        public SudokuCell(int row, int column, int value)
        {
            Row = row;
            Column = column;
            Block = (int)(3 * Math.Floor(row / 3.0) + Math.Floor(column / 3.0));
            Value = value;

            if (value == 0)
            {
                for (int i = 1; i <= 9; i++)
                {
                    PossibleValues.Add(i);
                }
            }
        }

        /// <summary>
        /// remove a value from possible values, if one possible value is left this cells value is set
        /// returns number of updated values
        /// </summary>
        /// <param name="value"></param>
        public int RemoveFromPossibleValues(int value)
        {
            if (!PossibleValues.Contains(value))
            {
                return 0;
            }

            PossibleValues.Remove(value);

            if (PossibleValues.Count == 1)
            {
                Value = PossibleValues.First();
            }

            return 1;
        }

        /// <summary>
        /// set value for this cell
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int SetValue(int value)
        {
            if (Value != 0 || !PossibleValues.Contains(value))
            {
                return 0;
            }

            Value = value;

            PossibleValues = new List<int>() { value };

            return 1;
        }
    }
}
