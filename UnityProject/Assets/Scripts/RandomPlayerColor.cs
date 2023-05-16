using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RandomPlayerColor : MonoBehaviour
{
    //private Alteruna.Avatar _avatar; //multiplayer api avatar component

    public List<Material> PlayerMaterials;

    void Start(){
        //_avatar = GetComponent<Alteruna.Avatar>(); //gets Alteruna avatar component

        //if (!_avatar.IsMe) //if Avatar is not owned by User
        //    return;             //do not run script

        if (PlayerMaterials.Count > 0){
            Material m = PlayerMaterials[Random.Range(0, PlayerMaterials.Count)];

            GetComponent<Renderer>().material = m;
        }
    }
}
