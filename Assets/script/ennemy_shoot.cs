using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ennemy_shoot : MonoBehaviour
{
    Vector3 direction;
    public float force = 100f;
    public int damage = 10;
    public int destroy_time = 5;
    private GameObject to_attack_object;

    void Start()
    {
        to_attack_object = gameObject.GetComponentInParent<e_warrior_movment>().to_attack_object;

        direction = (to_attack_object.transform.position - gameObject.transform.position).normalized;
        gameObject.GetComponent<Rigidbody>().AddForce(direction * force);
        StartCoroutine("destroy");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == 8)
        {
            //GameObject @object = collision.transform.Find("health_bar").gameObject;
            //collision.transform.Find("health_bar").gameObject.SetActive(true);
            //@object.SetActive(true);

            collision.gameObject.GetComponent<character_information>().health -= damage;
            if (collision.gameObject.GetComponent<character_information>().health <= 0)
            {
                collision.gameObject.GetComponent<character_information>().destruction();    //destruction methode of collision object
            }
            Destroy(gameObject);
        }
        else if(collision.collider.gameObject.layer == 9)
        {
            collision.gameObject.GetComponent<building_information>().current_health -= damage;
            if (collision.gameObject.GetComponent<building_information>().current_health <= 0)
            {
                collision.gameObject.GetComponent<building_information>().destruction();    //destruction methode of collision object
            }
            Destroy(gameObject);
        }
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(destroy_time);
        Destroy(gameObject);
    }
}
