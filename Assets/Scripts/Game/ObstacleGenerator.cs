using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private CoinGenerator _coinGenerator;

    [SerializeField] private List<GameObject> _obstacles = new List<GameObject>();
    private List<GameObject> _activeObstacles = new List<GameObject>();

    [SerializeField] private int _itemSpace = 5;
    [SerializeField] private int _itemCountInMap = 5;

    [SerializeField] private int _obstaclesCount;

    void Start()
    {
        ResetChunks();

    }

    void Update()
    {
        if (GameManager.IsPause) return;

        foreach (var chunk in _activeObstacles)
            chunk.transform.position -= new Vector3(0, 0, GameManager.Speed * Time.deltaTime);

        if (_activeObstacles.Count != 0 && _activeObstacles[0].transform.position.z < ConstVar.ChunkDeleteDistance)
        {
            PoolManager.PutObject(_activeObstacles[0]);
            _activeObstacles.RemoveAt(0);

            CreateNextChunk();
        }
    }

    public void ResetChunks()
    {
        while (_activeObstacles.Count > 0)
        {
            PoolManager.PutObject(_activeObstacles[0]);
            _activeObstacles.RemoveAt(0);
        }

        _coinGenerator.Reset();

        for (int i = 0; i < _obstaclesCount; i++)
            CreateNextChunk();
    }

    private void CreateNextChunk()
    {
        var randomChunk = Random.Range(0, _obstacles.Count);
        var chunk = PoolManager.GetObject(_obstacles[randomChunk]);

        chunk.transform.position = _activeObstacles.Count > 0 ?
            _activeObstacles[_activeObstacles.Count - 1].transform.position + Vector3.forward * _itemCountInMap * _itemSpace : new Vector3(0, 0, ConstVar.StartOfSpawn);
        chunk.SetActive(true);
        _activeObstacles.Add(chunk);

        var res = chunk.GetComponent<Obstacle>();
        if (res != null)
        {
            Vector3 obstaclePos = new Vector3((int)res.TrackPosition * ConstVar.LaneOffset, 0, chunk.transform.position.z);// chunk.transform.position;//new Vector3((int)TrackPos.Center * laneOffset, 0, _itemCountInMap * _itemSpace);
            _coinGenerator.CreateCoins(res.Style, obstaclePos);
        }
    }






    //[SerializeField] private GameManager _gameManager;

    //[SerializeField] private GameObject _obstacleTopPrefab;
    //[SerializeField] private GameObject _obstacleBottomPrefab;
    //[SerializeField] private GameObject _obstacleFullPrefab;
    //[SerializeField] private GameObject _coinPrefab;

    //[SerializeField] private List<GameObject> _maps = new List<GameObject>();
    //[SerializeField] private List<GameObject> _activeMaps = new List<GameObject>();

    //[SerializeField] private int _itemSpace = 5;
    //[SerializeField] private int _itemCountInMap = 5;

    //enum CoinsStyle { Line, Jump, Ramp };
    //enum TrackPos { Left = -1, Center, Right };
    //[SerializeField] public float laneOffset = 1f;

    //[SerializeField] private int coinsCountInItem = 10;
    //[SerializeField] private float coinsHeight = 0.5f;

    //struct MapItem
    //{
    //    public GameObject Obstacle;
    //    public TrackPos TrackPos;
    //    public CoinsStyle CoinsStyle;

    //    public void SetValues(GameObject obstacle, TrackPos trackPos, CoinsStyle coinsStyle, GameManager gameManager)
    //    {
    //        this.Obstacle = obstacle;
    //        this.TrackPos = trackPos;
    //        this.CoinsStyle = coinsStyle;
    //    }
    //}

    //void Start()
    //{
    //    _maps.Add(MakeMap1());
    //    _maps.Add(MakeMap1());
    //    foreach (var map in _maps)
    //    {
    //        map.SetActive(false);
    //    }
    //}

    //void Update()
    //{
    //    if (_gameManager.IsPause) return;

    //    foreach (var map in _activeMaps)
    //    {
    //        map.transform.position -= new Vector3(0, 0, _gameManager.Speed * Time.deltaTime);

    //    }
    //    if (_activeMaps[0].transform.position.z < -_itemCountInMap * _itemSpace)
    //    {
    //        RemoveFirstActiveMap();
    //        AddActiveMap();
    //    }
    //}
    //void RemoveFirstActiveMap()
    //{
    //    _activeMaps[0].SetActive(false);
    //    _maps.Add(_activeMaps[0]);
    //    _activeMaps.RemoveAt(0);
    //}

    //public void ResetMaps()
    //{
    //    while (_activeMaps.Count > 0)
    //    {
    //        RemoveFirstActiveMap();
    //    }
    //    AddActiveMap();
    //    AddActiveMap();
    //}

    //void AddActiveMap()
    //{
    //    int r = Random.Range(0, _maps.Count);
    //    GameObject go = _maps[r];
    //    go.SetActive(true);
    //    foreach (Transform item in go.transform)
    //    {
    //        item.gameObject.SetActive(true);
    //    }
    //    go.transform.position = _activeMaps.Count > 0 ?
    //        _activeMaps[_activeMaps.Count - 1].transform.position + Vector3.forward * _itemCountInMap * _itemSpace : new Vector3(0, 0, 10);

    //    _maps.RemoveAt(r);
    //    _activeMaps.Add(go);
    //}


    //GameObject MakeMap1()
    //{
    //    GameObject result = new GameObject("Map1");
    //    result.transform.SetParent(transform);
    //    MapItem item = new MapItem();
    //    for (int i = 0; i < _itemCountInMap; i++)
    //    {
    //        item.SetValues(null, TrackPos.Center, CoinsStyle.Line, _gameManager);

    //        //if (i == 2)
    //        //{
    //        //    item.SetValues(_rampPrefab, TrackPos.Left, CoinsStyle.Ramp);

    //        //}
    //        if (i == 2)
    //        {
    //            item.SetValues(_obstacleBottomPrefab, TrackPos.Left, CoinsStyle.Jump, _gameManager);

    //        }
    //        if (i == 3)
    //        {
    //            item.SetValues(_obstacleBottomPrefab, TrackPos.Center, CoinsStyle.Jump, _gameManager);

    //        }
    //        else if (i == 4)
    //        {
    //            item.SetValues(_obstacleBottomPrefab, TrackPos.Right, CoinsStyle.Jump, _gameManager);

    //        }

    //        Vector3 obstaclePos = new Vector3((int)item.TrackPos * laneOffset, 0, i * _itemSpace);
    //        CreateCoins(item.CoinsStyle, obstaclePos, result);

    //        if (item.Obstacle != null)
    //        {
    //            GameObject go = Instantiate(item.Obstacle, obstaclePos, Quaternion.identity);
    //            go.transform.SetParent(result.transform);
    //        }
    //    }
    //    return result;
    //}

    //void CreateCoins(CoinsStyle coinStyle, Vector3 position, GameObject parent)
    //{
    //    Vector3 coinPosition = Vector3.zero;
    //    if (coinStyle == CoinsStyle.Line)
    //    {
    //        for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
    //        {
    //            coinPosition.y = coinsHeight;
    //            coinPosition.z = i * ((float)_itemSpace / coinsCountInItem);
    //            GameObject go = Instantiate(_coinPrefab, coinPosition + position, Quaternion.identity);
    //             go.transform.SetParent(parent.transform);
    //        }
    //    }
    //    else if (coinStyle == CoinsStyle.Jump)
    //    {
    //        for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
    //        {
    //            coinPosition.y = Mathf.Max(-1 / 2f * Mathf.Pow(i, 2) + 3, coinsHeight);
    //            coinPosition.z = i * ((float)_itemSpace / coinsCountInItem);
    //            GameObject go = Instantiate(_coinPrefab, coinPosition + position, Quaternion.identity);
    //            go.transform.SetParent(parent.transform);
    //        }
    //    }
    //}
}