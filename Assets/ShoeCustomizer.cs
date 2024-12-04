using UnityEngine;

public class ShoeCustomizer : MonoBehaviour
{
    public Material redMaterial;
    public Material blueMaterial;
    public Material greenMaterial;
    public Material purpleMaterial;
    public Material orangeMaterial;
    public Material rainbowMaterial;

    private void Start()
    {
        ShoeManager.shoeMaterialLeft = redMaterial;
        ShoeManager.shoeMaterialRight = redMaterial;
    }

    public void setRedMaterial(string foot)
    {
        if (foot == "Left")
        {
            ShoeManager.shoeMaterialLeft = redMaterial;
        }
        else
        {
            ShoeManager.shoeMaterialRight = redMaterial;
        }
    }

    public void setBlueMaterial(string foot)
    {
        if (foot == "Left")
        {
            ShoeManager.shoeMaterialLeft = blueMaterial;
        }
        else
        {
            ShoeManager.shoeMaterialRight = blueMaterial;
        }
    }

    public void setGreenMaterial(string foot)
    {
        if (foot == "Left")
        {
            ShoeManager.shoeMaterialLeft = greenMaterial;
        }
        else
        {
            ShoeManager.shoeMaterialRight = greenMaterial;
        }
    }

    public void setPurpleMaterial(string foot)
    {
        if (foot == "Left")
        {
            ShoeManager.shoeMaterialLeft = purpleMaterial;
        }
        else
        {
            ShoeManager.shoeMaterialRight = purpleMaterial;
        }
    }

    public void setOrangeMaterial(string foot)
    {
        if (foot == "Left")
        {
            ShoeManager.shoeMaterialLeft = orangeMaterial;
        }
        else
        {
            ShoeManager.shoeMaterialRight = orangeMaterial;
        }
    }

    public void setRainbowMaterial(string foot)
    {
        if (foot == "Left")
        {
            ShoeManager.shoeMaterialLeft = rainbowMaterial;
        }
        else
        {
            ShoeManager.shoeMaterialRight = rainbowMaterial;
        }
    }


}
