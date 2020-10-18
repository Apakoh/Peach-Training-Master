using System.Collections.Generic;
using UnityEngine;

namespace AI_TP1
{
    public class LineManager : MonoBehaviour
    {
        public List<LineRenderer> list_lines = new List<LineRenderer>();
        public GameObject lines_parent;
        public LineRenderer line_map;
        public LineRenderer line_path;

        public GameMasterAITP1 gm;

        public LineRenderer LineSpawn(Vector3 g1, Vector3 g2, float y, LineRenderer line)
        {
            Vector3 g1_bis = new Vector3(g1.x, y, g1.z);
            Vector3 g2_bis = new Vector3(g2.x, y, g2.z);
            this.line_map.SetPosition(0, g1_bis);
            line_map.SetPosition(1, g2_bis);
            LineRenderer linetemp = Instantiate(line);
            linetemp.transform.parent = this.lines_parent.transform;
            return linetemp;
        }

        public void DrawMap()
        {
            RemoveMapLines();
            List<Node> visited_node = new List<Node>();

            foreach (Node n in this.gm.mat_graph)
            {
                foreach (Node neighbor in n.GetNeightboors())
                {
                    if (!visited_node.Contains(neighbor))
                    {
                        MapLine(this.gm.mat_api.NodetoVect3(n), this.gm.mat_api.NodetoVect3(neighbor), -1f);
                    }
                }
                visited_node.Add(n);
            }
        }

        public List<LineRenderer> DrawPath(List<Node> path)
        {
            List<LineRenderer> list = new List<LineRenderer>();
            for (int i = 0; i < path.Count; i++)
            {
                if (path[i] != path[path.Count - 1])
                {
                    list.Add(PathLine(this.gm.mat_api.NodetoVect3(path[i]), this.gm.mat_api.NodetoVect3(path[i + 1]), 0f));
                }
            }
            return list;
        }

        private LineRenderer PathLine(Vector3 g1, Vector3 g2, float y)
        {
            return LineSpawn(g1, g2, y, this.line_path);
        }

        private void MapLine(Vector3 g1, Vector3 g2, float y)
        {
            LineRenderer line_temp = LineSpawn(g1, g2, y, line_map);
            Gradient grad = new Gradient();
            grad.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
            );
            line_temp.colorGradient = grad;
            this.list_lines.Add(line_temp);
        }

        private void RemoveMapLines()
        {
            if (this.list_lines != null && this.list_lines.Count > 0)
            {
                foreach (LineRenderer ln in this.list_lines)
                {
                    Destroy(ln);
                }
            }
        }
    }
}