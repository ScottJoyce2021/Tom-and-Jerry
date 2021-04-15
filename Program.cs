using System;
using System.IO;
using System.Collections.Generic;


namespace compressions
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<int>();
            char header = ' ';
            string fileName = @"C:\Users\scjoy\source\repos\Tom and Jerry\bin\Dice.pgm";
            {
                byte[] line;
                line = File.ReadAllBytes(fileName);
                for (int i = 0; i < line.Length; i++)
                {
                    header = (char)line[i];
                    if (line[i] > 127) { break; }
                    Console.Write(header);
                }
                comppress(line);
                deCompress(line);
            }

            Console.ReadKey();
        }
        static int comppress(byte[] arr)
        {
            int maxVal = 255;
            int count = 1;
            char check = ' ';
            char header = ' ';
            int rv = 0;
            using (BinaryWriter writer = new BinaryWriter(File.Open(@"C:\Users\scjoy\source\repos\Tom and Jerry\bin\pgm-compressed.pgm", FileMode.Create)))
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    header = (char)arr[i];
                    if (arr[i] > 127) { break; }
                    writer.Write(header);
                }
                for (int i = 16; i < arr.Length; i++)
                {
                    if (check == ' ')
                    {
                        check = (char)arr[i];
                        count++;
                    }
                    else if (check == (char)arr[i] && count < maxVal)
                    {
                        count++;
                    }
                    else
                    {
                        writer.Write((byte)count);
                        writer.Write((byte)check);
                        count = 1;
                        check = (char)arr[i];
                    }
                }
            }
            return rv;
        }
        static int deCompress(byte[] arr)
        {
            char header = ' ';
            int rv = 0;
            using (BinaryWriter writer = new BinaryWriter(File.Open(@"C:\Users\scjoy\source\repos\Tom and Jerry\bin\pgm-decompressed.pgm", FileMode.Create)))
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    header = (char)arr[i];
                    if (arr[i] > 127) { break; }
                    writer.Write(header);
                }
            }
            return rv;
        }
    }
}
