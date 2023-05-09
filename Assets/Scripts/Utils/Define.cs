using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Layer
    {
        Monster = 6,
        Ground,
        Block
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game
    }

    public enum Sound
    {
        Bgm, Sfx, MaxCount
    }

    public enum UIEvent
    {
        Click
    }

    public enum CameraMode
    {
        QuarterView
    }

    public enum MouseEvent
    {
        Press, PointerDown, PointerUp, Click
    }

    public enum CharacterState
    {
        Die, Moving, Idle, Attack, SkillAttack, SkillBuff
    }

    public enum WorldObject
    {
        Unknown, Player, Monster
    }

    public enum MonsterType
    {
        BananaMan = 100
    }
}