using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    float m_leftLimit = -6.5f;
    float m_rightLimit = 8.5f;

    public Transform m_player;
    public Transform m_enemy;
	// Use this for initialization
	void Start () {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
	}
	
	// Update is called once per frame
	void Update () {
        float cameraPosX = (m_player.position.x + m_enemy.position.x) / 2;
        if (cameraPosX < m_leftLimit)
        {
            cameraPosX = m_leftLimit;
        }
        else if (cameraPosX > m_rightLimit)
        {
            cameraPosX = m_rightLimit;
        }
        transform.position = new Vector3(cameraPosX, transform.position.y, transform.position.z);
	}
}
