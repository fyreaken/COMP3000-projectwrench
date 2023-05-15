using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponSys : MonoBehaviour
{
    RigBuilder rigb;
    public GameObject Weapon_Rocket;

    // Start is called before the first frame update
    void Start()
    {
        rigb = GetComponent<RigBuilder>();
    }

    // Update is called once per frame
    void Update()
    {
        // press 1 - disable All Weapons and Rigs except Weapon_None
        // press [num] - disable All Weapons and Rigs other than [Insert Weapon] & Weapon_None
        // .. etc etc (weapon_none is default player pose)
            /*if (Input.GetKey(KeyCode.LeftShift)) {
                rigb.layers[2].active = false;
                Weapon_Rocket.SetActive(false);
            }
            else {
                rigb.layers[2].active = true;
                Weapon_Rocket.SetActive(true);
            } */
    }
}
