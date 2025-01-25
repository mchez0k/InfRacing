using Core.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class MenuPanel : PanelBase
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button garageButton;

        public InfoPanel infoPanel;

        public override void Initialize()
        {
            //playButton.onClick.AddListener();
            //garageButton.onClick.AddListener();

            infoPanel = EntryPoint
                .Get<UIManager>()
                .GetPanel<InfoPanel>();
        }

        public override void Open()
        {
            base.Open();
            infoPanel.Open();
        }
    }
}