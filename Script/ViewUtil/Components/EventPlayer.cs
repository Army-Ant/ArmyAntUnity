using System;
using System.Collections.Generic;
using System.Xml.Linq;

using UnityEngine;

namespace ArmyAnt.ViewUtil.Components
{
    public class EventPlayer<T_Event, T_Data> : MonoBehaviour
    {
        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            if (eventMap.Count > 0)
            {
                var msg = eventMap.Dequeue();
                if (listenerMap.ContainsKey(msg._event))
                {
                    var list = listenerMap[msg._event];
                    foreach (var listener in list)
                    {
                        listener.Value.callback(msg._event, msg.classData);
                    }
                }
            }
        }

        public int Listen(T_Event _event, Action<T_Event, T_Data[]> callback)
        {
            if (callback == null)
            {
                return -1;
            }
            var ld = new ListenerData
            {
                _event = _event,
                callback = callback
            };
            if (!listenerMap.ContainsKey(_event))
            {
                listenerMap.Add(_event, new Dictionary<int, ListenerData>());
            }
            var list = listenerMap[_event];
            int id = 1;
            while (list.ContainsKey(id))
            {
                ++id;
            }
            list.Add(id, ld);
            return id;
        }

        public bool Unlisten(T_Event _event, int id)
        {
            if (listenerMap.ContainsKey(_event))
            {
                var list = listenerMap[_event];
                if (list.ContainsKey(id))
                {
                    list.Remove(id);
                    return true;
                }
            }
            return false;
        }

        public bool Notify(T_Event _event, params T_Data[] param)
        {
            var msg = new EventData
            {
                _event = _event,
                classData = new T_Data[param.Length],
            };
            for (int i = 0; i < param.Length; ++i)
            {
                msg.classData[i] = param[i];
            }
            eventMap.Enqueue(msg);
            return true;
        }

        private readonly Queue<EventData> eventMap = new Queue<EventData>();
        private readonly Dictionary<T_Event, Dictionary<int, ListenerData>> listenerMap = new Dictionary<T_Event, Dictionary<int, ListenerData>>();

        private struct EventData
        {
            public T_Event _event;
            public T_Data[] classData;
        }

        private struct ListenerData
        {
            public T_Event _event;
            public Action<T_Event, T_Data[]> callback;
        }
    }
}
