using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZone : MonoBehaviour
{
    [SerializeField] float goalSnowSize = 1.75f;
    [SerializeField] Transform ballCenter1;
    [SerializeField] Transform ballCenter2;
    [SerializeField] Transform ballCenter3;

    [SerializeField] bool hasBall1;
    [SerializeField] bool hasBall2;
    [SerializeField] bool hasBall3;

    List<GameObject> snowBalls = new List<GameObject>();

    [SerializeField] GameObject hat;
    [SerializeField] GameObject carrot;
    [SerializeField] GameObject eye1;
    [SerializeField] GameObject eye2;
    [SerializeField] GameObject button1;
    [SerializeField] GameObject button2;
    [SerializeField] GameObject button3;
    [SerializeField] GameObject branch1;
    [SerializeField] GameObject branch2;

     bool hatB = false;
    bool carrotB = false;
     bool eye1B = false;
     bool eye2B = false;
    bool button1B = false;
     bool button2B = false;
    bool button3B = false;
     bool branch1B = false;
     bool branch2B = false;



    private void OnTriggerEnter(Collider other)
    {
        
        
        var snowball = other.gameObject.GetComponent<Snowball>();
        if (snowball)
        {
            snowBalls.Add(snowball.gameObject);

            if (hasBall1 && hasBall2 && hasBall3) return;

            Destroy(snowball.GetComponent<Rigidbody>());

            Vector3 targetPosition = ballCenter1.position;

            if (!hasBall1)
            {
                targetPosition = ballCenter1.position;
                hasBall1 = true;
                
            }
            else if (!hasBall2)
            {
                targetPosition = ballCenter2.position;
                hasBall2 = true;
            }
            else if (!hasBall3)
            {
                targetPosition = ballCenter3.position;
                hasBall3 = true;
                
            }

            StartCoroutine(MoveAtSpeedCoroutine(snowball.transform, targetPosition, .1f));

            //if (hasBall3)
            //    StartCoroutine(StartDecorationMode());

            //Destroy(snowball);
        }


        var player = other.gameObject.GetComponent<Player>();
        if(player != null)
        {
            if (hasBall1 && hasBall2 && hasBall3)
            {



                foreach (var item in GameManager.DecorationManager.pickedUp)
                {
                    if (item.decoG == PickUpDeco.deco.hat && !hatB)
                    {
                        hat.SetActive(true);
                        hatB = true;
                        PlayStickSound();
                    }

                    if (item.decoG == PickUpDeco.deco.carrot && !carrotB)
                    {
                        carrot.SetActive(true);
                        carrotB = true;
                        PlayStickSound();
                    }

                    if (item.decoG == PickUpDeco.deco.eye1 && !eye1B)
                    {
                        eye1.SetActive(true);
                        eye1B = true;
                        PlayStickSound();
                    }

                    if (item.decoG == PickUpDeco.deco.eye2 && !eye2B)
                    {
                        eye2.SetActive(true);
                        eye2B = true;
                        PlayStickSound();
                    }

                    if (item.decoG == PickUpDeco.deco.button1 && !button1B)
                    {
                        button1.SetActive(true);
                        button1B = true;
                        PlayStickSound();
                    }

                    if (item.decoG == PickUpDeco.deco.button2 && !button2B)
                    {
                        button2.SetActive(true);
                        button2B = true;
                        PlayStickSound();
                    }

                    if (item.decoG == PickUpDeco.deco.button3 && !button3B)
                    {
                        button3.SetActive(true);
                        button3B = true;
                        PlayStickSound();
                    }

                    if (item.decoG == PickUpDeco.deco.branch1 && !branch1B)
                    {
                        branch1.SetActive(true);
                        branch1B = true;
                        PlayStickSound();
                    }

                    if (item.decoG == PickUpDeco.deco.branch2 && !branch1B)
                    {
                        branch2.SetActive(true);
                        branch1B = true;
                        PlayStickSound();
                    }

                }
            }

        }



    }

    void PlayStickSound()
    {

        FMOD.Studio.EventInstance Sound = FMODUnity.RuntimeManager.CreateInstance("event:/Decoration/StickToSnowman");
        Sound.start();
        Sound.release();
    }

    

    IEnumerator StartDecorationMode()
    {
        yield return new WaitForSeconds(2f);
        //Debug.Log("GoalZone enter deco mode");
        //GameManager.DecorationManager.enterDecorationScene(snowBalls);
    }


    private void OnTriggerExit(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            if (hasBall1 && hasBall2 && hasBall3)
            {
                GameManager.UI.interactText.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator MoveAtSpeedCoroutine(Transform obj, Vector3 end, float speed)
    {
        //while you are far enough away to move
        while (Vector3.Distance(obj.transform.position, end) > speed)
        {
            //MoveTowards the end position by a given distance
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, end, speed);
            //wait for a frame and repeat
            yield return 0;
        }
        //Since you are really really close, now you can just go to the final position.
        obj.transform.position = end;
    }
}
