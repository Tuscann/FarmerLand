using System;

namespace FarmerLand
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Write size : ");
            int size = int.Parse(Console.ReadLine());

            if (size == 6)
            {
                int[,] input = new int[6, 6] {
                 { 1, 0 , 0, 1, 0, 1},
                 { 1, 0 , 0, 1, 0, 1},
                 { 0, 0 , 0, 0, 0, 0},
                 { 1, 0 , 0, 0, 1, 0},
                 { 0, 1 , 0, 0, 1, 0},
                 { 0, 0 , 0, 0, 1, 0}
                };

                int w = input.GetLength(0);
                int h = input.GetLength(1);

                int[] component = new int[w * h];

                void DoUnion(int a, int b)
                {
                    while (component[a] != a)
                    {
                        a = component[a];
                    }

                    while (component[b] != b)
                    {
                        b = component[b];
                    }
                    component[b] = a;
                }

                void UnionCoords(int x, int y, int x2, int y2)
                {
                    if (y2 < h && x2 < w && input[x, y] == 0 && input[x2, y2] == 0)
                        DoUnion(x * h + y, x2 * h + y2);
                }

                for (int i = 0; i < w * h; i++)
                {
                    component[i] = i;
                }

                for (int x = 0; x < w; x++) 
                {
                    for (int y = 0; y < h; y++)
                    {
                        UnionCoords(x, y, x + 1, y);
                        UnionCoords(x, y, x, y + 1);
                    }
                }  
               
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        if (input[x, y] == 0)
                        {
                            Console.Write(" ");
                            continue;
                        }
                        int c = x * h + y;
                        while (component[c] != c)
                        {
                            c = component[c];
                        }
                        Console.Write("F");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
