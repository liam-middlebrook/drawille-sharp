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

            Canvas c = new Canvas(16, 16);
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    if (x % 2 == 0 && y % 2 == 0) c.Set(x, y);
                }
            }
            Console.Write(c.Frame());
            Console.ReadLine();
        }
    }
}
