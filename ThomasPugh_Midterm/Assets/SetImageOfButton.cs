using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonType { playerButton, hardnessButton };

public class SetImageOfButton : MonoBehaviour
{

    public GameObject gameManager;

    public ButtonType bt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameManagerScript gms = gameManager.GetComponent<GameManagerScript>();
        if (bt == ButtonType.playerButton)
            gameObject.GetComponent<RawImage>().texture = gms.playerObjects[gms.playerIndex].GetComponent<SpriteRenderer>().sprite.texture;
        else
            gameObject.GetComponent<RawImage>().texture = gms.difficultyIcons[gms.difficulty];
    }
}
