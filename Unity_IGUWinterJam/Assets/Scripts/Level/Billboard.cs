using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        
        transform.rotation = Quaternion.Euler(cam.transform.rotation.eulerAngles.x, 0f, 0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.LookAt(cam.transform);

        //transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
