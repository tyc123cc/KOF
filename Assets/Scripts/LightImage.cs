using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightImage : MonoBehaviour {
    public List<Sprite> m_sprites;
    public int timeIndex = 0;
    private Image spriteRenderer;
    float timer = 0;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        
        int index = timeIndex % m_sprites.Count;
        spriteRenderer.overrideSprite = m_sprites[index];
        timer ++;
        if (timer >= 2f)
        {
            timeIndex++;
            timer = 0;
        }
        if (timeIndex == m_sprites.Count)
        {
            timeIndex = 0;
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }

	}
}
