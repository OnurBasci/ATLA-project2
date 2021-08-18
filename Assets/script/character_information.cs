using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_information : MonoBehaviour
{
    //this script is for some variables
    public float speed = 10f;
    public int health = 100;

    public void destruction()
    {
        Destroy(gameObject);  //used in shoot script
    }
}
