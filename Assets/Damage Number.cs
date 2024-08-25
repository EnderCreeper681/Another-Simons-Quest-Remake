using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    private float numberTimer;
    void Update()
    {
        transform.position += 60 * Time.deltaTime * new Vector3(0, 0.06f, 0);
        numberTimer += Time.deltaTime;
        if(numberTimer > 1)
        {
            Destroy(gameObject);
        }
    }
}
