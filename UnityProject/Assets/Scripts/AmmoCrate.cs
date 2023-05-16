using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class AmmoCrate : AttributesSync
{
    public PlayerShoot _playerShoot; //gets PlayerShoot component
    [SerializeField] private float respawnTime; // object respawn delay float
    public int ammoNum = 50;

    public GameObject rifleAmmo; //top half of Ammo Container

    void OnTriggerEnter(Collider other) //if player collides with AmmoContainer
    {
        if (other.gameObject.CompareTag("Player")){ //non functional
            PlayerShoot ps = other.GetComponentInChildren<PlayerShoot>();
            ps.ammo = ps.ammo += ammoNum;
        } else if (other.gameObject.CompareTag("PlayerSelf")){ //non functional
            PlayerShoot ps = other.GetComponentInChildren<PlayerShoot>();
            ps.ammo = ps.ammo += ammoNum;
        }
        BroadcastRemoteMethod("RemoveObject"); //disables object in session
        StartCoroutine(Wait()); //starts respawn delay
    }

    IEnumerator Wait() { //function called by coroutine
        yield return new WaitForSeconds(respawnTime); //waits amount of seconds equal to respawnTime float
        BroadcastRemoteMethod("AddObject"); //enables object in session
    }

    [SynchronizableMethod]
    void RemoveObject(){ //disables object
        rifleAmmo.gameObject.SetActive(false);
    }

    [SynchronizableMethod]
    void AddObject(){ //enables object
        rifleAmmo.gameObject.SetActive(true);
    }
}
