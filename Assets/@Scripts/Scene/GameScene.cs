using Cysharp.Threading.Tasks;
using Define;
using UnityEngine;

public class GameScene : BaseScene
{
    public override Escene SceneType => Escene.Game;


    public override async UniTask OnEnter()
    {
        await Managers.Game.Init();
    }
}
