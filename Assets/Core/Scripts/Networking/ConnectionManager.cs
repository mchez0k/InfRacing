using Core.Systems;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Core.Networking
{
    public class ConnectionManager : MonoBehaviourPunCallbacks, IInitializable
    {
        [SerializeField] private AssetReference sceneReference;

        [HideInInspector] public bool IsInitialized { get; private set; }

        private TypedLobby customLobby = new TypedLobby("DefaultLobby", LobbyType.Default);

        public void Initialize()
        {
            Connect();
            IsInitialized = true;
        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected) return;

            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            PhotonNetwork.NickName = DataProvider.GetPlayerName();
            PhotonNetwork.JoinLobby(customLobby);
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            LoadSceneAsync(sceneReference);
        }

        // Can be used in Utils with static
        private async void LoadSceneAsync(AssetReference sceneReference)
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
