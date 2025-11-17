using Unity.VisualScripting;
using UnityEngine;

public class UI_SavePopup : UI_Popup
{
    public void OnClickCheckButton()
    {
        Managers.UI.CloseAllPopupUI();
    }
}
