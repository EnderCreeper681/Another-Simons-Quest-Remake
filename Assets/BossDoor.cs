using UnityEngine;

public class BossDoor : MonoBehaviour
{
    private Saving saving;
    [SerializeField] private string boss;
    private GameObject activeBoss;
    private float ypos;
    private Enemy bossInfo;

    void Start()
    {
        saving = FindObjectOfType<Saving>();
        if (saving.defeatedBosses.Contains(boss)) { Destroy(gameObject); }
        activeBoss = GameObject.Find(boss);
        if (activeBoss != null) 
        { 
            ypos = transform.position.y - 3f;
            bossInfo = activeBoss.GetComponent<Enemy>();
        }
        else { ypos = transform.position.y; }
    }

    
    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, ypos, 0.1f * Time.deltaTime * 60), transform.position.z);
        if (bossInfo != null && bossInfo.health <= 0) { ypos += 3f; }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<Movement>() != null && activeBoss == null) { ypos += 3f; }
    }
}
