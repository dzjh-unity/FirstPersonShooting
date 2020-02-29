using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("GameScript/AutoDestroy")]

public class AutoDestroy : MonoBehaviour
{
    public float m_timer = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 可采用缓存的方式来避免在游戏中频繁使用Instantia和Destroy
        Destroy(this.gameObject, m_timer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
