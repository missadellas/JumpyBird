using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour {

	class PoolObject
    {
        public Transform transform;
        public bool inUse;
        public PoolObject(Transform t ){ transform = t; }
        public void Use() { inUse = true; }
        public void Dispose() { inUse = false; }
    }

    [System.Serializable]
    public struct YSpawnRange {
        public float min;
        public float max;
    }


    public GameObject Prefab;
    public int poolSize;
    public float shiftSpeed;
    public float spawnRate;
    public Vector3 defaultSpawnPos;
    public bool spawnImmediate; //partical prewarm
    public Vector3 immediateSpawnPos;
    public Vector2 targetAspectRatio;
    public YSpawnRange ySpawnRange;

    float spawnTimer;

    PoolObject[] poolObjects;
    float targetAspect;
    GameManager game;

    void Awake()
    {

    }
    void Start()
    {
        game = GameManager.Instance;
    }
    void OnEnable()
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }
    void OnDisable()
    {

        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;

    }
    void OnGameOverConfirmed()
    {

    }
    void Configure()
    {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[poolSize];
        for(int i = 0; i < poolObjects.Length; i++)
        {
            GameObject go = Instantiate(Prefab) as GameObject;
            Transform t = go.transform;
        }
    }
    void Spawn()
    {

    }
    void SpawnImmediate()
    {

    }
    void Shift()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].transform.position += -Vector3.right * shiftSpeed * Time.deltaTime;
            CheckDisposeObject(poolObjects[i]);
        }
    }
    void CheckDisposeObject(PoolObject poolObject)
    {
        if(poolObject.transform.position.x < -defaultSpawnPos.x)
        {
            poolObject.Dispose();
            poolObject.transform.position = Vector3.one * 1000;
        }
    }
    PoolObject GetPoolObject()
    {
        for(int i=0; i < poolObjects.Length;i++){
            if (!poolObjects[i].inUse)
            {
                poolObjects[i].Use();
                return poolObjects[i];
            }
        }
        return null;
    }
}
