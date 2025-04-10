using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolManager : MonoBehaviour
{
    public static ParticlePoolManager Instance;
    [SerializeField] private GameObject _particleEffect;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> particlePool = new Queue<GameObject>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
        InitializePool();
    }

    private void InitializePool()
    {
        for(int i = 0; i < poolSize; i++)// Pool size kadar dönüyoruz
        {
            GameObject obj = Instantiate(_particleEffect); // Objeyi yaratýyoruz
            obj.SetActive(false);   
            particlePool.Enqueue(obj);
        }
    }

    public GameObject GetParticle()
    {
        if(particlePool.Count > 0)
        {
            GameObject obj = particlePool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        GameObject newObj = Instantiate(_particleEffect);
        return newObj;
    }

    public void ReturnParticle(GameObject obj)
    {
        obj.SetActive(false);
        particlePool.Enqueue(obj);
    }
}
