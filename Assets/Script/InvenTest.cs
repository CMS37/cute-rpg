using UnityEngine;
using Managers;

public class InventoryTest : MonoBehaviour
{
    void Start()
    {
        var inv = InventoryManager.Instance;
        inv.Add("1001", 3);
        inv.Add("1002", 5);
        inv.Add("2001", 2);
    }
}
