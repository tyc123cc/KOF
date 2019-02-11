using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIjoysticks : MonoBehaviour
{

    public static bool m_drag = false;
    //虚拟方向按钮初始位置
    public Vector3 initPosition;
    //虚拟方向按钮可移动的半径
    public float r;
    //border对象
    public Transform border;

    public Transform m_leftScreen;
    public Transform m_rightScreen;

    private bool m_mouseDown = false;//鼠标按下
    public static bool m_jumpBack = false;
    public static bool m_ready = false;
    private float m_runTimer = 0;
    public bool m_run = false;

    public float m_speedWalk = 5f;
    public float m_speedRun = 12f;

    public K m_player;
    public Transform m_enemy;

    public float m_leftLimit = -13.2f;
    public float m_rightLimit = 15.5f;

    void Start()
    {
        //获取border对象的transform组件
        border = GameObject.Find("border").transform;
        initPosition = GetComponentInParent<RectTransform>().position;
        r = Vector3.Distance(transform.position, border.position);
        m_leftScreen = GameObject.Find("Left").transform;
        m_rightScreen = GameObject.Find("Right").transform;
    }


    public void OnMouseDown()
    {
        m_mouseDown = true;
        m_drag = true;
        m_run = false;
        if (m_runTimer == 0)
        {
            StartCoroutine(RunTiming());
        }
        else if (m_runTimer < 0.5f)
        {

            m_run = true;
            StopCoroutine(RunTiming());
        }

    }
    //一秒内快速点击两次则进入跑步状态
    IEnumerator RunTiming()
    {
        while (m_runTimer < 0.5f)
        {
            m_runTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }
        m_runTimer = 0;
        yield return 0;
    }
    //鼠标拖拽
    public void Update()
    {
        if (m_player.PlayerState == State.JumpBack)//后跳过程无论松不松手都要完成后移
        {
            if (m_player.transform.position.x - m_enemy.transform.position.x < 0)//玩家在敌人左侧 则向左位移
            {
                m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_player.transform.position - Vector3.right, m_speedRun * Time.deltaTime);//后跳也要移动
            }
            else//玩家在敌人右侧 向右位移
            {
                m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_player.transform.position + Vector3.right, m_speedRun * Time.deltaTime);//后跳也要移动
            }
        }
        //if (m_ready == false)
        //{
        //    return;
        //}
#if UNITY_EDITOR_WIN
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position = new Vector3(initPosition.x - r, initPosition.y, initPosition.z);
            OnMouseDown();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position = new Vector3(initPosition.x, initPosition.y - r, initPosition.z);
            OnMouseDown();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position = new Vector3(initPosition.x + r, initPosition.y, initPosition.z);
            OnMouseDown();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position = new Vector3(initPosition.x, initPosition.y + r, initPosition.z);
            OnMouseDown();
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            OnDragEnd();
        }
        if (m_mouseDown == false || m_player.PlayerState == State.Attack || m_ready == false)
        {
            return;
        }
      
#elif UNITY_ANDROID
         if (m_mouseDown == false || m_player.PlayerState == State.Attack || m_ready == false)
        {
            return;
        }
        Touch touch = new Touch();
        if (Input.touchCount > 1)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            if (Vector2.Distance(touch1.position, m_leftScreen.position) < Vector2.Distance(touch2.position, m_leftScreen.position))
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
        //如果鼠标到虚拟键盘原点的位置 < 半径r
        if (Vector3.Distance(touch.position, initPosition) < r)
        {
            //虚拟键跟随鼠标
            transform.position = touch.position;
        }
        else
        {
            //计算出鼠标和原点之间的向量
            Vector3 dir = new Vector3(touch.position.x, touch.position.y, 0) - initPosition;
            //这里dir.normalized是向量归一化的意思，实在不理解你可以理解成这就是一个方向，就是原点到鼠标的方向，乘以半径你可以理解成在原点到鼠标的方向上加上半径的距离
            transform.position = initPosition + dir.normalized * r;
        }
#endif
        if (m_player.PlayerState == State.JumpBack)//后跳过程不可取消、开场动画播放完毕前无法有动作
        {
            return;
        }

        //轮盘右侧
        if (transform.localPosition.x > 0)
        {
            float angle = Mathf.Atan(transform.localPosition.y / transform.localPosition.x);
            if (m_player.PlayerState != State.Jump)//地面状态上方跳跃 前方移动 下方下蹲
            {
                if (angle > Mathf.PI / 4)
                {
                    //跳跃
                    m_player.PlayerState = State.Jump;
                    m_player.m_isSky = true;
                    //m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_player.transform.position + Vector3.right, transform.localPosition.x * Time.deltaTime / 150);

                }
                else if (angle < -Mathf.PI / 4)
                {
                    //下蹲
                    m_player.PlayerState = State.Squat;
                    m_player.m_isSky = false;
                }

                else
                {
                    if (m_run == false)
                    {
                        //右走
                        m_player.PlayerState = State.Walk_Right;
                        m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_player.transform.position + Vector3.right, m_speedWalk * Time.deltaTime);
                        m_player.m_isSky = false;
                    }
                    else
                    {
                        if (m_player.transform.position.x - m_enemy.transform.position.x < 0)//玩家在敌人左侧 则双击右侧轮盘为跑步
                        {
                            //右跑
                            m_player.PlayerState = State.Run;
                            m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_player.transform.position + Vector3.right, m_speedRun * Time.deltaTime);
                            m_player.m_isSky = false;
                        }
                        else//玩家在敌人右侧 则双击右侧轮盘为后跳
                        {
                            if (m_jumpBack == false)//第一次移向右侧时为后跳 后跳完毕后变为行走
                            {
                                m_player.PlayerState = State.JumpBack;
                                m_player.m_isSky = false;

                            }
                            else
                            {
                                m_player.PlayerState = State.Walk_Right;
                                m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_player.transform.position + Vector3.right, m_speedWalk * Time.deltaTime);
                                m_player.m_isSky = false;
                            }
                        }

                    }


                }
            }
            else//空中状态  仅控制移动
            {
                m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_player.transform.position + Vector3.right, transform.localPosition.x * Time.deltaTime / 150);
            }

        }
        //轮盘左侧
        else if (transform.localPosition.x <= 0)
        {
            float angle = Mathf.Atan(transform.localPosition.y / -transform.localPosition.x);
            //print("angle:" + Mathf.Atan(transform.localPosition.y / -transform.localPosition.x));
            //print("y:" + transform.localPosition.y);
            //print("x:" + transform.localPosition.x);
            if (m_player.PlayerState != State.Jump)//地面状态上方跳跃 前方移动 下方下蹲
            {
                if (angle > Mathf.PI / 4)
                {
                    //跳跃
                    m_player.PlayerState = State.Jump;
                    m_player.m_isSky = true;

                }
                else if (angle < -Mathf.PI / 4)
                {
                    //下蹲
                    m_player.PlayerState = State.Squat;
                    m_player.m_isSky = false;
                }
                else
                {
                    if (m_run == false)
                    {
                        //左走
                        m_player.PlayerState = State.Walk_Left;
                        m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_player.transform.position - Vector3.right, m_speedWalk * Time.deltaTime);
                        m_player.m_isSky = false;
                    }
                    else
                    {
                        if (m_player.transform.position.x - m_enemy.transform.position.x > 0)//玩家在敌人右侧 则双击左侧轮盘为跑步
                        {
                            //左跑
                            m_player.PlayerState = State.Run;
                            m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_player.transform.position - Vector3.right, m_speedRun * Time.deltaTime);
                            m_player.m_isSky = false;
                        }
                        else//玩家在敌人右侧 则双击右侧轮盘为后跳
                        {
                            if (m_jumpBack == false)//第一次移向右侧时为后跳 后跳完毕后变为行走
                            {
                                m_player.PlayerState = State.JumpBack;
                                m_player.m_isSky = false;

                            }
                            else
                            {
                                m_player.PlayerState = State.Walk_Left;
                                m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_player.transform.position - Vector3.right, m_speedWalk * Time.deltaTime);
                                m_player.m_isSky = false;
                            }
                        }

                    }

                }
            }
            else//空中状态  仅控制移动
            {
                m_player.transform.position = Vector3.MoveTowards(m_player.transform.position, m_player.transform.position - Vector3.right, -transform.localPosition.x * Time.deltaTime / 150);
            }

        }
    }

    //鼠标松开
    public void OnDragEnd()
    {
        //松开鼠标虚拟摇杆回到原点
        transform.position = initPosition;
        m_mouseDown = false;
        m_drag = false;
        m_jumpBack = false;
        m_player.m_isSky = false;
        if (m_player.PlayerState != State.Jump && m_player.PlayerState != State.JumpBack && m_player.PlayerState != State.Attack)
        {
            m_player.PlayerState = State.Idle;
            m_player.m_ani.Play("Ready02");
        }
        else
        {

        }
    }
}
