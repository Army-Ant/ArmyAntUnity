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

        public int Listen(T_EventID eventId, System.Action<IEventManager<T_EventID>.IEventArgs> callback) {
            if (callback == null) {
                return -1;
            }
            var ld = new ListenerData {
                eventId = eventId,
                callback = callback
            };
            if (!listenerMap.ContainsKey(eventId)) {
                listenerMap.Add(eventId, new Dictionary<int, ListenerData>());
            }
            var list = listenerMap[eventId];
            int listenerId = 1;
            while (list.ContainsKey(listenerId)) {
                ++listenerId;
            }
            list.Add(listenerId, ld);
            return listenerId;
        }

        public bool Unlisten(T_EventID eventId, int listenerId) {
            if (listenerMap.ContainsKey(eventId)) {
                var list = listenerMap[eventId];
                if (list.ContainsKey(listenerId)) {
                    list.Remove(listenerId);
                    return true;
                }
            }
            return false;
        }

        public bool NotifyAsync(IEventManager<T_EventID>.IEventArgs msg) {
            eventMap.Enqueue(msg);
            return true;
        }

        public void NotifySync(IEventManager<T_EventID>.IEventArgs msg) {
            if(listenerMap.ContainsKey(msg.EventId)) {
                var list = listenerMap[msg.EventId];
                foreach(var listener in list) {
                    listener.Value.callback(msg);
                }
            }
        }

        private readonly Queue<IEventManager<T_EventID>.IEventArgs> eventMap = new Queue<IEventManager<T_EventID>.IEventArgs>();
        private readonly Dictionary<T_EventID, Dictionary<int, ListenerData>> listenerMap = new Dictionary<T_EventID, Dictionary<int, ListenerData>>();

        private struct ListenerData {
            public T_EventID eventId;
            public System.Action<IEventManager<T_EventID>.IEventArgs> callback;
        }
    }
}
