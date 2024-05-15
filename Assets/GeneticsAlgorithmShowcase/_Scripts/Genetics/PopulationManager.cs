using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GeneticsAlgorithm
{
	public class PopulationManager : MonoBehaviour
	{

		public static PopulationManager instance;

		[SerializeField] private Transform[] hideouts;
		[SerializeField] private Transform fireBurn;

		[SerializeField] DNA personPrefab;
		[SerializeField] int populationSize = 10;
		[SerializeField] private int trialTime = 10;

		private List<DNA> population = new();

		private int generation = 1;

		private GUIStyle guiStyle = new();
		public Transform FireBurn => fireBurn;
		public static float Elapsed { get; set; } = 0;

		private void Awake()
		{
			instance = this;
		}

		public void ResetFireBurn(Transform target)
		{
			FireBurn.GetComponent<ParticleSystem>().Play();
			FireBurn.position = target.position;
			FireBurn.rotation = target.rotation;
		}

		void OnGUI()
		{
			guiStyle.fontSize = 50;
			guiStyle.normal.textColor = Color.white;

			GUI.Label(new Rect(10, 10, 100, 20), "Generation: " + generation, guiStyle);
			GUI.Label(new Rect(10, 65, 100, 20), "Trial Time: " + (int)Elapsed, guiStyle);
		}

		void Start()
		{
			for (int i = 0; i < populationSize; i++)
			{
				Transform hideout = hideouts[Random.Range(0, hideouts.Length)];

				Vector3 pos = hideout.position;

				DNA agent = Instantiate(personPrefab, pos, hideout.rotation);

				agent.ShuffleDNA();

				population.Add(agent);
			}
		}

		DNA Breed(DNA parent1, DNA parent2)
		{
			Transform hideout = hideouts[Random.Range(0, hideouts.Length)];

			Vector3 pos = hideout.position;

			DNA offspring = Instantiate(personPrefab, pos, hideout.rotation);

			DNA father = parent1.GetComponent<DNA>();
			DNA mother = parent2.GetComponent<DNA>();

			//swap parent dna
			if (Random.Range(0, 1000) > 5)
			{
				offspring.GetComponent<DNA>().Bread(father, mother);
			}
			else
			{
				offspring.GetComponent<DNA>().ShuffleDNA();

			}
			return offspring;
		}

		void BreedNewPopulation()
		{
			List<DNA> newPopulation = new();

			//get rid of unfit individuals
			List<DNA> sortedList = population.OrderByDescending(o => o.GetComponent<DNA>().TimeToDie).ToList();

			population.Clear();

			//breed upper half of sorted list
			for (int i = (int)(sortedList.Count / 2.0f); i < sortedList.Count - 1; i++)
			{
				for (int j = (int)(sortedList.Count / 2.0f) + 1; i < sortedList.Count; i++)
				{
					population.Add(Breed(sortedList[i], sortedList[j]));
					population.Add(Breed(sortedList[j], sortedList[i]));
				}
			}

			for (int i = 0; i < sortedList.Count; i++)
			{
				Destroy(sortedList[i]);
			}

			generation++;
		}

		void Update()
		{
			Elapsed += Time.deltaTime;

			if (Elapsed > trialTime)
			{
				BreedNewPopulation();
				Elapsed = 0;
			}
		}
	}
}