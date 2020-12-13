using System;
using System.IO;
using System.Text;
using RC4Lib;
using System.Diagnostics;

namespace RC4App
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args.Length)
            {
                case 3:
                    {
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();

                        var key = Encoding.ASCII.GetBytes(args[0]);

                        using (var srcFile = new FileStream(args[1], FileMode.Open))
                        {
                            using (var destFile = new FileStream(args[2], FileMode.Create))
                            {
                                Console.WriteLine("Encoding...");

                                var rc4 = new RC4Stream(key, srcFile);
                                rc4.CopyTo(destFile);

                                var elapsed = stopwatch.Elapsed;
                                var speed = (srcFile.Length * 1f / (1 << 20)) / (elapsed.TotalMilliseconds / 1000);

                                Console.WriteLine($"Completed in {elapsed.TotalSeconds:F1} seconds at {speed:F1} MB/s");
                            }
                        }
                    }
                    break;

                case 1:
                    {
                        var key = Encoding.ASCII.GetBytes(args[0]);

                        Console.WriteLine("Enter input text:");
                        var text = Console.ReadLine();
                        var input = Encoding.ASCII.GetBytes(text);

                        var rc4 = new RC4(key);
                        var output = rc4.TransformFinalBlock(input, 0, input.Length);

                        Console.WriteLine("Output bytes:");
                        foreach (byte b in output)
                        {
                            Console.Write($"{b:x2} ");
                        }
                        Console.WriteLine();
                    }
                    break;

                default:
                    Console.WriteLine($"Usage: RC4App key [source] [destination]");
                    break;

            }
        }
    }
}
