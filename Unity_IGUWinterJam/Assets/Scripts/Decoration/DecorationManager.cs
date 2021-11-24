using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DecorationManager : MonoBehaviour
{
    // Misc Variables
    public List<GameObject> help1;
    public Camera m_mainCam;

    // Decoration Process
    public bool snowBallSelected;
    public bool placing;
    public GameObject selectedObject;
    public GameObject selectedSnowball;
    Vector3 v_previousPosition = Vector3.zero;


    void Start()
    {
        m_mainCam = Camera.main;
    }

    void Update()
    {
        // Debugging Function to start DecorationScene
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            help1 = GetSnowBalls(3);
            foreach (GameObject snowB in help1)
            {
                DontDestroyOnLoad(snowB);
                snowB.GetComponent<Rigidbody>().isKinematic = true;
            }
            DontDestroyOnLoad(this.gameObject);
            SceneManager.LoadScene("DecorationScene");
        }

        // Stack Snowballs according to their size to form a snow man
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

        // Logic of placing objects and stopping the placing process
        if (placing && selectedSnowball && selectedObject)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                selectedObject = null;
                selectedSnowball = null;
                snowBallSelected = false;
                placing = false;
            }
            positionObjectToPlace(selectedObject, selectedSnowball);
        }

        // Debugging with mouse position
        if ( Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (snowBallSelected)
            {
                selectedObject = chooseObject();
                if (selectedObject == null)
                {
                    if (startDecoration() != null)
                    {
                        selectedSnowball = startDecoration();
                    }
                }
            }
            else if (!placing)
            {
                selectedSnowball = startDecoration();
            }

            if (placing && !selectedObject && !selectedSnowball)
            {
                placing = false;
            }
        }

        if (!placing && selectedSnowball)
        {
            rotateCamAroundObject(selectedSnowball, 10f);
        }
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

    // Start the decoration mode for a single snowball of the snowman
    // In:  /
    // Out: GameObject of decorated snowball
    GameObject startDecoration()
    {
        GameObject t_targetedSnowBall = null;
        
        Ray t_ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit t_rayHit;
        if(Physics.Raycast(t_ray, out t_rayHit))
        {
            t_targetedSnowBall = t_rayHit.collider.gameObject;
            snowBallSelected = true;
        }
        return t_targetedSnowBall;
    }

    // Choose a Object to place on the currently selected snowball
    // In:  /
    // Out: Object that has been placed
    GameObject chooseObject()
    {
        GameObject g_object = null;

        Ray t_ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit t_rayHit;
        if (Physics.Raycast(t_ray, out t_rayHit))
        {
            if (t_rayHit.transform.CompareTag("DecoObj"))
            {
                g_object = t_rayHit.transform.gameObject;
                placing = true;
            }
        }
        return g_object;
    }

    // Place object on a snowball relative to mouseposition
    // In:  snowball to place, gameobject to place the snowball on
    // Out: /
    void positionObjectToPlace(GameObject objectToPlace, GameObject objectToPlaceOn)
    {
        float f_radius = selectedSnowball.GetComponent<SphereCollider>().bounds.size.x;
        Vector3 v_posWithDepth = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 5f));
        Vector3 v_mouseToRadius = (v_posWithDepth - objectToPlace.transform.position).normalized;

        print(v_posWithDepth);
        objectToPlace.transform.position = objectToPlaceOn.transform.position + f_radius * v_mouseToRadius;

    }

    void rotateCamAroundObject(GameObject selectedObject, float distanceToTarget)
    {
        Vector3 v_mousePos = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 5f);
        Camera cam = Camera.main;
        if (Mouse.current.middleButton.wasPressedThisFrame)
        {
            v_previousPosition = cam.ScreenToViewportPoint(v_mousePos);
        }
        else if (Mouse.current.middleButton.isPressed)
        {
            Vector3 newPosition = cam.ScreenToViewportPoint(v_mousePos);
            Vector3 direction = v_previousPosition - newPosition;

            float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
            float rotationAroundXAxis = direction.y * 180; // camera moves vertically

            cam.transform.position = selectedObject.transform.position;

            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); // <— This is what makes it work!

            cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

            v_previousPosition = newPosition;
        }
    }

}
