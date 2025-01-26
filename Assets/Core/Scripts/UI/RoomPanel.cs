using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.UI
{
    public class RoomPanel : PanelBase
    {
        [SerializeField] private TextMeshProUGUI roomName;

        [Space(2)]
        [SerializeField] private RoomPlayerItem prefab;
        [SerializeField] private Transform content;

        [Space(2)]
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button leaveRoomButton;

        public UnityEvent OnStartPressed = new UnityEvent();
        public UnityEvent OnLeavePressed = new UnityEvent();
        public override void Initialize()
        {
            startGameButton.onClick.AddListener(StartGame);
            leaveRoomButton.onClick.AddListener(Leave);
        }

        public void UpdateRoom(Room room)
        {
            UpdateRoomName(room);
            UpdatePlayers(room);
        }

        public void StartGame()
        {
            OnStartPressed.Invoke();
        }

        public void Leave()
        {
            OnLeavePressed.Invoke();
        }

        public override void Open()
        {
            base.Open();
            var room = PhotonNetwork.CurrentRoom;
            startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
            UpdateRoom(room);
        }

        public override void Close()
        {
            ClearPlayers();
            base.Close();
        }

        private void UpdateRoomName(Room room)
        {
            roomName.text = room.Name;
        }

        private void UpdatePlayers(Room room)
        {
            ClearPlayers();
            foreach (var player in room.Players)
            {
                var playerInfo = player.Value;
                Instantiate(prefab, content)
                    .Initialize(playerInfo.NickName, playerInfo.IsMasterClient);
            }
        }

        private void ClearPlayers()
        {
            foreach (Transform element in content) Destroy(element.gameObject);
        }
    }
}

