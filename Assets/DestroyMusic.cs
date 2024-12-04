using UnityEngine;

public class DestroyMusic : MonoBehaviour
{
    public GameObject particleExplosion;
    public float rotationSpeed = 50f;

    public string musicColour;

    private float timeElapsed;

    private void Update()
    {
        transform.Rotate(0, -(rotationSpeed * Time.deltaTime), 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UIManager.gameScore += 1;
            switch (musicColour)
            {
                case "Piano":
                    UIManager.piano += 1;
                    break;
                case "Drums":
                    UIManager.drums += 1;
                    break;
                case "Bells":
                    UIManager.bell += 1;
                    break;
                case "Orchestral":
                    UIManager.orchestral += 1;
                    break;
                default:
                    break;
            }
            
            //GameObject bugExplosion = Instantiate(particleExplosion, transform.position, particleExplosion.transform.rotation);
            Destroy(gameObject);
        }
    }
}

