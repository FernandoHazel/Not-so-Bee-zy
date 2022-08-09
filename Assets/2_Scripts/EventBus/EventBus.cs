using UnityEngine.Events;
using System.Collections.Generic;

namespace EventBus
{
    public class GameEventBus
    {
        private static readonly 
            IDictionary<GameEventType, UnityEvent> 
            Events = new Dictionary<GameEventType, UnityEvent>();

        public static void Subscribe
            (GameEventType eventType, UnityAction listener) {
            
            UnityEvent thisEvent;

            if (Events.TryGetValue(eventType, out thisEvent)) {
                thisEvent.AddListener(listener);
            }
            else {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Events.Add(eventType, thisEvent);
            }
        }

        public static void Unsubscribe
            (GameEventType type, UnityAction listener) {

            UnityEvent thisEvent;

            if (Events.TryGetValue(type, out thisEvent)) {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void Publish(GameEventType type) {

            UnityEvent thisEvent;

            if (Events.TryGetValue(type, out thisEvent)) {
                thisEvent.Invoke();
            }
        }
    }
}
