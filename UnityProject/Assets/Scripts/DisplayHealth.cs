using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayHealth : MonoBehaviour
{
    [SerializeField] private GameObject healthText; //field for Player Health gameobject
    private PlayerShoot _playerShoot; //playerShoot gameobject

    private void Start(){
        _playerShoot = GetComponent<PlayerShoot>(); //gets playerShoot script on gameobject
    }

    private void Update(){
        healthText.GetComponent<TMPro.TextMeshProUGUI>().text = _playerShoot.health.ToString(); //updates Player Health text with Player Health value
    }
}
