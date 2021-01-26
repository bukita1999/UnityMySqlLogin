using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class logtest : MonoBehaviour
{
    public InputField userNameInput;
    public InputField passwordInput;
    public GameObject failUI;
    public GameObject loseUI;
    private AsyncOperation async;
    [HideInInspector] public static int count = 0;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnClickedButton()
    {

        Login(new string[] { userNameInput.text, passwordInput.text });
    }

    // Update is called once per frame

    private void Login(string[] str)
    {
        Dictionary<string, string> myDic = new Dictionary<string, string>();
        myDic.Clear();
        string connStr = "Database=EnglishARCore;datasource=aliyun.yanduduandchuangdudu.club;port=3306;user=caochuang;pwd=RFD7W6jjPeYbTfW4;";
        MySqlConnection conn = new MySqlConnection(connStr);
        if(conn.State == ConnectionState.Open)
        {
            Debug.Log("数据库连接成功");
        }
        else
        {
            Debug.Log("数据库连接失败");
        }
        conn.Open();
        #region 查询
        MySqlCommand cmd = new MySqlCommand("select * from user", conn);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string username = reader.GetString("name");
            string password = reader.GetString("password");
            myDic.Add(username, password);
        }
        if (myDic.ContainsKey(str[0]))
        {
            Debug.Log("账号存在！");
            string vale;
            if (myDic.TryGetValue(str[0], out vale))
            {
                if (vale == str[1])
                {
                    Debug.Log("登录成功！");
                    PlayerPrefs.SetString("username", userNameInput.text);
                    Debug.Log(PlayerPrefs.GetString("username"));
                    //判断是否跳转成功还有记录次数，不需要的可以不写下面直接用SceneManager.loadlevel来跳转
                    async = SceneManager.LoadSceneAsync("LevelMenu");
                    if (async.isDone)
                    {
                        count++;
                    }
                    PlayerPrefs.SetInt("menuload", count);
                    Debug.Log(PlayerPrefs.GetInt("menuload"));
                }
                else
                {

                    Debug.Log("密码错误，请重新输入！");
                    failUI.SetActive(true);
                }
            }
        }
        else
        {
            Debug.Log("账号不存在！");
            loseUI.SetActive(true);
        }
        #endregion
        reader.Close();//关闭读取
        conn.Close();//关闭与数据库的连接
    }
}