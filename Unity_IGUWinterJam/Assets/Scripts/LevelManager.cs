using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    [SerializeField] private List<string> sceneNames;

    public List<string> sceneList;
    
    private void Awake()
    {
        instance = this;
    }
}
