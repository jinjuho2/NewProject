using Cysharp.Threading.Tasks;
using Extensions;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    protected bool _isInit = false;

    protected RectTransform _rect;

    protected CanvasGroup _canvasGroup;

    protected Dictionary<string, Button> _buttons = new Dictionary<string, Button>();
    protected Dictionary<string, Image> _images = new Dictionary<string, Image>();
    protected Dictionary<string, TMP_Text> _texts = new Dictionary<string, TMP_Text>();





    protected virtual void Awake()
    {
        Init();
    }


    protected virtual bool Init()
    {
        if (_isInit) return false;

        GameObject eventSystem = GameObject.Find("EventSystem");
        if (eventSystem == null)
        {
            Managers.Resource.InstantiateAsync("EventSystem").Forget();
        }

        _rect = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        BindButtons();
        BindTexts();
        BindImages();

        if (_canvasGroup == null)
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();


        _isInit = true;
        return true;
    }
    #region BindButtons
    private void BindButtons()
    {
        Button[] buttons = GetComponentsInChildren<Button>(true);

        foreach (var button in buttons)
        {
            string buttonName = button.gameObject.name;
            _buttons[buttonName] = button;
            string methodName = $"OnClick{buttonName}";

            MethodInfo method = GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (method == null)
            {
                continue;
            }

            bool isAsync = method.ReturnType == typeof(UniTask) || method.ReturnType == typeof(UniTaskVoid);

            if (isAsync)
            {
                button.BindAsync(() =>
                {
                    object result = method.Invoke(this, null);
                    return result switch
                    {
                        UniTask task => task,
                        UniTaskVoid _ => default(UniTask),
                        _ => default(UniTask)
                    };
                });
            }
            else
            {
                button.onClick.AddListener(() => method.Invoke(this, null));
            }
        }
    }
    #endregion

    #region Text Binding
    private void BindTexts()
    {
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(true);
        foreach (var text in texts)
        {
            string key = text.gameObject.name;
            if (!_texts.ContainsKey(key))
                _texts.Add(key, text);
        }
    }
    #endregion

    #region void BindImages()
    private void BindImages()
    {
        Image[] images = GetComponentsInChildren<Image>(true);
        foreach (var image in images)
        {
            string key = image.gameObject.name;
            if (!_images.ContainsKey(key))
                _images.Add(key, image);
        }
    }
    #endregion

    #region Getter
    protected Image GetImage(string name)
    {
        _images.TryGetValue(name, out var image);
        return image;
    }

    protected TMP_Text GetText(string name)
    {
        _texts.TryGetValue(name, out var text);
        return text;
    }
    #endregion
}
