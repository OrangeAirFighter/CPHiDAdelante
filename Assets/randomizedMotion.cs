using UnityEngine;

public class randomizedMotion : MonoBehaviour
{
    public Vector3 motionRange = new Vector3(0.1f, 0.1f, 0.1f);
    public float speed = 1.0f;

    private Vector3 originalPosition;
    private Vector3 noiseOffset;

    void Start()
    {
        // Store the original position of the object
        originalPosition = transform.position;

        // Generate a unique offset for each instance
        noiseOffset = new Vector3(
            Random.Range(0f, 100f),
            Random.Range(0f, 100f),
            Random.Range(0f, 100f)
        );
    }

    void Update()
    {
        // Calculate offsets using Perlin noise with unique seeds
        float offsetX = Mathf.PerlinNoise(Time.time * speed + noiseOffset.x, noiseOffset.y) * 2 - 1;
        float offsetY = Mathf.PerlinNoise(noiseOffset.x, Time.time * speed + noiseOffset.z) * 2 - 1;
        float offsetZ = Mathf.PerlinNoise(Time.time * speed + noiseOffset.z, noiseOffset.y) * 2 - 1;

        // Apply the motion within the specified range
        Vector3 randomOffset = new Vector3(offsetX, offsetY, offsetZ);
        transform.position = originalPosition + Vector3.Scale(randomOffset, motionRange);
    }
}
