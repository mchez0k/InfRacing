using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Core.Systems;
using Core.Gameplay;
using Core.UI;

namespace Core.Networking
{
    public class RoomNetworkManager : MonoBehaviourPunCallbacks, IInitializable
    {
        public bool IsInitialized { get; private set; }

        private CarSpawner carSpawner;
        private UIManager uiManager;

        public void Initialize()
        {
            uiManager = EntryPoint.Get<UIManager>();
            carSpawner = EntryPoint.Get<CarSpawner>();

            IsInitialized = true;
            StartGame();
        }

        public void StartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("RPC_StartGame", RpcTarget.All);
            }
        }

        [PunRPC]
        private void RPC_StartGame()
        {
            Debug.Log("Game started!");
            uiManager.Open(typeof(GamePanel));

            carSpawner.SpawnCar(PhotonNetwork.LocalPlayer.ActorNumber);
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

        private void UpdateRoomInfo()
        {
            Debug.Log($"Players in room: {PhotonNetwork.CurrentRoom.PlayerCount}");
        }
    }
}