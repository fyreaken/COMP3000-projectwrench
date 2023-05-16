using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    private Alteruna.Avatar _avatar;
    private Spawner _spawner;

    [SerializeField] private int indexToSpawn = 0; //0-3 - objects in spawner index
    [SerializeField] private LayerMask despawnLayer; //layer used for despawning

    [SerializeField] private float spawnRange = 0f; //customizable range for object spawning

    [SerializeField] private GameObject shapeField; //shape type text field ("Cube", "Wall", etc..)


    private void Awake(){
        _avatar = GetComponent<Alteruna.Avatar>(); //Alteruna Avatar component
        _spawner = GameObject.Find("NetworkManager").GetComponent<Spawner>(); //Alteruna Spawner component
    }

    private void Update(){
        if (!_avatar.IsMe) //ownership check
            return;

        if (Input.GetKeyDown(KeyCode.F)){ //if key pressed
            SpawnCube(); //spawns object
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)){ //if key pressed
            DespawnCube(); //despawns object
        }

        if (Input.GetKeyDown(KeyCode.R)){ //if key pressed, add 1 to indexToSpawn
            indexToSpawn = indexToSpawn + 1;
            if (indexToSpawn == 1){ //put "Slope" in shape ui text field
                shapeField.GetComponent<TMPro.TextMeshProUGUI>().text = "Slope";
            }
            if (indexToSpawn == 2){ //put "Floor" in shape ui text field
                shapeField.GetComponent<TMPro.TextMeshProUGUI>().text = "Floor";
            }
            if (indexToSpawn == 3){ //put "Wall" in shape ui text field
                shapeField.GetComponent<TMPro.TextMeshProUGUI>().text = "Wall";
            }
            if (indexToSpawn == 4){
                indexToSpawn = 0; //reset indexToSpawn to 0 if outside of usable index
                shapeField.GetComponent<TMPro.TextMeshProUGUI>().text = "Cube"; 
            }           //put "Cube" in shape ui text field
        }
    }

    void SpawnCube(){
        if (indexToSpawn == 0){ //spawns a cube, discards rotation
            _spawner.Spawn(indexToSpawn, Camera.main.transform.position + Camera.main.transform.forward * spawnRange, Camera.main.transform.rotation = Quaternion.identity, new Vector3(0.5f,0.5f,0.5f));
        }
        if (indexToSpawn == 1){ //spawns a slope at an angle
            _spawner.Spawn(indexToSpawn, Camera.main.transform.position + Camera.main.transform.forward * spawnRange, Camera.main.transform.rotation * Quaternion.Euler(35,0,0), new Vector3(4f,4f,0.5f));
        }
        if (indexToSpawn == 2){ //spawns a floor, discards rotation
            _spawner.Spawn(indexToSpawn, Camera.main.transform.position + Camera.main.transform.forward * spawnRange, Camera.main.transform.rotation = Quaternion.identity, new Vector3(4f,0.5f,4f));
        }
        if (indexToSpawn == 3){ //spawns a wall
            _spawner.Spawn(indexToSpawn, Camera.main.transform.position + Camera.main.transform.forward * spawnRange, Camera.main.transform.rotation, new Vector3(4f,4f,0.5f));
        }
    }

    void DespawnCube(){ //when raycast hits spawned object, remove it
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity, despawnLayer)){
            _spawner.Despawn(hit.transform.gameObject);
        }   //raycast works same way as Player's weapon
    }
}
