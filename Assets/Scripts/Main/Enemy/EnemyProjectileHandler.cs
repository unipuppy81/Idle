using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileHandler : ProjectileHandlerBase
{
    protected override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        TrackingTarget(Manager.Game.player.transform.position, speed);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetLayerMask.value == (targetLayerMask.value | (1 << collision.gameObject.layer)) || Vector2.Distance(transform.position, targetPosition) < Mathf.Epsilon)
        {
            VFXOff();
            ReleaseObject();
        }
    }
}
