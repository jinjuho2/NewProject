using Cysharp.Threading.Tasks;
using Data.Contents;
using Define;
using Extensions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_SlotActionPopup : UI_Popup
{

    private TMP_Text playTime;
    private TMP_Text level;

    protected override bool Init()
    {
        if (!base.Init())
            return false;
        playTime = GetText("PlayTime");
        level = GetText("Level");

        return true;
    }

    public override void OpenPopup()
    {
        base.OpenPopup();
        SetSlotUI(Managers.Save.LoadData());
    }

    private void SetSlotUI(SaveData data)
    {
        var saveData = Managers.Save.LoadData();

        if (saveData != null)
        {
            playTime.text = Extension.FormatPlayTime(saveData.playTime);
            level.text = $"Level: {saveData.playerData.level}";
        }
    }

    public void OnClickDeleteButton()
    {
        Managers.Save.DeleteData();
        Managers.UI.CloseAllPopupUI();

    }

    public async UniTask OnClickLoadButton()
    {
        await Managers.Scene.LoadSceneAsync(Define.Escene.Game);
    }

    public void OnClickCancelButton()
    {
        ClosePopup();
    }


}
