using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private bool isPermanent;
    [SerializeField] private string pickupID;
    private Stats stats;
    [SerializeField] private AudioClip heart;
    [SerializeField] private AudioClip gold;
    [SerializeField] private AudioClip maxup;
    [SerializeField] private int value;

    private void Start()
    {
        stats = FindObjectOfType<Stats>();
        if (isPermanent && stats.pickups.Contains(pickupID)) { Destroy(gameObject); }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (CompareTag("Gold"))
            {
                collision.GetComponent<Stats>().gold += value;
                AudioManager.instance.PlaySound(gold, transform, 0.6f);
            }
            if (CompareTag("Heart"))
            {
                for (int i = 1; i <= value; i++)
                {
                    if (collision.GetComponent<Stats>().currentHearts < collision.GetComponent<Stats>().maxHearts) { collision.GetComponent<Stats>().currentHearts++; }
                }
                AudioManager.instance.PlaySound(heart, transform, 1f);
            }
            if (CompareTag("LifeMaxup"))
            {
                collision.GetComponent<Stats>().maxHealth += 30;
                collision.GetComponent<Stats>().currentHealth = collision.GetComponent<Stats>().maxHealth;
            }
            if (CompareTag("HeartMaxup"))
            {
                collision.GetComponent<Stats>().maxHearts += 10;
                collision.GetComponent<Stats>().currentHearts = collision.GetComponent<Stats>().maxHearts;
            }
            if (isPermanent) { stats.pickups.Add(pickupID); }
            Destroy(gameObject);
        }
    }       
}