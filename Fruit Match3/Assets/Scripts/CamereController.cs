using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamereController : MonoBehaviour
{
    [SerializeField] private float _zoomMultip = 1.2f;
    private Camera _camera;
    [SerializeField] AudioSource _destroySource;
    private void Start()
    {
        _camera = Camera.main;
    }

    private void DestroySound()
    {
        if(_destroySource != null)
        {
            _destroySource.Play();
        }
    }
    private void SizeFilterCam(int gridWidth , int gridHeight)
    {
        float aspect = (float)Screen.width / Screen.height;
        // Ekranýn En boy oranýný bulduk.Her ekran için farklý.


        float targetWidth = gridWidth + 1f; 
        float targetHeight = gridHeight + 1f;
        // Grid boyutunu aldýk ve padding boþluk verdik

        float camSizeByHeight = targetHeight / 2f;
        float camSizeByWidth = (targetWidth / aspect) / 2f;

        //Kameranýn ortgraphic size'ýný hesaplýyoruz
        

        float finalSize = Mathf.Max(camSizeByHeight, camSizeByWidth);
        _camera.orthographicSize = finalSize;

        // grid taþmamasý için yüksek olan deðeri aldýk
        // ekran dar ve uzun ise = geniþliðe göre çekil
        // ekran geniþ ise = yüksekliðe göre ayarla.

        // Kamerayý ortala
        float centerX = (gridWidth - 1) / 2f;
        float centerY = (gridHeight - 1) / 2f;

        _camera.transform.position = new Vector3(centerX, centerY, -10f);

        // gridin tam ortasýný bulup kamerayý konumlandýrýyoruz.


        //float centerX = gridWidth / 2.25f;
        //float centerY = gridHeight / 2.5f;

        //_camera.transform.position = new Vector3(centerX, centerY, -10);

        //float maxSize = Mathf.Max(gridWidth, gridHeight);
        //_camera.orthographicSize = (maxSize / 2f)* _zoomMultip;
        //Debug.Log("Cam is work");
    }

    private void OnEnable()=> RegisterEvents();
    private void OnDisable()=>UnRegisterEvents();


    private void RegisterEvents() {

        //CameraFilterEvents.CameraEvents += SizeFilterCam;
        GridManager.GridManagerEvent += SizeFilterCam;
        CameraEvents.CameraEvent += DestroySound;
    }
    private void UnRegisterEvents() {
        GridManager.GridManagerEvent -= SizeFilterCam;
        CameraEvents.CameraEvent -= DestroySound;
        //CameraFilterEvents.CameraEvents -= SizeFilterCam;
    }  



}
