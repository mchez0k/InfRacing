using Core.Systems;
using Photon.Pun;
using UnityEngine;

namespace Core.Networking
{
    public class ConnectionManager : MonoBehaviour, IInitializable
    {
        [HideInInspector] public bool IsInitialized { get; private set; }
        public void Initialize()
        {
            Connect();
            IsInitialized = true;
        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected) return;

            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
        }
    }
}
