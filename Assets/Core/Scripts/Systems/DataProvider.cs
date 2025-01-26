using Core.Misc;
using Core.Utility;
using Photon.Pun;
using UnityEngine;

namespace Core.Systems
{
    /// <summary>
    /// Class providing data, can replace Data with a abstract data
    /// </summary>
    public class DataProvider
    {
        public static PlayerPrefsData Data { get; } = new PlayerPrefsData();

        public static void Initialize()
        {
            if (!Data.HasKey(GameConstants.PLAYER_NICKNAME_SAVE_KEY))
            {
                var name = "Player " + Random.Range(11111, 99999);
                PhotonNetwork.LocalPlayer.NickName = name;
                SetPlayerName(name);
            }

            if (!Data.HasKey(GameConstants.PLAYER_SKIN_SAVE_KEY))
            {
                SetCurrentSkin((int)ECarSkin.BlackSkin);
            }
        }

        public static void SetPlayerName(string value) => Data.SetString(GameConstants.PLAYER_NICKNAME_SAVE_KEY, value);
        public static string GetPlayerName() => Data.GetString(GameConstants.PLAYER_NICKNAME_SAVE_KEY);

        public static void SetCurrentSkin(int value) => Data.SetInt(GameConstants.PLAYER_SKIN_SAVE_KEY, value);
        public static ECarSkin GetCurrentSkin() => (ECarSkin)Data.GetInt(GameConstants.PLAYER_SKIN_SAVE_KEY);

    }
}

