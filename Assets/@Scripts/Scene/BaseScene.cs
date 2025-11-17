using UnityEngine;
using Define;
using Cysharp.Threading.Tasks;

public abstract class BaseScene : MonoBehaviour
{
    public abstract Escene SceneType { get; }

    protected bool _isInitialized = false;

    public virtual async UniTask Init()
    {
        if(_isInitialized) return;

        await Managers.UI.ShowSceneUI<UI_Scene>("UI_" + SceneType.ToString() + "Scene");

        _isInitialized = true;
        await UniTask.CompletedTask;
    }

    public virtual async UniTask OnEnter() { }

    public virtual void OnExit() 
    {
        Managers.Clear();
    }

}
