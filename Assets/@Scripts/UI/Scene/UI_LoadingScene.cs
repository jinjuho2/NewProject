using Cysharp.Threading.Tasks;
using Data.Contents;
using Define;
using Define;
using DG.Tweening;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingScene : UI_Scene
{
    #region Property
    private Slider progressBar;
    private Button touchButton;
    private TMP_Text playText;
    private Image logo;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        progressBar = FindAnyObjectByType<Slider>();
        touchButton = _buttons["TouchButton"];
        playText = GetText("PlayToTouchText");
        logo = GetImage("Logo");
        progressBar.value = 0f;


        RunPreload().Forget();
    }

    private async UniTask RunPreload()
    {
        await Managers.Resource.PreloadByLabelAsync("PreLoad", p => progressBar.value = p);

        touchButton.gameObject.SetActive(true);
        playText.gameObject.SetActive(true);
        logo.gameObject.SetActive(true);

        #region 로딩 연출
        playText.transform
            .DOScale(1.2f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo);

        playText
            .DOFade(0.5f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo);

        logo.transform.localScale = Vector3.zero;

        logo.transform
            .DOScale(1f, 1.5f)
            .SetEase(Ease.OutBack);

        #endregion
    }

    public async UniTask OnClickTouchButton()
    {
        await Managers.Scene.LoadSceneAsync(Escene.Lobby);
    }



}
