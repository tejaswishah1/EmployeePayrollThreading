using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EmployeePayrollUsingThread
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string[] words = CreateWordArray(@"http://www.gutenberg.org/files/54700/54700-0.txt");
            Parallel.Invoke(() =>
            {
                Console.WriteLine("Begin first task");
                GetLongestWord(words);
            },
            () =>
            {
                Console.WriteLine("Begin second task");
                GetMostCommonWords(words);
            },
            () =>
            {
                Console.WriteLine("begin third task");
                GetMostCommonWords(words);
            }
            );
            Console.ReadKey();
        }
        public static void GetMostCommonWords(string[] words)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into q
                                 orderby q.Count() descending
                                 select q.Key;
            var commonWord = frequencyOrder.Take(10);
        }
        public static string[] CreateWordArray(string uri)
        {
            Console.WriteLine("Retrieving from {0}", uri);
            string blog = new WebClient().DownloadString(uri);
            return blog.Split(
                new char[] { ' ', '\u000A', '.', ',', '-', '_', '/' },
                StringSplitOptions.RemoveEmptyEntries);
        }
        private static string GetLongestWord(string[] words)
        {
            string longestWord = (from w in words
                                  orderby w.Length descending
                                  select w).First();
            Console.WriteLine("Task 1 - the longest word is {0}", longestWord);
            return longestWord;
        }
    }
}

