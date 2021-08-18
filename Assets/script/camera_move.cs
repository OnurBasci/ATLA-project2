using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_move : MonoBehaviour
{
    public int speed = 10;
    public float scrolling_speed = 20f;
    
    
    Vector3 differnce_mouse_position;
    Vector3 middle_screen_vector = new Vector3(0.5f,0.5f,0);
    Vector3 x_z_based_vector = Vector3.zero;
    Camera mCamera;
    public Vector2 scrolling_edges = new Vector2(20, 120);
    private int ignore_layer = 9;
    private void Awake()
    {
        mCamera = gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        camera_movement();
        //camera_follow();   //for now i find this unnecesssary

    }

    void camera_movement()
    {
        if (Input.GetMouseButtonDown(2))
        {
            differnce_mouse_position = mCamera.ScreenToViewportPoint(Input.mousePosition) - middle_screen_vector;
            x_z_based_vector.x = differnce_mouse_position.x;
            x_z_based_vector.z = differnce_mouse_position.y;
        }
        else if (Input.GetMouseButton(2))
        {
            gameObject.transform.Translate((x_z_based_vector * speed) * Time.deltaTime, Space.World);
        }

        float scrolling_value = Input.GetAxisRaw("Mouse ScrollWheel");
        if (transform.position.y < scrolling_edges.y && transform.position.y > scrolling_edges.x - 10 && scrolling_value < 0)
        {
            gameObject.transform.Translate(new Vector3(0, 0, scrolling_value) * scrolling_speed * 100f * Time.deltaTime);
        }
        if (transform.position.y < scrolling_edges.y + 10 && transform.position.y > scrolling_edges.x && scrolling_value > 0)
        {
            gameObject.transform.Translate(new Vector3(0, 0, scrolling_value) * scrolling_speed * 100f * Time.deltaTime);
        }
    }

    void camera_follow()
    {
        //camera_follows selected object in level_manager script
        if(level_manager.selected_object != null)
        {
            Ray ray = new Ray(level_manager.selected_object.transform.position, new Vector3(0, Mathf.Sqrt(2)/3, -0.3f));
            RaycastHit hit;
            //Debug.DrawRay(level_manager.selected_object.transform.position, new Vector3(0, (Mathf.Sqrt(2) / 3) * 100, -100/3), Color.black);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, ignore_layer))
            {
                Vector3 difference = (hit.point - transform.position).normalized;
                if (Vector3.Distance(transform.position, hit.point) > 1f)
                {

                    transform.Translate(difference * level_manager.selected_object.GetComponent<character_information>().speed * Time.deltaTime, Space.World);
                }

            }
        }
    }
}
