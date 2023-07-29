using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;

    [Header("Level Data")]
    [SerializeField] LevelData levelDataSO;
    public float levelWidth = 28f;

    [Header("Containers")]
    [SerializeField] GameObject mapContainer;
    [SerializeField] GameObject groundContainer;
    [SerializeField] GameObject gateContainer;
    [SerializeField] GameObject enemyContainer;
    [SerializeField] GameObject endPortalPrefab;

    [Header("Platforms")]
    [SerializeField] GameObject bigPlatformPrefab;
    [SerializeField] float[] bigPlatformSpawnPositionX;
    public float bigPlatformMinSpacingY = 9f;
    public float bigPlatformMaxSpacingY = 12f;

    [Header("Gates")]
    [SerializeField] float[] gateSpawnPositionX;

    [Header("Vegetation")]
    public List<Vegetation> vegetationList;

    [Header("Enemies")]
    [SerializeField] private float enemyYOffset = 8f;
    [SerializeField] private float minSpawnPositionX = -8f;
    [SerializeField] private float maxSpawnPositionX = 8f;

    private int numBigPlatforms;
    private int gateDensityBigPlatforms = 5;
    private int numSmallPlatforms = 100;
    private Vector2 lastBigPlatformPosition = Vector2.zero;
    private GameObject lastBigPlatform;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        numBigPlatforms = levelDataSO.levels[levelDataSO.currentLevel - 1].numBigPlatforms;
        gateDensityBigPlatforms = levelDataSO.levels[levelDataSO.currentLevel - 1].gateDensityBigPlatforms;
        numSmallPlatforms = levelDataSO.levels[levelDataSO.currentLevel - 1].numSmallPlatforms;

        // Generate layers
        for (int i = 0; i < numBigPlatforms; i++)
        {
            GenerateLayer(i);
        }

        // End Game
        Vector2 endPortalPosition = new Vector2(0f, lastBigPlatformPosition.y + bigPlatformMinSpacingY);
        GameObject spawnedEndPortal = Instantiate(endPortalPrefab, endPortalPosition, Quaternion.identity, mapContainer.transform);
    }

    void GenerateLayer(int index)
    {

        // Generate big platforms
        int sideMultiplier = UnityEngine.Random.Range(0, 2);
        sideMultiplier = sideMultiplier == 0 ? -1 : 1;
        lastBigPlatform = GenerateBigPlatform(index, sideMultiplier);

        // Generate vegetation
        GenerateVegetationLayer();

        // Gate dependent values
        int generateGateRand = UnityEngine.Random.Range(0, gateDensityBigPlatforms);
        if (generateGateRand == 0)
        {
            GameObject gate = GenerateGate(sideMultiplier);
            GameObject enemy = GenerateGateEnemies(gate);
        }
    }

    private GameObject GenerateBigPlatform(int index, int sideMultiplier)
    {
        Vector3 spawnPosition = Vector3.zero;
        float offsetY = UnityEngine.Random.Range(bigPlatformMinSpacingY, bigPlatformMinSpacingY);
        spawnPosition.y += lastBigPlatformPosition.y + offsetY;

        if (sideMultiplier == -1)
        {
            spawnPosition = new Vector2(bigPlatformSpawnPositionX[0], spawnPosition.y);
        }
        else if (sideMultiplier == 1)
        {
            spawnPosition = new Vector2(bigPlatformSpawnPositionX[1], spawnPosition.y);
        }

        GameObject platform = Instantiate(bigPlatformPrefab, spawnPosition, Quaternion.identity, groundContainer.transform);
        lastBigPlatformPosition = spawnPosition;

        return platform;
    }

    private void GenerateVegetationLayer()
    {
        // Loop through each scriptable object
        for (int i = 0; i < vegetationList.Count; i++)
        {
            Vegetation vegetation = vegetationList[i];
            GenerateVegetationFromSO(vegetation);
        }
    }

    private void GenerateVegetationFromSO(Vegetation vegetation)
    {
        int numSpawns = UnityEngine.Random.Range(vegetation.minSpawns, vegetation.maxSpawns + 1);
        List<Vector2> spawnPositions = new();
        BoxCollider2D platformBoxCollider = lastBigPlatform.GetComponent<BoxCollider2D>();

        float platformMinPositionX = lastBigPlatform.transform.position.x - (platformBoxCollider.size.x / 2f) + platformBoxCollider.offset.x;
        float platformMaxPositionX = lastBigPlatform.transform.position.x + (platformBoxCollider.size.x / 2f) + platformBoxCollider.offset.x;

        // Loop through all spawns of vegetation
        for (int j = 0; j < numSpawns; j++)
        {
            // Create and instantiate new gameobject
            GameObject spawnedVegetation = new GameObject(vegetation.sprite.name + "_" + j.ToString());
            spawnedVegetation.transform.parent = lastBigPlatform.transform;
            spawnedVegetation.transform.localScale = new Vector3(1f, 1f, 1f);
            //GameObject spawnedVegetation = Instantiate(vegetationGO, Vector3.zero, Quaternion.identity, lastBigPlatform.transform);

            // Create sprite renderer and update values from scriptable object
            SpriteRenderer spriteRenderer = spawnedVegetation.AddComponent<SpriteRenderer>();
            bool randomBool = UnityEngine.Random.Range(0, 2) == 1;
            bool flipSprite = vegetation.canFlipX ? randomBool : false;
            spriteRenderer.sprite = vegetation.sprite;
            spriteRenderer.sortingOrder = vegetation.sortOrder;
            spriteRenderer.flipX = flipSprite;

            // Determine min/max spawn positions
            float minSpawnPositionX = platformMinPositionX + spriteRenderer.bounds.size.x / 2;
            float maxSpawnPositionX = platformMaxPositionX - spriteRenderer.bounds.size.x / 2;
            float positionX = UnityEngine.Random.Range(minSpawnPositionX, maxSpawnPositionX);
            Vector2 spawnPosition = new Vector2(positionX, lastBigPlatform.transform.position.y + lastBigPlatform.transform.localScale.y * vegetation.offsetY);

            // Make sure new position is valid, otherwise don't spawn
            bool validPosition = CheckIfWithinMinSpacing(spawnPosition, spawnPositions, vegetation.minSpacing);
            if (validPosition)
            {
                spawnedVegetation.transform.position = spawnPosition;
                spawnPositions.Add(spawnPosition);
            }
            else
            {
                Destroy(spawnedVegetation);
            }
        }
    }

    private bool CheckIfWithinMinSpacing(Vector2 targetPosition, List<Vector2> positionsList, float minSpacing)
    {
        // Iterate through the list of Vector2s
        foreach (Vector2 position in positionsList)
        {
            // Check if the x component of the current position is within 0.6 units of the target position's x component
            if (Mathf.Abs(targetPosition.x - position.x) <= minSpacing)
            {
                return false;
            }
        }

        return true;
    }

    private GameObject GenerateGate(int sideMultiplier)
    {
        Vector3 spawnPosition = Vector3.zero;
        float offsetY = lastBigPlatform.transform.position.y;

        if (sideMultiplier == -1)
        {
            spawnPosition = new Vector2(gateSpawnPositionX[0], offsetY);
        }
        else if (sideMultiplier == 1)
        {
            spawnPosition = new Vector2(gateSpawnPositionX[1], offsetY);
        }

        int randomGateIndex = UnityEngine.Random.Range(0, GameManager.Instance.gatePrefabs.Length);
        GameObject gatePrefab = GameManager.Instance.gatePrefabs[randomGateIndex];
        GameObject spawnedGate = Instantiate(gatePrefab, spawnPosition, Quaternion.identity, gateContainer.transform);

        return spawnedGate;
    }

    private GameObject GenerateGateEnemies(GameObject gate)
    {
        Vector2 spawnPosition = new Vector2(UnityEngine.Random.Range(minSpawnPositionX, maxSpawnPositionX), gate.transform.position.y - enemyYOffset);
        int randomEnemyIndex = UnityEngine.Random.Range(0, GameManager.Instance.enemyPrefabs.Length);
        GameObject randomEnemy = GameManager.Instance.enemyPrefabs[randomEnemyIndex];
        GameObject spawnedEnemy = Instantiate(randomEnemy, spawnPosition, Quaternion.identity, enemyContainer.transform);
        Key key = gate.GetComponent<Gate>().keySO;
        spawnedEnemy.GetComponent<SpawnedEnemy>().SetEnemyKeyType(key);

        return spawnedEnemy;
    }

    private T GetRandomEnumValue<T>()
    {
        // Get all the values of the enum
        T[] values = (T[])Enum.GetValues(typeof(T));

        // Get a random index
        int randomIndex = UnityEngine.Random.Range(0, values.Length);

        // Return the random enum value
        return values[randomIndex];
    }
}
