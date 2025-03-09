using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CheckMatches : MonoBehaviour
{
    public List<Tile> FindTileMatches(Tile[,] tiles, int gridX, int gridY)
    {
        List<Tile> matchTile = new List<Tile>();

        for(int i = 0; i < gridY; i++)  // kaç tane satır varsa döner 
        {
            for(int j = 0; j < gridX - 2; j++) // Aynı satırları kontrol eder 
            //( İlk döngü birinci satırı döner İKİNCİ döngü o satırı kotnrol eder);
            {

                Tile firstMatch = tiles[j, i];
                Tile secondMatch = tiles[j + 1, i];    
                Tile thirdMatch = tiles[j + 2, i];
                
                if(firstMatch != null && secondMatch != null && thirdMatch != null)
                {
                    if(firstMatch.TileID == secondMatch.TileID && secondMatch.TileID == thirdMatch.TileID)
                    {
                        matchTile.Add(firstMatch);
                        matchTile.Add(secondMatch);
                        matchTile.Add(thirdMatch);

                        
                        Debug.Log("MATCH HORİZONTAL");
                        Debug.Log($"MATCH FOUND (HORIZONTAL) at ({i},{j}), ({i + 1},{j}), ({i + 2},{j})");
                    }
                }
            } 

        }
        // Satır Aaşağı doğru iner  o yüzden kaç tane olduğunu Y dönerek görüyoruz sonra onları kotnrol


        for(int x=0; x < gridX; x++) // bu sütünlarda ilerler (kaç tane sütün varsa o akdar döner)
        {
            for(int y=0; y < gridY - 2; y++)  // Aynı sütünları kontrol eder
            {
                Tile firstMatchY = tiles[x,y];
                Tile secondMatchY = tiles[x , y + 1];
                Tile thirdMatchY = tiles[x , y + 2];
                
                if(firstMatchY != null && secondMatchY != null && thirdMatchY != null)
                {
                    if(firstMatchY.TileID == secondMatchY.TileID && secondMatchY.TileID == thirdMatchY.TileID)
                    {
                        matchTile.Add(firstMatchY);
                        matchTile.Add(secondMatchY);
                        matchTile.Add(thirdMatchY);

                        Debug.Log($"MATCH FOUND (VERTICAL) at ({x},{y}), ({x},{y + 1}), ({x},{y + 2})");

                    }
                }
            }

        }

        // Sütün yatay ilerler kaç tane olduğunu X den görüyoruz sonra kontrol ediyoruz
        return matchTile;  // Eşleşen taşların Listesini döndürüyoruz
    }

    public Tile FindHint(Tile[,] tiles, int gridX, int gridY)
    {
        for (int x = 0; x < gridX; x++) // sütünları kontrol ediyoruz
        { 
            for (int y = 0; y < gridY; y++)  // satıları kontrol ediyoruz
            {
                // en soldan en sağa kontrol  En yukarıdan en aşağıya kontrol

                Tile currentTile = tiles[x, y];
                if (currentTile == null) continue;
                // Kontrolden sonra taş yok o satırda o satırı atla

                // Sağa değişim kontrolü (Swap Right)
                if (x < gridX - 1) // Grid en sağ sütündaysak -1 yapmayız zaten en sondanyız
                {
                    Tile swappedTile = tiles[x + 1, y]; // Diğer taş
                    SwapTiles(tiles, x, y, x + 1, y);  // mevcut tile ile sağdakini takas ederiz
                    var matchedTiles = FindTileMatches(tiles, gridX, gridY);
                    // Sonra bu eşleşme bir oluşturdumu 

                    SwapTiles(tiles, x, y, x + 1, y); 
                    // Takası geri alırım sonra 

                    if (matchedTiles.Contains(swappedTile)) // Eğer eşleşme içinde bu taş varsa
                    {
                        return swappedTile; // Eşleşmeyi tamamlayan taş
                    }
                    if (matchedTiles.Contains(currentTile)) // Eğer kendi taşı eşleşmeyi tamamlıyorsa
                    {
                        return currentTile;
                    }
                }

                // Aşağı değişim kontrolü (Swap Down)
                if (y < gridY - 1)
                {
                    Tile swappedTile = tiles[x, y + 1];
                    SwapTiles(tiles, x, y, x, y + 1);
                    var matchedTiles = FindTileMatches(tiles, gridX, gridY);
                    SwapTiles(tiles, x, y, x, y + 1); // Swap'ı geri al

                    if (matchedTiles.Contains(swappedTile))
                    {
                        return swappedTile;
                    }
                    if (matchedTiles.Contains(currentTile))
                    {
                        return currentTile;
                    }
                }
            }
        }

        return null; // Hiçbir eşleşme bulunamazsa null döndür
    }
    private void SwapTiles(Tile[,] tiles, int x1, int y1, int x2, int y2)
    {
        Tile temp = tiles[x1, y1];
        tiles[x1, y1] = tiles[x2, y2];
        tiles[x2, y2] = temp;
    }
}
