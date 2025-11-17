using UnityEngine;
using Data.Contents;

[CreateAssetMenu(fileName = "PlayerDefaultDataSO", menuName = "Data/PlayerDefaultDataSO")]
public class PlayerDefaultDataSO : ScriptableObject
{
    public int level = 1;
    public float exp = 0f;
    public float maxHealth = 100f;
    public float maxMana = 50f;
    public float atk = 10f;
    public float def = 5f;
    public float speed = 5f;
    public Vector3 startPosition = Vector3.zero;

    public PlayerData SetPlayerData()
    {
        return new PlayerData
        {
            level = level,
            exp = exp,
            maxHealth = maxHealth,
            currentHealth = maxHealth,
            maxMana = maxMana,
            currentMana = maxMana,
            atk = atk,
            def = def,
            speed = speed,
            pos = startPosition
        };
    }
}
