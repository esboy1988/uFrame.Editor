using System;
using System.Collections.Generic;

namespace Invert.Core
{
    public class EventManager<T> : IEventManager where T : class
    {
        private List<T> _listeners;

        public List<T> Listeners
        {
            get { return _listeners ?? (_listeners = new List<T>()); }
            set { _listeners = value; }
        }

        public void Signal(Action<object> obj)
        {
            foreach (var item in Listeners)
            {
                obj(item);
            }
        }
        public void Signal(Action<T> action)
        {
            foreach (var item in Listeners)
            {
                action(item);
            }
        }

        public Action Subscribe(T listener)
        {
            if (!Listeners.Contains(listener))
                Listeners.Add(listener);

            return () => { Unsubscribe(listener); };
        }

        private void Unsubscribe(T listener)
        {
            Listeners.Remove(listener);
        }

        public Action AddListener(object listener)
        {
            return Subscribe(listener as T);
        }
    }
}