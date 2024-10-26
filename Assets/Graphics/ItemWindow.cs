using UnityEngine;

public class ItemWindow : MonoBehaviour
{
    private void Delete()
    {
        FindObjectOfType<Pausing>().TextUnpause();
        Destroy(gameObject);
    }
}
