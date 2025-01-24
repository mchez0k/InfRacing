using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Core.Systems
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private AssetReference sceneReference;
        [SerializeField] private List<MonoBehaviour> managers = new List<MonoBehaviour>();

        private void Awake()
        {
            InitializeManagers();
            LoadSceneAsync();
        }

        private void InitializeManagers()
        {
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

        private async void LoadSceneAsync()
        {
            var handle = sceneReference.LoadSceneAsync(LoadSceneMode.Single);

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Scene loaded successfully.");
            }
            else
            {
                Debug.LogError($"Failed to load scene: {handle.Status}");

                if (handle.OperationException != null)
                {
                    Debug.LogError($"Error details: {handle.OperationException.Message}");
                }
            }
        }
    }
}