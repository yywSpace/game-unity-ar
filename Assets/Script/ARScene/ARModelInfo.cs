using GoogleARCore;
using Script.PutModelScene;
using UnityEngine;

namespace Script.ARScene
{
    /// <summary>
    /// 包含一个被创建的AR物体的所哟信息
    /// </summary>
    public class ArModelInfo
    {
        public Anchor Anchor { get; set; }
        public GameObject ArGameObject { get; set; }
        public RotateAndUpDown RotateAndUpDown { get; set; }
        public TransfromAroundAndDistance TransfromAroundAndDistance { get; set; }
        public DoubleClickChangeStatus DoubleClickChangeStatus { get; set; }
        public Pose Pose { get; set; }
    }
}