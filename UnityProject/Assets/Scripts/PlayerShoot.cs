using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class PlayerShoot : AttributesSync //script gets synchronized over a online session
{
    [SynchronizableField] public int health = 100; //player health
    [SerializeField] private int damage = 10; //damage value
    [SynchronizableField] public int ammo = 50;
    [SerializeField] private int ammoSpent = 2;

    public int kills = 0;
    public Alteruna.Avatar _avatar; //multiplayer api avatar component
    Camera mainCamera;

    public GameObject SpawnPoint; //empty field
    public GameObject DeathPoint; //empty field
    //public GameObject PlayerModel; //empty field

    [SerializeField] private float respawnTime; // respawn delay float
    private bool isDead;

    [SerializeField] private LayerMask playerLayer; // Player Tag
    [SerializeField] private int playerSelfLayer; // playerSelf Tag
    public ParticleSystem[] muzzleFlash; //particle system

    //public List<Material> PlayerMaterials;

    private void Start(){
        _avatar = GetComponentInParent<Alteruna.Avatar>(); //gets Alteruna avatar component

        if (_avatar.IsMe){ //if the Player is not owned by the User, return, don't run remaining script
            _avatar.gameObject.layer = playerSelfLayer; //sets playerSelf Tag to Player object if User owns it

            SpawnPoint = GameObject.Find("SpawnPoint"); //assigns SpawnPoint object to a Player (Player is moved to SpawnPoint after they've been moved to DeathPoint)
            DeathPoint = GameObject.Find("DeathPoint"); //assigns DeathPoint object to a Player (Player is moved to DeathPoint when they die)
            //PlayerModel = GameObject.FindGameObjectWithTag("Character 07");

            mainCamera = Camera.main;
            //BroadcastRemoteMethod("RandomMaterial");
        }
    }

    private void Update(){

        if (!_avatar.IsMe)
            return; //ownership check, if Avatar is not owned by User do not run script

        if (Input.GetKeyDown(KeyCode.Mouse0)){ //if Left Mouse (Mouse 0) button is pressed
            if (ammo > 0) { //if ammmo = 0, player cannot shoot
                Shoot(); //shoots
                BroadcastRemoteMethod("MuzzleFlash"); //spawns particles when player shoots
                ammo = ammo -= ammoSpent; //whenever player shoots, remove 2 from ammo count
            }
        }
    }

    void Shoot(){ //raycast from player object's Third Person Camera to anything it hits
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, Mathf.Infinity, playerLayer)){
            PlayerShoot playerShoot = hit.transform.GetComponentInChildren<PlayerShoot>(); //gets other player's PlayerShoot.cs script component
            playerShoot.Hit(damage); //deals damage variable value to other player
        }
    }

    public void Hit(int damageTaken){
        health -= damageTaken; // takes damage value away from player's health value

        if (health <= 0){ //runs player death method across online session if player health = 0
            BroadcastRemoteMethod("Die");
        }
    }

    [SynchronizableMethod]
    void Die(){ //player death method
        //Debug.Log("Player Died"); //when player dies, they are moved to the DeathPoint. (player died)
        _avatar.gameObject.transform.position = DeathPoint.gameObject.transform.position;

        StartCoroutine(Delay()); //starts respawn timer (5s), respawns player when timer = 0
    }

    [SynchronizableMethod]
    void Respawn(){ //player respawn method
        //Debug.Log("Player Respawned"); //when player respawns, they are given full health
        health = 100;                  //and moved to SpawnPoint (player respawned)

        int randomChild = Random.Range(0,4); //picks number between 0 and 4

        _avatar.gameObject.transform.position = SpawnPoint.transform.GetChild(randomChild).gameObject.transform.position;
        //moves player to position of child object in SpawnPoint, based on randomChild number which is equal to
                                                                //the position of the child object in the editor's hierarchy
                // SpawnPoint (game object)
                //      SpawnPoint1 (child object)
                //      SpawnPoint2         //^^ int randomChild = 0, so SpawnPoint1 is picked for player to move to
                //      SpawnPoint3
                //      SpawnPoint4
                //      SpawnPoint5
    }

    IEnumerator Delay() { //function called by coroutine
        yield return new WaitForSeconds(respawnTime); //waits amount of seconds equal to respawnTime float
        BroadcastRemoteMethod("Respawn"); //broadcasts respawn method
    }

    [SynchronizableMethod]
    void MuzzleFlash(){
        foreach(var particle in muzzleFlash){ //handles particles
            particle.Emit(1); //creates muzzleflash particle
        }
    }

    //[SynchronizableMethod]
    //void RandomMaterial(){
    //    if (PlayerMaterials.Count > 0){
    //        PlayerModel.GetComponent<Renderer>();
    //        Material m = PlayerMaterials[Random.Range(0, PlayerMaterials.Count)];
    //        PlayerModel.GetComponent<Renderer>().material = m;
    //    }
    //}
}
