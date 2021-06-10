using SentenceAndWordCounter.Core;
using SentenceAndWordCounter.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SentenceAndWordCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please write the file path and thread size as parameter. Default thread size is 5.");
                return;
            }
            string path = args[0];
            string text = Tools.readFile(path);
            int threadSize = 5;
            if (args.Length > 1)
            {
                try
                {
                    threadSize = Convert.ToInt32(args[1]);
                }
                catch { }
            }
            var pc = new ProducerConsumer();
            pc.StartChannel(text,threadSize);
            Console.ReadKey();
        }
    }
}
