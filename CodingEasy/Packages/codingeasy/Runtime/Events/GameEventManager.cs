using System;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class GameEventManager : Singleton<GameEventManager>
    {
        private Dictionary<Type, IEvent> m_events;

        public override void Dispose()
        {
            m_events.Clear();
        }

        public override void Init()
        {
            m_events = new Dictionary<Type, IEvent>();

            var types = this.GetType().Assembly.GetTypes();

            var eventType = typeof(EventAttribute);
            foreach (var type in types)
            {
                if(type.IsAbstract)
                    continue;
                var objects = type.GetCustomAttributes(eventType,true);
                if(objects.Length == 0)
                    continue;
                
                IEvent obj = Activator.CreateInstance(type) as IEvent;
                var dataType = obj.GetEventType();
                m_events.Add(dataType,obj);
                Debug.Log($"Add eventType={type} , dataType = {dataType}");
            }
        }

        public void Register<T>(Action<T> callback) where T:struct 
        {
            var type = typeof(T);
            if (!m_events.TryGetValue(type, out var e)){
            
                m_events.Add(type, e);
            }
            var handler = e as EventHandler<T>;
            handler.AddListiener(callback);
        }
        public void UnRegister<T>(Action<T> callback) where T:struct 
        {
            var type = typeof(T);
            if (!m_events.TryGetValue(type, out var e)){
                Debug.LogError($"no event has param with {type}");
                return;
            }
            var handler = e as EventHandler<T>;
            handler.RemoveListiener(callback);
        }

        public void PublicMessage<T>(T data) where T :struct
        {
            if (m_events.TryGetValue(typeof(T), out var e)){
                var handler = e as EventHandler<T>;
                handler.Public(data);
            }
        }
    }

}




