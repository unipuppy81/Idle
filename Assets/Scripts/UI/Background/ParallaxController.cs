using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    #region Serialize Fields

    [SerializeField] private float[] layerSpeed = new float[6];
    [SerializeField] private GameObject[] layerObjects = new GameObject[6];

    #endregion

    #region Properties

    public float[] layer_Speed => layerSpeed;
    public GameObject[] layer_Objects => layerObjects;

    #endregion

    public void LayerMove()
    {
        for (int i = 1; i < layerObjects.Length; i++)
        {
            layerObjects[i].transform.GetChild(0).position += Vector3.left * Time.deltaTime * layerSpeed[i];
            layerObjects[i].transform.GetChild(1).position += Vector3.left * Time.deltaTime * layerSpeed[i];

            if (layerObjects[i].transform.GetChild(0).localPosition.x <= -40.0f)
            {
                layerObjects[i].transform.GetChild(0).localPosition = new Vector2(layerObjects[i].transform.GetChild(0).localPosition.x + 80.0f, 0);
            }

            if (layerObjects[i].transform.GetChild(1).localPosition.x <= -40.0f)
            {
                layerObjects[i].transform.GetChild(1).localPosition = new Vector2(layerObjects[i].transform.GetChild(1).localPosition.x + 80.0f, 0);
            }
        }
    }
}
