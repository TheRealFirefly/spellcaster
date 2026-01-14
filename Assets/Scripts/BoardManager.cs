using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{

    private Tilemap m_Tilemap;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;

    private Grid m_Grid;

    public PlayerController Player;

    public GameObject[] Obstacle;
    public GameObject[] Walls;

    public void Init()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();
      

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                
                Vector2Int cell = new Vector2Int(x, y);
                Vector3 worldPos = CellToWorld(cell);

                bool isWall = (x == 0 || y == 0 || x == Width - 1 || y == Height - 1);

                if (isWall)
                {
                    int index = Random.Range(0, Walls.Length);
                    Instantiate(Walls[index], worldPos, Quaternion.identity, transform);
                    
                    Tile tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
                else
                {
                    Tile tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    

                    // Büsche random spawnen
                    if (Random.value < 0.1f && !(x == Width/2 && y== Height/2))
                    {
                        int index = Random.Range(0, Obstacle.Length);
                        Instantiate(Obstacle[index], worldPos, Quaternion.identity, transform);
                        
                    }
                }
            }
        }

        Player.Spawn(this, new Vector2Int(Width / 2, Height / 2));
    }

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }


    public Vector2Int WorldToCell(Vector2 worldPos)
    {
        Vector3Int cellPos = m_Grid.WorldToCell(worldPos);
        return new Vector2Int(cellPos.x, cellPos.y);
    }
}
