using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using TMPro;

public class Snowball : MonoBehaviour
{
    [SerializeField] LayerMask snowLayerMask;
    [SerializeField] int snowPickupRadius = 8;

    [SerializeField] TextMeshProUGUI snowText;

    int snowPoints = 0;

    public bool melt = false;
    [SerializeField] int meltRate = 50;

    [SerializeField] float snowStartSize = 0.5f;
    [SerializeField] float snowSize = 0.5f;


    private void Start()
    {
        snowSize = snowStartSize;
        UpdateSnowSize();
    }

    void Update()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.1f, Vector3.down, 2f, snowLayerMask);

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
    }

    void CheckForSnow(SnowField snowField)
    {
        Vector2 where = snowField.GetPosition(transform);
        snowPoints += snowField.CheckForSnow(where.x, where.y, snowPickupRadius);


        UpdateSnowSize();


    }



    void UpdateSnowSize()
    {
        snowSize = snowStartSize + (snowPoints / 100000f);

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
