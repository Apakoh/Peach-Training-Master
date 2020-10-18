using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class Tile : MonoBehaviour
    {
        public GameObject turret_spawn_go;
        public Vector3 turret_spawn_location;

        private Renderer tile_renderer;
        public Shader shader_not_highlight;

        public Shader shader_highlight;

        public bool occupied;

        private void Start()
        {
            this.occupied = false;
            this.tile_renderer = this.GetComponent<Renderer>();
            this.shader_not_highlight = this.tile_renderer.material.shader;
            this.turret_spawn_location = this.turret_spawn_go.transform.position;
            this.shader_highlight = Shader.Find("Self-Illumin/Outlined Diffuse");
        }

        public void SetTileShader(Shader shader)
        {
            if(this.tile_renderer != null)
            {
                this.tile_renderer.material.shader = shader;
            }            
        }
    }
}

