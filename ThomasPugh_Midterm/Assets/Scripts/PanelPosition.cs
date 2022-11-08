using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPosition : MonoBehaviour
{
    public int x;
    public int y;

    public RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform.anchoredPosition3D = new Vector3(Screen.width * (x) ,Screen.height * (-y), 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
