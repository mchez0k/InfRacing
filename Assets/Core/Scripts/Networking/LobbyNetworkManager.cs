using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Core.Systems;
using UnityEngine.Events;
namespace Core.Networking
{
    public class LobbyNetworkManager : MonoBehaviourPunCallbacks, IInitializable
    {
        public bool IsInitialized { get; private set; }

        public UnityEvent OnJoinRoomFailedEvent = new UnityEvent();
        public void Initialize()
        {
            IsInitialized = true;
        }

        public void JoinOrCreateRoom(string roomName = null)
        {
            if (PhotonNetwork.IsConnected)
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = 8;
                if (roomName == null) 
                    roomName = $"Room {Random.Range(0, 1000)}";

                PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
            }
            else
            {
                OnJoinRoomFailedEvent.Invoke();
                Debug.LogError("Not connected to Photon.");
            }
        }

        public override void OnJoinedRoom()
        {
            var room = PhotonNetwork.CurrentRoom;
            Debug.Log($"Joined Room {room.Name} with {room.PlayerCount} players");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            OnJoinRoomFailedEvent.Invoke();
            Debug.LogError($"Code {returnCode}\nFailed to join room: {message}");
        }
    }
}