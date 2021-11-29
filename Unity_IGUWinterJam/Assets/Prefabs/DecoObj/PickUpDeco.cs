using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDeco : MonoBehaviour
{
    bool pickedup;
    public deco decoG;
    public enum deco
    {
        hat, 
        carrot, 
        eye1,
        eye2,
        button1, 
        button2, 
        button3, 
        branch1,
        branch2,
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null && pickedup == false)
        {

            this.transform.parent = null;
            GameManager.DecorationManager.pickedObjects.Add(this.gameObject);
            GameManager.DecorationManager.pickedUp.Add(this);
            pickedup = true;
            transform.position = new Vector3(9999, 9999, 999);
            PlaySound();



        }
    }


    void PlaySound()
    {
        FMOD.Studio.EventInstance Sound = FMODUnity.RuntimeManager.CreateInstance("event:/Decoration/PickUpCarrot");
        Sound.start();
        Sound.release();
    }
}
