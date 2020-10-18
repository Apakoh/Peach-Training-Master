using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

namespace MG_TP2
{
    public class ReadFile
    {
        public string[] ReadFileToString(string name_file, string path_relative)
        {
            StreamReader sr = new StreamReader(Application.dataPath + path_relative + name_file);
            string fileContents = sr.ReadToEnd();
            sr.Close();

            string[] lines = fileContents.Split("\n"[0]);

            return lines;
        }

        public void WriteFileOff(Mesh msh, string name_file, string path_relative)
        {
            using (StreamWriter sw = new StreamWriter(Application.dataPath + path_relative + name_file))
            {
                sw.WriteLine("OFF");
                sw.WriteLine(msh.vertexCount + " " + (msh.triangles.Length / 3) + " " + msh.triangles.Length);

                for (int v = 0; v < msh.vertexCount; v++)
                {
                    sw.Write(msh.vertices[v].x.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    sw.Write(" ");
                    sw.Write(msh.vertices[v].y.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    sw.Write(" ");
                    sw.Write(msh.vertices[v].z.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    sw.Write("\n");
                }

                for (int t = 0; t < msh.triangles.Length; t += 3)
                {
                    sw.WriteLine("3 " + msh.triangles[t] + " " + msh.triangles[t + 1] + " " + msh.triangles[t + 2]);
                }

                sw.Close();
            }
        }

        public float ParseFloat(string string_to_convert)
        {
            return float.Parse((string_to_convert).Trim('\r', ' '), CultureInfo.InvariantCulture);
        }

        public int ParseInt(string string_to_convert)
        {
            return int.Parse((string_to_convert).Trim('\r', ' '), CultureInfo.InvariantCulture);
        }
    }
}