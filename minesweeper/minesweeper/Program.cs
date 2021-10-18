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
        public static int width = 0;
        public static int countMines = 0;
        public static bool lost = false;
        public static bool isRunning = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Minesweeper!\n\n f- Flagge setzen. \n c- Feld zurücksetzen \n o- Feld öffnen\n\n");
            
            NewGame();           

            Console.Write("Taste drücken zum Verlassen...");
            Console.ReadKey();
        }

        static void NewGame()
        {
            isRunning = true;
            lost = false;
            Console.Write("Feldgröße angeben (Kantenlänge): ");
            width = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Okay!");
            Console.Write("\nAnzahl Minen angeben: ");
            countMines = Convert.ToInt32(Console.ReadLine());
            CalculateField(width, countMines);

            DisplayMinefield(false);

            while (isRunning)
            {
                Console.Write("\nFeld und Aktion (x,y,c/f/o) : ");
                GetAction(Console.ReadLine());
                CheckMap();
                if(!lost)
                {
                    DisplayMinefield(false);
                }                
            }

            Console.Write("\n\n Neues Spiel starten? (Y/N) :");
            if(Console.ReadLine()=="Y")
            {
                NewGame();
            }
            else if(Console.ReadLine()=="N")
            {
                // Nothing.
            }
        }

        static void GetAction(string action)
        {
            string[] values = action.Split(',');
            int x = Convert.ToInt32(values[0])-1;
            int y = Convert.ToInt32(values[1])-1;
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
                mines[x, y].flagged = false;
                mines[x, y].open = true;
            }
        }

        static void OpenField(int x, int y)
        {
            int topLeft = 0;
            int topRight = 0;
            int downLeft = 0;
            int downRight = 0;

            if(x==0 && y==0)
            {
                for(int x_=0;x_<=x_+1;x_++)
                {
                    for (int y_ = 0; y_ <= y_ + 1; y_++)
                    {
                        if(!mines[x_, y_].open && !mines[x_,y_].mine)
                        {
                            OpenField(x_, y_);
                        }
                    }
                }
            }
        }

        static void CheckMap()
        {
            int countFlagged=0;

            foreach(field f in mines)
            {
                if(f.mine && f.open && !f.flagged)
                {
                    Loose();
                }
                if(f.mine && f.flagged)
                {
                    countFlagged++;
                }
            }

            if(countFlagged == countMines)
            {
                Win();
            }
        }

        static void Loose()
        {
            Console.WriteLine("\n\n Verloren! \n\n");
            DisplayMinefield(true);
            lost = true;
            isRunning = false;
        }

        static void Win()
        {
            Console.WriteLine("\n\nGewonnen!");
            lost = true;
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

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < w; y++)
                {
                    if(x!=0 && y!=0)
                    {
                        if (minefield[x - 1, y - 1].mine) minefield[x, y].count++;
                    }
                    if(x!=0)
                    {
                        if (minefield[x - 1, y].mine) minefield[x, y].count++;
                    }

                    if(y!=0)
                    {
                        if (minefield[x, y - 1].mine) minefield[x, y].count++;
                    }

                   if(x!=w-1)
                    {

                        if (minefield[x + 1, y].mine) minefield[x, y].count++;
                    }
                    
                   if(y!=w-1)
                    {
                        if (minefield[x, y + 1].mine) minefield[x, y].count++;
                    }

                   if(x!=0 && y!=w-1)
                    {
                        if (minefield[x - 1, y + 1].mine) minefield[x, y].count++;
                    }

                   if (x!=w-1 && y!=0)
                    {
                        if (minefield[x + 1, y - 1].mine) minefield[x, y].count++;
                    }
                    
                   if(x!=w-1 && y!=w-1)
                    {
                        if (minefield[x + 1, y + 1].mine) minefield[x, y].count++;
                    }               
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
