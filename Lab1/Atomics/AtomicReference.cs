using System;
using System.IO;
using System.Threading;

namespace Lab1.Atomics
{
    public class AtomicReference<T> where T : class {
        private T _value;

        public AtomicReference(T value){
            _value = value;
        }

        public T Value{
            get{
                object obj = _value;
                return (T) Thread.VolatileRead(ref obj);
            }
        }

        public bool CompareAndSet(T newValue, T oldValue){
            return ReferenceEquals(Interlocked.CompareExchange(ref _value, newValue, oldValue), oldValue);
        }
        
        public void Set(T newValue){
            Interlocked.Exchange(ref _value, newValue);
        }
    }
}