using UnityEngine;

namespace Core.Utility
{
    /// <summary>
    /// For faster develop used this one
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance => instance;
        [SerializeField] private bool isDontDestroyOnLoad;

        protected virtual void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            var instances = FindObjectsOfType<T>(); // TODO: expensive, should upgrade or replace with DI
            var count = instances.Length;
            if (count == 1)
            {
                instance = instances[0];
            }
            else if (count > 1)
            {
                for (var i = 1; i < instances.Length; i++)
                {
                    Destroy(instances[i]);
                }
                instance = instances[0];
            }
            else
            {
                instance = new GameObject($"({nameof(Singleton<T>)}){typeof(T)}").AddComponent<T>();
                Debug.LogWarning($"Created a new '{typeof(T)}' ");
            }

            SetDestroyOnLoad();
        }

        private void SetDestroyOnLoad()
        {
            if (isDontDestroyOnLoad)
            {
                transform.SetParent(null);
                DontDestroyOnLoad(instance.gameObject);
            }
        }
    }
}