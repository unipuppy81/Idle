using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UiTopScene : UiBase
{
    #region Serialize Fields

    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject playingPanel;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private GameObject completeBtn;
    [SerializeField] private TextMeshProUGUI assetDownloadTxt;
    [SerializeField] private GameObject sceneCloseBtn;

    #endregion

    #region Fields

    private Player player;

    #endregion

    #region Playing Init

    private void InitTopUI()
    {
        SetTexts();
        SetButtons();

        player = Manager.Game.player;
        Manager.UI.InitTop(this);
    }

    private void SetTexts()
    {
        SetUI<TextMeshProUGUI>();
    }

    private void SetButtons()
    {
        SetUI<Button>();
    }

    public void SetCloseButton(bool active)
    {
        sceneCloseBtn.SetActive(active);
    }

    #endregion

    #region Button Events

    public void OnGameStart()
    {
        Manager.Data.Load();
    }

    public void DataLoadFinished()
    {
        Manager.Game.Initialize();
        InitTopUI();

        loadingPanel.SetActive(false);
        playingPanel.SetActive(true);
    }

    #endregion

    #region Update UI

    public void UpdateLoading(int count, int totalCount)
    {
        loadingSlider.value = (float)count / totalCount;
        assetDownloadTxt.text = $"에셋 다운로드중... ({count}/{totalCount})";
    }

    public void UpdateLoadingComplete()
    {
        loadingSlider.gameObject.SetActive(false);
        completeBtn.SetActive(true);

        assetDownloadTxt.text = "로딩이 완료됐습니다.";
    }

    #endregion
}
