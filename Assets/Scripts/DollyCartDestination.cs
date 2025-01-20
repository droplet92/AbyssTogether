using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DollyCartDestination : MonoBehaviour
{
    [SerializeField] private List<GameObject> levels;
    
    void Awake()
    {
        int level = PlayerPrefs.GetInt("level");
        var smoothPath = GetComponent<CinemachineSmoothPath>();
        var destination = smoothPath.m_Waypoints[1];
        
        destination.position = new Vector3
            (
                destination.position.x,
                Math.Max(levels[level - 1].transform.position.y - 800, -2180),
                destination.position.z
            );
        smoothPath.m_Waypoints[1] = destination;
    }
}
