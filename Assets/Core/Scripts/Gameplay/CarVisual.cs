using Core.Systems;
using Core.Utils;
using UnityEngine;
namespace Core.Gameplay
{
    public class CarVisual : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;

        public void Initialize()
        {
            if (!meshRenderer) 
                meshRenderer.GetComponent<MeshRenderer>();

            ResourceLoader
                .LoadAddressableResource<Material>(DataProvider.GetCurrentSkin().ToString(), material =>
            {
                meshRenderer.material = material;
            });
        }
    }
}