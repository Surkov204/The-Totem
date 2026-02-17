using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject occupant;

    public bool IsOccupied
    {
        get
        {
            if (occupant == null)
            {
                occupant = null;
                return false;
            }

            return true;
        }
    }

    public void Place(GameObject defender)
    {
        occupant = defender;
    }

    public void Clear()
    {
        occupant = null;
    }
}