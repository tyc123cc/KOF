using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCollider : MonoBehaviour
{

    public int m_damageType;
    public K enemy;
    public Transform m_player;
    public float m_stiffnessTime;//硬直时间
    public bool m_hit = false;
    public bool m_hide = false;

    public GameObject m_hitEffect;
    public AudioSource m_audio;
    // Use this for initialization
    void Start()
    {      
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform.GetComponent<K>();
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_audio = gameObject.AddComponent<AudioSource>();
        transform.parent = m_player;
        enemy.m_stiffnessTime = m_stiffnessTime;
        //StartCoroutine(CancelStiffness());
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        //string aniName = m_player.GetComponent<K>().m_ani.GetCurrentAnimatorClipInfo(0)[0].clip.name;//正在播放动画名称
        float aniLength = m_player.GetComponent<K>().m_ani.GetCurrentAnimatorStateInfo(0).normalizedTime;//正在播放动画长度
        string spriteName = m_player.GetComponent<K>().m_render.sprite.name;//获得当前帧的名称
        if (aniLength >= 1.0f)
        {
            Destroy(gameObject);
        }
        if (m_damageType == State.Attack_Punch_Ground)//地面拳
        {
            if (spriteName.CompareTo("K’_210-3") == 0)//动态调整碰撞体位置（每帧算）
            {
                m_hide = false;
                transform.localPosition = new Vector3(0.426f, 0.334f, -1f);
            }
            else if (spriteName.CompareTo("K’_210-4") == 0)
            {
                m_hide = false;
                transform.localPosition = new Vector3(0.426f, 0.293f, -1f);
            }
            else if (spriteName.CompareTo("K’_210-5") == 0)
            {
                m_hide = false;
                transform.localPosition = new Vector3(0.426f, 0.293f, -1f);
            }
            else if (spriteName.CompareTo("K’_210-6") == 0)
            {
                m_hide = false;
                transform.localPosition = new Vector3(0.426f, 0.293f, -1f);
            }
            else
            {
                m_hide = true;
            }
        }
        else if (m_damageType == State.Attack_Kick_Ground)//地面腿
        {
            if (spriteName.CompareTo("K’_220-2") == 0)//动态调整碰撞体位置（每帧算）
            {
                m_hide = false;
                transform.localPosition = new Vector3(0.458f, 0.248f, -1f);
            }
            else
            {
                m_hide = true;
            }
        }
        else if (m_damageType == State.Attack_Punch_Sky)//空中拳
        {
            if (spriteName.CompareTo("K’_600-1") == 0)//动态调整碰撞体位置（每帧算）
            {
                m_hide = false;
                transform.localPosition = new Vector3(0.258f, -0.311f, -1f);
            }
            else if (spriteName.CompareTo("K’_600-2") == 0)//动态调整碰撞体位置（每帧算）
            {
                m_hide = false;
                transform.localPosition = new Vector3(0.276f, -0.311f, -1f);
            }
            else
            {
                m_hide = true;
            }
        }
        else if (m_damageType == State.Attack_Kick_Sky)//空中腿
        {
            if (spriteName.CompareTo("K’_620-1") == 0 || spriteName.CompareTo("K’_620-2") == 0)//动态调整碰撞体位置（每帧算）
            {
                m_hide = false;
                transform.localPosition = new Vector3(0.403f, -0.258f, -1f);
            }
            else
            {
                m_hide = true;
            }
        }
    }
    public void ChangeScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {

        if (m_hide == true || m_hit == true)
        {
            return;
        }

        if (collision.transform.tag.CompareTo("Enemy") == 0)
        {
            //print(m_damageType);

            m_hit = true;
          
            if (collision.transform.GetComponent<K>().m_standUp == true)
            {
                return;
            }
            Transform hitEffect = Instantiate(m_hitEffect, transform.position + Vector3.back * 3, transform.rotation).transform;
            hitEffect.eulerAngles = new Vector3(0, m_player.eulerAngles.y + 180, 0);
            m_audio.loop = false;
            m_audio.clip = Resources.Load("Sounds/Injured") as AudioClip;
            m_audio.Play();
            if (m_damageType == State.Attack_Punch_Ground || m_damageType == State.Attack_Kick_Sky || m_damageType == State.Attack_Punch_Sky)
            {
                enemy.m_injuredState = State.Injured_01;
                enemy.PlayerState = State.Injured;
                //enemy.m_injured = true;
            }
            else if (m_damageType == State.Attack_Kick_Ground)
            {
                enemy.m_injuredState = State.Injured_02;
                enemy.PlayerState = State.Injured;
            }

        }
    }



    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    print(collision.transform.tag);
    //    if (collision.transform.tag.CompareTo("Enemy") == 0)
    //    {

    //        collision.transform.GetComponent<K>().PlayerState = State.Injured;
    //        collision.transform.GetComponent<K>().m_ani.Play("Ready01");
    //        print("22");
    //    }
    //}



}
