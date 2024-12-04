using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroySelf : MonoBehaviour
{
    private void Start()
    {
        Invoke("destroyer", 0.8f);
    }
    public void destroyer()
    {
        Destroy(gameObject);
    }
}
