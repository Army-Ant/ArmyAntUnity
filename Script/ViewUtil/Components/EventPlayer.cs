using System;
using System.Collections.Generic;

using UnityEngine;

namespace ArmyAnt.ViewUtil.Components
{
    public class EventPlayer<T_Event, T_Key> : MonoBehaviour
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
                        listener.Value.callback(msg._event, msg.key, msg.classData);
                    }
                }
            }
        }

        public int Listen(T_Event _event, Action<T_Event, T_Key, object[]> callback)
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

        public bool Notify(T_Event _event, T_Key key, params object[] param)
        {
            var msg = new EventData
            {
                _event = _event,
                key = key,
                classData = new object[param.Length],
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
            public T_Key key;
            public object[] classData;
        }

        private struct ListenerData
        {
            public T_Event _event;
            public Action<T_Event, T_Key, object[]> callback;
        }
    }
}
