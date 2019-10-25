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
    public class GameClient
    {        
        public Action<List<ClientUser>> OnGameUserListReceive;
        /// <summary>
        /// 参数为"name,ip,x,y;name1,ip1,x1,y1;"此种类型字符串
        /// </summary>
        public Action<string> OnGameUserListStrReceive;
        public Action<string, string, string> OnGameUserMessageReceive;
        public ClientUser User;
        public IPEndPoint RemoteEndPoint { get; set; }
        private UdpClient _client;
        
        public void Login()
        {
            _client = new UdpClient(User.UserEndPoint);
            System.Threading.Tasks.Task.Run(() => Receive());
            MessageData data = new MessageData() {Message = User.UserName, MessageType = MessageType.Login};
            string msg = JsonConvert.SerializeObject(data);
            Debug.Log("GameClient:Login " + msg);
            SendMessage(msg);
        }

        public void Logout()
        {
            MessageData data = new MessageData() {Message = User.UserName, MessageType  = MessageType.LogOut};
            string msg = JsonConvert.SerializeObject(data);
            Debug.Log("GameClient:Logout " + msg);
            SendMessage(msg);
        }

        public void Location(float x, float y)
        {
            MessageData data = new MessageData() {
                Message = User.UserName + "," + x + "," + y,
                MessageType  = MessageType.Location
                
            };
            string msg = JsonConvert.SerializeObject(data);
            Debug.Log("GameClient:Location " + msg);
            SendMessage(msg);
        }

        public void UserList()
        {
            string json = JsonConvert.SerializeObject(new MessageData()
            {
                Message = User.UserName,
                MessageType = MessageType.UserList
            });
            Debug.Log("Client:UserList:" +json);
            SendMessage(json);
        }
        public void ChatToUser(string otherUser, string message)
        {
            MessageData data = new MessageData()
            {
                Message = User.UserName + "," + otherUser + "," + message,
                MessageType = MessageType.ChatToUser
            };
            string msg = JsonConvert.SerializeObject(data);
            Debug.Log("Client:ChatToUser:" +msg);
            SendMessage(msg);
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
                    MessageData data = JsonConvert.DeserializeObject<MessageData>(message);
                    switch (data.MessageType)
                    {
                        case MessageType.UserList:
                            // UserList(UserList:name,ip,x,y;name1,ip1;')
                            List<ClientUser> users = new List<ClientUser>();
                            foreach (var user in data.Message.Split(';'))
                            {
                                if (user == "")
                                    continue;
                                string[] userArg = user.Split(',');
                                string userName = userArg[0];
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
                            OnGameUserListStrReceive?.Invoke(data.Message);
                            OnGameUserListReceive?.Invoke(users);
                            break;
                      
                        case MessageType.ChatToUser:
                            string[] args = data.Message.Split(',');
                            OnGameUserMessageReceive?.Invoke(args[0], args[1], args[2]);
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