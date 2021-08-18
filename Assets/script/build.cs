using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class build : MonoBehaviour
{
    //this methode is for building mechanics of builder unit
    public GameObject[] buildings;
    public GameObject building_pos;
    public GameObject building_menu;
    public GameObject button_type;
    public GameObject[] buttons = new GameObject[10];

    private void Awake()
    {
        building_menu = GameObject.Find("b_buttons");
        buttons[0] = GameObject.Find("train_area_button");
        buttons[1] = GameObject.Find("farm_build");
    }
    private void Start()
    {
        if (building_menu != null)
        {
            building_menu.SetActive(false);  //i desactivate the building menu because if it is not active unity can not find it
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown("e") && level_manager.selected_object == gameObject)
        {
            create_menu();
        }
        if(level_manager.selected_object != null && level_manager.selected_object.tag != "builder")
        {
            building_menu.SetActive(false);
        }
        change_listener();
    }
    public void create_menu()
    {
        building_menu.SetActive(true);
        level_manager.clicked_button = "builder_button";
    }

    private void change_listener()
    {
        if(level_manager.selected_object == gameObject && Input.GetMouseButtonDown(0))
        {
            buttons[0].GetComponent<Button>().onClick.RemoveAllListeners();
            buttons[1].GetComponent<Button>().onClick.RemoveAllListeners();
            buttons[0].GetComponent<Button>().onClick.AddListener(delegate { gameObject.GetComponent<spawn>().Spawn(buildings[0]); });
            buttons[1].GetComponent<Button>().onClick.AddListener(delegate { gameObject.GetComponent<spawn>().Spawn(buildings[1]); });
            //actually this part is really interesting if i put this code in to a for loop or while loop it does not work
            //i guess the mousebuttondown condition does not permet to loops

            /*for(int i = 0; i < 2; i++)
            {
                buttons[i].GetComponent<Button>().onClick.AddListener(delegate { gameObject.GetComponent<spawn>().Spawn(buildings[i]); });
            }*/
        }
    }
}
