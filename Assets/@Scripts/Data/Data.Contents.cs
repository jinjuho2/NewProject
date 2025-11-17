using System;
using UnityEngine;

namespace Data.Contents  
{
    #region SaveData
    [Serializable]
    public class SaveData
    {
        public float playTime;
        public PlayerData playerData;
    }
    #endregion

    #region PlayerData
    [Serializable]
    public class PlayerData
    {
        public float level;
        public float exp;
        public float maxHealth;
        public float currentHealth;
        public float maxMana;
        public float currentMana;
        public float atk;
        public float def;
        public float speed;

        public Vector3 pos;
    }
    #endregion
}
