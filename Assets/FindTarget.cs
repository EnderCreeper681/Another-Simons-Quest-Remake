using UnityEngine;
using Cinemachine;

public class FindTarget : MonoBehaviour
{
    void Start()
    {
        GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("Simon").transform;
        GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = GameObject.Find("Camera Bounds").GetComponent<Collider2D>();
    }
}
