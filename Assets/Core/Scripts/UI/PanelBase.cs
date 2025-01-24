using UnityEngine;

namespace Core.UI
{
    public class PanelBase : MonoBehaviour
    {
        public virtual void Initialize()
        {

        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
