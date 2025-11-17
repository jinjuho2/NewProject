using Cysharp.Threading.Tasks;
using UnityEngine;
using Define;

public class UI_GameScene : UI_Scene
{
    public async UniTask OnClickOptionButton()
    {
       await Managers.UI.ShowPopupUI<UI_OptionPopup>("UI_OptionPopup");
    }

    public void OnClickLevelupButton()
    {
        Player player = Object.FindFirstObjectByType<Player>();
        player.PlayerData.level += 1;
        Debug.Log($"Player leveled up to {player.PlayerData.level}!");
    }
}
