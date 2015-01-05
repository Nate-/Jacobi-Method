//Nathaniel Quan
//Jacobi iteration

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class Jacobi_Method
{
    static void Main()
    {
        //Print introduction
        Console.WriteLine("Jacobi Iteration\n\n");

        //Constants for comparisons
        const double TOL = 1E-8;
        const int MAX_ITERATIONS = 100;

        //Create a list of integers to store numbers
        List<double> values = new List<double>();

        //Integers to store rows and columns and total size of matrix
        int rows = 0, col = 0;

        //Prompt user for filepath
        Console.Write("\tEnter filepath: ");
        string filePath = Console.ReadLine();

        //Open file and read in data
        foreach (string input in File.ReadAllLines(filePath))
        {
            //Check for empty line caused by white spaces before and after valid values
            if (input.Trim() == "") continue;

            //Each line of text represents a row in the matrix
            rows++;

            //Remove extra spaces before, after, and between values
            string line = System.Text.RegularExpressions.Regex.Replace(input, @"\s+", " ");
            line = line.Trim();

            //Break up line at separating spaces
            string[] substr = line.Split(' ');

            //Add the values to list
            foreach (string s in substr)
            {
                values.Add(Double.Parse(s));
            }
        }

        //The number of rows times the number of columns is the size of the matrix
        col = values.Count() / rows;

        //Create a matrix with a 2D array
        double[,] A = new double[rows, col];
        double[] X0 = new double[rows];
        double[] X1 = new double[rows];
        int indexOfX = 0;
        bool maxIterationsReached = true;
        bool solutionExists = true;

        //Initialize A and U with values from text file
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < col; c++)
            {
                A[r, c] = values[r * col + c];
            }

        //Output original matrix
        Console.WriteLine(String.Format("{0,30}", "\n\nOriginal Matrix\n"));
        printMatrix(A, rows, col);

        //Check to see if any diagonal elements of A are 0, if so no solution exists
        for (int r = 0; r < rows; r++)
            if (A[r, r] == 0) solutionExists = false;

        if (solutionExists == true)
        {
            //Solve the matrix A for the x value of the row
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < col; c++)
                    if (r != c)
                        A[r, c] = -1* A[r, c] / A[r, r];

            for (int r = 0; r < rows; r++)
                A[r, col - 1] = -1 * A[r, col - 1];

            //Set the diagonal elements to 0
            for (int r = 0; r < rows; r++)
                A[r, r] = 0;

            //Do iterations until difference between values of X0 and X1 are within TOL or MAX_ITERATIONS complete
            solutionExists = false;
            for (int i = 0; i < MAX_ITERATIONS; i++)
            {
                if (i % 2 == 0)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        //Reset the storage array
                        X1[r] = 0;

                        //Add the multiplier and inserted variable
                        for (int c = 0; c < col - 1; c++)
                            X1[r] += A[r, c] * X0[c];

                        //Add the constant
                        X1[r] += A[r, col - 1];
                    }
                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        //Reset storage array
                        X0[r] = 0;

                        //Add the multiplier and inserted variable
                        for (int c = 0; c < col - 1; c++)
                            X0[r] += A[r, c] * X1[c];

                        //Add the constant
                        X0[r] += A[r, col - 1];
                    }
                }

                //Test for convergence
                int maxIndex;
                if (i % 2 == 0)
                {
                    maxIndex = getMax(X0, rows);
                    if (Math.Abs(X0[maxIndex] - X1[maxIndex]) / Math.Abs(X0[maxIndex]) < TOL)
                    {
                        indexOfX = 0;
                        solutionExists = true;
                        maxIterationsReached = false;
                        break;
                    }

                }
                else
                {
                    maxIndex = getMax(X1, rows);
                    if (Math.Abs(X1[maxIndex] - X0[maxIndex]) / Math.Abs(X1[maxIndex]) < TOL)
                    {
                        indexOfX = 1;
                        solutionExists = true;
                        maxIterationsReached = false;
                        break;
                    }
                }

            } // End max iteration loop
        } //End if solution exists check

        //Output solutions
        if (solutionExists)
        {
            Console.WriteLine("\nApproximate solutions:");
            if (indexOfX == 0)
                for (int i = 0; i < rows; i++)
                    Console.WriteLine("\tx" + (i + 1) + " = " + String.Format("{0:0.00000000}",X0[i]));
            else
                for (int i = 0; i < rows; i++)
                    Console.WriteLine("\tx" + (i + 1) + " = " + String.Format("{0:0.00000000}",X1[i]));
        }
        else
        {
            if (maxIterationsReached)
                Console.WriteLine("\nMaximum number of iterations exceeded.");
            Console.WriteLine("\nNo solution.");
        }

        //Pause the screen when program is done running
        Console.Write("\nPress any key to continue . . . ");
        Console.ReadKey(true);
    
    } //End Main

    static public void printMatrix(double[,] mat, int r, int c)
    //Prints out the matrix to the screen
    {
        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < c; j++)
                //Write the matrix value with up to 4 decimal points and format output
                //to fit in 10 character squares, right-aligned
                Console.Write(String.Format("{0,10}", String.Format("{0:0.####}", mat[i, j])));

            //Start each row on a new line
            Console.Write("\n");
        }

        //Blank line for formatting purposes
        Console.WriteLine("\n");
    }

    static public void dSwap(ref double x, ref double y)
    //Swaps the values of two doubles; x and y
    {
        double temp = x;
        x = y;
        y = temp;
    }

    static public int getMax(double[] x, int size)
    //Return the index with the max value
    {
        int index = 0;
        for (int i = 1; i < size; i++)
            if (x[index] < x[i]) index = i;
        return index;
    }
}