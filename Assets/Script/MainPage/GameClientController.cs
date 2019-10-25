using System.Collections.Generic;
using System.Net;
using Script.Client.clients;
using Script.Client.users;
using UnityEngine;
using User = Script.MainPage.UserManagement.User;

namespace Script.MainPage
{
    public class GameClientController : MonoBehaviour
    {
        /// <summary>
        /// 记录此用户所有的同话记录
        /// </summary>
        public  List<Friend> friends = new List<Friend>();
        readonly IPEndPoint _localEndPoint = new IPEndPoint(GetLocalIpAddress(), 3001);
        readonly IPEndPoint _remoteEndPoint = new IPEndPoint(IPAddress.Parse("47.96.152.133"), 3000);
        public bool hasReceiveMessage;

        private GameClient _gameClient;

        // Start is called before the first frame update
        void Start()
        {
            _gameClient = ClientLab.GetGameClient();
            _gameClient.User = new ClientUser() {UserName = GetLocalIpAddress().ToString(), UserEndPoint = _localEndPoint};
            _gameClient.RemoteEndPoint = _remoteEndPoint;
            _gameClient.OnGameUserListReceive = (userList) => { };
            _gameClient.OnGameUserMessageReceive = (userName, otherName, message) =>
            {
                // 如果信息是自己发的，此时userName为自己，otherName 为朋友名字
                if (userName == _gameClient.User.UserName)
                {
                    Friend friend = friends.Find(f => f.UserName == otherName);
                    // 如果列表中没有此人记录
                    if (friend == null)
                    {
                        friend = new Friend {UserName = otherName};
                        friend.MessageList.Add(userName, message);
                        friends.Add(friend);
                    }
                    else
                        friend.MessageList.Add(userName, message);
                }
                // userName 为此消息的发送人，otherName为此消息的接收人
                // 当接受到的消息不是本人发送的时，记录发送者
                // 此时userName为朋友名字，otherName为自己
                if (userName != _gameClient.User.UserName)
                {
                    Friend friend = friends.Find(f => f.UserName == userName);
                    // 如果列表中没有此人记录
                    if (friend == null)
                    {
                        friend = new Friend {UserName = userName};
                        friend.MessageList.Add(userName, message);
                        friends.Add(friend);
                    }
                    else
                        friend.MessageList.Add(userName, message);
                }

                hasReceiveMessage = true;
            };
            _gameClient.OnGameUserListStrReceive = (userListStr) => { };
        }
 

        static IPAddress GetLocalIpAddress()
        {
            string hostname = Dns.GetHostName();
            IPHostEntry localhost = Dns.GetHostEntry(hostname);
            return localhost.AddressList[0];
        }
    }

}
