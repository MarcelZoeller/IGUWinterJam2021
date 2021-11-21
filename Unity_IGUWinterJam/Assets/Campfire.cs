using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        var snowball = other.gameObject.GetComponent<Snowball>();
        if(snowball)
        {
            snowball.melt = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var snowball = other.gameObject.GetComponent<Snowball>();
        if (snowball)
        {
            snowball.melt = false;
        }
    }
}
