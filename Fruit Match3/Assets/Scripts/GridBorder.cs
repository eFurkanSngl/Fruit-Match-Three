using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridBorder : MonoBehaviour
{
    [Header("Grid Border Settings")]
    [SerializeField] private GameObject _borderTop;
    [SerializeField] private GameObject _borderRight;
    [SerializeField] private GameObject _borderLeft;
    [SerializeField] private GameObject _borderBottom;
  
    private List<GameObject> _borderObjects = new List<GameObject>();


    private void CreateGridBorder(int gridX, int gridY)
    {
        foreach (var obj in _borderObjects)
        {
            Destroy(obj);
        }
        _borderObjects.Clear();

        // Ba�lang�� noktas�
        float startX = transform.position.x;  // objelerin ba�lang�� x ve y sini al�yoruz ( 0 )
        float startY = transform.position.y;

        for (int i = 0; i < gridX; i++)  // Grid Boyutu kadar Obje yarat�r
        {
            // Top border'�n Y'si gridY pozisyonunda olacak
            _borderObjects.Add(Instantiate(_borderTop, new Vector3(startX + i, startY + (gridY - 0.89f), 0), Quaternion.identity, transform));

            // Alt border'�n Y'si -1 pozisyonunda olacak
            _borderObjects.Add(Instantiate(_borderBottom, new Vector3(startX + i, startY - 0.13f, 0), Quaternion.identity, transform));
        }

        // Sol ve sa� kenarlar� yerle�tiriyoruz
        for (int i = 0; i < gridY; i++)
        {
            // Sol border'�n X'si -1 pozisyonunda olacak
            _borderObjects.Add(Instantiate(_borderLeft, new Vector3(startX - 1, startY + i, 0), Quaternion.identity, transform));

            // Sa� border'�n X'si gridX pozisyonunda olacak
            _borderObjects.Add(Instantiate(_borderRight, new Vector3(startX + gridY - 0.85f, startY + i, 0), Quaternion.identity, transform));
        }
        Debug.Log("ba�lang�� poz" + startX);
        Debug.Log("ba�lang�� poz" + startY);

    }


    private void OnEnable() => RegisterEvents();
    private void OnDisable() => UnRegisterEvents();

    private void RegisterEvents() => GridUIEvents.GridBorderEvents += CreateGridBorder;
    private void UnRegisterEvents() => GridUIEvents.GridBorderEvents -= CreateGridBorder;

}      
 

