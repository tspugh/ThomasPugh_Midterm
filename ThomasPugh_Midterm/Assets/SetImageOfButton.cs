using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetImageOfButton : MonoBehaviour
{

    public GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameManagerScript gms = gameManager.GetComponent<GameManagerScript>();
        gameObject.GetComponent<RawImage>().texture = gms.playerObjects[gms.playerIndex].GetComponent<SpriteRenderer>().sprite.texture;
    }
}
