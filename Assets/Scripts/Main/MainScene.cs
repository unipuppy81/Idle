using UnityEngine;

public class MainScene : MonoBehaviour
{
    #region Fields

    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform[] enemySpawnPoint;
    private bool isLoadComplete = false;

    #endregion

    #region Unity Flow

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        Manager.Asset.LoadAllAsync((count, totalCount) =>
        {
            if (count >= totalCount)
            {
                isLoadComplete = true;
            }
        });
    }


    #endregion

    #region Scene Setting

    public void SceneStart()
    {
        Manager.Game.PlayerInit(playerSpawnPoint.position);
    }

    private Transform BossSpawnPointAdd()
    {
        var spawnPointTransform = this.transform.Find("Enemy Spawn Point");
        var bossSpawnPoint = Instantiate(new GameObject("Boss Spawn Point"), spawnPointTransform.position, Quaternion.identity);
        bossSpawnPoint.transform.position = new Vector2(3.5f, 2.0f);
        bossSpawnPoint.transform.parent = spawnPointTransform;

        return bossSpawnPoint.transform;
    }

    #endregion
}
