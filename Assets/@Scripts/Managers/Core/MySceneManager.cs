using UnityEngine;
using Define;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class MySceneManager
{
    public Escene CurrentSceneType { get; private set; } = Escene.None;
    public BaseScene CurrentScene { get; private set; }

    private bool _isLoading = false;

    public event Action<Escene> OnSceneChanged;

    public void Init()
    {
        var baseSceneGO = UnityEngine.Object.FindFirstObjectByType<BaseScene>();
        if (baseSceneGO != null)
        {
            CurrentScene = baseSceneGO;
            CurrentSceneType = CurrentScene.SceneType;
        }
        else
        {
            CurrentSceneType = Escene.None;
        }
    }

    public async UniTask LoadSceneAsync(Escene targetScene,float minimumLoadingTime = 0f)
    {
        if (_isLoading) return;

        _isLoading = true;

        if(CurrentScene != null)
        {
            CurrentScene.OnExit();
        }

        float startTime = Time.realtimeSinceStartup;

        string sceneName = GetSceneName(targetScene);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        op.allowSceneActivation = true;

        while (!op.isDone)
        {
            await UniTask.Yield();
        }

        CurrentScene = UnityEngine.Object.FindFirstObjectByType<BaseScene>();
        CurrentSceneType = targetScene;

        if(CurrentScene != null)
        {
            await CurrentScene.Init();
        }

        float elpasedTime = Time.realtimeSinceStartup - startTime;
        if(elpasedTime < minimumLoadingTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(minimumLoadingTime - elpasedTime));
        }

        CurrentScene?.OnEnter();

        OnSceneChanged?.Invoke(CurrentSceneType);
        _isLoading = false;
    }


    private string GetSceneName(Escene type)
    {
        return type switch
        {
            Escene.Title => "TitleScene",
            Escene.Lobby => "LobbyScene",
            Escene.Game => "GameScene",
            _ => "TitleScene",
        };
    }


}
