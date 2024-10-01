using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPopup : UiBase
{
    protected override void Init()
    {
        base.Init();
        Manager.UI.SetCanvasPopup(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Manager.UI.ClosePopup();
        }
    }
}
