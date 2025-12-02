using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab;   // le prefab "Cloud"
    public Sprite[] cloudSprites;    // tous tes sprites de nuages
    public int cloudCount = 6;       // combien de nuages en même temps

    void Start()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            SpawnOneCloud();
        }
    }

    void SpawnOneCloud()
    {
        if (cloudPrefab == null || cloudSprites == null || cloudSprites.Length == 0)
            return;

        GameObject go = Instantiate(cloudPrefab);

        // sprite random
        var sr = go.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = cloudSprites[Random.Range(0, cloudSprites.Length)];
        }
    }
}