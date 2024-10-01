using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Serialize Fields

    [SerializeField] private Transform ProjectilePoint;
    [SerializeField] private Transform[] FollowerPosition;

    #endregion

    #region Fields

    // 동료 프로퍼티
    private GameObject[] followerPrefab = new GameObject[5];

    // 플레이어  스크립트
    private PlayerAnimController playerAnimController;
    private PlayerFollowerHandler playerFollowerHandler;

    // 배경 스크립트
    private ParallaxController parallaxController;
    #endregion

    #region Properties

    public PlayerState State = PlayerState.None;

    #endregion
    #region Init

    public void Initialize()
    {
        playerAnimController = GetComponent<PlayerAnimController>();
        playerFollowerHandler = GetComponent<PlayerFollowerHandler>();
        parallaxController = FindObjectOfType<ParallaxController>();
    }

    #endregion

    #region Unity Flow

    private void Update()
    {
        if (State != PlayerState.Die)
        {
            Walk();
        }
        else if (State != PlayerState.Battle & State != PlayerState.Die)
        {
            Battle();
        }
    }

    #endregion

    #region State

    public void Walk()
    {
        State = PlayerState.Move;
        playerAnimController.OnWalk();
        parallaxController.LayerMove();
        playerFollowerHandler.InvokeFollowerWalk();
    }

    public void Battle()
    {
        State = PlayerState.Battle;
        playerAnimController.OnIdle();
        playerFollowerHandler.InvokeFollowerBattle();
    }

    private IEnumerator Dead()
    {
        //이전 스테이지로, Hp 리셋
        State = PlayerState.Die;
        playerAnimController.OnDead();

        yield return new WaitForSeconds(2.0f);
        Revive();
    }

    private void Revive()
    {
        State = PlayerState.None;
        playerAnimController.OnRevive();
    }

    #endregion
}
