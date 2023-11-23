using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 100;

    void Start()
    {
        // _rotationSpeed += Random.Range(0, _rotationSpeed / 4f);
    }

    void Update()
    {
        transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
       // PoolManager.PutObject(gameObject);

       transform.gameObject.SetActive(false);
    }
}