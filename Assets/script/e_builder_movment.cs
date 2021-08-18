using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_builder_movment : MonoBehaviour
{
    public GameObject[] buildings;
    public Transform spawn_pos;
    int decide_time = 3;
    bool decided = true;
    Vector3 dir = Vector3.zero;
    Vector3 go_to;
    bool triggered = false;
    bool constructed = false;

    void Update()
    {
        if(decided)
        {
            StartCoroutine("decide");
        }
        move();
    }
    IEnumerator decide()
    {
        decided = false;
        int random = Random.Range(0, 2);
        yield return new WaitForSeconds(decide_time);
        switch(random)
        {
            case (0):
                chose_direction();
                break;
            case (1):
                construct();
                break;
        }

        decided = true;
    }
    void chose_direction()
    {
        dir.x = Random.Range(-100, 100);
        dir.z = Random.Range(-100, 100);
        dir.y = 0;
        go_to = transform.position + dir;
        triggered = false;
        constructed = false;
    }
    void move()
    {
        transform.Translate(dir.normalized * Time.deltaTime * gameObject.GetComponent<character_information>().speed, Space.World);
        Quaternion _lookrotation = Quaternion.LookRotation((go_to - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookrotation, Time.deltaTime * 10);
    }
    void construct()
    {
        int probability = 50;
        if(ennemy_infomation.Gold < 500)
        {
            probability = 80;  //if the ennemy does not have a lot of money it prefers farms
        }
        else
        {
            probability = 20;
        }
        if(!triggered && !constructed)
        {
            int random = Random.Range(0, 100);
            if(random > probability && ennemy_infomation.Gold > buildings[0].GetComponent<building_information>().required_gold)
            {
                Instantiate(buildings[0], spawn_pos.position, Quaternion.identity); //training area
                ennemy_infomation.Gold -= buildings[0].GetComponent<building_information>().required_gold;
            }
            else if(ennemy_infomation.Gold > buildings[1].GetComponent<building_information>().required_gold)
            {
                Instantiate(buildings[1], spawn_pos.position, Quaternion.identity); //farm
                ennemy_infomation.Gold -= buildings[1].GetComponent<building_information>().required_gold;
            }
            triggered = true;
            constructed = true;
            dir = Vector3.zero;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        triggered = true;
    }
}
