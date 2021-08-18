using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gain_gold : MonoBehaviour
{
    public int gold_par_time;
    public int gain_periode;
    bool gainable = true;

    private void Update()
    {
        if(gainable)
        {
            StartCoroutine("gain");
        }
    }
    IEnumerator gain()
    {
        gainable = false;
        if(gameObject.layer == 9)
        {
            level_manager.Gold += gold_par_time;  //for the player money
        }
        else if(gameObject.layer == 10)
        {
            ennemy_infomation.Gold += gold_par_time;  //for the ai money
        }
        yield return new WaitForSeconds(gain_periode);
        gainable = true;
    }
}
