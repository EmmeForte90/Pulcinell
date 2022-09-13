using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformdatabase : MonoBehaviour
{
	public static Platformdatabase Instance = null;

	[Header("Ripristino per piattaforma che cade")]

	[SerializeField]
	GameObject platformPrefab;
	[SerializeField]
	public float Delay;

	[Header("Apparizione piattaforma casuale")]

	[SerializeField]
	GameObject[] platforms;

	int numberOfPlatforms;

	int toggleTime;

	[SerializeField]
	float cycleTime = 2f;

	// Inizializzazione
	void Start()
	{
		//Numero delle piattaforme che aarirano e compariranno random
		numberOfPlatforms = platforms.Length;

		if (numberOfPlatforms - 1 == 0)
			toggleTime = 1;
		else
			toggleTime = numberOfPlatforms - 1;

		StartCoroutine(StartManagingPlatforms());
	}

	//Calcolo
	IEnumerator StartManagingPlatforms()
	{
		for (int i = 0; i < numberOfPlatforms; i++)
		{
			StartCoroutine(ManagePlatform(platforms[i]));
			yield return new WaitForSeconds(cycleTime);
		}
	}

	//Ciclo temporale di calcolo quando una piattaforma si attiva e un altra si disattiva
	IEnumerator ManagePlatform(GameObject platform)
	{
		while (true)
		{
			platform.SetActive(true);
			yield return new WaitForSeconds(cycleTime);
			platform.SetActive(false);
			yield return new WaitForSeconds(toggleTime * cycleTime);
		}
	}
	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(gameObject);

	}
	
	

	IEnumerator SpawnPlatform(Vector2 spawnPosition)
	{
		yield return new WaitForSeconds(Delay);
		Instantiate(platformPrefab, spawnPosition, platformPrefab.transform.rotation);
	}


}
