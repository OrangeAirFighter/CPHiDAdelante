using UnityEngine;

public class UIUPMover : MonoBehaviour
{
    private bool menuUp;
    public GameObject menu;

    private void Update()
    {
        if (menuUp && (menu.GetComponent<RectTransform>().position.y > 100))
        {
            menu.GetComponent<RectTransform>().position += new Vector3(0, 2.2f, 0);
        }

        
    }

    public void startRoutine()
    {
        menuUp = true;
    }

}
