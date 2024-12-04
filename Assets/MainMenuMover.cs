using System.Collections;
using UnityEngine;

public class MainMenuMover : MonoBehaviour
{
    private bool logoUp;
    public GameObject logo;
    public GameObject button;

    private bool gamesLeft = false;
    public GameObject games;
    public GameObject menuScreen;


    private void FixedUpdate()
    {
        if (logoUp && (logo.GetComponent<RectTransform>().position.y > 100))
        {
            logo.GetComponent<RectTransform>().position += new Vector3(0, 1.8f, 0);
            button.GetComponent<RectTransform>().position -= new Vector3(0, 1.8f, 0);
        }
    }

    public void startRoutine()
    {
        StartCoroutine(plannedRoutine());
    }

    IEnumerator plannedRoutine()
    {
        logoUp = true;
        yield return new WaitForSeconds(2.5f);
        menuScreen.SetActive(true);
        yield return new WaitForSeconds(1);
        if (!gamesLeft)
        {
            games.GetComponent<Animator>().Play("ScreenDown");
            gamesLeft = true;
        }
    }
}
