using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour {
    public Text m_LoadText;
    public bool m_loadOver = false;
    public GameObject m_music;
	// Use this for initialization
	void Start () {
        StartCoroutine(StartLoading());
        DontDestroyOnLoad(m_music);
	}

    private IEnumerator StartLoading()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("battle");
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            m_LoadText.text = "Loading..." + (op.progress + 0.1f) * 100 + "%";
            yield return new WaitForEndOfFrame();
        }
        m_LoadText.text = "加载完毕，点击屏幕继续";
        while (!m_loadOver)
        {
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            m_loadOver = true;
        }
	}
}
