﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    public event Action SetCoinEvent;

    [SerializeField] private GameObject _coinPrefab;
    private List<GameObject> _activeCoins = new List<GameObject>();

    [SerializeField] private int coinsCountInItem = 10;
    [SerializeField] private float coinsHeight = 0.5f;

    void Start()
    {
        Reset();
    }

    void Update()
    {
        if (GameManager.IsPause) return;

        foreach (var chunk in _activeCoins)
            chunk.transform.position -= new Vector3(0, 0, GameSpeedController.Speed * Time.deltaTime);

        if (_activeCoins.Count != 0 && _activeCoins[0].transform.position.z < Helper.ChunkDeleteDistance)
        {
            PoolManager.PutObject(_activeCoins[0]);
            _activeCoins.RemoveAt(0);
        }
    }

    public void Reset()
    {
        while (_activeCoins.Count > 0)
        {
            PoolManager.PutObject(_activeCoins[0]);
            _activeCoins.RemoveAt(0);
        }
    }

    private void RemoveCoin(GameObject coin) => _activeCoins.Remove(coin);
    private void SetCoin() => SetCoinEvent?.Invoke();

    public void CreateCoins(Style coinStyle, Vector3 position)
    {
        Vector3 coinPosition = Vector3.zero;
        for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
        {
            var coin = PoolManager.GetObject(_coinPrefab);
            if (coin.TryGetComponent<Coin>(out var res))
            {
                if (!res.IsSubscribedCoinRemoveEvent)
                {
                    res.CoinRemoveEvent += RemoveCoin;
                    res.IsSubscribedCoinRemoveEvent = res.CheckSubscribedCoinRemoveEvent();
                }
                if (!res.IsSubscribedSetCoinEvent)
                {
                    res.SetCoinEvent += SetCoin;
                    res.IsSubscribedSetCoinEvent = res.CheckSubscribedSetCoinEvent();
                }
            }
            coinPosition.y = (coinStyle == Style.Line) ? coinsHeight : Mathf.Max(Helper.GetPointParabola(i), coinsHeight);
            coinPosition.z = i * ((float)1);
            coin.transform.position = coinPosition + position;
            coin.SetActive(true);
            _activeCoins.Add(coin);
        }
    }

    ~CoinGenerator()
    {
        if (SetCoinEvent != null)
        {
            foreach (Delegate d in SetCoinEvent.GetInvocationList())
            {
                SetCoinEvent -= (Action)d;
            }
        }
    }
}