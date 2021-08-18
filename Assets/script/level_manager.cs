using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class level_manager : MonoBehaviour
{
    /*
     this script send a ray from the camera to the plane if it
     detects something it returns the position of the intersection point.
     */

    public Button spawn_button;

    public static int Gold = 1000;
    public static GameObject selected_object;
    public static List<GameObject> selected_objects = new List<GameObject>();
    public LayerMask ignored_object;
    public GameObject selected_area_object;
    //this list is for create selected objects groupe to make them going to their direction while the user is using other objects
    public IDictionary<List<GameObject>, Vector3> objects_directions = new Dictionary<List<GameObject>, Vector3>();

    bool building_clicked = false;
    GameObject UI;
    public static GameObject clicked_object;  //this is a static object for using in diffrent scripts too
    private GameObject problem_solving_clicked_object;  //this is for bulding layer problem
    public static Vector3 go_to;  //vector wich defines the direction that the selected obhject go to
    public static string clicked_button = "";  //i put variables wich i use in diffrent scripts in this script -->build, spawn
    bool arrived = true;
    Vector3 distance;
    private int ray_id = -1; //for ignoring object elements
    private int turning_speed = 5;
    Vector3 start_point;
    Vector3 end_point;
    GameObject area;
    Vector3 area_start;
    Vector3 area_end;
    bool area_created = true;

    private void Awake()
    {
#if !UNITY_EDITOR
        ray_id = 0;
#endif

    }
    void Update()
    {
        select_objects_in_box();
        detect_area();
        //delete_clon();
        if (EventSystem.current.IsPointerOverGameObject(ray_id))
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                clicked_object = hit.transform.gameObject;

                //if ray touches to the area it makes go the selectend object
                if (hit.transform.name == "area")
                {
                    if (selected_objects.Count > 0)
                    {
                        go_to = hit.point;
                        go_to.y = selected_objects[0].transform.position.y;
                        if(objects_directions.ContainsKey(selected_objects))
                        {
                            //associating the groupe with its direction to go
                            objects_directions[selected_objects] = go_to;
                        }
                        arrived = false;
                    }
                    if (UI != null)
                    {
                        UI.GetComponent<Image>().enabled = false;
                    }
                    building_clicked = false;
                    spawn_button.onClick.RemoveAllListeners();
                }
                //else chose the object
                else if(hit.transform.gameObject.layer == 8)  //layer 8 is for selectable objects
                {
                    selected_object = hit.transform.gameObject;
                    building_clicked = false;
                    if(UI != null)
                    {
                        UI.GetComponent<Image>().enabled = false;
                    }
                    spawn_button.onClick.RemoveAllListeners();
                }
                else if(hit.transform.gameObject.layer == 9)
                {
                    selected_object = null;
                    selected_objects = new List<GameObject>();
                    if (!building_clicked)
                    {
                        if(hit.transform.gameObject.tag == "training_area") 
                        {
                            problem_solving_clicked_object = hit.transform.gameObject;
                            //instintiates buildings ui information
                            GameObject canvas = GameObject.Find("Canvas");
                            UI = hit.transform.gameObject.GetComponent<building_information>().UI_information;  //i reach to the ui object associated to the building object
                            UI.GetComponent<Image>().enabled = true;
                            UI.GetComponent<ui_show_up>().object_to_follow = hit.transform.gameObject;

                            spawn_button.onClick.AddListener(delegate {problem_solving_clicked_object.GetComponent<Spaw_unit>().Spawn(problem_solving_clicked_object.GetComponent<Spaw_unit>().spawn_object);});
                            clicked_button = "spawn_button";
                            building_clicked = true;
                        }

                    }
                    else if (hit.transform.gameObject != problem_solving_clicked_object)
                    {
                        UI.GetComponent<Image>().enabled = false;
                        building_clicked = false;
                        spawn_button.onClick.RemoveAllListeners();
                    }

                }
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            //if right clicked it stops moving and initalise selected object
            arrived = true;
            selected_object = null;
            selected_objects = new List<GameObject>();
            objects_directions = new Dictionary<List<GameObject>, Vector3>();

            if(UI != null)
            {
                UI.GetComponent<Image>().enabled = false;
            }

            GameObject building_menu = GameObject.Find("b_buttons");
            if (building_menu != null)
            {
                building_menu.SetActive(false);
            }
        }
        if(!arrived && go_to != null && selected_objects.Count > 0)
        {
            move_object();
        }
    }

    void move_object()
    {
        List<GameObject> @objects = new List<GameObject>();  //this list is to make move the object just one time for loop
        //i serch ewery key of the dictionnery and make go the keys to the associated vector3
        foreach (List<GameObject> list in objects_directions.Keys)
        {
            foreach(GameObject selected in list)
            {
                if (Vector3.Distance(selected.transform.position, objects_directions[list]) < 1)
                {
                    if(selected_objects != list)
                    {
                        objects_directions.Remove(list);
                    }
                    if (objects_directions.Count <= 0)
                    {
                        arrived = true;
                        return;
                    }
                }
                else
                {
                    if(!@objects.Contains(selected))
                    {
                        //translate
                        Vector3 difference = objects_directions[list] - selected.transform.position;
                        difference = difference.normalized;
                        selected.transform.Translate(difference * Time.deltaTime * selected.GetComponent<character_information>().speed);
                        //rotate
                        Transform object_model = selected.transform.GetChild(0); //first child is the model of the object
                        Quaternion _lookrotation = Quaternion.LookRotation((objects_directions[list] - object_model.position).normalized);
                        object_model.rotation = Quaternion.Slerp(object_model.transform.rotation, _lookrotation, Time.deltaTime * turning_speed);
                    }
                    @objects.Add(selected);
                }
            }
        }
        @objects.Clear();
        /*foreach(GameObject selected in selected_objects)
        {
            //translate
            Vector3 difference = go_to - selected.transform.position;
            difference = difference.normalized;
            selected.transform.Translate(difference * Time.deltaTime * selected.GetComponent<character_information>().speed);
            //rotate
            Transform object_model = selected.transform.GetChild(0); //first child is the model of the object
            Quaternion _lookrotation = Quaternion.LookRotation((go_to - object_model.position).normalized);
            object_model.rotation = Quaternion.Slerp(object_model.transform.rotation, _lookrotation, Time.deltaTime * turning_speed);

            if (Vector3.Distance(selected.transform.position, go_to) < 1)
            {
                arrived = true;
            }
        }*/
    }
    private void select_objects_in_box()
    {
        Vector3 origin_point;

        if (Input.GetMouseButtonDown(0))
        {
            //the ui of the area selected
            area_created = false;
            area_start = Input.mousePosition;
            //I choose the start position of the selection area
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                start_point = hit.point;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Destroy(area); //while the mouse is up selected area is destroyed
            //i choose the end position of the selection area
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                end_point = hit.point;
            }

            //selected_objects = new List<GameObject>();  //i intitialite the list

            //i create a box to detect the objects in a box defined by the positions of the mouse clicked positions
            origin_point = (end_point + start_point)/2;
            Vector3 halfextents = new Vector3(Mathf.Abs(end_point.x - start_point.x), Mathf.Abs(end_point.y - start_point.y), Mathf.Abs(end_point.z - start_point.z));

            Collider[] selecteds = Physics.OverlapBox(origin_point, halfextents/2, Quaternion.Euler(0,0,0), ignored_object);
            if(area != null)  //if we are choosing an area
            {
                foreach (Collider collider in selecteds)
                {
                    if (!selected_objects.Contains(collider.gameObject)) //if the list does not contains the object
                    {
                        selected_objects.Add(collider.gameObject);
                    }
                }
                objects_directions[selected_objects] = Vector3.zero;
            }

            if(selected_objects.Count > 0)
            {
                selected_object = selected_objects[0];
            }
            //make the first element of the list the selected object
            /*if (selected_objects.Count > 0)
            {
                selected_object = selected_objects[0];
            }*/
        }

    }
    void detect_area()
    {
        if (Mathf.Abs(Input.mousePosition.x - area_start.x) > 30 && !area_created) //it is to avoid the click problem with ui
        {
            GameObject canvas = GameObject.Find("Canvas");
            area = Instantiate(selected_area_object, area_start, Quaternion.identity, canvas.transform);
            area_created = true;
            //delete list of selected objects if the player choose new persons
            if (selected_objects.Count > 0)
            {
                selected_objects = new List<GameObject>();
            }
        }
        else if(Mathf.Abs(Input.mousePosition.x - area_start.x) < 30 && Input.GetMouseButtonUp(0))
        {
            area_created = true;
        }
        if(area != null)
        {
            area_end = Input.mousePosition;
            area.transform.position = (area_start + area_end)/2;
            area.transform.localScale = (area_end - area_start)/100;
        }
    }

    //for now useless
    void delete_clon()
    {
        bool selected = false;
        List<GameObject> first_group = new List<GameObject>();
        //if there are same gameobjects in different keys of objects_direction i delete the first one
        foreach(List<GameObject> list in objects_directions.Keys)
        {
            if(selected)
            {
                foreach (GameObject object1 in list)
                {
                    foreach(GameObject object2 in first_group)
                    {
                        if(object1 == object2)
                        {
                            list.Remove(object1);
                            foreach (GameObject object3 in list)
                            {
                                Debug.Log(object3);
                            }
                        }
                    }
                }
            }
            if (!selected)
            {
                // i choose the first group
                first_group = list;
                selected = true;
            }
        }
    }
}
