using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour {
    public Animator m_ani;
	// Use this for initialization
	void Start () {
        m_ani = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        float length = m_ani.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (length >= 1.0f)
        {
            Destroy(gameObject);           
        }
	}
}
