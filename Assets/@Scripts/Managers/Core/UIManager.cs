using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{

    private UI_Scene _sceneUI;

    private Transform _uiRoot;

    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();

    public void Init()
    {
        if (_uiRoot == null)
        {
            var go = GameObject.Find("@UI_Root");
            if (go == null)
            {
                go = new GameObject { name = "@UI_Root" };
            }

            Object.DontDestroyOnLoad(go);
            _uiRoot = go.transform;
        }

    }

    public void SetSceneUI(UI_Scene sceneUI)
    {
        _sceneUI = sceneUI;
        _sceneUI.transform.SetParent(_uiRoot);
    }

    public T GetSceneUI<T>() where T : UI_Scene
    {
        return _sceneUI as T;
    }

    public async UniTask<T> ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        name ??= typeof(T).Name;

        if (_sceneUI != null)
        {
            Managers.Resource.Release($"{name}", _sceneUI.gameObject);
            _sceneUI = null;
        }

        GameObject go = await Managers.Resource.InstantiateAsync($"{name}");
        go.transform.SetParent(_uiRoot, false);

        T sceneUI = go.GetComponent<T>();
        _sceneUI = sceneUI;
        return sceneUI;
    }

    public async UniTask<T> ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        name ??= typeof(T).Name;

        foreach (var popup in _popupStack)
        {
            if (popup.name == name)
            {
                return (T)popup;
            }
        }

        GameObject go = await Managers.Resource.InstantiateAsync($"{name}");
        go.transform.SetParent(_uiRoot, false);

        T newPopup = go.GetComponent<T>();
        newPopup.OpenPopup();

        _popupStack.Push(newPopup);
        return newPopup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0) return;
        if (_popupStack.Peek() != popup)
        {
            Debug.LogWarning($"[UIManager] Try close {popup.name} but it's not top of stack.");
            return;
        }

        Managers.Resource.Release($"{popup.name}", popup.gameObject);
        _popupStack.Pop();
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            var popup = _popupStack.Pop();
            Managers.Resource.Release($"{popup.name}", popup.gameObject);
        }
    }

    public void Clear()
    {
        CloseAllPopupUI();
        if (_sceneUI != null)
        {
            Managers.Resource.Release($"{_sceneUI.name}", _sceneUI.gameObject);
            _sceneUI = null;
        }
    }


}
