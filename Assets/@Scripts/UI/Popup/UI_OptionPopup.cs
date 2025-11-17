using Cysharp.Threading.Tasks;
using Define;
using UnityEngine;

public class UI_OptionPopup : UI_Popup
{
    public async UniTask OnClickSaveButton()
    {
        var popup = await Managers.UI.ShowPopupUI<UI_SelectSavePopup>("UI_SelectSavePopup");
        ESavePopupResult result = await popup.WaitForSelectionAsync();  // 0/1/2

        switch (result)
        {
            case ESavePopupResult.Slot0:
            case ESavePopupResult.Slot1:
            case ESavePopupResult.Slot2:
                Managers.Save.SetSlot((int)result);
                Managers.Save.Save();
                await Managers.UI.ShowPopupUI<UI_SavePopup>("UI_SavePopup");
                break;

            case ESavePopupResult.Cancel:
            default:
                popup.ClosePopup();
                break;
        }
    }

    public async UniTask OnClickLoadButton()
    {
        var popup = await Managers.UI.ShowPopupUI<UI_SelectSavePopup>("UI_SelectSavePopup");
        ESavePopupResult result = await popup.WaitForSelectionAsync();  // 0/1/2

        switch (result)
        {
            case ESavePopupResult.Slot0:
            case ESavePopupResult.Slot1:
            case ESavePopupResult.Slot2:
                Managers.Save.SetSlot((int)result);
                if(Managers.Save.LoadData() == null)
                {
                    Debug.Log("저장된 데이터가 없습니다.");
                    break;
                }
                await Managers.Scene.LoadSceneAsync(Escene.Game);
                break;

            case ESavePopupResult.Cancel:
            default:
                popup.ClosePopup();
                break;
        }
    }

    public async UniTask OnClickGotoLobbyButton()
    {
        await Managers.Scene.LoadSceneAsync(Escene.Lobby);
    }

    public void OnClickCancelButton()
    {
         ClosePopup();
    }


}
