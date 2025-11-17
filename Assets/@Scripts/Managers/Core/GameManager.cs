using Cysharp.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager
{
    public async UniTask Init()
    {
        if (Managers.Save.Data.playerData == null)
        {
            var so = ScriptableObject.CreateInstance<PlayerDefaultDataSO>();
            GameObject newGo = await Managers.Resource.InstantiateAsync("Player");
            Player newPlayer = newGo.AddComponent<Player>();
            newPlayer.Init(so.SetPlayerData());
            Managers.Time.SetPlayTime(0f);
            return;
        }

        Managers.Save.LoadData();
        GameObject go = await Managers.Resource.InstantiateAsync("Player");
        Player player = go.AddComponent<Player>();
        player.Init(Managers.Save.Data.playerData);
        Managers.Time.SetPlayTime(Managers.Save.Data.playTime);

    }
}
