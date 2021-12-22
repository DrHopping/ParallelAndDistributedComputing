using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using Lab1.Atomics;

namespace Lab1
{
    public class Mutex
    {
        private List<Thread> waitingThreads;
        private AtomicReference<Thread> lockThread;

        public Mutex()
        {
            waitingThreads = new List<Thread>();
            lockThread = new AtomicReference<Thread>(null);
        }

        public void Lock()
        {
            while (!lockThread.CompareAndSet(Thread.CurrentThread, null))
            {
                Thread.Yield();
            }
        }

        public void Release()
        {
            lockThread.Set(null);
        }

        public bool IsLocked()
        {
            return lockThread.Value != null;
        }

        public void Wait()
        {
            waitingThreads.Add(Thread.CurrentThread);
            Release();

            while (waitingThreads.Contains(Thread.CurrentThread))
            {
                Thread.Yield();
            }
            
            Lock();
        }

        public void Notify()
        {
            if (waitingThreads.Count > 0)
            {
                waitingThreads.RemoveAt(RandomNumberGenerator.GetInt32(0, waitingThreads.Count));
            }
        }

        public void NotifyAll()
        {
            waitingThreads.Clear();
        }

    }
}