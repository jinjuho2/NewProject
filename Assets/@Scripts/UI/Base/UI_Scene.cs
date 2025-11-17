using System.Security.Authentication.ExtendedProtection;
using UnityEngine;

public class UI_Scene : UI_Base
{
    protected override void Awake()
    {
        base.Awake();
        Managers.UI.SetSceneUI(this);
    }

   
}
