using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Data.Contents;

public class TimeManager : ISaveSection
{
    public float PlayTime { get; private set; }


    private CancellationTokenSource _cts;


    public void Init()
    {
        if(_cts != null)
        {
            _cts?.Cancel();
            _cts = null;
        }
        Managers.Save.Register(this);
        _cts = new CancellationTokenSource();
        UpdatePlayTimeAsync(_cts.Token).Forget();
    }

   
    private async UniTask UpdatePlayTimeAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            PlayTime += 1f;
            await UniTask.Delay(1000, DelayType.DeltaTime, PlayerLoopTiming.Update, token);
        }
    }

    public void Capture(ref SaveData data)
    {
        data.playTime = PlayTime;
    }

    public void SetPlayTime(float time)
    {
        PlayTime = time;
    }
}
