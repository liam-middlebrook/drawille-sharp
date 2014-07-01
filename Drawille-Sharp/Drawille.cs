using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawille_Sharp
{
    public class Canvas
    {
        private static int[,] PIXEL_MAP = new int[,]
        {
            {0x01, 0x08},
            {0x02,0x10},
            {0x04,0x20},
            {0x40,0x80}
        };
        private Dictionary<Tuple<uint, uint>, int> chars;
        public uint width;
        public uint height;

        public Canvas(uint width, uint height)
        {
            chars = new Dictionary<Tuple<uint, uint>, int>();
            width = width / 2;
            height = height / 4;
        }

        public void Clear()
        {
            chars.Clear();
        }

        public void Set(uint x, uint y)
        {
            Tuple<uint, uint> loc = new Tuple<uint, uint>(x / 2, y / 4);
            if (!chars.ContainsKey(loc))
            {
                chars[loc] = 0;
            }
            chars[loc] |= PIXEL_MAP[y % 4, x % 2];
        }
        public void UnSet(uint x, uint y)
        {
            Tuple<uint, uint> loc = new Tuple<uint, uint>(x / 2, y / 4);
            if (!chars.ContainsKey(loc))
            {
                chars[loc] = 0;
            }
            chars[loc] &= PIXEL_MAP[y % 4, x % 2];
        }
        public void Toggle(uint x, uint y)
        {
            Tuple<uint, uint> loc = new Tuple<uint, uint>(x / 2, y / 4);
            if (!chars.ContainsKey(loc))
            {
                chars[loc] = 0;
            }
            chars[loc] ^= PIXEL_MAP[y % 4, x % 2];
        }

        public bool Get(uint x, uint y)
        {
            int dot_index = PIXEL_MAP[y % 4, x % 2];
            Tuple<uint, uint> loc = new Tuple<uint, uint>(x / 2, y / 4);
            if (!chars.ContainsKey(loc))
            {
                return false;
            }
            int _char = chars[loc];
            return (_char & dot_index) != 0;
        }

        public List<string> Rows()
        {
            uint maxrow = width;
            uint maxcol = height;

            List<string> result = new List<string>();

            for (int y = 0; y < maxcol + 1; y++)
            {
                string row = "";
                for (int x = 0; x < maxrow + 1; x++)
                {
                    Tuple<uint, uint> loc = new Tuple<uint, uint>((uint)(x / 2), (uint)(y / 4));
                    if (chars.ContainsKey(loc))
                    {
                        row = row + chars[loc];
                    }
                    else
                    {
                        row = row + " ";
                    }
                }
                result.Add(row);
            }
            return result;
        }

        public string Frame()
        {
            List<string> rows = Rows();
            string result = "";
            foreach (string row in rows)
            {
                result += row.Trim('\n');
            }
            return result;
        }

        public List<Tuple<uint, uint>> Line_Vec(uint x1, uint y1, uint x2, uint y2)
        {
            int xDiff = (int)(Math.Max(x1, x2) - Math.Min(x1, x2));
            int yDiff = (int)(Math.Max(y1, y2) - Math.Min(y1, y2));

            int xDir = (x1 <= x2) ? 1 : -1;
            int yDir = (y1 <= y2) ? 1 : -1;

            int r = Math.Max(xDiff, yDiff);

            List<Tuple<uint, uint>> result = new List<Tuple<uint, uint>>();

            for (int i = 0; i < r+1; i++)
            {
                int x = (int)x1;
                int y = (int)y1;

                if (yDiff != 0)
                {
                    y += (i * yDiff) / r * yDir;
                }
                if (xDiff != 0)
                {
                    x += (i * xDiff) / r * xDir;
                }
                result.Add(new Tuple<uint, uint>((uint)x, (uint)y));
            }
            return result;
        }

        public void Line(uint x1, uint y1, uint x2, uint y2)
        {
            List<Tuple<uint, uint>> results = Line_Vec(x1, y1, x2, y2);
            foreach (Tuple<uint, uint> res in results)
            {
                Set(res.Item1, res.Item2);
            }
        }
    }

    public class Turtle
    {
        public float x, y;
        public bool brush;
        public float rotation;
        Canvas cvs;

        public Turtle(float x, float y)
        {
            cvs = new Canvas(0, 0);
            this.x = x;
            this.y = y;
            brush = true;
            rotation = 0.0f;
        }

        public Turtle Width(uint width)
        {
            cvs.width = width;
            return this;
        }

        public Turtle Height(uint height)
        {
            cvs.height = height;
            return this;
        }

        public void Up()
        {
            brush = false;
        }

        public void Down()
        {
            brush = true;
        }

        public void Toggle()
        {
            brush = !brush;
        }

        public void Forward(float distance)
        {
            float _x = x + (float)Math.Cos(rotation * Math.PI / 180.0f) * distance;
            float _y = y + (float)Math.Sin(rotation * Math.PI / 180.0f) * distance;
            Move(x, y);
        }
        public void Back(float distance)
        {
            Forward(-distance);
        }

        public void Move(float x, float y)
        {
            if (brush)
            {
                cvs.Line((uint)Math.Max(0, (int)Math.Round(this.x)),
                    (uint)Math.Max(0, (int)Math.Round(this.y)),
                    (uint)Math.Max(0, (int)Math.Round(x)),
                    (uint)Math.Max(0, (int)Math.Round(x)));
            }
            this.x = x;
            this.y = y;
        }

        public void Right(float angle)
        {
            rotation += angle;
        }

        public void Left(float angle)
        {
            rotation -= angle;
        }

        public string Frame()
        {
            return cvs.Frame();
        }
    }

}
