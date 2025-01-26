using Core.Networking;
using Core.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class MenuPanel : PanelBase
    {
        [SerializeField] private TextMeshProUGUI onlineOnfo;
        [SerializeField] private Button playButton;
        [SerializeField] private Button garageButton;

        private InfoPanel infoPanel;

        public override void Initialize()
        {
            playButton.onClick.AddListener(PressPlay);

            //garageButton.onClick.AddListener();

            infoPanel = EntryPoint
                .Get<UIManager>()
                .GetPanel<InfoPanel>();
        }

        public override void Open()
        {
            base.Open();
            infoPanel.Open();
            RestoreButton();
        }

        public void UpdateOnlineInfo(int rooms, int players)
        {
            onlineOnfo.text = $"Total rooms: {rooms}\nTotal players: {players}";
        }

        public void PressPlay()
        {
            playButton.interactable = false;

            var lobbyNetwork = EntryPoint
                .Get<LobbyNetworkManager>();
            lobbyNetwork.OnJoinRoomFailedEvent.AddListener(RestoreButton);
            lobbyNetwork.JoinOrCreateRoom();
        }

        private void RestoreButton()
        {
            playButton.interactable = true;
        }
    }
}