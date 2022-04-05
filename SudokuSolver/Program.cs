using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
		static Stack<Tuple<int, int>> PlacementHistory = new Stack<Tuple<int, int>>();
        static void Main(string[] args)
        {
			int[,] sudoku = new int[9, 9]
			{
				{3,6,5,0,9,2,7,0,8},
				{0,4,0,0,6,1,0,0,0},
				{1,0,7,0,5,8,9,6,4},
				{7,0,2,0,8,0,0,0,0},
				{0,0,0,6,0,0,0,0,2},
				{0,8,3,0,0,0,4,7,0},
				{5,0,0,8,0,0,6,2,0},
				{0,3,0,0,7,0,0,0,0},
				{0,0,0,5,4,0,1,0,7}
			};
			Console.WriteLine("Input:");
			PrintSudoku(sudoku);

			Console.WriteLine("Output:");
			SolveSudoku(ref sudoku);
			PrintSudoku(sudoku);

			Console.ReadLine();

        }

		static void PrintSudoku(int[,] sudoku)
        {
			for(int i = 0; i < 9; i++)
            {
				if(i % 3 == 0)
					Console.Write("\n");
				for (int j = 0; j <= 9; j++)
                {
					if (j % 3 == 0)
					{
						Console.Write(" | ");
						if (j == 9) break;
					}
					else
						Console.Write(" - ");
					Console.Write($" {sudoku[i, j]} ");
                }
				Console.Write("\n");
			}				
        }

		static void SolveSudoku(ref int[,] sudoku)
		{
			var rc = GetNextFreeCell(sudoku, 0);
			if (TrySolveSudoku(ref sudoku, rc.Item1, rc.Item2))
				Console.WriteLine("Valid Sudoku!");
			else
				Console.WriteLine("Cannot solve Sudoku!");
		}

		static Tuple<int, int> GetNextFreeCell(int[,] sudoku, int row)
		{
			for (int i = row; i < 9; i++)
				for (int j = 0; j < 9; j++)
					if (sudoku[i, j] == 0)
						return new Tuple<int, int>(i, j);
			return null;
		}
		

		static bool TrySolveSudoku(ref int[,] sudoku, int row, int col, int tryValue = 1)
		{
			if (row == 9) return true;

			// if we have exhausted all values in current cell, 
			// go back a cell and continue from there
			// however, if there are no more cells to be validated, this is a error scenario
			if (tryValue > 9)
			{
				sudoku[row, col] = 0;
				if (PlacementHistory.Count == 0)
					return false; // somethings not right!
				var t = PlacementHistory.Pop();
				row = t.Item1;
				col = t.Item2;
				// if we have backtracked to reach here, instead of starting over 
				// from 1 as the probable placement, start from value + 1. 
				tryValue = sudoku[row, col] + 1;
				return TrySolveSudoku(ref sudoku, row, col, tryValue);
			}
			
			if (IsValidPlacement(sudoku, row, col, tryValue))
			{
				sudoku[row, col] = tryValue;
				PlacementHistory.Push(new Tuple<int, int>(row, col)); 
				var rc = GetNextFreeCell(sudoku, row);
				if (rc == null)
					return true;
				return TrySolveSudoku(ref sudoku, rc.Item1, rc.Item2);
			}
			else
            {
				return TrySolveSudoku(ref sudoku, row, col, tryValue + 1);
			}				
		}

		static bool IsValidPlacement(int[,] sudoku, int row, int col, int tryValue)
		{
			// check if this value is present in that row/col/subMatrix
			// 1. Is value present in row?
			for (int i = 0; i < 9; i++)
				if (sudoku[row, i] == tryValue)
				{
					return false;
				}

			// 2. Is value present in col? 
			for (int i = 0; i < 9; i++)
				if (sudoku[i, col] == tryValue)
				{
					return false;
				}

			// 3. Is value present in sub-grid? 
			if (!IsPresentInSubgrid(sudoku, row, col, tryValue))
			{
				return false;
			}
			return true;
		}

		static bool IsPresentInSubgrid(int[,] sudoku, int row, int col, int tryValue)
        {
			// sub matrix 1
			if((row == 0 || row == 1 || row == 2) &&
				(col == 0 || col == 1 || col == 2))
            {
				for (int i = 0; i <= 2; i++)
					for (int j = 0; j <= 2; j++)
						if (sudoku[i, j] == tryValue)
							return false;
            }

			// sub matrix 2
			if ((row == 0 || row == 1 || row == 2) &&
				(col == 3 || col == 4 || col == 5))
			{
				for (int i = 0; i <= 2; i++)
					for (int j = 3; j <= 5; j++)
						if (sudoku[i, j] == tryValue)
							return false;
			}

			// sub matrix 3
			if ((row == 0 || row == 1 || row == 2) &&
				(col == 6 || col == 7 || col == 8))
			{
				for (int i = 0; i <= 2; i++)
					for (int j = 6; j <= 8; j++)
						if (sudoku[i, j] == tryValue)
							return false;
			}

			// sub matrix 4
			if ((row == 3 || row == 4 || row == 5) &&
				(col == 0 || col == 1 || col == 2))
			{
				for (int i = 3; i <= 5; i++)
					for (int j = 0; j <= 2; j++)
						if (sudoku[i, j] == tryValue)
							return false;
			}

			// sub matrix 5
			if ((row == 3 || row == 4 || row == 5) &&
				(col == 3 || col == 4 || col == 5))
			{
				for (int i = 3; i <= 5; i++)
					for (int j = 3; j <= 5; j++)
						if (sudoku[i, j] == tryValue)
							return false;
			}

			// sub matrix 6
			if ((row == 3 || row == 4 || row == 5) &&
				(col == 6 || col == 7 || col == 8))
			{
				for (int i = 3; i <= 5; i++)
					for (int j = 6; j <= 8; j++)
						if (sudoku[i, j] == tryValue)
							return false;
			}

			// sub matrix 7
			if ((row == 6 || row == 7 || row == 8) &&
				(col == 0 || col == 1 || col == 2))
			{
				for (int i = 6; i <= 8; i++)
					for (int j = 0; j <= 2; j++)
						if (sudoku[i, j] == tryValue)
							return false;
			}

			// sub matrix 8
			if ((row == 6 || row == 7 || row == 8) &&
				(col == 3 || col == 4 || col == 5))
			{
				for (int i = 6; i <= 8; i++)
					for (int j = 3; j <= 5; j++)
						if (sudoku[i, j] == tryValue)
							return false;
			}

			// sub matrix 8
			if ((row == 6 || row == 7 || row == 8) &&
				(col == 6 || col == 7 || col == 8))
			{
				for (int i = 6; i <= 8; i++)
					for (int j = 6; j <= 8; j++)
						if (sudoku[i, j] == tryValue)
							return false;
			}

			return true;
        }

	}
}
