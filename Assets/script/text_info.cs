using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class text_info : MonoBehaviour
{
    Text txt;
    private void Awake()
    {
        txt = gameObject.GetComponent<Text>();
    }
    void Update()
    {
        txt.text = "GOLD: " + level_manager.Gold.ToString();
    }
}
