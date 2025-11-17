using UnityEngine;
using Data.Contents;
public class Player : MonoBehaviour, ISaveSection
{
    private PlayerData playerData;
    public PlayerData PlayerData => playerData;

    private bool _isInit = false;


    public void Init(PlayerData data)
    {
        if(_isInit) return;
        Managers.Save.Register(this);
        playerData = data;
        transform.position = playerData.pos;
        Debug.Log($"[Player] Initialized with Level: {playerData.level}");

        _isInit = true;
    }

    private void OnDisable()
    {
        Managers.Save.Unregister(this);
    }

    void ISaveSection.Capture(ref SaveData data)
    {
        playerData.pos = transform.position;
        data.playerData = playerData;
    }

}
