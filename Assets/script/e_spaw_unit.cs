using System.Collections;
using UnityEngine;

public class e_spaw_unit : MonoBehaviour
{
    //this script is for spawning unit for ai
    public GameObject spawn_object;
    public int unit_cost = 50;
    public GameObject spawnpoint;
    public int spawn_time = 10;
    public int when_to_spawn = 200; //from how much gold should the ai spawn unit
    public float decision_time = 10;

    private  float current_time;
    bool spawned = false;  //it is to avoid creating multiple units at the same time

    private void Awake()
    {
        current_time = decision_time;
    }
    private void Update()
    {
        decision();
        if(ennemy_infomation.Gold > when_to_spawn && !spawned)
        {
            StartCoroutine("spawn");
        }
    }
    IEnumerator spawn()
    {
        spawned = true;
        ennemy_infomation.Gold -= unit_cost;
        yield return new WaitForSeconds(spawn_time);
        Instantiate(spawn_object, spawnpoint.transform.position, spawn_object.transform.rotation);
    }
    void decision()
    {
        //this function is to let time to ai to gain gold before it uses all of it for units
        current_time -= Time.deltaTime;
        if(current_time < 0)
        {
            current_time = decision_time;
            spawned = false;
        }
    }
}
