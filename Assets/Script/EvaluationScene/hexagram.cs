using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hexagram : MonoBehaviour
{
    public int starsCount;
    // 属性值占总体的比例, 顺时针
    public float[] attributesRatio = { .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f};
    public string[] attributesName;
    public float fullStrength = 100f;
    private Vector3 startDirection = new Vector3(0, 1, 0);
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        CanvasRenderer canvasRenderer = GetComponent<CanvasRenderer>();
        float deltaAngle = 360 / starsCount;
        List<Vector3> vertices = new List<Vector3>
        {
            Vector3.zero
        };
        // 最外层
        for (int i = 0; i < starsCount; i++)
        {
            float angle = deltaAngle * i;
            // 求出每点相对于中心点的方向
            Vector3 direction = Quaternion.AngleAxis(angle, Vector3.forward) * startDirection;
            // 根据方向求出顶点位置，外层
            vertices.Add(direction * fullStrength);
            // 根据方向和长度求出当前属性值定点的位置，内层
            vertices.Add(direction * fullStrength * attributesRatio[i]);
            // 定点外文字说明
            GameObject textGo = Instantiate(Resources.Load("Text")) as GameObject;
            textGo.name = attributesName[i];
            textGo.GetComponent<Text>().text = attributesName[i];
            textGo.transform.parent = transform;
            textGo.transform.localPosition = direction * fullStrength;
         }

        mesh.SetVertices(vertices);
        // 设置索引属性
        int[] indices = new int[3 * starsCount * 2];
        for (int i = 0; i < starsCount; ++i)
        {
            int index = 6 * i;
            // 外层三角形
            indices[index] = 0;
            indices[index + 1] = i * 2 + 1;
            indices[index + 2] = (i * 2 + 3) > starsCount * 2 ? (i * 2 + 3 - starsCount * 2) : (i * 2 + 3);
            // 内层三角形
            indices[index + 3] = 0;
            indices[index + 4] = i * 2 + 2;
            indices[index + 5] = (i * 2 + 4) > starsCount * 2 ? (i * 2 + 4 - starsCount * 2) : (i * 2 + 4);
        }
        // Triangles属性为通过indices中每三点确定一个三角形
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);

        List<Color> colors = new List<Color>
        {
            Color.white
        };
        for (int i = 1; i < vertices.Count; i++)
        {
            if (i % 2 == 0)
                colors.Add(Color.gray);
            else
                colors.Add(Color.cyan);

        }
        mesh.SetColors(colors);

        canvasRenderer.SetMesh(mesh);

        Material material = new Material(Shader.Find("UI/Default"));
        canvasRenderer.SetMaterial(material, null);
    }
}
