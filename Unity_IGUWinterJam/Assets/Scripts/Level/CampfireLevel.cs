using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampfireLevel : MonoBehaviour
{
    [SerializeField] [Required] private SphereCollider triggerArea;

    [SerializeField] private bool activated = false;

    [SerializeField] private ParticleSystem fire;

    [SerializeField] private int levelIndex;

    [SerializeField] private Canvas canvas;

    private PlayAnimation anim;

    bool canEnterLevel = false;
    
    private void Start()
    {
        //triggerArea = GetComponent<SphereCollider>();

        anim = GetComponent<PlayAnimation>();
        
        var em = fire.emission;
        em.enabled = activated;
    }

    [Button(ButtonSizes.Large)]
    public void ToggleFire()
    {
        var em = fire.emission;
       
        activated = !activated;
        em.enabled = activated;
    }

    private void Update()
    {
        if (canEnterLevel && GameManager.InputManager.interact)
        {
            GameManager.InputManager.interact = false;
            SceneManager.LoadScene(levelIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvas.gameObject.SetActive(true);
            canEnterLevel = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvas.gameObject.SetActive(false);
            canEnterLevel = false;
        }

        //if (other.gameObject.CompareTag("Player")) StartCoroutine(WaitForAnimEnd(anim.clip2.length));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator WaitForAnimEnd(float duration)
    {
        anim.PlayExitAnim();
        yield return new WaitForSeconds(duration);
        canvas.gameObject.SetActive(false);

    }
}
