using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    [System.Serializable]
    public struct Footstep
    {
        [SerializeField] string name;
        [SerializeField]
        SurfaceType surfaceType;
        [SerializeField] AudioClip[] stepClips;
        public AudioClip Clip => stepClips[Random.Range(0, stepClips.Length)];
        public SurfaceType SurfaceType { get => surfaceType; }
        public AudioClip[] StepClips { get => stepClips; }
    }
}