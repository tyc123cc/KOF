using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    int i = 0;
    public List<Transform> m_skillUI;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowName()
    {
        foreach (var item in m_skillUI)
        {
            if (Vector2.Distance(Input.mousePosition, item.position) < 20)
            {
                //print(item.name);
            }
        }
    }
}
