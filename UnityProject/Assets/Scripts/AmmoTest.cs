using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTest : MonoBehaviour
{
    [SerializeField] public GameObject ammoText; //field for ammo Text gameobject
    private PlayerShoot _playerShoot; //playerShoot gameobject

    private void Start(){
        _playerShoot = GetComponent<PlayerShoot>();
    }       //gets playerShoot script on gameobject

    private void Update(){
        ammoText.GetComponent<TMPro.TextMeshProUGUI>().text = _playerShoot.ammo.ToString();
    }               //updates Player Kills text with Player KillAmount value
}
