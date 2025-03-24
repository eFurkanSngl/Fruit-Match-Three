using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamereController : MonoBehaviour
{
    [SerializeField] private float _zoomMultip = 1.2f;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }
    private void SizeFilterCam(int gridWidth , int gridHeight)
    {
        float centerX = gridWidth / 2.25f;
        float centerY = gridHeight / 2.5f;

        _camera.transform.position = new Vector3(centerX, centerY, -10);

        float maxSize = Mathf.Max(gridWidth, gridHeight);
        _camera.orthographicSize = (maxSize / 1.9f)* _zoomMultip;
        Debug.Log("Cam is work");
    }

    private void OnEnable()=> RegisterEvents();
    private void OnDisable()=>UnRegisterEvents();


    private void RegisterEvents() {

        //CameraFilterEvents.CameraEvents += SizeFilterCam;
        GridManager.GridManagerEvent += SizeFilterCam;
    }
    private void UnRegisterEvents() {
        GridManager.GridManagerEvent -= SizeFilterCam;
        //CameraFilterEvents.CameraEvents -= SizeFilterCam;
    }  



}
