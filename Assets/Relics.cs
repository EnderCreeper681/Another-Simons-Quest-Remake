using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Relics : MonoBehaviour 
{
    public string title;
    public string description;
    public GameObject descriptionWindow;
    private Pausing pausing;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private GameObject descriptionText;
    [SerializeField] private bool isSubweapon;
    

    private void Start()
    {
        Stats stats = FindObjectOfType<Stats>();
        Attacking attacking = FindObjectOfType<Attacking>();
        pausing = FindObjectOfType<Pausing>();
        if (stats.relics.Contains(title) && !isSubweapon) { Destroy(gameObject); }

        foreach (Subweapon sub in attacking.subweaponsNew) 
        { 
            if (sub.title == title && isSubweapon) { Destroy(gameObject); }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Stats stats = collider.GetComponent<Stats>();
            Attacking attacking = collider.GetComponent<Attacking>();


            Subweapon thisSubweapon = new Subweapon();
            thisSubweapon.title = title;
            thisSubweapon.description = description;
            thisSubweapon.icon = GetComponent<SpriteRenderer>().sprite;

            if (!isSubweapon) { stats.relics.Add(title); }          
            else { attacking.subweaponsNew.Add(thisSubweapon); }



            titleText.text = title;
            Instantiate(descriptionWindow, Camera.main.transform.position + new Vector3(0, 0, 10), Quaternion.identity);
            GameObject desc = Instantiate(descriptionText, Camera.main.transform.position + new Vector3(0, 0, 10), Quaternion.identity);
            


            
            desc.GetComponent<TMP_Text>().text = description;
                
            
            pausing.TextPause();
            Destroy(gameObject);
        }
    }
}