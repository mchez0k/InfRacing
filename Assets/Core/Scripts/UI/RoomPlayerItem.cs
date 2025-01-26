using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class RoomPlayerItem : MonoBehaviour
    {
        [SerializeField] private Image playerIcon;
        [SerializeField] private TextMeshProUGUI nickname;
        [SerializeField] private GameObject crownIcon;

        public void Initialize(string name, bool isHost = false)
        {
            SetNickname(name);
            crownIcon.SetActive(isHost);
        }

        private void SetNickname(string name)
        {
            nickname.text = name;
        }
    }
}