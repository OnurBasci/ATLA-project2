using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    //this scripts spawns an given object on a given space  it is for spawn buttons
    public GameObject spawn_object;
    public GameObject spawnpoint;
    public int spawn_time = 10;
    public static bool spawned = false;

    //this script is to spawn buildings
    public void Spawn(GameObject s_o)
    {
        if(level_manager.Gold >= s_o.GetComponent<building_information>().required_gold)
        {
            StartCoroutine(actual_spawn(s_o));
        }
        else
        {
            Debug.Log("gereken altin sayisi: " + (s_o.GetComponent<building_information>().required_gold - level_manager.Gold));
        }
    }

    IEnumerator actual_spawn(GameObject s_o)
    {
        level_manager.Gold -= s_o.GetComponent<building_information>().required_gold;
        yield return new WaitForSeconds(spawn_time);
        Instantiate(s_o, spawnpoint.transform.position, s_o.transform.rotation);

        if (level_manager.clicked_button == "builder_button")
        {
            //this part is for builder and not other spawnable objects
            GameObject building_menu = GameObject.Find("b_buttons");
            if(building_menu != null)
            {
                building_menu.SetActive(false); 
            }

            GameObject create_button = GameObject.Find("create_button");
            create_button.GetComponent<ui_show_up>().enabled = true;
            create_button.GetComponent<ui_show_up>().object_to_follow = GameObject.Find("training area(Clone)");
        }
    }
}
