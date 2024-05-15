using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    public class SurfaceBehaviour : MonoBehaviour
    {
        [SerializeField]
        SurfaceType surfaceType = SurfaceType.Generic;
        public SurfaceType GetSurfaceType { get { return surfaceType; } }
    }
}