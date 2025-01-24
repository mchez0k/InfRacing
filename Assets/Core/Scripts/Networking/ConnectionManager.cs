using Core.Systems;
using Photon.Pun;
using UnityEngine;

namespace Core.Networking
{
    public class ConnectionManager : MonoBehaviour, IInitializable
    {
        public void Initialize()
        {
            Connect();
        }

        public void Connect()
        {
            if (PhotonNetwork.IsConnected) return;

            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
        }
    }
}
