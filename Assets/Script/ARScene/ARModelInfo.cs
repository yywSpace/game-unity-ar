using GoogleARCore;
using UnityEngine;

/// <summary>
/// 包含一个被创建的AR物体的所哟信息
/// </summary>
public class ARModelInfo
{
    public Anchor Anchor { get; set; }
    public GameObject ARGameObject { get; set; }
    public RotateAndUpDown RotateAndUpDown { get; set; }
    public TransfromAroundAndDistance TransfromAroundAndDistance { get; set; }
    public DoubleCickChangeStatus DoubleCickChangeStatus { get; set; }
    public Pose Pose { get; set; }
}