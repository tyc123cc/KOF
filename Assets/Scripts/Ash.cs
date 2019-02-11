using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ash : MonoBehaviour {
    public Animator m_ani;
	// Use this for initialization
	void Start () {
        m_ani = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
	}
}
