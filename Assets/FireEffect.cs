using UnityEngine;

public class FireEffect : MonoBehaviour
{

    private void Update()
    {
        transform.position += 60 * Time.deltaTime * new Vector3(0, 0.02f, 0);
    }
    public void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
