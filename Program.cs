using System;
using System.IO;
using System.Collections.Generic;


namespace compressions
{
    class Program
    {
        static Dictionary<byte, byte> myDictionary = new Dictionary<byte, byte>();
        static byte seperater = 16;
        static void Main(string[] args)
        {
            var list = new List<int>();
            char header = ' ';
            string fileName = @"C:\Users\scjoy\source\repos\Tom and Jerry\bin\Tom_Jerry.pgm";
            {
                byte[] line;
                line = File.ReadAllBytes(fileName);
                for (int i = 0; i < line.Length; i++)
                {
                    header = (char)line[i];
                    if (line[i] > 127) { break; }
                    Console.Write(header);
                }
                createDictionary(line);
                comppress(line);
                byte[] newByte = File.ReadAllBytes(@"C:\Users\scjoy\source\repos\Tom and Jerry\bin\pgm-compressed.pgm");
                deCompress(newByte);
            }

            Console.ReadKey();
        }
        static void createDictionary(byte[] arr) 
        {
            int streaks = 1;
            byte grayscaleVal = arr[16];
            int[] saveStreak = new int [256];
            for(int i = 17; i < arr.Length; i++)
            {
                if(arr[i] == grayscaleVal)
                {
                    streaks++;
                    if(streaks == 3)
                    {
                        saveStreak[grayscaleVal]++;
                        streaks = 0;
                    }
                }
                else
                {
                    streaks = 1;
                    grayscaleVal = arr[i];
                }
                
            } 
            for(int j = 0; j < 256; j++)
            {
                int highIndex = 0;
                int high = -1;
                for(int h = 0; h < 256; h++)
                {
                    if(saveStreak[h] > high)
                    {
                        high = saveStreak[h];
                        highIndex = h;
                    }
                }
                myDictionary.Add((byte)highIndex, (byte)j);
                saveStreak[highIndex] = -2;
            }
        }
        static int comppress(byte[] arr)
        {
            int maxVal = 255;
            byte count = 1;
            byte check = 1;
            byte header;
            int rv = 0;
            using (BinaryWriter writer = new BinaryWriter(File.Open(@"C:\Users\scjoy\source\repos\Tom and Jerry\bin\pgm-compressed.pgm", FileMode.Create)))
            {
                for (int i = 0; i < 16; i++)
                {
                    header = arr[i];
                    writer.Write(header);
                }
                for (int i = 16; i < arr.Length; i++)
                {
                    check = arr[i];
                    if (myDictionary[check] < seperater)
                    {
                        i++;
                        while ( i < arr.Length && check == arr[i])
                        {
                            count++;                    
                            if (count == maxVal)
                            {
                                writer.Write(check);
                                writer.Write(count);
                                count = 0;
                            }
                            i++;
                        }
                        writer.Write(check);
                        writer.Write(count);
                        count = 1;
                        if(i < arr.Length)
                        {
                            check = arr[i];
                        }
                    }
                    else
                    {
                        writer.Write(check);
                    }
                }
                writer.Write(check);
                writer.Write(count);
            }
            return rv;
        }
        static int deCompress(byte[] arr)
        {
            byte header;
            byte count;
            int rv = 0;
            using (BinaryWriter writer = new BinaryWriter(File.Open(@"C:\Users\scjoy\source\repos\Tom and Jerry\bin\pgm-decompressed.pgm", FileMode.Create)))
            {
                for(int i = 0; i < 16; i++)
                {
                    header = arr[i];        
                    writer.Write(header);
                }
                for (int i = 16; i < arr.Length; i+=2)
                {
                    header = arr[i];
                    count = arr[i+1];
                    for (int j = 0; j < count;j++)
                    {
                        writer.Write(header);
                    }
                }
            }
            return rv;
        }
    }
}
