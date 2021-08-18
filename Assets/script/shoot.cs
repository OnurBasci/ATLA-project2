using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    Vector3 direction;
    public float force = 100f;
    public int damage = 10;
    public int destroy_time = 5;
    private void Start()
    {
        direction = (gameObject.GetComponentInParent<character_abelities>().to_attack_object.transform.position - gameObject.transform.position).normalized;
        gameObject.GetComponent<Rigidbody>().AddForce(direction * force);
        StartCoroutine("destroy");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == 10)
        {
            collision.gameObject.GetComponent<enemy_training_info>().health -= damage;
            if(collision.gameObject.GetComponent<enemy_training_info>().health <=0)
            {
                collision.gameObject.GetComponent<enemy_training_info>().destruction();    //destruction methode of collision object
            }
            Destroy(gameObject);
        }
        else if(collision.collider.gameObject.layer == 11)
        {
            collision.gameObject.GetComponent<character_information>().health -= damage;
            if (collision.gameObject.GetComponent<character_information>().health <= 0)
            {
                collision.gameObject.GetComponent<character_information>().destruction();    //destruction methode of collision object
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
