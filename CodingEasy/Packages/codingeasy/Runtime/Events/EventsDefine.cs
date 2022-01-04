using System;
using System.Collections.Generic;

namespace Events
{
    public interface IEvent
    {
        Type GetEventType();
    }


    [Event]
    public abstract class EventHandler<T>:IEvent where  T:struct
    {
        protected List<Action<T>> callbacks = new List<Action<T>>();
        public int Count => callbacks.Count;

        public Type GetEventType()
        {
            return typeof (T);
        }

        public void AddListiener(Action<T> callBack)
        {
            callbacks.Add(callBack);
        }
        public void RemoveListiener(Action<T> callBack)
        {
            callbacks.Remove(callBack);
        }

        public void Public(T data)
        {
            if (callbacks.Count > 0)
            {
                for (int i = callbacks.Count - 1; i >= 0; i--)
                {
                    callbacks[i].Invoke(data);
                }
            }
        }

    }
    
}