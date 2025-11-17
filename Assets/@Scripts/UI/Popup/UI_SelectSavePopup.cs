using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using Extensions;
using Define;

public class UI_SelectSavePopup : UI_Popup
{
    #region Property
    private UniTaskCompletionSource<ESavePopupResult> _tcs;

    private TMP_Text playTime1;
    private TMP_Text playTime2;
    private TMP_Text playTime3;
    private TMP_Text level1;
    private TMP_Text level2;
    private TMP_Text level3;
    #endregion

    protected override bool Init()
    {
        if (!base.Init())
            return false;

        playTime1 = GetText("PlayTime1");
        playTime2 = GetText("PlayTime2");
        playTime3 = GetText("PlayTime3");
        level1 = GetText("Level1");
        level2 = GetText("Level2");
        level3 = GetText("Level3");
        return true;
    }

    public override void OpenPopup()
    {
        base.OpenPopup();
        Prepare();
    }

    private void Prepare()
    {
        _tcs = new UniTaskCompletionSource<ESavePopupResult>();

        SetSlotUI(0, playTime1, level1);
        SetSlotUI(1, playTime2, level2);
        SetSlotUI(2, playTime3, level3);
    }

    private void SetSlotUI(int slotIndex, TMP_Text playTimeText, TMP_Text levelText)
    {
        Managers.Save.SetSlot(slotIndex);
        var saveData = Managers.Save.LoadData();

        if (saveData != null)
        {
            playTimeText.text = Extension.FormatPlayTime(saveData.playTime);
            levelText.text = $"Level: {saveData.playerData.level}";
        }
        else
        {
            playTimeText.text = "Empty Slot";
            levelText.text = "";
        }
    }

    public void OnClickSaveButton1()
    {
        _tcs.TrySetResult(ESavePopupResult.Slot0);
        ClosePopup();
    }

    public void OnClickSaveButton2()
    {
        _tcs.TrySetResult(ESavePopupResult.Slot1);
        ClosePopup();
    }

    public void OnClickSaveButton3()
    {
        _tcs.TrySetResult(ESavePopupResult.Slot2);
        ClosePopup();
    }

    public void OnClickDeleteButton()
    {
        _tcs.TrySetResult(ESavePopupResult.Delete);
        ClosePopup();
    }

    public void OnClickCancelButton()
    {
        _tcs.TrySetResult(ESavePopupResult.Cancel);
        ClosePopup();
    }



    public UniTask<ESavePopupResult> WaitForSelectionAsync()
    {
        if (_tcs == null)
            Prepare();

        return _tcs.Task;
    }
}
