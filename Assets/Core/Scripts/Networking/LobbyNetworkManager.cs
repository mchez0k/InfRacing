using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Core.Systems;
using UnityEngine.Events;
using Core.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
namespace Core.Networking
{
    /// <summary>
    /// Controls the lobby state, joining and create rooms
    /// </summary>
    public class LobbyNetworkManager : MonoBehaviourPunCallbacks, IInitializable
    {
        [SerializeField] private float updateTime = 5f;
        public bool IsInitialized { get; private set; }

        public UnityEvent OnJoinRoomFailedEvent = new UnityEvent();

        private UIManager uiManager;

        public void Initialize()
        {
            uiManager = EntryPoint.Get<UIManager>();
            InvokeRepeating("UpdateOnlineInfo", 0f, updateTime);
            IsInitialized = true;
        }

        public void JoinOrCreateRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                OnJoinRoomFailedEvent.Invoke();
                Debug.LogError("Not connected to Photon.");
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);

            UpdateOnlineInfo();
        }

        public override void OnJoinedRoom()
        {
            var room = PhotonNetwork.CurrentRoom;

            uiManager.Open(typeof(RoomPanel));

            var panel = uiManager.GetPanel<RoomPanel>();
            panel.UpdateRoom(room);
            if (PhotonNetwork.IsMasterClient)
                panel.OnStartPressed.AddListener(StartGame);
            panel.OnLeavePressed.AddListener(LeaveRoom);

            Debug.Log($"Joined Room {room.Name} with {room.PlayerCount} players");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"Player {newPlayer.NickName} entered the room.");
            UpdateRoomInfo();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"Player {otherPlayer.NickName} left the room.");
            UpdateRoomInfo();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            OnJoinRoomFailedEvent.Invoke();
            Debug.LogError($"Code {returnCode}\nFailed to join room: {message}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"No random room available. Creating a new room");

            var roomName = $"Room {Random.Range(0, 1000)}";
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = 8,
                IsOpen = true,
                IsVisible = true
            };

            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Left the room.");
            uiManager.Open(typeof(MenuPanel), false);
        }
        private void StartGame()
        {
            Debug.Log("Starting game");
            photonView.RPC("RPC_LoadLevel", RpcTarget.All, 2);
        }

        [PunRPC]
        private void RPC_LoadLevel(int sceneIndex)
        {
            StartCoroutine(LoadSceneAsync(sceneIndex));
        }

        private IEnumerator LoadSceneAsync(int sceneIndex)
        {
            uiManager.Open(typeof(LoadingPanel));

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

            while (!asyncLoad.isDone)
            {
                uiManager.GetPanel<LoadingPanel>().UpdateProgress(asyncLoad.progress);
                yield return null;
            }
        }

        private void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void UpdateOnlineInfo()
        {
            uiManager.GetPanel<MenuPanel>()
                .UpdateOnlineInfo(PhotonNetwork.CountOfRooms, PhotonNetwork.CountOfPlayers);
        }

        private void UpdateRoomInfo()
        {
            uiManager.GetPanel<RoomPanel>()
                .UpdateRoom(PhotonNetwork.CurrentRoom);
        }
    }
}