using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _gridX;
    [SerializeField] private int _gridY;
    [SerializeField] private GameObject[] _gameObject;
    [SerializeField] private Camera _camera;

    private void Start()
    {
        CreateGrid();
        _camera = Camera.main;
    }


    private void CreateGrid()
    {
        for (int i = 0; i < _gridX; i++)
        {
            for (int j = 0; j < _gridY; j++)
            {

                Vector3 pos = new Vector3(transform.position.x + i, transform.position.y + j, 0);
                int randomIndex = Random.Range(0, _gameObject.Length);

                GameObject Obj = Instantiate(_gameObject[randomIndex], pos, Quaternion.identity);
                Obj.transform.parent = transform;
            }
        }
    }

#if UNITY_EDITOR_64
    private void OnDrawGizmos()
    {
        for (int i = 0; i < _gridX; i++)
        {
            for (int j = 0; j < _gridY; j++)
            {
                Gizmos.DrawWireCube(new Vector3(transform.position.x + i, transform.position.y + j, 0), new Vector3(1, 1, 1));
            }
        }
    }
#endif
}
