using System.Collections.Generic;

namespace ArmyAnt.ViewUtil.Components {

    public class EventManager<T_EventID> : UnityEngine.MonoBehaviour {
        public interface IEventArgs {
            T_EventID EventId { get; }
        }

        protected virtual void Update() {
            if (eventMap.Count > 0) {
                var msg = eventMap.Dequeue();
                if (listenerMap.ContainsKey(msg.EventId)) {
                    var list = listenerMap[msg.EventId];
                    foreach (var listener in list) {
                        listener.Value.callback(msg);
                    }
                }
            }
        }

        public int Listen(T_EventID eventId, System.Action<IEventArgs> callback) {
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

        public bool Notify(IEventArgs args) {
            eventMap.Enqueue(args);
            return true;
        }

        private readonly Queue<IEventArgs> eventMap = new Queue<IEventArgs>();
        private readonly Dictionary<T_EventID, Dictionary<int, ListenerData>> listenerMap = new Dictionary<T_EventID, Dictionary<int, ListenerData>>();

        private struct ListenerData {
            public T_EventID eventId;
            public System.Action<IEventArgs> callback;
        }
    }
}
