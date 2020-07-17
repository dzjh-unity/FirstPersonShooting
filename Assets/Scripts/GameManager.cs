using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[AddComponentMenu("GameScript/GameManager")]

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    // 游戏得分
    public int m_score = 0;

    // 游戏最高得分
    public static int m_maxscore = 0;

    // 弹药数量
    public int m_ammo = 100;

    // 游戏主角
    Player m_player;

    // UI文字
    Text textAmmo;
    Text textScore;
    Text textHP;
    Button btnRestart;

    // Start is called before the first frame update
    void Start()
    {
         Instance = this;

         m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

         GameObject oldGameObj = GameObject.Find("Canvas/OldGameObject");
         foreach(Transform t in oldGameObj.transform.GetComponentsInChildren<Transform>()) {
             if (t.name.CompareTo("TextAmmo") == 0) {
                textAmmo = t.GetComponent<Text>();
             } else if (t.name.CompareTo("TextHP") == 0) {
                textHP = t.GetComponent<Text>();
             } else if (t.name.CompareTo("TextScore") == 0) {
                textScore = t.GetComponent<Text>();
             } else if (t.name.CompareTo("ButtonRestart") == 0) {
                btnRestart = t.GetComponent<Button>();
                btnRestart.onClick.AddListener(delegate(){ // 设置重新开始游戏按钮事件
                    // 读取当前关卡
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });
                btnRestart.gameObject.SetActive(false); // 游戏刚加载时，隐藏重新开始按钮
             }
         }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 增加分数
    public void AddScore(int score) {
        m_score += score;
        textScore.text = "分数 <color=yellow>" + m_score.ToString() + "</color>";
    }

    // 减少弹药
    public void ReduceAmmo(int ammo) {
        m_ammo -= ammo;
        // 如果弹药数小于0，重新装弹
        if (m_ammo <= 0) {
            m_ammo = 100 - m_ammo;
        }
        textAmmo.text = m_ammo.ToString() + "/100";
    }

    // 更新生命值
    public void SetHP(int hp) {
        textHP.text = hp.ToString();
        // 如果主角生命值为0时，显示重新开始游戏按钮
        if (hp <= 0) {
            btnRestart.gameObject.SetActive(true);
        }
    }
}
