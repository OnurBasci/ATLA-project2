using System.Collections;
using UnityEngine;

public class character_abelities : MonoBehaviour
{
    //this variables is for warrior objects
    public GameObject attack_object;
    public Transform attack_position;
    public float view_radius = 20f;
    bool spawn = true;
    private bool can_attack = false;  //it is for distinguish this object from other warrior object
    private bool is_attacking = false;  //it is to avoid the changment of attacking object
    [HideInInspector]
    public GameObject to_attack_object;

    private void Update()
    {
        if(level_manager.clicked_object != null && (level_manager.clicked_object.layer == 10 || level_manager.clicked_object.layer == 11) && Input.GetMouseButtonUp(0))  //10 is for ennemy building
        {
            StartCoroutine("essai");
        }
        if (spawn && to_attack_object != null)
        {
            StartCoroutine("attack");
        }
        if(Input.GetMouseButtonUp(0) && level_manager.selected_objects.Contains(gameObject))
        {
            can_attack = false;
            is_attacking = false;
        }
    }
    IEnumerator attack()
    {
        if (can_attack && is_attacking)
        {
            spawn = false;
            Instantiate(attack_object, attack_position.position, Quaternion.identity, gameObject.transform);
            yield return new WaitForSeconds(1);
            spawn = true;
        }
    }
    IEnumerator essai()
    {
        yield return new WaitForSeconds(0.1f);
        if (level_manager.selected_objects.Contains(gameObject))
        {
            can_attack = true;
        }
        Collider[] hitcollider = Physics.OverlapSphere(gameObject.transform.position, view_radius);
        foreach (var collider in hitcollider)
        {
            if (level_manager.clicked_object == collider.gameObject && !is_attacking)
            {
                to_attack_object = collider.gameObject;
                is_attacking = true;
            }
        }
    }
}
