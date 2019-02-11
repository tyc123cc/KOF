using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EntranceEffect : MonoBehaviour {
    public Transform m_line1Up;
    public Transform m_line1Down;
    public Transform m_line2Up;
    public Transform m_line2Down;
    public Transform m_line3Up;
    public Transform m_line3Down;
    public Transform m_EntranceUp;
    public Transform m_EntranceDown;
    public Transform m_player;
    public Transform m_enemy;

    public Image m_pause;
    public Sprite m_pauseSprite;
    public Sprite m_startSprite;

    public int m_state = 1;
    public bool m_ready = false;
	// Use this for initialization
	void Start () {
        m_line1Up = transform.Find("Line1_Up");
        m_line1Down = transform.Find("Line1_Down");
        m_line2Up = transform.Find("Line2_Up");
        m_line2Down = transform.Find("Line2_Down");
        m_line3Up = transform.Find("Line3_Up");
        m_line3Down = transform.Find("Line3_Down");
        m_EntranceUp = transform.Find("Entrance1");
        m_EntranceDown = transform.Find("Entrance2");
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        
	}
	
	// Update is called once per frame
	void Update () {
        if (m_state == 1)
        {
            Vector2 size  = new Vector2(100, Mathf.MoveTowards(m_line1Up.GetComponent<RectTransform>().rect.height, 408, 200 * Time.deltaTime));
            Vector2 pos = new Vector2(m_line1Up.localPosition.x + (size.y - m_line1Up.GetComponent<RectTransform>().rect.height) / 2,m_line1Up.localPosition.y);
            m_line1Up.GetComponent<RectTransform>().sizeDelta = size;
            m_line1Up.localPosition = pos;
            size = new Vector2(100, Mathf.MoveTowards(m_line1Down.GetComponent<RectTransform>().rect.height, 408, 200 * Time.deltaTime));
            pos = new Vector2(m_line1Down.localPosition.x + (size.y - m_line1Down.GetComponent<RectTransform>().rect.height) / 2, m_line1Down.localPosition.y);
            m_line1Down.GetComponent<RectTransform>().sizeDelta = size;
            m_line1Down.localPosition = pos;
            if (size.y == 408)
            {
                m_state = 2;
            }
        }
        else if (m_state == 2)
        {
            Vector2 size = new Vector2(100, Mathf.MoveTowards(m_line2Up.GetComponent<RectTransform>().rect.height, 128, 200 * Time.deltaTime));
            Vector2 pos = new Vector2(m_line2Up.localPosition.x, m_line2Up.localPosition.y + (size.y - m_line2Up.GetComponent<RectTransform>().rect.height) / 2);
            m_line2Up.GetComponent<RectTransform>().sizeDelta = size;
            m_line2Up.localPosition = pos;
            size = new Vector2(100, Mathf.MoveTowards(m_line2Down.GetComponent<RectTransform>().rect.height, 128, 200 * Time.deltaTime));
            pos = new Vector2(m_line2Down.localPosition.x, m_line2Down.localPosition.y + (size.y - m_line2Down.GetComponent<RectTransform>().rect.height) / 2);
            m_line2Down.GetComponent<RectTransform>().sizeDelta = size;
            m_line2Down.localPosition = pos;
            if (size.y == 128)
            {
                m_state = 3;
            }
        }
        else if (m_state == 3)
        {
            Vector2 size = new Vector2(100, Mathf.MoveTowards(m_line3Up.GetComponent<RectTransform>().rect.height, 400, 200 * Time.deltaTime));
            Vector2 pos = new Vector2(m_line3Up.localPosition.x + (size.y - m_line3Up.GetComponent<RectTransform>().rect.height) / 2, m_line3Up.localPosition.y);
            m_line3Up.GetComponent<RectTransform>().sizeDelta = size;
            m_line3Up.localPosition = pos;
            size = new Vector2(100, Mathf.MoveTowards(m_line3Down.GetComponent<RectTransform>().rect.height, 400, 200 * Time.deltaTime));
            pos = new Vector2(m_line3Down.localPosition.x + (size.y - m_line3Down.GetComponent<RectTransform>().rect.height) / 2, m_line3Down.localPosition.y);
            m_line3Down.GetComponent<RectTransform>().sizeDelta = size;
            m_line3Down.localPosition = pos;
            if (size.y == 400)
            {
                m_state = 4;
            }
        }
        if (m_state == 4)
        {
            m_EntranceUp.localPosition = new Vector2(m_EntranceUp.localPosition.x,Mathf.MoveTowards(m_EntranceUp.localPosition.y,450,200 * Time.deltaTime));
            m_line1Up.localPosition = new Vector2(m_line1Up.localPosition.x, Mathf.MoveTowards(m_line1Up.localPosition.y, 450, 200 * Time.deltaTime));
            m_line2Up.localPosition = new Vector2(m_line2Up.localPosition.x, Mathf.MoveTowards(m_line2Up.localPosition.y, 450, 200 * Time.deltaTime));
            m_line3Up.localPosition = new Vector2(m_line3Up.localPosition.x, Mathf.MoveTowards(m_line3Up.localPosition.y, 450, 200 * Time.deltaTime));
            m_EntranceDown.localPosition = new Vector2(m_EntranceDown.localPosition.x, Mathf.MoveTowards(m_EntranceDown.localPosition.y, -450, 200 * Time.deltaTime));
            m_line1Down.localPosition = new Vector2(m_line1Down.localPosition.x, Mathf.MoveTowards(m_line1Down.localPosition.y, -450, 200 * Time.deltaTime));
            m_line2Down.localPosition = new Vector2(m_line2Down.localPosition.x, Mathf.MoveTowards(m_line2Down.localPosition.y, -450, 200 * Time.deltaTime));
            m_line3Down.localPosition = new Vector2(m_line3Down.localPosition.x, Mathf.MoveTowards(m_line3Down.localPosition.y, -450, 200 * Time.deltaTime));
            if (m_ready == false)
            {
                m_ready = true;
                m_player.GetComponent<K>().PlayerState = State.Ready;
                m_enemy.GetComponent<K>().PlayerState = State.Ready;
                //UIjoysticks.m_ready = true;
            }
        }
	}

    public void Pause()
    {
        if (m_pause.sprite == m_pauseSprite)//暂停
        {
            Time.timeScale = 0;
            m_pause.sprite = m_startSprite;
        }
        else
        {
            Time.timeScale = 1;
            m_pause.sprite = m_pauseSprite;
        }
       
    }

    public void Back()
    {
        SceneManager.LoadScene("Choose");
    }
}
