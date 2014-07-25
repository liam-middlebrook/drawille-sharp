using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawille_Sharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            Canvas c = new Canvas(Console.BufferWidth, Console.BufferHeight);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    c.Set(i, j);
                }
            }
            Console.Write(c.Frame());
            Console.ReadLine();
        }
    }
}
