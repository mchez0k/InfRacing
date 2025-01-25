using UnityEngine;

namespace Core.Utility
{
    /// <summary>
    /// For faster develop used this one
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance => _instance;
        [SerializeField] private bool _isDontDestroyOnLoad;

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            var instances = FindObjectsOfType<T>(); // TODO: expensive, should upgrade or replace with DI
            var count = instances.Length;
            if (count == 1)
            {
                _instance = instances[0];
            }
            else if (count > 1)
            {
                for (var i = 1; i < instances.Length; i++)
                {
                    Destroy(instances[i]);
                }
                _instance = instances[0];
            }
            else
            {
                _instance = new GameObject($"({nameof(Singleton<T>)}){typeof(T)}").AddComponent<T>();
                Debug.LogWarning($"Created a new '{typeof(T)}' ");
            }

            SetDestroyOnLoad();
        }

        private void SetDestroyOnLoad()
        {
            if (_isDontDestroyOnLoad)
            {
                transform.SetParent(null);
                DontDestroyOnLoad(_instance.gameObject);
            }
        }
    }
}