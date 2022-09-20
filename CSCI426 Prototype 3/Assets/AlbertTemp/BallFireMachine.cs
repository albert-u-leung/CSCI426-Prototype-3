using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BallFireMachine : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab, ballBossPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireTime;
    [SerializeField] private int waveCount = 5;
    private AudioSource audioSource;

    // Update is called once per frame
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(FireBall()); 
    }

    IEnumerator FireBall()
    {
       
        if (waveCount >= 0)
        {
            waveCount--;
            audioSource.Play();
            if (waveCount == 0)
            {
                Instantiate(ballBossPrefab, firePoint.position, quaternion.identity);
            }
            Instantiate(ballPrefab, firePoint.position, quaternion.identity);
            yield return new WaitForSeconds(fireTime);
            yield return StartCoroutine(FireBall());
        }
    }
}
