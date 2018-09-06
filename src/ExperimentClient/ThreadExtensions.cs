using System.Collections.Generic;
using System.Threading;

namespace ExperimentClient
{
    public static class ThreadExtension
    {
        public static void StartAll (this IEnumerable<Thread> threads)
        {
            if (threads != null)
            {
                foreach (var thread in threads) { thread.Start (); };
            }
        }

        public static void WaitAll (this IEnumerable<Thread> threads)
        {
            if (threads != null)
            {
                foreach (Thread thread in threads) { thread.Join (); }
            }
        }

        public static void AbortAll (this IEnumerable<Thread> threads)
        {
            if (threads != null)
            {
                foreach (Thread thread in threads) { thread.Abort (); }
            }
        }
    }
}