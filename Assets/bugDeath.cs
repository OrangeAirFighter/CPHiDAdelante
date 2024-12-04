using UnityEngine;

public class bugDeath : MonoBehaviour
{
    void Start()
    {
        Invoke("destruction", 3f);
    }

    public void destruction()
    {
        Destroy(gameObject);
    }
}
