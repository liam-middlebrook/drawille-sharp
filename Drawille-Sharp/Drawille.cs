using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Drawille_Sharp
{
    public class Canvas
    {
        private static int[,] PIXEL_MAP = new int[,]
        {
            {0x1, 0x8},
            {0x2, 0x10},
            {0x4, 0x20},
            {0x40,0x80}
        };
        private Dictionary<int, int> chars;
        public int width;
        public int height;

        public Canvas(int width, int height)
        {
            if (width % 2 != 0)
            {
                throw new ArgumentException("Width must be multiple of 2");
            }
            if (height % 4 != 0)
            {
                throw new ArgumentException("Height must be multiple of 4");
            }
            chars = new Dictionary<int, int>();
            this.width = width;
            this.height = height;
            Clear();
        }

        public void Clear()
        {
            chars.Clear();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int nx = (int)(x / 2);
                    int ny = (int)(y / 4);
                    int coord = nx + width / 2 * ny;
                    if (!chars.ContainsKey(coord))
                    {
                        chars.Add(coord, 0);
                    }
                }
            }
        }

        public void Set(int x, int y)
        {
            Tuple<int,int> data = GetMethodData(x,y);
            _Set(data.Item1, data.Item2);
        }

        private void _Set(int coord, int mask)
        {
            chars[coord] |= mask;
        }
        
        public void UnSet(int x, int y)
        {
            Tuple<int,int> data = GetMethodData(x,y);
            _UnSet(data.Item1, data.Item2);
        }
        private void _UnSet(int coord, int mask)
        {
            chars[coord] &= ~mask;
        }

        public void Toggle(int x, int y)
        {
            Tuple<int,int> data = GetMethodData(x,y);
            _Toggle(data.Item1, data.Item2);
        }
        private void _Toggle(int coord, int mask)
        {
            chars[coord] ^= mask;
        }

        private Tuple<int, int> GetMethodData(int x, int y)
        {
            int nx = (int)(x / 2);
            int ny = (int)(y / 4);
            int coord = nx + this.width / 2 * ny;
            int mask = PIXEL_MAP[y % 4, x % 2];
            return new Tuple<int,int>(coord,mask);
        }

        public string Frame()
        {
            string result = string.Empty;
            for (int i = 0, j = 0; i < chars.Count; i++, j++)
            {
                if (j == this.width / 2)
                {
                    result += '\n';
                    j = 0;
                }
                if (this.chars[i] == 0)
                {
                    result += ' ';
                }
                else
                {
                    string newChar = "\\u" +(2800 + this.chars[i]);
                    result += UnescapeCodes(newChar);
                }
            }
            result += '\n';
            return result;
        }
        public static string UnescapeCodes(string src)
        {
            Regex rx = new Regex(@"\\[uU]([0-9A-Fa-f]{4})");
            string result = src;
            result = rx.Replace(result, match => ((char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());
            return result;
        }
    }
}
