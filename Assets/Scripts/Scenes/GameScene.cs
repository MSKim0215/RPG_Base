using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        gameObject.GetOrAddComponent<CursorController>();

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "Player");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        GameObject spawningPool = new GameObject { name = "SpawningPool" };
        SpawningPool pool = spawningPool.GetOrAddComponent<SpawningPool>();
        pool.SetKeepMonsterCount(5);

        // ���� UI ����
        Managers.UI.ShowSceneUI<UI_UnitFrame>();
        Managers.UI.ShowSceneUI<UI_ExpBar>();
        Managers.UI.ShowSceneUI<UI_Joystick>();
        Managers.UI.ShowSceneUI<UI_AttackButton>().SetPlayer(player);
    }

    public override void Clear()
    {
        
    }
}