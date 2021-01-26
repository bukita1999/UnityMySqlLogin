
using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

public class register : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField userNameInput;
    public InputField passwordInput;
    public GameObject sueccedUI;
    public GameObject failUI;
    void Start()
    {

    }

    public void OnClickedButton()
    {

        Register(new string[] { userNameInput.text, passwordInput.text });
    }

    private void Register(string[] strRegister)
    {
        string connStr = "Database=EnglishARCore;datasource=aliyun.yanduduandchuangdudu.club;port=3306;user=caochuang;pwd=RFD7W6jjPeYbTfW4;";
        MySqlConnection conn = new MySqlConnection(connStr);
        if (conn.State == ConnectionState.Open)
        {
            Debug.Log("数据库连接成功");
        }
        else
        {
            Debug.Log("数据库连接失败");
        }
        conn.Open();
        //先要查询一下目前数据库是否有重复的数据。
        MySqlCommand myCommand = new MySqlCommand("select*from user", conn);
        MySqlDataReader reader = myCommand.ExecuteReader();
        List<string> user = new List<string>();
        while (reader.Read())
        {
            string username = reader.GetString("name");
            string password = reader.GetString("password");
            user.Add(username);
        }
        //**避免账号重复。**
        foreach (var item in user)
        {
            if (user.Contains(strRegister[0]))
            {
                Debug.Log("账号已存在！");
                failUI.SetActive(true);//这个是我的提示界面
                break;
            }
            else
            {
                reader.Close();//**先将查询的功能关闭。**
                MySqlCommand cmd = new MySqlCommand("insert into user set name ='" + strRegister[0] + "'" + ",password='" + strRegister[1] + "'", conn);
                cmd.Parameters.AddWithValue("name", strRegister[0]);
                cmd.Parameters.AddWithValue("password", strRegister[1]);
                cmd.ExecuteNonQuery();
                Debug.Log("注册成功！");
                sueccedUI.SetActive(true);//也是提示界面
                break;
            }
        }
    }
}