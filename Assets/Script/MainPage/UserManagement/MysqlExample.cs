using System;
using System.IO;
using MySql.Data.MySqlClient;
using UnityEngine;

namespace Script.MainPage.UserManagement
{
    public class MysqlExample : MonoBehaviour
    {
        private void Start()
        {
            User user = UserLab.GetUser("1095204049");
              print(user.ReceiveTaskNum);
              print(user.AccomplishTaskNum);
              print(user.ReleaseTaskNum);
              print("1345");
        }

        
    }
}