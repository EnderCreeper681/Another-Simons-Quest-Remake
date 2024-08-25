using UnityEngine;

public class Afterimage : MonoBehaviour
{
    private float transparency = 1.0f;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private float r = 1;
    [SerializeField] private float g = 1;
    [SerializeField] private float b = 1;
    void Start()
    {
        
    }

    void Update()
    {
        transparency -= Time.deltaTime * 2;
        sr.color = new Color(r, g, b, transparency);
        if(transparency <= 0) { Destroy(gameObject); }
    }
}
