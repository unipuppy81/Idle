using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.AddressableAssets.Build.Layout.BuildLayout;

public class DataManager
{
    #region Fields

    private UiTopScene topScene;

    #endregion

    #region Load

    public void Load()
    {
        topScene = GameObject.FindObjectOfType<UiTopScene>();
        topScene.DataLoadFinished();
    }
    #endregion
}
