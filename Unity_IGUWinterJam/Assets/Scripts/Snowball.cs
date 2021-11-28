using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using TMPro;
using FMODUnity;

public class Snowball : MonoBehaviour
{
    [SerializeField] LayerMask snowLayerMask;
    [SerializeField] int snowPickupRadius = 8;

    [SerializeField] TextMeshProUGUI snowText;

    [SerializeField] int snowPoints = 0;

    public bool melt = false;
    [SerializeField] int meltRate = 50;

    [SerializeField] float snowStartSize = 0.5f;
    [SerializeField] float snowSize = 0.5f;

    [SerializeField] StudioEventEmitter StudioEventEmitter;
    [SerializeField] Rigidbody rb;
    [SerializeField] FMOD.Studio.EventInstance instance;
    [SerializeField] float magnitude;
    [SerializeField] float snowGrowSpeed = 100000f;

    public bool decorate = false;

    private void Start()
    {
        if (!decorate)
        {
            snowSize = snowStartSize;
            UpdateSnowSize();
        }
    }

    void Update()
    {
        if (decorate) return;

        if (rb == null) return;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up, 0.1f, Vector3.down, 20f, snowLayerMask);

        foreach (var hit in hits)
        {
            var snowfield = hit.collider.gameObject.GetComponent<SnowField>();
            if (snowfield != null)
            {
                CheckForSnow(snowfield);
            }
        }

        if (melt)
        {
            snowPoints -= meltRate;
            UpdateSnowSize();
        }

        magnitude = rb.velocity.magnitude;

        if (rb.velocity.magnitude > 1)
        {
            instance = FMODUnity.RuntimeManager.CreateInstance("event:/Snowball/SnowballRolling");
            instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
            instance.start();
        }
        else
        {
            //instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }

    }

    




    void CheckForSnow(SnowField snowField)
    {
        Vector2 where = snowField.GetPosition(transform);
        snowPoints += snowField.CheckForSnow(where.x, where.y, snowPickupRadius);


        UpdateSnowSize();


    }


    
    void UpdateSnowSize()
    {
        snowSize = snowStartSize + (snowPoints / snowGrowSpeed);

        transform.localScale = new Vector3(snowSize, snowSize, snowSize);

        if (snowText != null)
            snowText.text = "Snow: " + snowPoints.ToString() + " Snow Size: " + snowSize;

        if(snowSize < .3f)
        {
            DestroySnowBall();
        }
    }

    void DestroySnowBall()
    {
        Destroy(gameObject);
        GameManager.instance.currentSnowBallSpawner.SpawnSnowball();
    }
    
    


}
