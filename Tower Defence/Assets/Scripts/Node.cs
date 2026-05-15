using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;
    public Turret CurrentTurret { get; private set; } = null;

    [Header("Visuals")]
    public Color hoverColor = Color.green;
    public Color occupiedColor = new Color(0.8f, 0.2f, 0.2f);

    private Renderer rend;
    private Color startColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    void OnMouseEnter()
    {
        if (IsPointerOverUI() || IsOccupied) return;
        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        if (IsOccupied)
            rend.material.color = occupiedColor;
        else
            rend.material.color = startColor;
    }

    void OnMouseDown()
    {
        if (IsPointerOverUI()) return;

        if (CurrentTurret != null)
        {
            BuildManager.Instance.OpenUpgradeMenu(CurrentTurret, this);
            return;
        }

        if (IsOccupied)
        {
            return;
        }

        BuildManager.Instance.OpenBuildMenu(this);
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    public void Occupy(Turret turret)
    {
        IsOccupied = true;
        CurrentTurret = turret;
        rend.material.color = occupiedColor;
    }

    public void FreeUp()
    {
        IsOccupied = false;
        CurrentTurret = null;
        rend.material.color = startColor;
    }
}