using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("GameScript/Player")]

public class Player : MonoBehaviour
{
    Transform m_transform;

    // 角色控制器组件
    CharacterController m_ch;

    // 角色移动速度
    public float m_moveSpeed = 6.0f;

    // 重力
    float m_gravity = 2.0f;

    // 生命值
    public int m_life = 5;

    // 摄像机Transform
    Transform m_camTransform;

    // 摄像机旋转角度
    Vector3 m_camRot;

    // 摄像机高度（即主角的脚相对于眼睛高度）
    float m_camHeight = 1.9f;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();

        // 获取摄像机
        m_camTransform = Camera.main.transform;
        
        // 设置摄像机初始位置（使用TransformPoint获取Player在X轴便宜一定高度的位置）
        m_camTransform.position = m_transform.TransformPoint(0, m_camHeight, 0);

        // 设置摄像机的旋转方向与主角一致
        m_camTransform.rotation = m_transform.rotation;
        m_camRot = m_camTransform.eulerAngles;

        // 锁定鼠标
        Screen.lockCursor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_life <= 0) {
            return;
        }
        Control();
    }

    void Control() {
        Vector3 motion = Vector3.zero; // 移动的方向
        motion.x = Input.GetAxis("Horizontal") * m_moveSpeed * Time.deltaTime;
        motion.z = Input.GetAxis("Vertical") * m_moveSpeed * Time.deltaTime;
        motion.y -= m_gravity * Time.deltaTime; // 重力
        // 使用角色控制器提供的Move函数进行移动，其会自动检测碰撞
        m_ch.Move(m_transform.TransformDirection(motion));

        // 获取鼠标移动距离
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");
        // 旋转摄像机
        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camTransform.eulerAngles = m_camRot;

        // 使主角的面向方向与摄像机一致
        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0;
        camrot.z = 0;
        m_transform.eulerAngles = camrot;

        // 更新摄像机位置（始终与Player一致）
        m_camTransform.position = m_transform.TransformPoint(0, m_camHeight, 0);
    }

    // 在编辑器中为主角显示一个图标
    void OnDrawGizmons() {
        Gizmos.DrawIcon(this.transform.position, "Spawn.tif");
    }
}
