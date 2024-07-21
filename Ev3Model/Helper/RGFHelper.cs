using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace EV3ModelLib.Helper
{
    /// <summary>
    /// RGF - Raw Graphics File
    /// </summary>
    public static class RGFHelper
    {
        /// <summary>
        /// Bitmap container
        /// </summary>
        public class RGFBitmap
        {
            public int Width { get; internal set; }
            public int Height { get; internal set; }
            internal bool[,] bitmap;

            public RGFBitmap(string filename)
            {
                using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    ReadFromStream(stream);
                }
            }

            public RGFBitmap(Stream stream, long size = 0)
            {
                ReadFromStream(stream, size);
            }

            public RGFBitmap(byte[] data)
            {
                ReadFromData(data);
            }

            private void ReadFromStream(Stream stream, long size = 0)
            {
                if (size == 0) size = stream.Length;
                byte[] data = new byte[size];
                stream.Read(data, 0, data.Length);
                ReadFromData(data);
            }

            private void ReadFromData(byte[] data_in)
            {
                this.Width = data_in[0];
                this.Height = data_in[1];

                if (this.Width > 178 || this.Height > 128) return;

                // total size is 2 + ceil(width * height / 8)
                //byte[] data = new byte[data_in.Length - 2];
                //stream.Read(data, 0, (int)(stream.Length - 2));
                this.bitmap = new bool[this.Width, this.Height];

                int linebitwidth = this.Width + ((8 - (this.Width % 8)) % 8);

                for (int position = 0; position < linebitwidth * this.Height; position++)
                {
                    var y = position / linebitwidth;
                    var x = position - linebitwidth * y;
                    if (x < this.Width)
                    {
                        int bytepos = position / 8;
                        int bitpos = position % 8;
                        bool isfilled = (data_in[bytepos + 2] & (1 << bitpos)) != 0;
                        this.bitmap[x, y] = isfilled;
                    }
                }
            }

            public string ConvertToString(int scale, int maxwidth, int maxheight)
            {
                string retstr;
                int SCALING = scale > 0 ? scale :
                    (int)Math.Max(Math.Ceiling((double)Width / maxwidth), Math.Ceiling((double)Height / maxheight));
                //int SCALING = scale > 0 ? scale :
                //    (int)Math.Max(Math.Ceiling((double)Width / Console.WindowWidth), Math.Ceiling((double)Height / Console.WindowHeight));
                int SCALINGMAXVALUE = SCALING * SCALING;
                byte[] consoleScaledBitmapLine = new byte[(int)Math.Ceiling((1.0 * Width / SCALING))];

                StringBuilder sb = new StringBuilder();
                for (var y = 0; y < Height; y++)
                {
                    if (y % SCALING == 0) sb.AppendLine();
                    if (y % SCALING == 0) for (int i = 0; i < consoleScaledBitmapLine.Length; i++) consoleScaledBitmapLine[i] = 0;

                    for (var x = 0; x < Width; x++)
                    {
                        bool isfilled = bitmap[x, y];
                        consoleScaledBitmapLine[x / SCALING] += (byte)(isfilled ? 1 : 0);
                        if ((x % SCALING == SCALING - 1) && (y % SCALING == SCALING - 1))
                        {
                            char c = consoleScaledBitmapLine[x / SCALING] == SCALINGMAXVALUE ? 'O' :
                                consoleScaledBitmapLine[x / SCALING] > SCALINGMAXVALUE * 0.5 ? 'o' :
                                consoleScaledBitmapLine[x / SCALING] > SCALINGMAXVALUE * 0.25 ? '.' :
                                ' ';
                            sb.Append(c);
                        }
                    }
                }
                retstr = sb.ToString();
                return retstr;
            }

            public void PrintToConsole(int? scale)
            {
                const int CONSOLE_XY_FACTOR = 1; // later -- console text char is higher than wider
                int SCALINGX = scale.HasValue ? scale.Value :
                    (int)Math.Max(Math.Ceiling((double)Width / Console.WindowWidth), Math.Ceiling((double)Height / Console.WindowHeight / CONSOLE_XY_FACTOR));
                int SCALINGY = scale.HasValue ? scale.Value :
                    (int)(Math.Max(Math.Ceiling((double)Width / Console.WindowWidth), Math.Ceiling((double)Height / Console.WindowHeight / CONSOLE_XY_FACTOR)) * CONSOLE_XY_FACTOR);
                int SCALINGMAXVALUE = SCALINGX * SCALINGY;
                byte[] consoleScaledBitmapLine = new byte[(int)Math.Ceiling((1.0 * Width / SCALINGX))];

                ConsoleColor lastColor = ConsoleColor.Black;
                string bufferedText = null;
                void _writeBufferedColor(char c, ConsoleColor color, bool doForceFlushNewLine = false)
                {
                    if (color == lastColor && !doForceFlushNewLine) bufferedText += c;
                    else
                    {
                        Console.ForegroundColor = lastColor; Console.BackgroundColor = lastColor;
                        Console.Write(bufferedText);
                        lastColor = color; bufferedText = c.ToString();
                    }
                    if (doForceFlushNewLine) { Console.ForegroundColor = Console.BackgroundColor = ConsoleColor.Black; Console.WriteLine('|'); }
                }
                for (var y = 0; y < Height; y++)
                {
                    if (y % SCALINGY == 0) _writeBufferedColor(' ', ConsoleColor.Black, true);
                    if (y % SCALINGY == 0) for (int i = 0; i < consoleScaledBitmapLine.Length; i++) consoleScaledBitmapLine[i] = 0;

                    for (var x = 0; x < Width; x++)
                    {
                        bool isfilled = bitmap[x, y];
                        consoleScaledBitmapLine[x / SCALINGX] += (byte)(isfilled ? 1 : 0);
                        if ((x % SCALINGX == SCALINGX - 1) && (y % SCALINGY == SCALINGY - 1))
                        {
                            ConsoleColor backcolor =
                                consoleScaledBitmapLine[x / SCALINGX] == SCALINGMAXVALUE ? ConsoleColor.White :
                                consoleScaledBitmapLine[x / SCALINGX] > SCALINGMAXVALUE * 0.5 ? ConsoleColor.Gray :
                                consoleScaledBitmapLine[x / SCALINGX] > SCALINGMAXVALUE * 0.25 ? ConsoleColor.DarkGray :
                                ConsoleColor.Black;
                            char c = consoleScaledBitmapLine[x / SCALINGX] == SCALINGMAXVALUE ? 'O' :
                                consoleScaledBitmapLine[x / SCALINGX] > SCALINGMAXVALUE * 0.5 ? 'o' :
                                consoleScaledBitmapLine[x / SCALINGX] > SCALINGMAXVALUE * 0.25 ? '.' :
                                ' ';
                            _writeBufferedColor(c, backcolor);
                        }
                    }
                }
                _writeBufferedColor(' ', ConsoleColor.Black, true);
                Console.ResetColor();
                return;
            }

            //public void ReadFromBitmap(string bmpfilename)
            //{
            //    Bitmap bmp = new Bitmap(bmpfilename);
            //    if (bmp.Width > 178 || bmp.Height > 128) return;

            //    Width = bmp.Width;
            //    Height = bmp.Height;

            //    for (var y = 0; y < bmp.Height; y++)
            //    {
            //        byte valueTmp = 0;
            //        for (var x = 0; x < bmp.Width; x++)
            //        {
            //            var pix = bmp.GetPixel(x, y);
            //            bool isFilled = pix.GetBrightness() < 0.5 && pix.A > 127; // Color.Black;
            //            bitmap[x, y] = isFilled;
            //        }
            //    }
            //}
        }
    }
}