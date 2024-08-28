using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length; 
    private Vector2 startpos;
    public GameObject cam;
    [SerializeField] private float parallaxDepthX;
    [SerializeField] private float parallaxDepthY;

    void Start()
    {
        startpos = transform.position;
        cam = GameObject.Find("Main Camera");
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        float loop = cam.transform.position.x * (1 - parallaxDepthX);
        float distX = cam.transform.position.x * parallaxDepthX;
        float distY = cam.transform.position.y * parallaxDepthY;

        transform.position = new Vector3(startpos.x + distX, startpos.y + distY, transform.position.z);

        if (loop > startpos.x + length) startpos.x += length;
        else if (loop < startpos.x - length) startpos.x -= length;
    }
}
