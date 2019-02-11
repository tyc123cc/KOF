using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool m_choose = false;
    public Transform m_choosePanel;

    public Image m_1PImage;
    public Image m_2PImage;
    public Transform m_startButton;
    public Vector3 m_startButtonInitPos;
    public Transform m_quitButton;
    public Vector3 m_quitButtonInitPos;
    public Transform m_logo;
    public Vector3 m_logoInitPos;

    private int m_state = 0;//0为选择1P角色，1为选择2P角色，3为开始游戏

    public Sprite m_defaultSprite;
    public Sprite m_K;
    public Text m_hintText;

    private AudioSource m_1PAudio;
    private AudioSource m_2PAudio;
    // Use this for initialization
    void Start()
    {
        m_startButtonInitPos = m_startButton.localPosition;
        m_quitButtonInitPos = m_quitButton.localPosition;
        m_logoInitPos = m_logo.localPosition;
        m_1PAudio = m_1PImage.GetComponent<AudioSource>();
        m_2PAudio = m_2PImage.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_choose)
        {
            m_startButton.localPosition = new Vector3(Mathf.MoveTowards(m_startButton.localPosition.x, 500, 300 * Time.deltaTime),
                                                       m_startButton.localPosition.y, m_startButton.localPosition.z);
            m_quitButton.localPosition = new Vector3(Mathf.MoveTowards(m_quitButton.localPosition.x, -500, 300 * Time.deltaTime),
                                                       m_quitButton.localPosition.y, m_quitButton.localPosition.z);
            m_logo.localPosition = new Vector3(m_logo.localPosition.x, Mathf.MoveTowards(m_logo.localPosition.y, 365, 300 * Time.deltaTime), m_logo.localPosition.z);
            if (m_startButton.localPosition.x == 500)
            {
                float y = Mathf.MoveTowardsAngle(m_choosePanel.eulerAngles.y, 0, 30 * Time.deltaTime);
                m_choosePanel.eulerAngles = new Vector3(0, y, 0);
            }


        }
        else
        {
            float y = Mathf.MoveTowardsAngle(m_choosePanel.eulerAngles.y, 90, 30 * Time.deltaTime);
            m_choosePanel.eulerAngles = new Vector3(0, y, 0);
            if (m_choosePanel.eulerAngles.y == 90)
            {
                m_startButton.localPosition = new Vector3(Mathf.MoveTowards(m_startButton.localPosition.x, m_startButtonInitPos.x, 300 * Time.deltaTime),
                                                      m_startButton.localPosition.y, m_startButton.localPosition.z);
                m_quitButton.localPosition = new Vector3(Mathf.MoveTowards(m_quitButton.localPosition.x, m_quitButtonInitPos.x, 300 * Time.deltaTime),
                                                           m_quitButton.localPosition.y, m_quitButton.localPosition.z);
                m_logo.localPosition = new Vector3(m_logo.localPosition.x, Mathf.MoveTowards(m_logo.localPosition.y, m_logoInitPos.y, 300 * Time.deltaTime), m_logo.localPosition.z);
            }

        }
    }

    public void StartGame()
    {
        m_choose = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {

        switch (m_state)
        {
            case 0:
                m_choose = false;
                m_1PImage.sprite = m_defaultSprite;
                break;
            case 1:
                m_1PImage.sprite = m_defaultSprite;
                m_2PImage.sprite = m_defaultSprite;
                m_hintText.text = "选择1P角色";
                m_state = 0;
                break;
            case 2:
                m_2PImage.sprite = m_defaultSprite;
                m_hintText.text = "选择2P角色";
                m_state = 1;
                break;
            default:
                break;
        }

    }

    public void Next()
    {
        switch (m_state)
        {
            case 0:
                if (m_1PImage.sprite != m_defaultSprite)
                {
                    m_state = 1;
                    m_hintText.text = "选择2P角色";
                }
                break;
            case 1:
                if (m_2PImage.sprite != m_defaultSprite)
                {
                    m_state = 2;
                    m_hintText.text = "开始游戏";
                }
                break;
            case 2:
                //进入游戏
                SceneManager.LoadScene("Load");
                break;
            default:
                break;
        }
    }

    public void ChooseK()
    {
        switch (m_state)
        {
            case 0://选择1P角色
                m_1PImage.sprite = m_K;
                m_1PAudio.clip = Resources.Load("Sounds/K/K_Ready") as AudioClip;
                m_1PAudio.Play();
                //m_hintText.text = "选择1P角色";
                break;
            case 1://选择2P角色
                m_2PImage.sprite = m_K;
                m_2PAudio.clip = Resources.Load("Sounds/K/K_Ready") as AudioClip;
                m_2PAudio.Play();
                break;
            default:
                break;
        }

    }

   
}
