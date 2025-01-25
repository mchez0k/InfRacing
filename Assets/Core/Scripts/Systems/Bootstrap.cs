using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Core.Systems
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> managers = new List<MonoBehaviour>();

        private void Awake()
        {
            DataProvider.Initialize();
            InitializeManagers();
        }

        private void InitializeManagers()
        {
            Debug.Log(name);
            foreach (var manager in managers)
            {
                if (manager != null &&
                    manager is IInitializable initializable)
                {
                    initializable.Initialize();
                }
            }

            Debug.Log("All managers initialized");
        }
    }
}