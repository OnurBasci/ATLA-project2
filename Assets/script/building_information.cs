using UnityEngine;

public class building_information : MonoBehaviour
{
    //all information about building type, modifiable for each object

    public GameObject UI_information;
    public GameObject health_bar;
    public int required_gold = 100;
    public int max_health = 400;

    [HideInInspector]
    public int current_health;

    float bar_value;
    GameObject progress_bar;

    private void Awake()
    {
        UI_information = GameObject.Find("create_button");
        current_health = max_health;
    }

    private void Update()
    {
        if(level_manager.clicked_object == gameObject)
        {
            health_bar.SetActive(true);
        }
        else
        {
            health_bar.SetActive(false);
        }
        scale_healthbar();
    }
    void scale_healthbar()
    {
        if (progress_bar == null)
        {
            Transform hb = gameObject.transform.Find("health_bar").transform.Find("prgress_bar");  //2 for healthbar
            progress_bar = hb.gameObject;
        }
        bar_value = (float)current_health / max_health;
        progress_bar.transform.localScale = new Vector3(bar_value, 1, 1);
    }

    public void destruction()
    {
        Destroy(gameObject);
    }
}
