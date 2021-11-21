using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] InputManager inputManager;
    public static InputManager InputManager { get { return instance.inputManager; } }

    [SerializeField] UI ui;
    public static UI UI { get { return instance.ui; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void SnowBallDestroyed()
    {

    }

}
