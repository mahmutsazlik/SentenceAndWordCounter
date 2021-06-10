using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SentenceAndWordCounter.Service
{
    public class ProducerConsumer
    {
        Channel<string> channel = Channel.CreateUnbounded<string>();
        private static ConcurrentDictionary<string, int> lstWords = new ConcurrentDictionary<string, int>();
        static int totalWord = 0;
        static int totalSentence = 0;
        static int avgWord = 0;
        public void StartChannel(string text,int threadSize=5)
        {
            //text = "Was certainty remaining engrossed applauded now sir how discovery. Settled opinion how enjoyed greater joy adapted too shy? Now properly surprise expenses interest nor replying she she. Bore sir tall nay many many time yet less.";
            string[] splitSentences = Regex.Split(text, @"(?<=['""A-Za-z0-9][\.\!\?])\s+(?=[A-Z])");
            totalSentence = splitSentences.Length;
            Task producer = Task.Factory.StartNew(() => {
                foreach (var sentence in splitSentences)
                {
                    channel.Writer.TryWrite(sentence);
                }
                channel.Writer.Complete();
            });
            Task[] consumer = new Task[threadSize];
            for (int i = 0; i < consumer.Length; i++)
            {
                consumer[i] = Task.Factory.StartNew(async () => {
                    while (await channel.Reader.WaitToReadAsync())
                    {
                        if (channel.Reader.TryRead(out
                                var sentence))
                        {
                            int wordSize = 0;
                            char[] arrSplitChars = { ' ' };
                            string[] arrWords = sentence.Split(arrSplitChars, StringSplitOptions.RemoveEmptyEntries);
                            wordSize += arrWords.Length;
                            foreach (var item in arrWords)
                            {
                                lstWords.AddOrUpdate(item, 1, (key, value) => value + 1);
                            }
                            Console.WriteLine($"Consumer No.{Task.CurrentId} Total word:{wordSize}");
                        }
                    }
                });
            }
            producer.Wait();
            Task.WaitAll(consumer);
            Console.WriteLine($"Sentence Count : {totalSentence}");
            Console.WriteLine($"Avg. Word Count : {avgWord}");
            Console.WriteLine($"Thread Counts : {threadSize}");

            
            foreach (var item in lstWords)
            {
                Console.WriteLine($"{item.Key} {item.Value}");
            }
        }
    }
}
