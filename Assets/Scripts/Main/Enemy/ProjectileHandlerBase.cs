using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandlerBase : ObjectPoolable
{
    [HideInInspector]
    public long damage;
    public GameObject projectileVFX;
    [HideInInspector]
    public DamageType damageTypeValue = DamageType.Normal;

    public float speed = 0.1f;

    public Vector2 targetPosition;

    public LayerMask targetLayerMask;

    public SpriteRenderer projectileSprite;

    private bool has;

    protected virtual void Start()
    {
        if (projectileVFX != null)
        {
            GameObject go = Instantiate(projectileVFX, transform.position, Quaternion.identity, gameObject.transform);
            int index = go.name.IndexOf("(Clone)");
            if (index > 0)
            {
                go.name = go.name.Substring(0, index);
            }
        }
    }

    protected void TrackingTarget(Vector2 targetPosition, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetLayerMask.value == (targetLayerMask.value | (1 << collision.gameObject.layer)) || Vector2.Distance(transform.position, targetPosition) < Mathf.Epsilon)
        {
            ReleaseObject();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (targetLayerMask.value == (targetLayerMask.value | (1 << collision.gameObject.layer)))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(damage, damageTypeValue);
        }
    }

    public void SetProjectile(GameObject VFX, long Damage)
    {
        this.damage = Damage;
        this.projectileVFX = VFX;

        has = false;
        if (projectileVFX != null && transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (this.transform.GetChild(i).gameObject.name == projectileVFX.name)
                {
                    has = true;
                    this.transform.GetChild(i).gameObject.SetActive(true);
                    break;
                }
            }
            if (!has)
            {
                GameObject go = Instantiate(projectileVFX, transform.position, Quaternion.identity, gameObject.transform);
                int index = go.name.IndexOf("(Clone)");
                if (index > 0)
                {
                    go.name = go.name.Substring(0, index);
                }
            }
        }
    }
}
