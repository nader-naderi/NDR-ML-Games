using System;

using UnityEngine;

namespace GeneticsAlgorithm
{
    [Serializable]
    public class Gene
    {
        [SerializeField] private float value;
        [SerializeField] private string name;

        public float Value { get => value; set => this.value = value; }
        public string Name { get => name; set => name = value; }

        public Gene(float value, string name)
        {
            Value = value;
            Name = name;
        }
    }
}