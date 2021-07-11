using System;
using System.Collections.Generic;
using System.Linq;

namespace FarmLand
{
    class Program
    {
        static void Main()
        {
            Matrix initMatrix = new Matrix(6, 6);

            initMatrix.Set(0, 0, 1);
            initMatrix.Set(0, 1, 0);
            initMatrix.Set(0, 2, 0);
            initMatrix.Set(0, 3, 1);
            initMatrix.Set(0, 4, 0);
            initMatrix.Set(0, 5, 1);

            initMatrix.Set(1, 0, 1);
            initMatrix.Set(1, 1, 0);
            initMatrix.Set(1, 2, 0);
            initMatrix.Set(1, 3, 1);
            initMatrix.Set(1, 4, 0);
            initMatrix.Set(1, 5, 1);

            initMatrix.Set(2, 0, 0);
            initMatrix.Set(2, 1, 0);
            initMatrix.Set(2, 2, 0);
            initMatrix.Set(2, 3, 0);
            initMatrix.Set(2, 4, 0);
            initMatrix.Set(2, 5, 0);

            initMatrix.Set(3, 0, 1);
            initMatrix.Set(3, 1, 0);
            initMatrix.Set(3, 2, 0);
            initMatrix.Set(3, 3, 0);
            initMatrix.Set(3, 4, 1);
            initMatrix.Set(3, 5, 0);

            initMatrix.Set(4, 0, 0);
            initMatrix.Set(4, 1, 1);
            initMatrix.Set(4, 2, 0);
            initMatrix.Set(4, 3, 0);
            initMatrix.Set(4, 4, 1);
            initMatrix.Set(4, 5, 0);

            initMatrix.Set(5, 0, 0);
            initMatrix.Set(5, 1, 0);
            initMatrix.Set(5, 2, 0);
            initMatrix.Set(5, 3, 0);
            initMatrix.Set(5, 4, 1);
            initMatrix.Set(5, 5, 0);

            initMatrix.ToString();

        }

        public class Cell
        {
            #region Properties

            public int X;
            public int Y;
            public int Value;

            #endregion

            #region Constructors

            public Cell()
            {
                this.X = 0;
                this.Y = 0;
                this.Value = 0;
            }

            public Cell(int x, int y)
                : this()
            {
                this.X = x;
                this.Y = y;
            }

            public Cell(int x, int y, int value)
            {
                this.X = x;
                this.Y = y;
                this.Value = value;
            }

            #endregion
        }

        public class Matrix
        {

            #region Properties

            public int Width;
            public int Height;
            public int[,] Values;

            #endregion

            #region Constructors

            public Matrix(int width, int height)
            {
                this.Width = width;
                this.Height = height;
                this.Values = new int[width, height];
            }

            #endregion

            #region Methods

            public int Get(int x, int y)
            {
                return this.Values[x, y];
            }

            public int Get(double x, double y)
            {
                return this.Values[(int)x, (int)y];
            }

            public void Set(int x, int y, int value)
            {
                this.Values[x, y] = value;
            }

            public void Set(double x, double y, int value)
            {
                this.Values[(int)x, (int)y] = value;
            }

            public Cell GetNeighbor(int x, int y, Direction direction)
            {
                int neighborX = -1, neighborY = -1;
                switch (direction)
                {
                    case Direction.NW: neighborX = x - 1; neighborY = y - 1; break;
                    case Direction.N: neighborX = x; neighborY = y - 1; break;
                    case Direction.NE: neighborX = x + 1; neighborY = y - 1; break;
                    case Direction.E: neighborX = x + 1; neighborY = y; break;
                    case Direction.SE: neighborX = x + 1; neighborY = y + 1; break;
                    case Direction.S: neighborX = x; neighborY = y + 1; break;
                    case Direction.SW: neighborX = x - 1; neighborY = y + 1; break;
                    case Direction.W: neighborX = x - 1; neighborY = y; break;
                }

                if (neighborX >= 0 && neighborX < this.Width)
                {
                    if (neighborY >= 0 && neighborY < this.Height)
                    {
                        return new Cell(neighborX, neighborY, this.Values[neighborX, neighborY]);
                    }
                }
                return null;
            }

            public List<Cell> GetNeighbors(int x, int y, int connectivity)
            {                
                List<Cell> neighbors = new List<Cell>();
                if (connectivity == 4)
                {
                    neighbors.Add(GetNeighbor(x, y, Direction.N));
                    neighbors.Add(GetNeighbor(x, y, Direction.W));
                }
                else if (connectivity == 8)
                {
                    neighbors.Add(GetNeighbor(x, y, Direction.N));
                    neighbors.Add(GetNeighbor(x, y, Direction.W));
                    neighbors.Add(GetNeighbor(x, y, Direction.NW));
                    neighbors.Add(GetNeighbor(x, y, Direction.NE));
                }
                else if (connectivity == 0)
                {
                    neighbors.Add(GetNeighbor(x, y, Direction.N));
                    neighbors.Add(GetNeighbor(x, y, Direction.W));
                    neighbors.Add(GetNeighbor(x, y, Direction.NW));
                    neighbors.Add(GetNeighbor(x, y, Direction.NE));
                    neighbors.Add(GetNeighbor(x, y, Direction.E));
                    neighbors.Add(GetNeighbor(x, y, Direction.SE));
                    neighbors.Add(GetNeighbor(x, y, Direction.S));
                    neighbors.Add(GetNeighbor(x, y, Direction.SW));
                }
               
                return neighbors.Where(cell => cell != null).ToList();
            }

            public override string ToString()
            {
                string result = string.Empty;
                for (int y = 0; y < this.Height; y++)
                {
                    for (int x = 0; x < this.Width; x++)
                    {
                        result += this.Values[x, y] + ",";
                    }
                    result += Environment.NewLine;
                }
                return result;
            }

            #endregion


            public enum Direction
            {
                N, S, E, W, NE, NW, SE, SW
            }
        }

        public class Set : List<int>
        {
        }

        public class SetList : Dictionary<int, Set>
        {
        }

        public static Matrix MarkRegions(Matrix matrix, out SetList equivalentRegions)
        {
            Matrix regionMatrix = new Matrix(matrix.Width, matrix.Height);

            equivalentRegions = new SetList();

            int currentRegion = 1;
            for (int row = 0; row < matrix.Height; row++)
            {
                for (int column = 0; column < matrix.Width; column++)
                {
                    if (matrix.Values[column, row] == 1)
                    {
                        List<Cell> neighbors = matrix.GetNeighbors(column, row, 8);

                        int matchCount = neighbors.Count(cell => cell.Value > 0);

                        if (matchCount == 0)
                        {
                            regionMatrix.Values[column, row] = currentRegion;

                            equivalentRegions.Add(currentRegion, new Set() { currentRegion });

                            currentRegion += 1;
                        }
                        else if (matchCount == 1)
                        {
                            Cell oneCell = neighbors.First(cell => cell.Value == 1);

                            regionMatrix.Values[column, row] = regionMatrix.Values[oneCell.X, oneCell.Y];
                        }
                        else if (matchCount > 1)
                        {
                            List<int> distinctRegions = neighbors.Select(cell => regionMatrix.Values[cell.X, cell.Y]).Distinct().ToList();

                            while (distinctRegions.Remove(0)) ;

                            if (distinctRegions.Count == 1)
                            {
                                regionMatrix.Values[column, row] = distinctRegions[0];
                            }
                            else if (distinctRegions.Count > 1)
                            {
                                int firstRegion = distinctRegions[0];

                                regionMatrix.Values[column, row] = firstRegion;

                                foreach (int region in distinctRegions)
                                {
                                    if (!equivalentRegions[firstRegion].Contains(region))
                                    {

                                        equivalentRegions[firstRegion].Add(region);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return regionMatrix;
        }

        public class Vector : Dictionary<int, int>
        {
        }

        public class UnionFind
        {
            public SetList Sets { get; set; }
            public Vector Roots { get; set; }

            public UnionFind(SetList sets)
            {
                this.Sets = sets;
                this.Roots = new Vector();

                this.Initialize();
                this.Start();
            }

            public void Initialize()
            {
                Roots.Clear();
                foreach (int index in Sets.Keys)
                    foreach (int item in Sets[index])
                        if (!Roots.ContainsKey(item))
                            Roots.Add(item, -1);

                foreach (int index in Sets.Keys)
                    foreach (int item in Sets[index])
                        Roots[item] = Sets[index][0];
            }

            public void Unite(int item1, int item2)
            {
                int item1Root = Roots[item1];
                for (int index = 0; index < Roots.Count; index++)
                {
                    int item = Roots.Keys.ElementAt(index);
                    if (Roots[item] == item1Root)
                        Roots[item] = Roots[item2];
                }
            }

            public void Start()
            {
                foreach (int index in Sets.Keys)
                {
                    var set = Sets[index];
                    for (int i = 0; i < set.Count; i++)
                        for (int j = i + 1; j < set.Count; j++)
                            Unite(set[i], set[j]);
                }
            }
        }
    }
}
