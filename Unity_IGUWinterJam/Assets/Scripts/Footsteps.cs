using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] float footParameter = 1f;
    
    //void PlayFootstepEvent(string path)
    //{
    //    FMOD.Studio.EventInstance Footsteps = FMODUnity.RuntimeManager.CreateInstance(path);
    //    Footsteps.start();
    //    Footsteps.release();
    //}    
    
    
    void PlayFootstepEvent()
    {
        FMOD.Studio.EventInstance Footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/Footsteps/Footsteps");
        Footsteps.setParameterByName("FeetSurface", footParameter);
        Footsteps.start();
        Footsteps.release();
    }
}
