using System;
using System.Threading;
using System.Threading.Tasks;

namespace TasksSecondLesson
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(170, 42);
            for (int i = 0; i < 160; i++)
            {
                Matrix instance = new Matrix(i, true);
                new Thread(instance.Move).Start();
            }
        }
        public static void ThreadGetSumm()
        {
            Thread Thread1 = new Thread(new ThreadStart(GetSumm));
            Thread1.Start();
        }
        static async void SummAsync()
        {
            await Task.Run(() => Summ(1, 5));
            await Task.Run(() => Summ(5, 10));
            await Task.Run(() => Summ(2, 5));
            await Task.Run(() => Summ(10, 18));
        }
        public static void ParallelSumm()
        {
            Parallel.Invoke(GetSumm, () => GetSumm(), () => GetSumm());
        }
        public static void GetSumm()
        {
            int result = 0;
            for (int i = 1; i <= 10; i++)
            {
                result += i;
                System.Console.WriteLine(result);
                Thread.Sleep(1000);
            }
            Console.WriteLine($"Сумма от 1 до 10 ровен {result}");
        }
        public static void Summ(int begin, int end)
        {
            int result = 0;
            for (int i = begin; i <= end; i++)
            {
                result += i;
            }
            Console.WriteLine($"Сумма от {begin} до {end} ровен {result}");
        }
    }
}
class Matrix
{
    Random rand;
    static object locker = new object();
    const string litters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public int Colunm { get; set; }

    public bool NeedSecond { get; set; }

    public Matrix(int col, bool needSecond)
    {
        Colunm = col;
        rand = new Random((int)DateTime.Now.Ticks);
        NeedSecond = needSecond;
    }

    char GetChar()
    {
        return litters.ToCharArray()[rand.Next(0, 35)];
    }
    public void Move()
    {
        int lenght;
        int count;

        while (true)
        {
            count = rand.Next(3, 10);
            lenght = 0;
            Thread.Sleep(rand.Next(20, 5000));
            for (int i = 0; i < 42; i++)
            {
                lock (locker)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.CursorTop = i - lenght;
                    for (int j = i - lenght - 1; j < i; j++)
                    {
                        Console.CursorLeft = Colunm;
                        Console.WriteLine(" ");
                    }
                    if (lenght < count)
                        lenght++;
                    else if (lenght == count)
                        count = 0;
                    if (NeedSecond && i < 20 && i > lenght + 2 && (rand.Next(1, 5) == 3))
                    {
                        new Thread((new Matrix(Colunm, false)).Move).Start();
                        NeedSecond = false;
                    }

                    if (41 - i < lenght)
                        lenght--;
                    Console.CursorTop = i - lenght + 1;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    for (int j = 0; j < lenght - 2; j++)
                    {
                        Console.CursorLeft = Colunm;
                        Console.WriteLine(GetChar());
                    }
                    if (lenght >= 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.CursorLeft = Colunm;
                        Console.WriteLine(GetChar());
                    }
                    if (lenght >= 1)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.CursorLeft = Colunm;
                        Console.WriteLine(GetChar());
                    }

                    Thread.Sleep(5);
                }
            }
        }
    }
}