using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Random randy = new Random();

            int[,] map = new int[32, 32];
            int zahl = 0;

            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {

                    if (i == 0 && j == 0)
                    {
                        map[i, j] = 0;
                        continue;
                    }


                    if (j == 0)
                    {
                        map[i, j] = map[i - 1, j];
                    }
                    else if (i == 0)
                    {
                        map[i, j] = map[i, j-1];
                    }
                    else
                    {
                        switch( randy.Next(0, 3) + 1 )
                        {
                            case 1:
                                map[i, j] = map[i - 1, j];
                                break;
                            case 2:
                                map[i, j] = map[i, j - 1];
                                break;
                            case 3:
                                map[i, j] = map[i - 1, j - 1];
                                break;
                        }
                       
                    }

                    if (randy.Next(0, 16) % 8 == 0)
                    {
                        map[i, j ] = randy.Next(0, 8);
                    }

                }

            }


            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {


                    Console.Write( "" +map[i, j]);
                }
                Console.WriteLine();
                
            }

            Console.ReadLine();

        }
    }
}
