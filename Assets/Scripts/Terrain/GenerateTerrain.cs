using UnityEngine;
using System.Collections;

public class GenerateTerrain : MonoBehaviour {

    public Transform terrainBlock;

    void Start()
    {
        float t = Time.time;
        SimplexNoiseGenerator noiseGenerator = new SimplexNoiseGenerator();
        DestructibleSprite.seed = noiseGenerator.GetSeed();
        for (int x = 0; x < 15; x++)
        {
            Transform block = (Transform)Instantiate(terrainBlock, new Vector3(x, 0) * 2.62f, Quaternion.identity);
        }
    }
}
