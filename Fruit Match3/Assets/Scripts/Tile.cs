using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Color _originColor;
    private static Tile[] _selectedTile =  new Tile[2];
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originColor = _spriteRenderer.color;
    }

    private void OnMouseDown()
    {
        if (_selectedTile[0] == this)
        {
            DelesectTile(0);
            return;
        }
        else if (_selectedTile[1] == this)
        {
            DelesectTile(1);
            return;
        }


        if (_selectedTile[0] == null)
        {
            SelectTile(0);
        }
        else if(_selectedTile[1] == null)
        {
            SelectTile(1);
            Debug.Log("Ýki taþ seçildi....");
        }
    }

    private void SelectTile(int v)
    {
        _selectedTile[v] = this;
        _spriteRenderer.color = Color.gray;  // Seçili Taþý gri yaptýk
    }

    private void DelesectTile(int index)
    {
        if (_selectedTile[index]!= null)
        {
            _selectedTile[index]._spriteRenderer.color = _selectedTile[index]._originColor;
            _selectedTile[index] = null;
        }
    }
}
