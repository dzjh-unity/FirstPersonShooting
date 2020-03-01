using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("GameScript/Enemy")]

public class Enemy : MonoBehaviour
{
    Transform m_transform;

    Animator m_anim;

    // 主角
    Player m_player;
    // 寻路组件
    NavMeshAgent m_agent;

    // 移动速度
    float m_movSpeed = 2.5f;

    // 旋转速度
    float m_rotSpeed = 5.0f;

    // 计时器
    float m_initTime = 1;
    float m_timer = 1;

    // 生命值
    int m_life = 5;

    // 出生点
    protected EnemySpawn m_spawn;

    // 攻击距离
    float attackDist = 1.5f;

    public void Init(EnemySpawn spawn) {
        m_spawn = spawn;
        m_spawn.m_enemyCnt ++;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_transform = this.transform;

        m_anim = this.GetComponent<Animator>();

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        m_agent = this.GetComponent<NavMeshAgent>();
        m_agent.speed = m_movSpeed; // 设置寻路器的行走速度

        m_agent.SetDestination(m_player.transform.position); // 设置寻路目标
    }

    // Update is called once per frame
    void Update()
    {
        // 如果主角生命值为0，不做操作
        if (m_player.m_life <= 0) {
            return;
        }
        // 更新定时器
        m_timer -= Time.deltaTime;

        // 获取当前动画状态
        AnimatorStateInfo stateInfo = m_anim.GetCurrentAnimatorStateInfo(0);

        // 如果处于待机且不是过渡状态
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.idle") && !m_anim.IsInTransition(0)) {
            m_anim.SetBool("idle", false);

            // 停止寻路
            m_agent.ResetPath();

            // 待机一段时间
            if (m_timer > 0) {
                return;
            }

            // 如果距离主角距离小于1.5，进入攻击动画状态
            if (Vector3.Distance(m_transform.position, m_player.transform.position) < attackDist) {
                m_anim.SetBool("attack", true);
            } else {
                // 重置定时器
                m_timer = 1;
                // 设置寻路目标点
                m_agent.SetDestination(m_player.transform.position);
                // 进入跑步动画状态
                m_anim.SetBool("run", true);
            }
        }
        // 如果处于跑步且不是过渡状态
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.run") && !m_anim.IsInTransition(0)) {
            m_anim.SetBool("run", false);

            // 每隔1秒重新定位主角的位置
            if (m_timer < 0) {
                m_agent.SetDestination(m_player.transform.position);
                m_timer = 1;
            }

            // 如果距离主角距离小于1.5，进入攻击动画状态
            if (Vector3.Distance(m_transform.position, m_player.transform.position) < attackDist) {
                // 停止寻路
                m_agent.ResetPath();
                m_anim.SetBool("attack", true);
            }
        }
        // 如果处于攻击且不是过渡状态
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.attack") && !m_anim.IsInTransition(0)) {
            RotateTo(); // 面向主角
            m_anim.SetBool("attack", false);

            // 如果播完动画，重新进入待机状态
            if (stateInfo.normalizedTime >= 1.0f && !m_anim.GetBool("idle")) {
                m_anim.SetBool("idle", true);

                // 重置定时器
                m_timer = 0;
                // 伤害主角
                m_player.OnDamage(1);
            }
        }
        // 如果处于死亡且不是过渡状态
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.death") && !m_anim.IsInTransition(0)) {
            m_anim.SetBool("death", false);
            if (stateInfo.normalizedTime >= 1.0f) {
                GameManager.Instance.AddScore(100);
                Destroy(this.gameObject);
                // 更新出生点的计数
                m_spawn.m_enemyCnt --;
            }
        }
    }

    void RotateTo() {
        // 获取目标（Player）方向
        Vector3 targetdir = m_player.transform.position - m_transform.position;
        // 计算出新方向
        Vector3 newDir = Vector3.RotateTowards(m_transform.forward, targetdir, m_rotSpeed * Time.deltaTime, 0.0f);
        // 旋转至新方向
        m_transform.rotation = Quaternion.LookRotation(newDir);
    }

    public void OnDamage(int damage) {
        m_life -= damage;
        if (m_life <= 0) {
            m_agent.ResetPath();
            m_anim.SetBool("death", true);
        }
    }
}
