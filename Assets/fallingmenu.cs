using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] characterPrefabs;

    [Header("Spawn Settings")]
    public float spawnIntervalMin = 0.5f;
    public float spawnIntervalMax = 2f;
    public float spawnXMin = -12f;
    public float spawnXMax = 12f;
    public float spawnYMin = 6f;
    public float spawnYMax = 10f;

    private Camera mainCamera;
    private List<GameObject> spawnedCharacters = new List<GameObject>();

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnCharacters());
    }

    void Update()
    {
        CleanupCharacters();
    }

    IEnumerator SpawnCharacters()
    {
        while (true)
        {
            float delay = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(delay);

            float randomX = Random.Range(spawnXMin, spawnXMax);
            float randomY = Random.Range(spawnYMin, spawnYMax);
            Vector3 spawnPos = new Vector3(randomX, randomY, 0f);

            int randomIndex = Random.Range(0, characterPrefabs.Length);
            GameObject newChar = Instantiate(characterPrefabs[randomIndex], spawnPos, Quaternion.identity);

            // Add tracking script logic via tag (optional)
            newChar.AddComponent<TrackEntryAndDestroy>();

            spawnedCharacters.Add(newChar);
        }
    }

    void CleanupCharacters()
    {
        for (int i = spawnedCharacters.Count - 1; i >= 0; i--)
        {
            GameObject character = spawnedCharacters[i];

            if (character == null)
            {
                spawnedCharacters.RemoveAt(i);
                continue;
            }

            Vector3 viewportPos = mainCamera.WorldToViewportPoint(character.transform.position);
            TrackEntryAndDestroy tracker = character.GetComponent<TrackEntryAndDestroy>();

            if (tracker.hasEnteredView && viewportPos.y < 0)
            {
                Destroy(character);
                spawnedCharacters.RemoveAt(i);
            }
        }
    }

    // Internal helper class on spawned objects
    private class TrackEntryAndDestroy : MonoBehaviour
    {
        public bool hasEnteredView = false;

        void Update()
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            if (!hasEnteredView && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1)
            {
                hasEnteredView = true;
            }
        }
    }
}
