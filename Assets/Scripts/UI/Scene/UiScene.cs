using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiScene : UiBase
{
    protected override void Init()
    {
        base.Init();
        Manager.UI.SetCanvasScene(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Manager.UI.CheckPopupStack()) return;

            if (Manager.UI.CheckSceneStack())
            {
                Manager.UI.CloseSubScene();
                Manager.UI.Top.SetCloseButton(false);
            }
        }
    }

    private void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        Application.Quit();
#endif
    }
}
