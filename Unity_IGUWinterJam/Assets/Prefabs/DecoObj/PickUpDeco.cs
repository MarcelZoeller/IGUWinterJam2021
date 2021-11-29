using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDeco : MonoBehaviour
{
    bool pickedup;
    DecorationManager decoM;

    // Start is called before the first frame update
    void Start()
    {
        decoM = GameObject.Find("DecoManager").GetComponent<DecorationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!pickedup)
            {
                this.transform.parent = null;
                decoM.pickedObjects.Add(this.gameObject);
                pickedup = true;
                DontDestroyOnLoad(this);
                
            }
        }
    }
}
