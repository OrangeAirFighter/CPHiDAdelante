using UnityEngine;
using UnityEngine.UI;

public class DifficultySystem : MonoBehaviour
{
    public GameObject weightSlider;
    void Start()
    {
        weightSlider.GetComponent<Slider>().value = ShoeManager.weightFactor;
    }

    void Update()
    {
        ShoeManager.weightFactor = weightSlider.GetComponent<Slider>().value;
    }
}
