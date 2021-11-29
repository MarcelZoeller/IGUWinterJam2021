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

    [SerializeField] DecorationManager decorationManager;
    public static DecorationManager DecorationManager { get { return instance.decorationManager; } }

    MenuManager menuManager;
    
    public static MenuManager MenuManager { get { return instance.menuManager; } }
    
    public SnowBallSpawner currentSnowBallSpawner;

    public GoalZone currentGoalZone;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        inputManager = GetComponent<InputManager>();
        menuManager = GetComponent<MenuManager>();
    }

    public void SnowBallDestroyed()
    {
        
    }

}
