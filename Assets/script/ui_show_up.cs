using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_show_up : MonoBehaviour
{
    public GameObject object_to_follow;
    Camera mCamera;
    public Vector3 pos_from_object;

    void Awake()
    {
        mCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    private void OnGUI()
    {
        if(object_to_follow != null)
        {
            var pos = object_to_follow.GetComponent<Transform>();
            gameObject.transform.position = mCamera.WorldToScreenPoint(pos.position) + pos_from_object;
        }
    }
}
