using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Animator m_ani;
    public bool m_hit = false;
    public K enemy;
    public Transform m_player;
    public AudioSource m_audio;
    // Use this for initialization
    void Start()
    {
        m_ani = GetComponent<Animator>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<K>();
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float length = m_ani.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (length >= 1.0f)
        {
            transform.SetParent(m_player);
            transform.localPosition = new Vector3(0.5f, 0.4f, 0f);
            transform.position += Vector3.back;
            gameObject.SetActive(false);

        }
    }

    public void OnBecameVisible()
    {
        m_hit = false;
        transform.position += Vector3.back * 3;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag.CompareTo("Enemy") == 0 && m_hit == false)
        {
            m_hit = true;

            if (collision.transform.GetComponent<K>().m_standUp == true)
            {
                return;
            }
            m_audio.loop = false;
            m_audio.clip = Resources.Load("Sounds/Injured") as AudioClip;
            m_audio.Play();
            enemy.m_stiffnessTime = 0.5f;
            enemy.m_injuredState = State.Injured_01;
            enemy.PlayerState = State.Injured;
        }
    }




}
