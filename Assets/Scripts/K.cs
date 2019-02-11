using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K : MonoBehaviour
{
    //定义一个委托
    public delegate void StateChanged(object sender, int state);
    //与委托相关联的事件
    public event StateChanged OnStateChanged;

    public Animator m_ani;
    public SpriteRenderer m_render;
    public AudioSource m_audio;

    public Transform m_enemy;
    public GameObject m_ash;//灰尘
    public GameObject m_collider;
    public GameObject m_fire;
    public GameObject m_fireSecond;
    public GameObject m_dust;

    public List<Transform> m_punchSkill;
    public Transform m_punchButton;
    public Transform m_triggerButton;
    public GameObject m_triggerLightImage;
    public List<Transform> m_kickSkill;
    public Transform m_leftScreen;
    public Transform m_rightScreen;

    public Sprite m_injured01;
    public Sprite m_injured02;
    public Sprite m_injured03;

    private float m_standPoint;
    public float m_jumpPoint;
    public float m_jumpBackPoint;
    public bool m_jumpUp = true;
    public bool m_jumpBackUp = true;
    public bool m_isSky = false;//是否在空中
    public bool m_injured = false;
    public bool isInjured03 = false;
    public bool m_injured03Up = true;
    public bool m_standUp = false;

    public bool m_collision = false;

    private int SkyAttack;

    private AudioClip K_Ready;
    private AudioClip K_Punch;
    private AudioClip K_Trigger;
    private AudioClip K_SecondBullet;
    private AudioClip Fell;
    private AudioClip Injured;
    private AudioClip Jump;
    private AudioClip JumpBack;
    private AudioClip Run;

    public float m_leftLimit = -13.2f;
    public float m_rightLimit = 15.5f;
    //0表示没有攻击，1表示空中拳，2表示空中腿
    public int m_skyAttack
    {
        get
        {
            return SkyAttack;
        }
        set
        {
            SkyAttack = value;
            if (PlayerState == State.Jump)
            {
                if (m_skyAttack == 0)
                {
                    m_ani.Play("Jump");
                }
                else if (m_skyAttack == 1)
                {
                    m_ani.Play("Punch_Sky");
                    CreateCollider(State.Attack_Punch_Sky, 0.5f, new Vector3(0.4f, 0.4f, 1));
                }
                else if (m_skyAttack == 2)
                {
                    m_ani.Play("Kick_Sky");
                    CreateCollider(State.Attack_Kick_Sky, 0.5f, new Vector3(0.4f, 0.4f, 1));
                }
            }

        }
    }


    public int m_injuredState;
    public float m_stiffnessTime;//受伤状态1和状态2表示硬直时间，受伤状态3表示击飞高度
    public float m_injuredTimer = 0;
    private int m_state = State.Idle;

    public int PlayerState
    {
        get { return m_state; }
        set
        {
            if (m_state == State.Jump || m_state == State.JumpBack)//被改变的状态是跳跃，后跳，该状态结束后显示灰尘特效
            {
                m_audio.loop = false;
                m_audio.clip = JumpBack;
                m_audio.Play();

                if (transform.eulerAngles.y == 0)
                {
                    Instantiate(m_ash, new Vector3(transform.position.x - 1.8f, transform.position.y - 1.75f, 0), new Quaternion(0, 1, 0, 0));
                }
                else
                {
                    Instantiate(m_ash, new Vector3(transform.position.x + 1.8f, transform.position.y - 1.75f, 0), new Quaternion(0, 0, 0, 0));
                }

            }
            if (m_state == State.Run && value != State.Run)
            {
                m_audio.Stop();
                m_audio.clip = null;
            }
            if (!isInjured03 || value == State.StandUp)
            {
                m_state = value;
                if (OnStateChanged != null)
                {
                    OnStateChanged(this, value);
                }
            }
            else
            {
                m_injuredState = State.Injured_03;
            }


        }
    }
    // Use this for initialization
    void Start()
    {
        m_ani = GetComponent<Animator>();
        m_render = GetComponent<SpriteRenderer>();
        m_audio = GetComponent<AudioSource>();
        m_dust = transform.Find("Dust").gameObject;
        OnStateChanged += new StateChanged(ADSoldier_OnStateChanged);
        if (transform.tag.CompareTo("Player") == 0)
        {
            m_enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        }
        else
        {
            m_enemy = GameObject.FindGameObjectWithTag("Player").transform;
        }

        m_jumpPoint = transform.position.y + 4;
        m_jumpBackPoint = transform.position.y + 1;
        m_standPoint = transform.position.y;
        m_leftScreen = GameObject.Find("Left").transform;
        m_rightScreen = GameObject.Find("Right").transform;
        StartCoroutine(CreateAsh());

        JumpBack = Resources.Load("Sounds/JumpBack") as AudioClip;
        Jump = Resources.Load("Sounds/Jump") as AudioClip;
        Run = Resources.Load("Sounds/Run") as AudioClip;
        Fell = Resources.Load("Sounds/Fell") as AudioClip;
        Injured = Resources.Load("Sounds/Injured") as AudioClip;
        K_Ready = Resources.Load("Sounds/K/K_Ready") as AudioClip;
        K_Punch = Resources.Load("Sounds/K/Punch") as AudioClip;
        K_Trigger = Resources.Load("Sounds/K/Trigger") as AudioClip;
        K_SecondBullet = Resources.Load("Sounds/K/Second Bullet") as AudioClip;
    }
    void ADSoldier_OnStateChanged(object sender, int state)
    {

        transform.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 0f);
        transform.GetComponent<CapsuleCollider2D>().size = new Vector2(0.5f, 1.05f);
       
        if (state == State.Walk_Left)
        {
            //if (m_enemy.position.x - transform.position.x >= 0)
            //{
            //    m_ani.Play("Walk_forward");
            //}
            //else
            //{
            //    m_ani.Play("Walk_back");
            //}
        }
        else if (state == State.Idle)
        {
            m_isSky = false;
            
            //m_ani.Play("Ready02");
           
            if (transform.tag.CompareTo("Player") == 0 && UIjoysticks.m_drag == false)
            {
                if (m_ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    UIjoysticks.m_ready = true;
                }
                else
                {
                    m_ani.Play("Ready02");
                }

            }
            else if (transform.tag.CompareTo("Enemy") == 0)
            {
                if (m_ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {

                }
                else
                {
                    m_ani.Play("Ready02");
                }
            }
        }
        else if (state == State.Ready)
        {
            m_ani.Play("Ready01");
            m_audio.loop = false;
            m_audio.clip = K_Ready;
            m_audio.Play();
           
        }
        else if (state == State.Jump)
        {
            m_ani.Play("Jump");
            m_audio.loop = false;
            m_audio.clip = Jump;
            m_audio.Play();

        }
        else if (state == State.Squat)
        {
            m_ani.Play("Squat");
            transform.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, -0.25f);
            transform.GetComponent<CapsuleCollider2D>().size = new Vector2(0.5f, -0.55f);
        }
        else if (state == State.Injured)
        {
            //if (transform.tag.CompareTo("Enemy") == 0)
            //{
            //    print("State:" + m_injuredState);
            //}
            m_injuredTimer = 0;
            if (m_injuredState == State.Injured_01)//受伤状态1(小伤害，不浮空)
            {
                //m_render.sprite = m_injured01;
                int damageNum = m_ani.GetInteger("Injured01");
                if (damageNum == 1)
                {
                    m_ani.Play("Injured01");
                    m_ani.SetInteger("Injured01", 2);
                }
                else
                {
                    m_ani.Play("Injured01 0");
                    m_ani.SetInteger("Injured01", 1);
                }

            }
            else if (m_injuredState == State.Injured_02)//受伤状态2(大伤害，不浮空)
            {
                int damageNum = m_ani.GetInteger("Injured02");
                if (damageNum == 1)
                {
                    m_ani.Play("Injured02");
                    m_ani.SetInteger("Injured02", 2);
                }
                else
                {
                    m_ani.Play("Injured02 0");
                    m_ani.SetInteger("Injured02", 1);
                }
            }
            else if (m_injuredState == State.Injured_03)//受伤状态3(大伤害，浮空)
            {
                //int damageNum = m_ani.GetInteger("Injured03");
                //if (damageNum == 1)
                //{
                m_ani.Play("Injured03");
                isInjured03 = true;
                //    m_ani.SetInteger("Injured03", 2);
                //}
                //else
                //{
                //    m_ani.Play("Injured03 0");
                //    m_ani.SetInteger("Injured03", 1);
                //}
            }

        }
        if (state == State.StandUp)
        {
            m_ani.Play("StandUp");
            m_audio.loop = false;
            m_audio.clip = Fell;
            m_audio.Play();
            m_dust.SetActive(true);
            m_standUp = true;
        }


    }
    //跑步状态下创建灰尘特效
    IEnumerator CreateAsh()
    {
        while (true)
        {
            if (PlayerState == State.Run)
            {
                if (transform.eulerAngles.y == 0)
                {
                    Instantiate(m_ash, new Vector3(transform.position.x - 1.8f, transform.position.y - 1.75f, 0), new Quaternion(0, 1, 0, 0));
                }
                else
                {
                    Instantiate(m_ash, new Vector3(transform.position.x + 1.8f, transform.position.y - 1.75f, 0), new Quaternion(0, 0, 0, 0));
                }

                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }

        }

    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < m_leftLimit)
        {
            transform.position = new Vector3(m_leftLimit, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > m_rightLimit)
        {
            transform.position = new Vector3(m_rightLimit, transform.position.y, transform.position.z);
        }
        if (transform.tag.CompareTo("Player") == 0)
        {
            if (transform.position.x - m_enemy.position.x < -14)
            {
                transform.position = new Vector3(m_enemy.position.x - 14, transform.position.y, transform.position.z);
            }
            else if (transform.position.x - m_enemy.position.x > 14)
            {
                transform.position = new Vector3(m_enemy.position.x + 14, transform.position.y, transform.position.z);
            }
        }
        //调整角度
        if (m_enemy.position.x - transform.position.x >= 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if (m_collision == true)
        {
            if (m_enemy.transform.position.x - transform.position.x > 0)
            {
                m_enemy.transform.position = new Vector3(transform.position.x + 2.5f, m_enemy.position.y, m_enemy.position.z);
            }
            else
            {
                m_enemy.transform.position = new Vector3(transform.position.x - 2.5f, m_enemy.position.y, m_enemy.position.z);
            }
        }
        if (m_ani.GetCurrentAnimatorStateInfo(0).IsName("Ready02") && m_ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            UIjoysticks.m_ready = true;
        }
      
        if (PlayerState == State.Walk_Right)//右走状态
        {
            if (m_enemy.position.x - transform.position.x >= 0)
            {
                m_ani.Play("Walk_forward");
            }
            else
            {
                m_ani.Play("Walk_back");
            }
        }
        else if (PlayerState == State.Walk_Left)//左走状态
        {
            if (m_enemy.position.x - transform.position.x < 0)
            {
                m_ani.Play("Walk_forward");
            }
            else
            {
                m_ani.Play("Walk_back");
            }
        }
        else if (PlayerState == State.Run)
        {
            m_ani.Play("Run");
            if (m_audio.clip == null || m_audio.clip.name.CompareTo("Run") != 0)
            {
                m_audio.loop = true;
                m_audio.clip = Run;
                m_audio.Play();
            }


        }      
        else if (PlayerState == State.Idle)//空闲状态
        {
            m_isSky = false;
            //m_ani.Play("Ready02");
            if (transform.tag.CompareTo("Player") == 0 && UIjoysticks.m_drag == false)
            {
                if (m_ani.GetCurrentAnimatorStateInfo(0).IsName("Idle") || m_ani.GetCurrentAnimatorStateInfo(0).IsName("Ready02"))
                {
                    // m_ani.Play("Idle");
                    
                }
                else if (m_ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    m_ani.Play("Ready02");
                }

            }
            else if (transform.tag.CompareTo("Enemy") == 0)
            {
                if (m_ani.GetCurrentAnimatorStateInfo(0).IsName("Idle") || m_ani.GetCurrentAnimatorStateInfo(0).IsName("Ready02"))
                {
                    // m_ani.Play("Idle");
                    

                }
                else if (m_ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    m_ani.Play("Ready02");
                }
            }
        }
        else if (PlayerState == State.Jump)//跳跃状态
        {
            //if (m_skyAttack == 0)
            //{
            //    m_ani.Play("Jump");
            //}
            //else if (m_skyAttack == 1)
            //{
            //    m_ani.Play("Punch_Sky");
            //    CreateCollider(State.Attack_Punch_Sky, 0.5f, new Vector3(0.4f, 0.4f, 1));
            //}
            //else if (m_skyAttack == 2)
            //{
            //    m_ani.Play("Kick_Sky");
            //    CreateCollider(State.Attack_Kick_Sky, 0.5f, new Vector3(0.4f, 0.4f, 1));
            //}
            if (m_jumpUp == true)//向上跳
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, m_jumpPoint), 8 * Time.deltaTime);
                if (transform.position.y == m_jumpPoint)
                {
                    m_jumpUp = false;
                }
            }
            else//向下跳
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, m_standPoint), 8 * Time.deltaTime);
                if (transform.position.y == m_standPoint)
                {
                    m_jumpUp = true;
                    m_skyAttack = 0;
                    PlayerState = State.Idle;
                }
            }
        }
        else if (PlayerState == State.JumpBack)//后跳状态
        {

            m_ani.Play("Jump Back");
            if (m_jumpBackUp == true)//向上跳
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, m_jumpBackPoint), 4 * Time.deltaTime);
                if (transform.position.y == m_jumpBackPoint)
                {
                    m_jumpBackUp = false;
                }
            }
            else//向下跳
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, m_standPoint), 4 * Time.deltaTime);
                if (transform.position.y == m_standPoint)
                {
                    m_jumpBackUp = true;
                    PlayerState = State.Idle;
                    UIjoysticks.m_jumpBack = true;
                }
            }
            //if (m_render.sprite.name.CompareTo("K’_105-5") == 0)//后跳动画播放完毕 转为行走
            if (m_ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {

                PlayerState = State.Idle;
                UIjoysticks.m_jumpBack = true;

            }
        }
        else if (PlayerState == State.Attack)//攻击状态
        {
            //string aniName = m_ani.GetCurrentAnimatorClipInfo(0)[0].clip.name;//正在播放动画名称
            AnimatorStateInfo info = m_ani.GetCurrentAnimatorStateInfo(0);
            float aniLength = m_ani.GetCurrentAnimatorStateInfo(0).normalizedTime;//正在播放动画长度
            string spriteName = m_render.sprite.name;//获得当前帧的名称
            if (info.IsName("Second Bullet"))
            {
                if (m_audio.clip.name.CompareTo("Second Bullet") != 0)
                {
                    m_audio.loop = false;
                    m_audio.clip = K_SecondBullet;
                    m_audio.Play();
                }
               
                m_fireSecond.transform.SetParent(null);
                m_ani.SetBool("Second Bullet", false);
                m_fireSecond.SetActive(true);
            }
            //if (aniName.CompareTo("Punch_Ground") == 0)
            {
                //print("1");
                if (aniLength >= 1.0f)
                {
                    PlayerState = State.Idle;
                }
            }


        }
        else if (PlayerState == State.Injured)//受伤状态
        {
            AnimatorStateInfo aniInfo = m_ani.GetCurrentAnimatorStateInfo(0);//正在播放动画名称
            float aniLength = m_ani.GetCurrentAnimatorStateInfo(0).normalizedTime;//正在播放动画长度
            string spriteName = m_render.sprite.name;//获得当前帧的名称
            m_injuredTimer += Time.deltaTime;
            if ((aniInfo.IsName("Injured01") || aniInfo.IsName("Injured01 0") || aniInfo.IsName("Injured02") || aniInfo.IsName("Injured02 0"))
                && aniLength >= 1.0f && m_injuredTimer >= m_stiffnessTime)
            {
                PlayerState = State.Idle;
            }
            else if (aniInfo.IsName("Injured03") && transform.position.y < m_stiffnessTime && m_injured03Up)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, m_stiffnessTime), 10 * Time.deltaTime);
                if (transform.position.y == m_stiffnessTime)
                {
                    m_injured03Up = false;
                }
            }
            else if (aniInfo.IsName("Injured03") && m_injured03Up == false)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, -2.02f), 10 * Time.deltaTime);
                if (transform.position.y == -2.02f)
                {
                    m_injured03Up = true;
                    // isInjured03 = false;
                    PlayerState = State.StandUp;
                }
            }

        }
        else if (PlayerState == State.StandUp)
        {

            if (m_ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                m_standUp = false;
                isInjured03 = false;
                PlayerState = State.Idle;
            }
        }

    }

    public void PunchDown()
    {
        if (UIjoysticks.m_ready == false)
        {
            return;
        }
        //拳按钮按下后
        Vector3 endPos = new Vector3(-185.3f, 20.4f, 0);
        m_triggerButton.localPosition = endPos;
        m_triggerLightImage.SetActive(true);
    }

    public void PunchUp()
    {
        if (UIjoysticks.m_ready == false)
        {
            return;
        }
        //拳按钮松开后
        string skillName = "";
        Touch touch = new Touch();
#if UNITY_EDITOR_WIN
        touch.position = Input.mousePosition;
#elif UNITY_ANDROID
        
         if (Input.touchCount > 1)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            if (Vector2.Distance(touch1.position, m_rightScreen.position) < Vector2.Distance(touch2.position, m_rightScreen.position))
            {
                touch = touch1;
            }
            else
            {
                touch = touch2;
                
            }
        }
        else if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
        }
#endif

        foreach (var item in m_punchSkill)
        {
            if (Vector2.Distance(touch.position, item.position) < Screen.width / 800.0f * 30)
            {
                print(item.name);
                skillName = item.name;
                break;
            }
        }
        if (skillName.CompareTo("Punch") == 0)//普通拳
        {
            if (PlayerState == State.Jump && m_skyAttack == 0)//若在空中则为空中挥拳
            {
                m_skyAttack = 1;

            }
            else if (PlayerState == State.Squat)//蹲伏攻击
            {

            }
            else if (PlayerState != State.JumpBack && PlayerState != State.Attack && PlayerState != State.Jump)//不处于后退状态或攻击状态时进行挥拳攻击
            {

                m_ani.Play("Punch_Ground");
                PlayerState = State.Attack;
                m_audio.loop = false;
                m_audio.clip = K_Punch;
                m_audio.Play();
                CreateCollider(State.Attack_Punch_Ground, 1f, new Vector3(0.4f, 0.4f, 1));
            }
        }
        if (skillName.CompareTo("Trigger") == 0)//扳机
        {
            if (PlayerState != State.JumpBack && PlayerState != State.Attack && PlayerState != State.Jump && PlayerState != State.Squat)
            {
                PlayerState = State.Attack;
                m_ani.Play("Trigger");
                //if (m_audio.clip.name.CompareTo("Trigger") != 0 || !m_audio.isPlaying)
                {
                    m_audio.loop = false;
                    m_audio.clip = K_Trigger;
                    m_audio.Play();
                }              
                //GameObject fire =  Instantiate(m_fire, transform.position, transform.rotation);
                //fire.transform.SetParent(transform);
                //fire.transform.localPosition += new Vector3(0.5f, 0.4f, 0);
                m_fire.transform.SetParent(null);
                m_fire.SetActive(true);
            }

        }
        m_triggerButton.localPosition = m_punchButton.position;

    }

    public void KickDown()
    {
        //腿按钮按下后
        if (UIjoysticks.m_ready == false)
        {
            return;
        }
    }

    public void KickUp()
    {
        if (UIjoysticks.m_ready == false)
        {
            return;
        }
        //腿按钮松开后
        if (PlayerState == State.Jump && m_skyAttack == 0)//若在空中则为空中踢腿
        {
            m_skyAttack = 2;
        }
        else if (PlayerState == State.Squat)//蹲伏攻击
        {

        }
        else if (PlayerState != State.JumpBack && PlayerState != State.Attack && PlayerState != State.Jump)//不处于后退状态或攻击状态时进行踢腿攻击
        {

            m_ani.Play("Kick_Ground");
            PlayerState = State.Attack;
            //if (m_audio.clip.name.CompareTo("Punch") != 0 || !m_audio.isPlaying)
            {
                m_audio.loop = false;
                m_audio.clip = K_Punch;
                m_audio.Play();
            }
            CreateCollider(State.Attack_Kick_Ground, 1f, new Vector3(0.4f, 0.4f, 1));
        }
        else if (PlayerState == State.Attack && m_ani.GetCurrentAnimatorStateInfo(0).IsName("Trigger"))//第二弹
        {
            m_ani.SetBool("Second Bullet", true);
            
        }
    }
    public void CreateCollider(int type, float stiffnessTime, Vector3 scale)//创建碰撞体
    {
        Transform collider = Instantiate(m_collider, transform.position, transform.rotation).transform;
        BattleCollider col = collider.GetComponent<BattleCollider>();
        col.m_damageType = type;//地面拳攻击
        col.m_stiffnessTime = stiffnessTime;
        col.ChangeScale(scale);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.CompareTo("Enemy") == 0)
        {
            m_collision = true;

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.CompareTo("Enemy") == 0)
        {
            m_collision = false;

        }
    }










}
