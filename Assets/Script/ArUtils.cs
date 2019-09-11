using Mapbox.Utils;
using UnityEngine;

namespace Script
{
    public static class ArUtils
    {
        public static Vector3 GetObjectSize(GameObject go)
        {
            Vector3 realSize = Vector3.zero;

            Mesh mesh = go.GetComponent<MeshFilter>().mesh;
            if (mesh == null)
            {
                return realSize;
            }
            // 它模型网格的大小
            Vector3 meshSize = mesh.bounds.size;
            // 它的放缩
            Vector3 scale = go.transform.lossyScale;
            // 它在游戏中的实际大小
            realSize = new Vector3(meshSize.x * scale.x, meshSize.y * scale.y, meshSize.z * scale.z);

            return realSize;
        }

        public static Object LoadModel(string modelPath)
        {
            Object obj = Resources.Load(modelPath);
            return obj;
        }

        //地球半径，单位米
        private const double EARTH_RADIUS = 6378137;
        /// <summary>
        /// 计算两点位置的距离，返回两点的距离，单位：米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="latlng1">x代表纬度，y代表精度</param>
        /// <param name="latlng2"></param>
        /// <returns></returns>
        public static double GetDistance(Vector2d latlng1 , Vector2d latlng2)
        {
            float radLat1 = (float)latlng1.x * Mathf.Deg2Rad;
            float radLng1 = (float)latlng1.y * Mathf.Deg2Rad;
            float radLat2 = (float)latlng2.x * Mathf.Deg2Rad;
            float radLng2 = (float)latlng2.y * Mathf.Deg2Rad;
            float a = radLat1 - radLat2;
            float b = radLng1 - radLng2;
            double result = 2 * Mathf.Asin(Mathf.Sqrt(Mathf.Pow(Mathf.Sin(a / 2), 2) + Mathf.Cos(radLat1) * Mathf.Cos(radLat2) * Mathf.Pow(Mathf.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;
        }
    }
}
