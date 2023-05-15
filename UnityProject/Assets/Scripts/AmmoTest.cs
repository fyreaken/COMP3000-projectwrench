using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTest : MonoBehaviour
{
    public GameObject ammocountText;
    public int ammo = 0;

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1)){
            ammo = ammo + 1;
        }
        ammocountText.GetComponent<TMPro.TextMeshProUGUI>().text = "" + ammo;
    }
}
