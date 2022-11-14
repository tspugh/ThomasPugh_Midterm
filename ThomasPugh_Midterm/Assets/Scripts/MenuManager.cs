using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum MenuType { StartMenu, Intermediate, MainGame }

public class MenuManager : MonoBehaviour
{

    public RectTransform menuThing;

    public float transitionTime = .75f;

    private Vector3 destination;

    private void Awake()
    {
        GameEvents.DamagableDestroyed += ToIntermediate;
    }

    void Start()
    {
        Debug.Log(Screen.width);
    }

    void ChangeMenuType(MenuType menu)
    {
        Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
        if (menu == MenuType.Intermediate)
            destination = new Vector3(-Screen.width, 0f, 0f);
        else if (menu == MenuType.MainGame)
            destination = new Vector3(-Screen.width, Screen.height, 0f);
        else
            destination = new Vector3(0f, 0f, 0f);

        StartCoroutine(MenuAnimation(destination));
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnStartButtonPressed()
    {
        ChangeMenuType(MenuType.MainGame);
        GameEvents.InvokeNewGameBegin(transitionTime + 1);
    }

    public void OnBeginButtonPressed()
    {
        ChangeMenuType(MenuType.Intermediate);
    }

    public void OnBackButtonPressed()
    {
        ChangeMenuType(MenuType.StartMenu);
    }


    public void ToIntermediate(object sender, DestructionArgs e)
    {
        if(e.destroyedGameObject.CompareTag("Player"))
            ChangeMenuType(MenuType.Intermediate);
    }

    private IEnumerator MenuAnimation(Vector3 newPos)
    {
        float time = 0f;
        Vector3 oldPosition = menuThing.anchoredPosition3D;

        while(time<=transitionTime)
        {
            time += Time.deltaTime;
            menuThing.anchoredPosition3D = Vector3.Lerp(oldPosition, newPos, time / transitionTime);
            yield return null;
        }

    }
}
