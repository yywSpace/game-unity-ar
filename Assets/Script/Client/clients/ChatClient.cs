using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Script.Client.message;
using Script.Client.users;
using UnityEngine;

namespace Script.Client.clients
{
    /// <summary>
    /// 客户端, 向服务端发送消息
    /// 发送命令：
    ///     Login
    ///     Logout
    ///     Chat
    /// 接收命令：UserList
    /// </summary>
    class ChatClient
    {
        public Action<List<ClientUser>> OnUserListReceive;

        /// <summary>
        /// arg1:userName,arg2:userMessage
        /// </summary>
        public Action<string, string> OnChatMessageReceive;

        public IPEndPoint LocalEndPoint { get; set; }
        public IPEndPoint RemoteEndPoint { get; set; }
        public string UserName { get; set; }
        private UdpClient _client;

        public void Login()
        {
            _client = new UdpClient(LocalEndPoint);
            System.Threading.Tasks.Task.Run(() => Receive());
            MessageData data = new MessageData() {Message = UserName, MessageType = MessageType.Login};
            string msg = JsonUtility.ToJson(data);
            Debug.Log("Client:Login " + msg);
            SendMessage(msg);
        }

        public void Logout()
        {
            MessageData data = new MessageData() {Message =  UserName, MessageType = MessageType.LogOut};
            string msg = JsonUtility.ToJson(data);
            Debug.Log("Client:Logout " + msg);
            SendMessage(msg);
        }

        public void ChatInRoom(string message)
        {
            MessageData data = new MessageData()
                {Message = UserName + "," + message, MessageType = MessageType.ChatInRoom};
            string msg = JsonUtility.ToJson(data);
            Debug.Log("Client:Say:" +msg);
            SendMessage(msg);
        }

        public void Location(float x, float y)
        {
            MessageData data = new MessageData()
            {
                Message = UserName + "," + x + "," + y,
                MessageType = MessageType.Location
            };
            string msg = JsonConvert.SerializeObject(data);
            Debug.Log("GameClient:Location " + msg);
            SendMessage(msg);
        }
        
        public void UserList()
        {
            string json = JsonConvert.SerializeObject(new MessageData()
            {
                Message = UserName,
                MessageType = MessageType.UserList
            });
            Debug.Log("Client:UserList:" +json);
            SendMessage(json);
        }
        private void SendMessage(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            _client.Send(bytes, bytes.Length, RemoteEndPoint);
        }

        private void Receive()
        {
            var remote = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                try
                {
                    // 关闭_client时会产生异常
                    var result = _client.Receive(ref remote);
                    string message = Encoding.UTF8.GetString(result);
                    MessageData data = JsonUtility.FromJson<MessageData>(message);
        
                    string userName;

                    switch (data.MessageType)
                    {
                        case MessageType.UserList:
                            List<ClientUser> users = new List<ClientUser>();
                            Debug.Log(message);

                            foreach (var user in data.Message.Split(';'))
                            {
                                if (user == "")
                                    continue;
       
                                string[] userArg = user.Split(',');
                                  userName = userArg[0];
                                string[] userEndPointArg = userArg[1].Split(':');
                                IPEndPoint userIpEndPoint = new IPEndPoint(IPAddress.Parse(userEndPointArg[0]),
                                    int.Parse(userEndPointArg[1]));
                                users.Add(new ClientUser()
                                {
                                    UserName = userName,
                                    UserEndPoint = userIpEndPoint,
                                    MapLocation = new Location(){X=float.Parse(userArg[2]),Y=float.Parse(userArg[3])}
                                });
                            }
                            
                            OnUserListReceive?.Invoke(users);
                            break;
                        case MessageType.ChatInRoom:
                            string[] chatArgs = data.Message.Split(',');
                            userName = chatArgs[0];
                            string userSay = chatArgs[1];
                            OnChatMessageReceive?.Invoke(userName, userSay);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    break;
                }

            }
        }
    }
}