using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using Core.Systems;

namespace Core.Gameplay
{
    public class CarSpawner : MonoBehaviour, IInitializable
    {
        [SerializeField] private CarController carPrefab;
        [SerializeField] private List<Transform> spawnPoints;

        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            IsInitialized = true;
        }

        public void SpawnCar(int spawnId)
        {
            Debug.Log(spawnPoints.Count);
            if (spawnPoints.Count == 0)
            {
                Debug.LogError("No spawn points assigned");
                return;
            }

            Transform spawnPoint = spawnPoints[spawnId]; // For unique Spawns
            PhotonNetwork.Instantiate(carPrefab.name, spawnPoint.position, spawnPoint.rotation);
        }
    }
}