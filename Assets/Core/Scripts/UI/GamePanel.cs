using Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : PanelBase
{
    [SerializeField] private TextMeshProUGUI speed;
    [SerializeField] private Button settingButton;

    public void UpdateSpeed(string speed)
    {
        this.speed.text = $"{speed} km/h";
    }
}
