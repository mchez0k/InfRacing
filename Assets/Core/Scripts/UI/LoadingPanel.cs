using Core.Utils;
using UnityEngine;

namespace Core.UI
{
    public class LoadingPanel : PanelBase
    {
        [SerializeField] private ProgressBar progressBar;

        public override void Initialize()
        {
            progressBar.SetMinValue(0f);
            progressBar.SetMaxValue(100f);
            progressBar.SetProgress(0f);
        }

        public void UpdateProgress(float progress)
        {
            progressBar.SetProgress(progress);
        }
    }
}