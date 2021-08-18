using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaw_unit : MonoBehaviour
{
    //this scripts spawns an given object on a given space  it is for spawn buildings
    public GameObject spawn_object;
    public int unit_cost = 50;
    public GameObject spawnpoint;
    public int spawn_time = 10;
    bool spawned = true;  //it is to avoid creating multiple units at the same time


    public void Spawn(GameObject s_o)
    {
        //this script is for spawn units
        if (level_manager.Gold >= unit_cost && spawned)
        {
            StartCoroutine(actual_spawn(s_o));
        }
        else
        {
            Debug.Log("gereken altin sayisi: " + (unit_cost - level_manager.Gold));
        }
    }

    IEnumerator actual_spawn(GameObject s_o)
    {
        spawned = false;
        level_manager.Gold -= unit_cost;
        yield return new WaitForSeconds(spawn_time);
        Instantiate(s_o, spawnpoint.transform.position, s_o.transform.rotation);
        spawned = true;
        //level_manager.Gold -= s_o.GetComponent<building_information>().required_gold;
    }
}
