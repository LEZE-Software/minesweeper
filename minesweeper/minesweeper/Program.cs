using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    class Program
    {
        public static field[,] mines;
        public static int width;
        public static bool isRunning = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Minesweeper!\n");
            Console.Write("Feldgröße angeben: ");
            width = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Okay!");
            Console.Write("\nAnzahl Minen angeben: ");
            int count = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(count.ToString());
            CalculateField(width, count);

            DisplayMinefield(false);

            while (isRunning)
            {
                Console.Write("\nFeld und Aktion (x,y,c/f/o) : ");
                GetAction(Console.ReadLine());
                DisplayMinefield(false);
            }

            Console.Write("Taste drücken zum Verlassen...");
            Console.ReadKey();
        }

        static void GetAction(string action)
        {
            string[] values = action.Split(',');
            int x = Convert.ToInt32(values[0]);
            int y = Convert.ToInt32(values[1]);
            char a = Convert.ToChar(values[2]);

            if(a=='c')
            {
                mines[x, y].flagged = false;
                mines[x, y].open = false;
            }
            else if(a=='f')
            {
                mines[x, y].flagged = true;
                mines[x, y].open = true;
            }
            else if(a=='o')
            {
                if(mines[x,y].mine)
                {
                    Loose();
                }
                else
                {
                    mines[x, y].flagged = false;
                    mines[x, y].open = true;
                }
            }
        }

        static void Loose()
        {
            Console.WriteLine("\n\n Verloren! \n\n");
            DisplayMinefield(true);
            isRunning = false;
        }

        static void CalculateField(int w, int c)
        {
            field[,] minefield = new field[w, w];

            int countALl = w * w;
            int countSet = 0;
            float chance = countALl / c;

            Random R = new Random();

            for (int x=0;x<w;x++)
            {
                for(int y=0;y<w;y++)
                {                
                    int v = R.Next(0, w);
                    if (v <= chance/2 && countSet < c)
                    {
                        countSet++;
                        minefield[x,y].mine = true;
                    }
                }
            }

            for (int x = 1; x < w-1; x++)
            {
                for (int y = 1; y < w-1; y++)
                {
                    if (minefield[x - 1, y - 1].mine) minefield[x, y].count++;
                    if (minefield[x, y - 1].mine) minefield[x, y].count++;
                    if (minefield[x + 1, y - 1].mine) minefield[x, y].count++;
                    if (minefield[x - 1, y].mine) minefield[x, y].count++;
                    if (minefield[x + 1, y].mine) minefield[x, y].count++;
                    if (minefield[x - 1, y + 1].mine) minefield[x, y].count++;
                    if (minefield[x, y + 1].mine) minefield[x, y].count++;
                    if (minefield[x + 1, y + 1].mine) minefield[x, y].count++;
                }
            }

            mines = minefield;
        }

        static void DisplayMinefield(bool display)
        {
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write("[");

                    if(display)
                    {
                        if (mines[x, y].mine)
                        {
                            Console.Write("X");
                        }
                        else
                        {
                            Console.Write(mines[x, y].count);
                        }
                    }                    
                    else
                    {
                        if (mines[x, y].flagged && mines[x, y].open)
                        {
                            Console.Write("M");
                        }
                        else if (mines[x, y].open && !mines[x, y].flagged)
                        {
                            if (mines[x, y].count != 0)
                            {
                                Console.Write(mines[x, y].count);
                            }
                            else
                            {
                                Console.Write(".");
                            }
                        }
                        else if (!mines[x, y].open && !mines[x, y].flagged)
                        {
                            Console.Write(" ");
                        }
                    }                    

                    Console.Write("]");
                }
                Console.Write("\n");
            }
        }
    }

    public struct field
    {
        public int count;
        public bool mine;
        public  bool flagged;
        public bool open;
    }
}
