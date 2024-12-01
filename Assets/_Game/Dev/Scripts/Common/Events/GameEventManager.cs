using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GolfMaster.Events
{
    public class GameEvent
    {
        public int Priority { get; set; }
    }

    public delegate void EventListener<in EventArgT>(EventArgT e)
        where EventArgT : GameEvent;

    public static class GameEventManager
    {
        private struct EventItem
        {
            public Delegate Action;
            public int Priority;

            public EventItem(Delegate action, int priority)
            {
                this.Action = action;
                this.Priority = priority;
            }
        }

        private static Dictionary<Type, List<EventItem>> gameEventListeners;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            gameEventListeners = new Dictionary<Type, List<EventItem>>();
        }

        public static void On<EventArgT>(EventListener<EventArgT> listener, int priority = 0)
            where EventArgT : GameEvent
        {
            var type = typeof(EventArgT);
            if (!gameEventListeners.ContainsKey(type))
                gameEventListeners.Add(type, new List<EventItem>());

            gameEventListeners[type].Add(new EventItem(listener, priority));
            gameEventListeners[type] = gameEventListeners[type].OrderBy(x => x.Priority).ToList();
        }

        public static void Off<EventArgT>(EventListener<EventArgT> listener)
             where EventArgT : GameEvent
        {
            var type = typeof(EventArgT);
            if (listener != null && gameEventListeners.ContainsKey(type))
                gameEventListeners[type].RemoveAll(x => x.Action.Equals(listener));
        }

        public static void Fire<EventT>(EventT gameEvent = null)
            where EventT : GameEvent
        {
            var type = typeof(EventT);

            if (gameEventListeners.TryGetValue(type, out List<EventItem> listenerActions))
            {
                for (int i = listenerActions.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        if (gameEvent == null)
                            gameEvent = (EventT)Activator.CreateInstance(typeof(EventT));

                        gameEvent.Priority = listenerActions[i].Priority;
                        (listenerActions[i].Action as EventListener<EventT>)?.Invoke(gameEvent);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                }
            }
        }
    }
}
