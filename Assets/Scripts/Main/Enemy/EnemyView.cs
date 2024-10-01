using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class EnemyView : UiBase
{
    #region Serialize Fields

    [SerializeField] private Canvas UiCanvas;

    #endregion

    #region Fields

    private Image hpBackGround;
    private Image hpBar;
    private Image hpBarDamage;
    private TextMeshProUGUI hpBarText;

    private Vector2 normalSize = new Vector2(0.6f, 0.1f);
    private Vector2 bossSize = new Vector2(0.8f, 0.2f);

    private Coroutine damageEffectCoroutine;
    private Coroutine onHpBarCoroutine;

    private bool isDead = false;
    private float decreasePerFrame;
    private float timer;

    #endregion

    #region Properties

    private float fillAmountDifference => (hpBarDamage.fillAmount - hpBar.fillAmount) / 30.0f;

    #endregion

    #region Initialize

    public void SetHpBar(EnemyType enemyType)
    {
        if (enemyType == EnemyType.Normal)
        {
            hpBackGround.rectTransform.sizeDelta = normalSize;
            UiCanvas.enabled = false;
            hpBarText.enabled = false;
        }
        if (enemyType == EnemyType.Boss)
        {
            hpBackGround.rectTransform.sizeDelta = bossSize;
            UiCanvas.enabled = true;
            hpBarText.enabled = true;
        }

        isDead = false;
    }

    #endregion

    #region Unity Flow

    private void Awake()
    {
        SetUI<Image>();
        hpBackGround = GetUI<Image>("HpBackGround");
        hpBar = GetUI<Image>("HpBar");
        hpBarDamage = GetUI<Image>("HpBarDamage");

        SetUI<TextMeshProUGUI>();
        hpBarText = GetUI<TextMeshProUGUI>("CurrentHP");
    }

    #endregion

    #region Event Method

    public void SetHealthBar(float currentHpPercent, long currentHp, bool isInit = false)
    {
        hpBar.fillAmount = Mathf.Clamp(currentHpPercent, 0, 1);
        hpBarText.text = Utilities.ConvertToString(currentHp);

        if (isInit) return;

        if (!UiCanvas.enabled)
            UiCanvas.enabled = true;

        if (!isDead)
        {
            if (fillAmountDifference > decreasePerFrame)
                decreasePerFrame = fillAmountDifference;

            timer = 4.0f;

            if (damageEffectCoroutine == null)
            {
                damageEffectCoroutine = StartCoroutine(DamageEffect());
            }

            if (onHpBarCoroutine == null)
            {
                onHpBarCoroutine = StartCoroutine(OnHpBar());
            }
        }
    }

    public void ClearHpBar()
    {
        UiCanvas.enabled = false;
        hpBar.fillAmount = 1;
        hpBarDamage.fillAmount = 1;

        if (damageEffectCoroutine != null)
        {
            StopCoroutine(damageEffectCoroutine);
            damageEffectCoroutine = null;
        }

        if (onHpBarCoroutine != null)
        {
            StopCoroutine(onHpBarCoroutine);
            onHpBarCoroutine = null;
        }

        decreasePerFrame = 0f;
        timer = 0f;

        isDead = true;
    }

    #endregion

    #region Event Coroutine

    IEnumerator OnHpBar()
    {
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        UiCanvas.enabled = false;
        onHpBarCoroutine = null;
        yield break;
    }

    IEnumerator DamageEffect()
    {
        while (hpBar.fillAmount + decreasePerFrame < hpBarDamage.fillAmount)
        {
            hpBarDamage.fillAmount -= decreasePerFrame;
            yield return null;
        }

        if (hpBar.fillAmount < hpBarDamage.fillAmount)
        {
            hpBarDamage.fillAmount = hpBar.fillAmount;
        }
        decreasePerFrame = 0f;
        damageEffectCoroutine = null;
        yield break;
    }

    #endregion
}
