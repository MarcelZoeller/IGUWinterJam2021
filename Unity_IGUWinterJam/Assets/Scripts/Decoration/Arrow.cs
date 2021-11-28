using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public DecorationManager DecoMan;

    // Start is called before the first frame update
    void Start()
    {
        DecoMan = GameObject.Find("DecoManager").GetComponent<DecorationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DecoMan != null && DecoMan.selectedSnowball != null)
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
            Vector3 sBPos = DecoMan.selectedSnowball.transform.position;
            Vector3 forward = Camera.main.transform.position - sBPos;
            Vector3 left = -Vector3.Cross(forward.normalized, Vector3.up.normalized);
            this.transform.position = sBPos + left.normalized * 3;
            this.transform.LookAt(Camera.main.transform);
        }
        else
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
        }

    }
}
