using System.Collections.Generic;
using ArmyAnt.ProcessController;

namespace ArmyAnt.ViewUtil.Components {

    public class EventManager<T_EventID> : UnityEngine.MonoBehaviour, IEventManager<T_EventID> {
        protected virtual void Update() {
            if (eventMap.Count > 0) {
                var msg = eventMap.Dequeue();
                NotifySync(msg);
            }
        }

        public int Listen(T_EventID eventId, System.Action<IEventManager<T_EventID>.IEventArgs> callback, int siblingIndex = -1) {
            if (callback == null) {
                return -1;
            }
            if(siblingIndex < 0) {
                siblingIndex = int.MaxValue;
            }
            var ld = new ListenerData {
                eventId = eventId,
                callback = callback,
                siblingIndex = siblingIndex,
            };
            if (!listenerMap.ContainsKey(eventId)) {
                listenerMap.Add(eventId, new Dictionary<int, ListenerData>());
            }
            var list = listenerMap[eventId];
            int listenerId = 0;
            while (list.ContainsKey(++listenerId)) {
            }
            list.Add(listenerId, ld);
            return listenerId;
        }

        public bool Unlisten(T_EventID eventId, int listenerId) {
            if(listenerMap.TryGetValue(eventId, out var list)) {
                if(list.ContainsKey(listenerId)) {
                    list.Remove(listenerId);
                    return true;
                }
            }
            return false;
        }

        public bool Unlisten(T_EventID eventId, System.Action<IEventManager<T_EventID>.IEventArgs> callback) {
            if(listenerMap.TryGetValue(eventId, out var list)) {
                var deleteKeys = new List<int>();
                foreach(var i in list) {
                    if(i.Value.callback == callback) {
                        deleteKeys.Add(i.Key);
                    }
                }
                foreach(var i in deleteKeys) {
                    list.Remove(i);
                }
                return deleteKeys.Count > 0;
            }
            return false;
        }

        public bool NotifyAsync(IEventManager<T_EventID>.IEventArgs msg) {
            eventMap.Enqueue(msg);
            return true;
        }

        public void NotifySync(IEventManager<T_EventID>.IEventArgs msg) {
            if(listenerMap.TryGetValue(msg.EventId, out var list)) {
                var dataList = new List<ListenerData>();
                foreach(var listener in list) {
                    dataList.Add(listener.Value);
                }
                dataList.Sort((a, b) => a.siblingIndex - b.siblingIndex);
                foreach(var listener in dataList) {
                    listener.callback(msg);
                }
            }
        }

        private readonly Queue<IEventManager<T_EventID>.IEventArgs> eventMap = new Queue<IEventManager<T_EventID>.IEventArgs>();
        private readonly Dictionary<T_EventID, Dictionary<int, ListenerData>> listenerMap = new Dictionary<T_EventID, Dictionary<int, ListenerData>>();

        private struct ListenerData {
            public T_EventID eventId;
            public System.Action<IEventManager<T_EventID>.IEventArgs> callback;
            public int siblingIndex;
        }
    }
}
