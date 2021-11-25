using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallSpawner : MonoBehaviour
{
    [SerializeField] GameObject snowBallPrefab;
    [SerializeField] Transform spawnPoint;

    private void Start()
    {
        GameManager.instance.currentSnowBallSpawner = this;
    }

    public void SpawnSnowball()
    {
        Instantiate(snowBallPrefab, spawnPoint.position, Quaternion.identity);
    }

}
