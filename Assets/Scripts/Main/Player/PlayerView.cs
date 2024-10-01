using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    #region Serialize Fields

    [SerializeField] private Image hpBar;
    [SerializeField] private Canvas UICanvas;

    #endregion

    public void SetHealthBar(float currentHpPercent)
    {
        hpBar.fillAmount = Mathf.Clamp(currentHpPercent, 0, 1);
    }
}
