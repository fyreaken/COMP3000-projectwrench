using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour //script for buttons that disable/enable Main Menu elements
{
	[SerializeField] public GameObject MAIN_MENU;
    [SerializeField] public Button CREATE_GAME;
    [SerializeField] public GameObject OPTIONS_MENU;
    [SerializeField] public GameObject CONTROLS;
	[SerializeField] public GameObject UIRoomMenu_Obj;

    public void Play(){
        MAIN_MENU.SetActive(false);
        UIRoomMenu_Obj.SetActive(true);
    }

    public void Play_Back(){
        MAIN_MENU.SetActive(true);
        UIRoomMenu_Obj.SetActive(false);
    }

    public void Options(){
        MAIN_MENU.SetActive(false);
        OPTIONS_MENU.SetActive(true);
        CONTROLS.SetActive(true);
    }

    public void Options_Back(){
        MAIN_MENU.SetActive(true);
        OPTIONS_MENU.SetActive(false);
        CONTROLS.SetActive(false);
    }

    public void Quit(){ //quits application when Quit button is pressed
        Application.Quit();
    }
}
