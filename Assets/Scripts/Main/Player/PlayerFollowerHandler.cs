using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowerHandler : MonoBehaviour
{
    [SerializeField] private Transform[] _followerPosition;

    private event Action FollowerWalk;
    private event Action FollowerBattle;

    public void AddFollowerWalkEvent(Action action)
    {
        FollowerWalk += action;
    }
    public void RemoveFollowerWalkEvent(Action action)
    {
        FollowerWalk -= action;
    }
    public void InvokeFollowerWalk()
    {
        FollowerWalk?.Invoke();
    }

    public void AddFollowerBattleEvent(Action action)
    {
        FollowerBattle += action;
    }
    public void RemoveFollowerBattleEvent(Action action)
    {
        FollowerBattle -= action;
    }
    public void InvokeFollowerBattle()
    {
        FollowerBattle?.Invoke();
    }
}
