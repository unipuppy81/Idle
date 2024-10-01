using UnityEngine;

public class GameManager
{
    #region Properties

    public Player player { get; private set; }
    public MainScene main { get; private set; }

    public float screenRatio { get; private set; } = Screen.height / Screen.width * 0.1f;

    #endregion

    // 화면 비율 컨트롤

    #region Init

    /// <summary>
    /// 데이터 동기화 후 => 게임 세팅
    /// </summary>
    public void Initialize()
    {
        var playerClone = Manager.Asset.InstantiatePrefab("PlayerFrame");
        player = playerClone.GetComponent<Player>();

        main = GameObject.FindObjectOfType<MainScene>();
        main.SceneStart();
    }

    public void PlayerInit(Vector2 position)
    {
        Vector2 positionRatio = new Vector2(screenRatio, -screenRatio);
        Vector2 scaleRatio = new Vector2(screenRatio, screenRatio);

        player.transform.position = position + positionRatio;
        player.transform.localScale = Vector2.one - scaleRatio;
        player.Initialize();
    }

    #endregion
}
