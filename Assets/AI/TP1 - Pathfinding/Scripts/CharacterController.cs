using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP1
{
    public abstract class CharacterController : MonoBehaviour
    {
        public GameMasterAITP1 gm;

        public float speed_character;

        public List<Node> character_path;

        public Vector3 current_direction;
        public Node node_position;

        private List<LineRenderer> lines_path;

        public void Initialisation()
        {
            this.current_direction = this.transform.position;
        }

        public void Move(Vector3 pos_to_reach)
        {
            //this.transform.position = Vector3.Lerp(this.transform.position, pos_to_reach, this.speed_character * Time.deltaTime);
            this.transform.position = Vector3.MoveTowards(this.transform.position, pos_to_reach, this.speed_character * Time.deltaTime);
        }

        public void MoveAlongPath()
        {
            if (!EqualPosition())
            {
                Move(this.current_direction);
            }
            else
            {
                NextDirection();
                if (this.character_path != null)
                {
                    RemovePath();
                    this.lines_path = this.gm.line_manager.DrawPath(this.character_path);
                }
            }
        }

        private void NextDirection()
        {
            if (!EndOfPathPosition())
            {
                this.current_direction = this.gm.mat_api.NodetoVect3(this.character_path[0]);
                this.character_path.Remove(this.character_path[0]);
            }
        }

        public void SetPath(List<Node> p)
        {
            this.character_path = new List<Node>();
            this.character_path = p;
        }

        private bool EndOfPathPosition()
        {
            if (this.character_path != null)
            {
                return this.character_path.Count == 0;
            }
            return true;
        }

        private bool EqualPosition()
        {
            bool x1 = (this.current_direction.x - this.transform.position.x < 0.1f) && (this.current_direction.x - this.transform.position.x >= 0);
            bool z1 = (this.current_direction.z - this.transform.position.z < 0.1f) && (this.current_direction.z - this.transform.position.z >= 0);

            bool x2 = (this.transform.position.x - this.current_direction.x < 0.1f) && (this.transform.position.x - this.current_direction.x >= 0);
            bool z2 = (this.transform.position.z - this.current_direction.z < 0.1f) && (this.transform.position.z - this.current_direction.z >= 0);

            return (x1 && z1 || x2 && z2 || x1 && z2 || x2 && z1);
        }

        public void RemovePath()
        {
            if (this.lines_path != null && this.lines_path.Count > 0)
            {
                foreach (LineRenderer lr in this.lines_path)
                {
                    Destroy(lr);
                }
                this.lines_path = new List<LineRenderer>();
            }
        }
    }
}