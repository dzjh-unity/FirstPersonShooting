using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("GameScript/MiniCamera")]

public class MiniCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 获取屏幕分辨路比例
        float ratio = (float)Screen.width / (float)Screen.height;
        // 使摄像机视图永远使一个正方向，rect的前两个参数表示XY位置，后两个参数使XY的大小
        this.GetComponent<Camera>().rect = new Rect(1 - 0.2f, 1 - 0.2f * ratio, 0.2f, 0.2f * ratio);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
