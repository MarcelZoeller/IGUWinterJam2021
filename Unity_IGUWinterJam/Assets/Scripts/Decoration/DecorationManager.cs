using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DecorationManager : MonoBehaviour
{
    // Misc Variables
    
    Camera m_mainCam;

    // Decoration Process
    bool debuggingFunctions = false;                     // Bool to switch debugging functionalities on and off
    bool snowBallSelected;                               // Bool showing if a snowball is currently selected (currently redudant)
    bool placing;                                        // Bool showing if the player is currently placing an object onto the selected snowball
    bool decorationStarted = false;                      // Bool enabling the whole decoration mangager
   
    public GameObject selectedObject;                    // The object the player has selected to place. Null if no object is selected 
    public GameObject selectedSnowball;                  // The snowball the player has selected to place objects on. Null if no snowball is selected 
    public List<GameObject> placedObjects;               // List with all GameObjects already placed
    public List<GameObject> snowballs;                   // List of all Snowballs imported in the scene

    int resWidth;                                        // Width of current resolution
    int resHeight;                                       // Height of current resolution  

    Vector3 v_previousPosition = Vector3.zero;           // Variable to help with rotating the camera around a snowball
    Vector3 placingPosition;                             // Storing the position where the last object has been placed
    Vector3 v_offSet;                                    // Handling the offset between a already placed object and the mouse (currently not working) 


    //Pickup handling
    public List<GameObject> pickedObjects;
    public List<PickUpDeco> pickedUp;


    // Use Awake instead of Start to call earlier then start of snowBalls (resetting size if bool decorate in Snowballs is not set true)
    private void Awake()
    {
        SceneManager.sceneLoaded += this.OnLoadCallback;
    }

    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        DontDestroyOnLoad(this);
        m_mainCam = Camera.main;
        resWidth = Screen.currentResolution.width;
        resHeight = Screen.currentResolution.height;

        // Remove later only for testing 
        if (debuggingFunctions)
        {
            snowballs = GetSnowBalls(3);
            foreach (GameObject snowB in snowballs)
            {
                DontDestroyOnLoad(snowB);
                snowB.GetComponent<Rigidbody>().isKinematic = true;
                snowB.GetComponent<Snowball>().decorate = true;
            }
        }
        // =====================================

        // Place retrieved snowballs in the Decorationscene on top of each other according to the size
        if (SceneManager.GetActiveScene().name == "DecorationScene")
        {
            for (int j = 0; j < snowballs.Count; j++)
            {
                float sizesAdded = 0.0f;
                for (int k = 0; k < j + 1; k++)
                {
                    if (k == j)
                    {
                        sizesAdded += (snowballs[k].GetComponent<SphereCollider>().bounds.size.x / 2) * 0.8f;
                    }
                    else
                    {
                        sizesAdded += snowballs[k].GetComponent<SphereCollider>().bounds.size.x * 0.8f;
                    }

                }
                snowballs[j].transform.position = new Vector3(0, sizesAdded, 5);
                //snowballs[j].GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        // Clear List of picked up objects in any scene but decoration
        else
        {
            pickedObjects = new List<GameObject>();
        }
    }

    void Update()
    {
        // Debugging Function to start DecorationScene
        if (Keyboard.current.spaceKey.wasPressedThisFrame && debuggingFunctions)
        {
            snowballs = GetSnowBalls(3);
            foreach (GameObject snowB in snowballs)
            {
                DontDestroyOnLoad(snowB);
                snowB.GetComponent<Rigidbody>().isKinematic = true;
                //snowB.GetComponent<Snowball>().transfer = true;
            }
            DontDestroyOnLoad(this.gameObject);
            SceneManager.LoadScene("DecorationScene");
        }

        // Debugging Function: Stack Snowballs according to their size to form a snow man
        if (Keyboard.current.enterKey.wasPressedThisFrame && debuggingFunctions)
        {
            if (SceneManager.GetActiveScene().name == "DecorationScene")
            {
                for (int j = 0; j < snowballs.Count; j++)
                {
                    //snowballs[j].transform.localScale = Vector3.one * (0.3f * j);
                    float sizesAdded = 0.0f;
                    for (int k = 0; k < j; k++)
                    {
                        sizesAdded += snowballs[k].GetComponent<SphereCollider>().bounds.size.x;
                    }
                    snowballs[j].transform.position = new Vector3(0, sizesAdded, 5);
                    snowballs[j].GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }

        // Logic of placing objects and stopping the placing process
        if (placing && selectedSnowball && selectedObject)
        {
            positionObjectToPlace(selectedObject, selectedSnowball);
            if (Mouse.current.leftButton.wasPressedThisFrame && placing)
            {
                selectedObject = null;
                //selectedSnowball = null;
                //snowBallSelected = false;
                placing = false;
                placingPosition = Vector3.zero;
                v_offSet = Vector3.zero;
            }
           
        }

        // Handling different options for left clicks
        // Choosing a Snowball, placing an object, ending the placement process
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
            if (!placing)
            {
                GameObject savedSnowBall = selectedSnowball;
                selectedSnowball = startDecoration();
                if (selectedSnowball == null)
                {
                    selectedSnowball = savedSnowBall;
                }
            }

            if (placing && !selectedObject)
            {
                placing = false;
            }
        }

        // Enable the player to rotate the cam around the middle snowball with the middle mouse wheel
        if (decorationStarted && !placing)
        {
            rotateCamAroundObject(snowballs[1], 15f);
        }

        // Call for pickup object positioning
        if (SceneManager.GetActiveScene().name == "DecorationScene")
        {
            positionPickUps();
        }
       
        
    }

    // Start decoration scene and load in snowballs. Either through giving the function a list with snowballs or having the objects named after the convention: "Snowball (X)" where X are numbers from 1-3
    // In:  Optional list with snowball gameobjects
    // Out: /
    public void enterDecorationScene(List<GameObject> sB = null)
    {
        snowballs = sB;

        decorationStarted = true;
        if (sB == null)
        {
            sB = GetSnowBalls(3);
        }
        foreach (GameObject snowB in sB)
        {
            DontDestroyOnLoad(snowB);
            //snowB.GetComponent<Rigidbody>().isKinematic = true;
            snowB.GetComponent<Snowball>().decorate = true;
        }
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("DecorationScene");
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

        if (placedObjects.Contains(objectToPlace) && placingPosition != Vector3.zero && v_offSet == Vector3.zero)
        {
            v_offSet = (placingPosition - objectToPlace.transform.position).normalized;
        }
        if (placedObjects.Contains(objectToPlace) && placingPosition == Vector3.zero)
        {
            placingPosition = objectToPlace.transform.position;            
        }

        Vector3 v_newPosition = Vector3.zero;
        Vector3 extents2 = objectToPlace.GetComponent<Collider>().bounds.extents;
        Vector3 extents = objectToPlace.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.extents;
        extents.x *= objectToPlace.transform.localScale.x;
        extents.y *= objectToPlace.transform.localScale.y;
        extents.z *= objectToPlace.transform.localScale.z;
        float f_radius = (objectToPlaceOn.GetComponent<Collider>().bounds.size.x / 2) + (extents.x / 2);
        Debug.Log(f_radius);
        float f_distanceToCam = Vector3.Distance(objectToPlaceOn.transform.position, Camera.main.transform.position);
        Vector3 v_posWithDepth = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 10));
        Vector3 v_mouseToRadius = (v_posWithDepth - objectToPlace.transform.position).normalized;

        Vector3 v_placedPositionAdd = (v_posWithDepth - placingPosition).normalized;

        v_newPosition = (objectToPlaceOn.transform.position + f_radius * v_mouseToRadius);

        v_newPosition = new Vector3(v_newPosition.x, v_newPosition.y, v_newPosition.z);
        objectToPlace.transform.position = v_newPosition;
        Quaternion h = new Quaternion(v_mouseToRadius.x, v_mouseToRadius.y, v_mouseToRadius.z + 90, 0);
        //objectToPlace.transform.rotation = Quaternion.LookRotation(objectToPlaceOn);
        // objectToPlace.transform.rotation = new Quaternion(objectToPlace.transform.rotation.x, objectToPlace.transform.rotation.y, objectToPlace.transform.rotation.z - 180, 0);
        Vector3 ssio = objectToPlaceOn.transform.position - objectToPlace.transform.position;
        //objectToPlace.transform.LookAt(objectToPlaceOn.transform);
        objectToPlace.transform.rotation = Quaternion.LookRotation(ssio);
        if (!placedObjects.Contains(objectToPlace))
        {
            placedObjects.Add(objectToPlace);
        }
    }

    // Rotate the camera around a selected object by dragging the mouse while holding the mouse wheel button
    // In:  Object to rotate around, distance of the camera to the object
    // Out: /
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

            float rotationAroundYAxis = -direction.x * 180; 
            float rotationAroundXAxis = direction.y * 180; 

            cam.transform.position = selectedObject.transform.position;

            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);

            cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

            v_previousPosition = newPosition;
        }
    }

    void positionPickUps()
    {
        int i = 0;
        foreach (GameObject p in pickedObjects)
        {
            if (!placedObjects.Contains(p))
            {
                float x = Screen.width * (0.6f + (i % 5 * 0.1f));
                float y = Screen.height * (0.95f - (i * 0.2f));
                p.transform.LookAt(Camera.main.transform);
                Debug.Log((Screen.width, Screen.height));
                Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 5));
                p.transform.position = pos;

                i++;
            }

        }
    }

    // Screenshot feature
    // =================================================================================================
    private bool takeHiResShot = false;

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screenXXXXXX_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }

    private void LateUpdate()
    {
        takeHiResShot |= Keyboard.current.kKey.wasPressedThisFrame;
        if (takeHiResShot)
        {
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            Camera.main.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            Camera.main.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            Camera.main.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            takeHiResShot = false;
        }
    }
    // =================================================================================================
}
