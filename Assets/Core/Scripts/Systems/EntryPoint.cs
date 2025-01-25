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
        [SerializeField] private bool _isFindOnAwake = false;
        [SerializeField] List<MonoBehaviour> _components = new(32); 
        private Dictionary<Type, IInitializable> _cachedComponents = new();

        protected override void Awake()
        {
            base.Awake();
            if(!_isFindOnAwake) return;
            var components = FindObjectsOfType<MonoBehaviour>().Where(x => x is IInitializable && !_components.Contains(x));
            foreach (var component in components)
            {
                if(!_components.Contains(component))
                    _components.Add(component);
            }
        }
        
        private IEnumerator Start()
        {
            foreach (var item in _components)
            {
                if (item is IInitializable initializer)
                {
                    if (initializer.IsInitialized)
                        continue;
                    initializer.Initialize();
                    if(!initializer.IsInitialized)
                        yield return new WaitUntil(() => initializer.IsInitialized);
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
            if (Instance._cachedComponents.TryGetValue(typeof(T), out var component))
            {
                return (T)component;
            }
            else
            {
                var c = Instance._components.OfType<T>().FirstOrDefault();
                if (c != null) Instance._cachedComponents.Add(typeof(T), c);
                return c;
            }
        }

        private void OnValidate()
        {
            for (int i = _components.Count - 1; i >= 0; i--)
            {
                var component = _components[i];
                if (component is not IInitializable)
                    _components.Remove(component);
                foreach (var nextItem in component.gameObject.GetComponents(typeof(MonoBehaviour)))
                {
                    if (nextItem is IInitializable && !_components.Contains(nextItem))
                    {
                        _components.Add(nextItem as MonoBehaviour);
                    }
                }
            }
        }
    }
}