using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core.Utility;

namespace Core.Systems
{
    /// <summary>
    /// For faster develop used this one with singleton instead of DI
    /// </summary>
    public class EntryPoint : Singleton<EntryPoint>
    {
        [SerializeField] private bool isFindOnAwake = false;
        [SerializeField] List<MonoBehaviour> components = new(32); 
        private Dictionary<Type, IInitializable> cachedComponents = new();

        protected override void Awake()
        {
            base.Awake();
            if(!isFindOnAwake) return;
            var foundedComponents = FindObjectsOfType<MonoBehaviour>().Where(x => x is IInitializable && !components.Contains(x));
            foreach (var component in foundedComponents)
            {
                if(!components.Contains(component))
                    components.Add(component);
            }
        }
        
        private IEnumerator Start()
        {
            foreach (var item in components)
            {
                if (item is IInitializable initializer)
                {
                    Debug.Log($"{item.name} + {initializer.IsInitialized}");
                    if (initializer.IsInitialized)
                        continue;
                    initializer.Initialize();
                    Debug.Log("Initialized " + item.name);
                    //if(!initializer.IsInitialized)
                    //    yield return new WaitUntil(() => initializer.IsInitialized);
                }
                else
                {
                    Debug.LogError($"{nameof(EntryPoint)}: Component {item.name} does not implement {nameof(IInitializable)}");
                    yield return null;
                }
            }

        }

        /// <summary>
        /// Retrieves the first component of type T that implements the IInitializable interface.
        /// </summary>
        /// <typeparam name="T">The type of component to retrieve.</typeparam>
        /// <returns>The first component of type T, or null if none is found.</returns>
        public static T Get<T>() where T : MonoBehaviour, IInitializable
        {
            if (Instance.cachedComponents.TryGetValue(typeof(T), out var component))
            {
                return (T)component;
            }
            else
            {
                var c = Instance.components.OfType<T>().FirstOrDefault();
                if (c != null) Instance.cachedComponents.Add(typeof(T), c);
                return c;
            }
        }

        private void OnValidate()
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                var component = components[i];
                if (component is not IInitializable)
                    components.Remove(component);
                foreach (var nextItem in component.gameObject.GetComponents(typeof(MonoBehaviour)))
                {
                    if (nextItem is IInitializable && !components.Contains(nextItem))
                    {
                        components.Add(nextItem as MonoBehaviour);
                    }
                }
            }
        }
    }
}