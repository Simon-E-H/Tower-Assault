using UnityEngine;

public class CloseBuildMenu : MonoBehaviour
{
    private GameObject BuildMenuUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuildMenuUI = GameObject.Find("BuildMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseMenu()
    {
        Debug.Log("Clicked close menu");
        BuildMenuUI.SetActive(false);
    }
}
