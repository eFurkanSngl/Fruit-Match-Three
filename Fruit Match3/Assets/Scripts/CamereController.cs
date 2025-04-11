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
        // Ekran�n En boy oran�n� bulduk.Her ekran i�in farkl�.


        float targetWidth = gridWidth + 1f; 
        float targetHeight = gridHeight + 1f;
        // Grid boyutunu ald�k ve padding bo�luk verdik

        float camSizeByHeight = targetHeight / 2f;
        float camSizeByWidth = (targetWidth / aspect) / 2f;

        //Kameran�n ortgraphic size'�n� hesapl�yoruz
        

        float finalSize = Mathf.Max(camSizeByHeight, camSizeByWidth);
        _camera.orthographicSize = finalSize;

        // grid ta�mamas� i�in y�ksek olan de�eri ald�k
        // ekran dar ve uzun ise = geni�li�e g�re �ekil
        // ekran geni� ise = y�ksekli�e g�re ayarla.

        // Kameray� ortala
        float centerX = (gridWidth - 1) / 2f;
        float centerY = (gridHeight - 1) / 2f;

        _camera.transform.position = new Vector3(centerX, centerY, -10f);

        // gridin tam ortas�n� bulup kameray� konumland�r�yoruz.


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
