using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class TileManager : MonoBehaviour
    {
        private List<Vector3> path;

        public GameObject tile_prefab;
        public GameObject buy_menu;
        public GameObject tile_parent;

        private GoldManager gold_m;

        public Tile current_tile;
        public Tile activated_tile;

        private void Start()
        {
            this.gold_m = this.GetComponent<GoldManager>();
            InitializePath();
            SpawnTiles();
        }

        private void Update()
        {
            Tile tile_hit = RayMouseToTile();

            if (tile_hit == null || !tile_hit.occupied)
            {
                HighlightGestion(tile_hit);
                SetCurrentTile(tile_hit);
            }
            
            OnClick();
        }

        private void OnClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(this.current_tile != null && this.activated_tile != null)
                {
                    this.activated_tile.SetTileShader(this.activated_tile.shader_not_highlight);
                }
                if (this.current_tile != null && !this.current_tile.occupied)
                {
                    this.activated_tile = this.current_tile;
                    this.activated_tile.SetTileShader(this.current_tile.shader_highlight);
                    DisplayMenu(true);
                }
            }
        }

        private void HighlightGestion(Tile hit)
        {
            if (this.current_tile != hit)
            {
                if (this.current_tile != null && this.current_tile != this.activated_tile)
                {
                    this.current_tile.SetTileShader(this.current_tile.shader_not_highlight);
                }
                if (hit != null)
                {
                    hit.SetTileShader(hit.shader_highlight);
                }
            }
        }

        private void SetCurrentTile(Tile tile)
        {
            this.current_tile = tile;
        }

        private Tile RayMouseToTile()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = LayerMask.GetMask("Tile");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                return hit.collider.gameObject.GetComponent<Tile>();
            }

            return null;
        }

        private void InstantiateTurret(GameObject prefab)
        {
            Instantiate(prefab, this.activated_tile.turret_spawn_location, Quaternion.identity);
            this.activated_tile.occupied = true;
            DisplayMenu(false);
        }

        public void DisplayMenu(bool activated)
        {
            if(!activated)
            {
                this.activated_tile.SetTileShader(this.activated_tile.shader_not_highlight);
                this.activated_tile = null;
            }

            this.buy_menu.SetActive(activated);
        }

        private void SpawnTiles()
        {
            for (int x = -75; x <= 75; x += 10)
            {
                for (int y = -75; y <= 75; y += 10)
                {
                    Vector3 vec = new Vector3(x, 0.5f, y);
                    if (CheckCoordinateAvailable(vec))
                    {
                        GameObject tile_temp = Instantiate(tile_prefab, vec, Quaternion.identity, tile_parent.transform);
                        tile_temp.name = "Tile " + x + " / " + y;
                    }
                }
            }
        }

        private bool CheckCoordinateAvailable(Vector3 vec)
        {
            bool res = true;

            foreach (Vector3 coor in this.path)
            {
                if (coor.x == vec.x && coor.y == vec.y && coor.z == vec.z)
                {
                    res = false;
                }
            }

            return res;
        }

        public void SpawnTurret(GameObject prefab)
        {
            TurretBehavior turret = prefab.GetComponent<TurretBehavior>();
            if (this.gold_m.gold >= turret.stats.cost)
            {
                this.gold_m.gold -= turret.stats.cost;
                InstantiateTurret(prefab);
            }
        }

        private void InitializePath()
        {
            this.path = new List<Vector3>
            {
                new Vector3(-75, 0.5f, 5),
                new Vector3(-65, 0.5f, 5),
                new Vector3(-55, 0.5f, 5),
                new Vector3(-55, 0.5f, 15),
                new Vector3(-55, 0.5f, 25),
                new Vector3(-55, 0.5f, 35),
                new Vector3(-45, 0.5f, 35),
                new Vector3(-35, 0.5f, 35),
                new Vector3(-25, 0.5f, 35),
                new Vector3(-15, 0.5f, 35),
                new Vector3(-15, 0.5f, 25),
                new Vector3(-15, 0.5f, 15),
                new Vector3(-15, 0.5f, 5),
                new Vector3(-15, 0.5f, -5),
                new Vector3(-15, 0.5f, -15),
                new Vector3(5, 0.5f, -15),
                new Vector3(-5, 0.5f, -15),
                new Vector3(15, 0.5f, -15),
                new Vector3(25, 0.5f, -15),
                new Vector3(25, 0.5f, -25),
                new Vector3(25, 0.5f, -35),
                new Vector3(35, 0.5f, -35),
                new Vector3(45, 0.5f, -35),
                new Vector3(45, 0.5f, -25),
                new Vector3(45, 0.5f, -15),
                new Vector3(45, 0.5f, -5),
                new Vector3(45, 0.5f, 5),
                new Vector3(35, 0.5f, 5),
                new Vector3(25, 0.5f, 5),
                new Vector3(25, 0.5f, 15),
                new Vector3(25, 0.5f, 25),
                new Vector3(15, 0.5f, 25),
                new Vector3(15, 0.5f, 35),
                new Vector3(15, 0.5f, 45),
                new Vector3(15, 0.5f, 55),
                new Vector3(15, 0.5f, 65),
                new Vector3(25, 0.5f, 65),
                new Vector3(35, 0.5f, 65),
                new Vector3(45, 0.5f, 65),
                new Vector3(55, 0.5f, 65),
                new Vector3(55, 0.5f, 55),
            };
        }
    }
}
