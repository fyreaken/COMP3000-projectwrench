using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayKills : MonoBehaviour
{
    [SerializeField] private GameObject killsText; //field for Player Kills gameobject
    private PlayerShoot _playerShoot; //playerShoot gameobject

    private void Start(){
        _playerShoot = GetComponent<PlayerShoot>();
    }       //gets playerShoot script on gameobject

    private void Update(){
        killsText.GetComponent<TMPro.TextMeshProUGUI>().text = _playerShoot.kills.ToString(); 
    }               //updates Player Kills text with Player KillAmount value
}
