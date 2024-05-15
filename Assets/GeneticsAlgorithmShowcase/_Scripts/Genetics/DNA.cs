using NDRLiteFPS;

using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

namespace GeneticsAlgorithm
{

    public class DNA : Interactable
    {

        SpriteRenderer sRenderer;
        Collider sCollider;

        public float TimeToDie { get; private set; } = 0;
        public List<Gene> Genes { get; private set; }
        private void Awake()
        {
            Genes = new List<Gene>()
            {
                new (Random.Range(0.0f, 1.0f), "Red"),
                new (Random.Range(0.0f, 1.0f), "Green"),
                new (Random.Range(0.0f, 1.0f), "Blue"),
                new (Random.Range(0.03f, 0.08f), "Scale")
            };
        }
        void Start()
        {
            Genes = new List<Gene>()
            {
                new (Random.Range(0.0f, 1.0f), "Red"),
                new (Random.Range(0.0f, 1.0f), "Green"),
                new (Random.Range(0.0f, 1.0f), "Blue"),
                new (Random.Range(0.03f, 0.08f), "Scale")
            };

            var redGene = Genes.Find(gene => gene.Name == "Red");
            var greenGene = Genes.Find(gene => gene.Name == "Green");
            var blueGene = Genes.Find(gene => gene.Name == "Blue");
            var scaleGene = Genes.Find(gene => gene.Name == "Scale");

            sRenderer = GetComponent<SpriteRenderer>();
            sCollider = GetComponent<Collider>();

            sRenderer.color = new Color(redGene.Value, greenGene.Value, blueGene.Value);
            this.transform.localScale = new Vector3(scaleGene.Value, scaleGene.Value, scaleGene.Value);
        }

        public override void Interact()
        {
            base.Interact();
            Die();
        }

        public void Die()
        {
            TimeToDie = PopulationManager.Elapsed;
            sRenderer.enabled = false;
            sCollider.enabled = false;

            PopulationManager.instance.ResetFireBurn(transform);
        }


        public void ShuffleDNA()
        {
            foreach (Gene gene in Genes)
            {
                if (gene.Name == "Scale")
                    gene.Value = Random.Range(0.03f, 0.08f);
                else
                    gene.Value = Random.Range(0.0f, 1.0f);
            }
        }

        public void Bread(DNA father, DNA mother)
        {
            for (int i = 0; i < Genes.Count; i++)
                Genes[i].Value = Random.Range(0, 10) < 5 ? father.Genes[i].Value : mother.Genes[i].Value;
        }
    }
}