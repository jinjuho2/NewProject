using Cysharp.Threading.Tasks;
using Data.Contents;
using Define;
using UnityEngine;
using Define;

public class UI_LobbyScene : UI_Scene
{
    public async UniTask OnClickStartButton()
    {
        var popup = await Managers.UI.ShowPopupUI<UI_SelectSavePopup>("UI_SelectSavePopup");
        ESavePopupResult result = await popup.WaitForSelectionAsync();  // 0/1/2

        switch (result)
        {
            case ESavePopupResult.Slot0:
            case ESavePopupResult.Slot1:
            case ESavePopupResult.Slot2:
                Managers.Save.SetSlot((int)result);
                if (Managers.Save.LoadData() == null)
                {
                    await Managers.Scene.LoadSceneAsync(Escene.Game);
                    break;
                }
                await Managers.UI.ShowPopupUI<UI_SlotActionPopup>("UI_SlotActionPopup");
                break;

            case ESavePopupResult.Cancel:
            default:
                popup.ClosePopup();
                break;
        }
    }

    public void OnClickSettingButton()
    {
        Debug.Log("Setting Button Clicked");
    }

    public void OnClickExitButton()
    {
        Debug.Log("Exit Button Clicked");
    }

}
