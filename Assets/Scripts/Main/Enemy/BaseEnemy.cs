using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : ObjectPoolable, IDamageable
{
    [SerializeField] private Canvas UiCanvas;

    #region Fields

    private EnemyBlueprint enemyBlueprint;
    private EnemyView enemyView;

    private string enemyName;

    //ü��
    private long maxHp;
    private long currentHP;

    //����
    private long damage;
    private float attackSpeed;
    private float range;

    //�ӵ�
    private float moveSpeed;

    public long rewards;

    private bool inViewport;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Camera camera_;

    private Coroutine attackCoroutine;
    public int testWeight;

    #endregion

    #region Init

    public void SetEnemy(EnemyBlueprint blueprint, Vector2 position, long hpWeight, long atkWeight, long goldWeight)
    {
        enemyBlueprint = blueprint;
        enemyName = blueprint.EnemyName;
        spriteRenderer.sprite = blueprint.EnemySprite;

        maxHp = blueprint.HP;


        damage = blueprint.Damage;
        attackSpeed = blueprint.AttackSpeed;

        if (blueprint.EnemyType != EnemyType.Boss)
            range = blueprint.Range + Random.Range(-0.5f, 0.5f);
        else
            range = blueprint.Range;


        moveSpeed = blueprint.MoveSpeed;
        rewards = blueprint.Rewards;

        SetPosition(position);
        SetStatWeight(hpWeight, atkWeight);
        SetGoldWeight(goldWeight);
        ResetHealth();
        SetHpBar();
    }

    public void SetStatWeight(long hpWeight, long atkWeight)
    {
        long _hpWeight = hpWeight - 1;
        maxHp = maxHp + maxHp * _hpWeight;
        long _atkWeight = atkWeight - 1;
        damage = damage + damage * _atkWeight;
        ResetHealth();
    }

    public void SetGoldWeight(long Weight)
    {
        long _weight = Weight - 1;
        rewards = rewards + rewards * _weight;
    }

    public void SetPosition(Vector2 position)
    {
        float ratio = Manager.Game.screenRatio;
        position.x -= ratio;
        position.y -= ratio;
        transform.position = position;

    }

    public void SetHpBar()
    {
        enemyView.SetHpBar(enemyBlueprint.EnemyType);
        enemyView.SetHealthBar(GetCurrentHpPercent(), currentHP, true);
    }

    //���� ������Ʈ Ǯ�� �� �ʱ�ȭ �� �� �ְ� �޼���
    private void ResetHealth()
    {
        currentHP = maxHp;
    }
    #endregion

    #region Unity Flow

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        enemyView = GetComponent<EnemyView>();
        camera_ = Camera.main;
    }

    private void Update()
    {
        Vector3 viewport = camera_.WorldToViewportPoint(transform.position);
        if ((0 < viewport.x & viewport.x < 1) && (0 < viewport.y & viewport.y < 1))
        {
            inViewport = true;
        }
        else
        {
            inViewport = false;
        }
    }

    private void FixedUpdate()
    {
        EvaluateState();
    }

    #endregion

    #region StateMethod
    //�÷��̾� �������� �̵�
    private void EvaluateState()
    {
        //��Ÿ����� �Ÿ��� �ְų� ����Ʈ�� ������ �ʾ�����
        if (range < Vector2.Distance(Manager.Game.player.gameObject.transform.position, transform.position) | !inViewport)
        {
            rigid.velocity = new Vector2(
                Manager.Game.player.gameObject.transform.position.x - transform.position.x,
                0f
                ).normalized * moveSpeed;
        }
        else
        {
            rigid.velocity = Vector2.zero;

            attackCoroutine ??= StartCoroutine(AttackRoutine());
        }
    }
    #endregion

    #region Attack Method
    //�߻�ü�� ���� �� �ʱ�ȭ
    private void CreateProjectail()
    {
        //Resources �������� EnemyProjectileFrame(�߻�ü Ʋ)�� �����ϰ� go�� �Ҵ����
        var go = Manager.ObjectPool.GetGo("EnemyProjectileFrame");
        go.transform.position = gameObject.transform.position;

        //�߻�ü �ʱ�ȭ�� ���� ������ �Ѱ���
        go.GetComponent<EnemyProjectileHandler>().SetProjectile(enemyBlueprint.ProjectailVFX, damage);
    }

    //�߻�ü ���� �ڷ�ƾ
    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / attackSpeed);
            CreateProjectail();
        }
    }

    public void FloatingDamage(Vector3 position, long damage, DamageType damageTypeValue)
    {
        GameObject DamageHUD = Manager.ObjectPool.GetGo("Canvas_FloatingDamage");
        DamageHUD.transform.position = gameObject.transform.position + position;
    }

    #endregion

    #region Health Method

    public void TakeDamage(long damage, DamageType damageTypeValue)
    {
        AmountDamage(damage);
        FloatingDamage(new Vector3(0, 0.05f, 0), damage, damageTypeValue);
        enemyView.SetHealthBar(GetCurrentHpPercent(), currentHP);
    }

    private float GetCurrentHpPercent()
    {
        return (float)currentHP / maxHp;
    }

    private void AmountDamage(long damage)
    {
        if (currentHP == 0)
        {
            return;
        }
        if (!inViewport)
        {
            return;
        }
        if (currentHP - damage <= 0)
        {
            currentHP = 0;
            Die();
        }
        else
        {
            currentHP -= damage;
        }
    }

    private void Die()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");

        attackCoroutine = null;
        enemyView.ClearHpBar();
        ReleaseObject();
    }

    #endregion
}
