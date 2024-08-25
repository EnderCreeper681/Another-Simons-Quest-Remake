using UnityEngine;

public class MermanSpawner : MonoBehaviour
{
    [SerializeField] private GameObject zombie;
    [SerializeField] private float spawnDelay = 2;
    private float spawnTimer;
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool adjustToGround = true;
    [SerializeField] private float firstSpawnDelay = 0;
    [SerializeField] private Movement player;

    private void Start()
    {
        spawnPos = transform.position;
        spawnTimer = firstSpawnDelay;
        player = FindObjectOfType<Movement>();
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (Physics2D.Raycast(spawnPos, Vector2.down, 0.8f, groundLayer) && adjustToGround) { spawnPos.y += 0.2f; }
        if (spawnTimer <= 0)
        {
            spawnPos = transform.position + new Vector3((transform.localScale.x / 2) * player.direction, 0, 0);
            Instantiate(zombie, spawnPos, Quaternion.identity);            
            spawnTimer = spawnDelay;
        }
    }
}