using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSimulator
{
    public class GameEngine
    {
        public uint counterGeneration { get; private set; }
        private bool[,] field;
        private readonly int rows, cols;
        private Random rand = new Random();
        public GameEngine(int rows, int cols, int density)
        {
            this.cols = cols;
            this.rows = rows;
            field = new bool[cols, rows];
            for (int i = 0; i < cols; ++i)
            {
                for (int j = 0; j < rows; ++j)
                {
                    field[i, j] = rand.Next(density) == 0;
                }
            }
        }
        private bool ValidateCellPosition(int i, int j)
        {
            return i >= 0 && j >= 0 && i < cols && j < rows;
        }
        private void UpdatePosition(int i, int j, bool val)
        {
            if (ValidateCellPosition(i, j))
                field[i, j] = val;
        }
        public void AddCell(int i, int j)
        {
            UpdatePosition(i, j, true);
        }
        public void RemoveCell(int i, int j)
        {
            UpdatePosition(i, j, false);
        }
        public void NextGeneration()
        {
            
            var newfield = new bool[cols, rows];

            for (int i = 0; i < cols; ++i)
            {
                for (int j = 0; j < rows; ++j)
                {
                    int neighbours_count = CountNeighbours(i, j);
                    if (!field[i, j] && neighbours_count == 3)
                    {
                        newfield[i, j] = true;
                    }
                    else if (field[i, j] && (neighbours_count < 2 || neighbours_count > 3))
                    {
                        newfield[i, j] = false;
                    }
                    else
                    {
                        newfield[i, j] = field[i, j];
                    }
                    
                }
            }
            field = newfield;
            ++counterGeneration;
        }
        public bool[,] GetCurrentGeneration()
        {
            var res = new bool[cols, rows];
            for (int i = 0; i < cols; ++i)
            {
                for (int j = 0; j < rows; ++j)
                {
                    res[i,j] = field[i,j];
                }
            }
            return res;
        }
        private int CountNeighbours(int x, int y)
        {
            int count = 0, col, row;
            bool isChecked = false;

            for (int i = -1; i < 2; ++i)
            {
                for (int j = -1; j < 2; ++j)
                {
                    col = (x + i + cols) % cols;
                    row = (y + j + rows) % rows;
                    isChecked = col == x && row == y;
                    if (field[col, row] && !isChecked)
                    {
                        ++count;
                    }
                }
            }
            return count;
        }
    }
}
