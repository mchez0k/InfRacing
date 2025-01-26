using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class InfoPanel : PanelBase
    {
        [SerializeField] private Image avatarIcon; // TODO: avatarIcon �������� �� ������ ������� ������ ��� � nickname
        [SerializeField] private TextMeshProUGUI nickname;
        [SerializeField] private Button settingButton;

        public override void Initialize()
        {
            nickname.text = PhotonNetwork.NickName;
            //settingButton.onClick.AddListener();
        }
    }
}
