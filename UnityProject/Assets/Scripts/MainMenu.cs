using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour //script for buttons that disable/enable Main Menu elements
{
	[SerializeField] public GameObject LOGO;
	[SerializeField] public GameObject PLAY;
    [SerializeField] public GameObject PLAY_BACK;
	[SerializeField] public GameObject OPTIONS;
    [SerializeField] public GameObject OPTIONS_LOGO;
    [SerializeField] public GameObject OPTIONS_BACK;
	[SerializeField] public GameObject QUIT;
    [SerializeField] public GameObject CONTROLS;
	[SerializeField] public GameObject UIRoomMenu_Obj;

    public void Play(){
        LOGO.SetActive(false);
        PLAY.SetActive(false);
        OPTIONS.SetActive(false);
        QUIT.SetActive(false);
        PLAY_BACK.SetActive(true);
        UIRoomMenu_Obj.SetActive(true);
    }

    public void Play_Back(){
        LOGO.SetActive(true);
        PLAY.SetActive(true);
        OPTIONS.SetActive(true);
        QUIT.SetActive(true);
        PLAY_BACK.SetActive(false);
        UIRoomMenu_Obj.SetActive(false);
    }

    public void Options(){
        LOGO.SetActive(false);
        PLAY.SetActive(false);
        OPTIONS.SetActive(false);
        QUIT.SetActive(false);
        OPTIONS_LOGO.SetActive(true);
        OPTIONS_BACK.SetActive(true);
        CONTROLS.SetActive(true);
    }

    public void Options_Back(){
        LOGO.SetActive(true);
        PLAY.SetActive(true);
        OPTIONS.SetActive(true);
        QUIT.SetActive(true);
        OPTIONS_LOGO.SetActive(false);
        OPTIONS_BACK.SetActive(false);
        CONTROLS.SetActive(false);
    }

    public void Quit(){ //quits application when Quit button is pressed
        Application.Quit();
    }
}
