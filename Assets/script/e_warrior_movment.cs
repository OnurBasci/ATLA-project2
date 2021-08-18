using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e_warrior_movment : MonoBehaviour
{
    public float radius = 40;
    public float shoot_distance = 20;
    public GameObject attack_object;
    public GameObject spawn_point;
    public int shoot_time = 3;
    [HideInInspector]
    public GameObject to_attack_object;

    Vector3 dir = Vector3.forward;
    Vector3 go_to;
    bool decided = false;
    int decide_time = 5;
    bool attack_in = false;
    bool can_attack = true;

    private void Awake()
    {
        StartCoroutine("choose_direction");
    }
    private void Update()
    {
        if(to_attack_object == null || Vector3.Distance(transform.position, to_attack_object.transform.position) > shoot_distance)
        {
            move();
        }
        detect_colliders();
        if(!decided)
        {
            StartCoroutine("choose_direction");
        }
        if(can_attack && to_attack_object != null && Vector3.Distance(transform.position, to_attack_object.transform.position) < shoot_distance)
        {
            StartCoroutine("shoot");
        }
    }
    IEnumerator choose_direction()
    {
        if(to_attack_object == null)
        {
            decided = true;
            dir.x = Random.Range(-100, 100);
            dir.z = Random.Range(1, 100);
            dir.y = 0;
            go_to = transform.position + dir;
            yield return new WaitForSeconds(decide_time);
            decided = false;
        }
    }
    void move()
    {
        transform.Translate(dir.normalized * Time.deltaTime * gameObject.GetComponent<character_information>().speed, Space.World);
        Quaternion _lookrotation = Quaternion.LookRotation((go_to - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookrotation, Time.deltaTime * 10);
    }
    void detect_colliders()
    {
        Collider[] hitcollider = Physics.OverlapSphere(gameObject.transform.position, radius);
        attack_in = false;
        foreach(var collider in hitcollider)
        {
            if (collider.gameObject.layer == 8 || collider.gameObject.layer == 9)
            {
                if(to_attack_object == null)
                {
                    dir = collider.transform.position - gameObject.transform.position;
                    go_to = transform.position + dir;
                    to_attack_object = collider.gameObject;
                    return;
                }
                if(!attack_in && collider.gameObject == to_attack_object) 
                {
                    attack_in = true;
                }

            }
        }
        if(to_attack_object != null)  //follows always the player while in the area
        {
            dir = to_attack_object.transform.position - gameObject.transform.position;
            go_to = transform.position + dir;
        }
        if(!attack_in)
        {
            to_attack_object = null;
        }
    }
    IEnumerator shoot()
    {
        can_attack = false;
        Instantiate(attack_object, spawn_point.transform.position, Quaternion.identity, transform);
        yield return new WaitForSeconds(shoot_time);
        can_attack = true;
    }
}
