using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Tile : MonoBehaviour
{
    private Color _originColor;   // Start Color 
    private SpriteRenderer _spriteRenderer;  
    private int _gridX, _gridY; // Tilelarýn Grdi üzerinde ki yeri 
    private GameManager _gameManager;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originColor = _spriteRenderer.color;
    }

    private void OnMouseDown()
    {
       GameEvent.OnClickEvents?.Invoke(this);
        // Mouse Sol Týk Events
    }

    public void Initialize(int x,int y , GameManager manager)
    {
        _gridX = x;
        _gridY = y;
        _gameManager = manager;
    }
    private void SelectTile(Tile tile)
    {
        if(tile == this)  // Param olarak aldýðý Tile'ý gri yap
        {
            _spriteRenderer.color = Color.gray;  // Seçili Taþý gri yaptýk

        }
    }

    private void DeSelectedTile(int index) // Param olarak aldýðý Tile' eski rengi yap
    {
           _spriteRenderer.color = _originColor;

    }


    private void OnEnable() => RegisterEvents();
    private void OnDisable() => UnRegisterEvents();

    private void RegisterEvents()
    {
        GameEvent.UnSelectsTile += DeSelectedTile;
        GameEvent.SelectsTile += SelectTile;
    }
    private void UnRegisterEvents()
    {
        GameEvent.UnSelectsTile -= DeSelectedTile;
        GameEvent.SelectsTile -= SelectTile;
    }
    //  Seçim SOnrasý rengi eski haline getirme Eventi




}
