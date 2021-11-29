using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private static MenuManager instance;
    
    [SerializeField] private List<Button> buttons;
    [SerializeField] [Required] private Animator animator;

    private bool menuActive = true;

    private Player player;
    
    public enum MenuState
    {
        MainMenu,
        LevelSelection
    };
    
    public MenuState menuState = MenuState.MainMenu;
    
    void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        player = FindObjectOfType<Player>();
        //player = transform.root.GetComponentInChildren<Player>();
        Debug.Log(player);

        SetStateSettings(MenuState.MainMenu);
    }

    public void StartGame()
    {
        ToggleMenu();
        SetStateSettings(MenuState.LevelSelection);
    }

    public void QuitGame()
    {
        Application.Quit();
        EditorApplication.ExitPlaymode();
        Debug.Log("quit");
    }

    private void ToggleMenu()
    {
        if (!menuActive)
        {
            menuActive = true;
        }
        else
        {
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(false);
            }
            menuActive = false;
        }
    }

    private void SetStateSettings(MenuState state)
    {
        menuState = state;
        
        switch (menuState)
        {
            case MenuState.MainMenu:
                
                // TODO Penguins feet are clipping through the ground
                
                //player.gameObject.GetComponent<Player>().enabled = false;
                animator.Play("MainMenuCamera");
                break;
            case MenuState.LevelSelection:
                //player.gameObject.GetComponent<Player>().enabled = true;
                animator.Play("LevelSelectorCamera");
                break;
        }
    }
}
