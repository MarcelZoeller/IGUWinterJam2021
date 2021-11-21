using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DecorationManager : MonoBehaviour
{

    public List<GameObject> help1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debugging Function to start DecorationScene
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            help1 = GetSnowBalls(3);
            foreach (GameObject snowB in help1)
            {
                DontDestroyOnLoad(snowB);
            }
            DontDestroyOnLoad(this.gameObject);
            SceneManager.LoadScene("DecorationScene");
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if (SceneManager.GetActiveScene().name == "DecorationScene")
            {
                for (int j = 0; j < help1.Count; j++)
                {
                    //help1[j].transform.localScale = Vector3.one * (0.3f * j);
                    float sizesAdded = 0.0f;
                    for (int k = 0; k < j; k++)
                    {
                        sizesAdded += help1[k].GetComponent<SphereCollider>().bounds.size.x;
                    }
                    print(sizesAdded);
                    help1[j].transform.position = new Vector3(0, sizesAdded, 5);
                    help1[j].GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }

        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            if (SceneManager.GetActiveScene().name == "DecorationScene")
            {
                for (int j = 0; j < help1.Count; j++)
                {
                    help1[j].transform.localScale = Vector3.one * 1.2f;
                }
            }
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        
    }

    // Fetches all Snowballs from the scene that should not be destroyed
    // In: Number of snowballs that should be saved
    // Out: Snowball-GameObject-List
    List<GameObject> GetSnowBalls(int snowBallCount)
    {
        List <GameObject> snowBalls = new List<GameObject>();
        for (int i = 0; i < snowBallCount; i++)
        {
            string snowBallName = "Snowball " + "(" + (i+1).ToString() + ")";
            snowBalls.Add(GameObject.Find(snowBallName));
        }

        return snowBalls;
    }


}
