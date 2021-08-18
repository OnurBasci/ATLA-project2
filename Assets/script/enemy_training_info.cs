using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_training_info : MonoBehaviour
{
    public int max_health = 100;

    GameObject health_bar;
    float bar_value;
    [HideInInspector]
    public int health = 100;

    private void Awake()
    {
        health = max_health;
    }
    private void Update()
    {
        scale_healthbar();
        
    }
    void scale_healthbar()
    {
        if (health_bar == null)
        {
            Transform hb = gameObject.transform.Find("health_bar").transform.Find("prgress_bar");  //2 for healthbar
            health_bar = hb.gameObject;
        }
        bar_value = (float)health / max_health;
        health_bar.transform.localScale = new Vector3(bar_value, 1, 1);
    }
    public void destruction()
    {
        Destroy(gameObject);
    }
}
