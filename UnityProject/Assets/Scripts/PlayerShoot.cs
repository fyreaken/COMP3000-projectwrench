using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class PlayerShoot : AttributesSync //script gets synchronized over a online session
{
    [SynchronizableField] public int health = 100; //player health
    [SerializeField] private int damage = 10; //damage value
    public Alteruna.Avatar _avatar; //multiplayer api avatar component
    Camera mainCamera;

    public GameObject SpawnPoint;
    public GameObject DeathPoint;

    [SerializeField] private LayerMask playerLayer; // Player Tag
    [SerializeField] private int playerSelfLayer; // playerSelf Tag

    private void Start(){
        _avatar = GetComponentInParent<Alteruna.Avatar>(); //gets Alteruna avatar component

        if (_avatar.IsMe){ //if the Player is not owned by the User, return, don't run remaining script
            _avatar.gameObject.layer = playerSelfLayer; //sets playerSelf Tag to Player object if User owns it

            SpawnPoint = GameObject.Find("SpawnPoint"); //assigns SpawnPoint object to a Player (Player is moved to SpawnPoint after they've been moved to DeathPoint)
            DeathPoint = GameObject.Find("DeathPoint"); //assigns DeathPoint object to a Player (Player is moved to DeathPoint when they die)

            mainCamera = Camera.main;
        }
    }

    private void Update(){
        if (!_avatar.IsMe) //if the Player is not owned by the User, return, don't run remaining script
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0)){ //if Left Mouse (Mouse 0) button is pressed
            //Debug.Log("Mouse 0");
            Shoot(); //shoots
        }
    }

    void Shoot(){ //raycast from Player object's Third Person Camera to anything it hits
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, Mathf.Infinity, playerLayer)){
            PlayerShoot playerShoot = hit.transform.GetComponentInChildren<PlayerShoot>(); //gets other player's PlayerShoot.cs script component
            playerShoot.Hit(damage); //deals damage variable value to other player
        }
        Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward, Color.yellow); //debug for raycast
    }

    public void Hit(int damageTaken){
        health -= damageTaken; // takes damage value away from player's health value

        if (health <= 0){ //executes Die function across online session if a Player's health reaches 0
            BroadcastRemoteMethod("Die");
        }
    }

    [SynchronizableMethod]
    void Die(){ //Die function (incomplete)
        Debug.Log("Player Died"); //when player dies, they are moved to the DeathPoint. (player died)
                                    //their health is reset then (health reset)
                                    //they are moved to the SpawnPoint (respawned)
        _avatar.gameObject.transform.position = DeathPoint.gameObject.transform.position;
        health = 100;
        // enable RESPAWN UI element
        // if ( health == 0 ) {
        //    [uielement] .SetActive(! [uielement] .activeSelf
        //  https://www.techwithsach.com/post/how-to-add-a-simple-countdown-timer-in-unity
        //} https://gamedevbeginner.com/how-to-delay-a-function-in-unity/
        // 
        _avatar.gameObject.transform.position = SpawnPoint.gameObject.transform.position;
    }
}
