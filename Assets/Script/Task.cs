using Mapbox.Utils;
using UnityEngine;

namespace Script
{
    public class Task
    {
        public Vector2d TaskLocation { get; set; }
        public string TaskDesc { get; set; }
        public string TaskModelName { get; set; }
        public Quaternion TaskModelRotation { get; set; }
    }
}