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
                        Debug.Log("MATCH");
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
                        Debug.Log("MATCH");

                    }
                }
            }
        }
        // Sütün yatay ilerler kaç tane olduğunu X den görüyoruz sonra kontrol ediyoruz
        return matchTile;  // Eşleşen taşların Listesini döndürüyoruz
    }


}
