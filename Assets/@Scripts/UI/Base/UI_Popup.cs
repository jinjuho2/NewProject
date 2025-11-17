using Cysharp.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Extensions;


public class UI_Popup : UI_Base
{
    public virtual void OpenPopup()
    {

    }

    public virtual void ClosePopup()
    {
        Managers.UI.ClosePopupUI(this);
    }


    
}


