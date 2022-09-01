using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Helpers
{
    public class EventManager : MonoBehaviour
    {
        private static Dictionary<Type, List<Delegate>> eventDictionary = new();
        private static string eventSceneName;

        void Awake()
        {
            eventSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            eventDictionary = new Dictionary<Type, List<Delegate>>();
        }

        private static void CheckSceneMatches()
        {
            var currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (currentSceneName != eventSceneName)
            {
                throw new Exception("EventManager has not been initialized for the current scene");
            }
        }

        internal static void Listen<T>()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Register event listener
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="listener">Listener</param>
        public static void Listen<T>(EventCallback<T> listener)
        {
            CheckSceneMatches();
            if (eventDictionary.TryGetValue(typeof(T), out var listeners))
            {
                if (!listeners.Contains(listener))
                {
                    listeners.Add(listener);
                }
            }
            else
            {
                listeners = new List<Delegate> { listener };
                eventDictionary.Add(typeof(T), listeners);
            }
        }

        /// <summary>
        /// Get all listeners for event
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <returns>Event listeners</returns>
        public static List<Delegate> GetListenersOfEvent<T>()
        {
            CheckSceneMatches();
            return eventDictionary.TryGetValue(typeof(T), out var listeners) ? listeners : null;
        }

        /// <summary>
        /// Remove event listeners
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="listener">Event listener</param>
        public static void RemoveListener<T>(EventCallback<T> listener)
        {
            CheckSceneMatches();
            if (eventDictionary.TryGetValue(typeof(T), out var listeners))
            {
                listeners.Remove(listener);
            }
        }

        /// <summary>
        /// Remove event type
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        public static void RemoveEvent<T>()
        {
            CheckSceneMatches();
            eventDictionary.Remove(typeof(T));
        }

        /// <summary>
        /// Clear all event types
        /// </summary>
        public static void RemoveAllEvents()
        {
            CheckSceneMatches();
            eventDictionary = new Dictionary<Type, List<Delegate>>();
        }

        /// <summary>
        /// Trigger event
        /// </summary>
        /// <typeparam name="T">Event type</typeparam>
        /// <param name="data">Payload</param>
        public static void Trigger<T>(T data)
        {
            CheckSceneMatches();
            if (!eventDictionary.TryGetValue(typeof(T), out var listeners)) return;
            foreach (var listener in listeners.ToArray())
            {
                listener.DynamicInvoke(data);
            }
        }
    }

    /// <summary>
    /// Event callback
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    public delegate void EventCallback<in T>(T data);
}