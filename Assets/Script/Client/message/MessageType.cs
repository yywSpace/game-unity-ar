namespace Script.Client.message
{
    /// <summary>
    /// 简单的协议类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 聊天室中聊天
        /// </summary>
        ChatInRoom = 0,
        /// <summary>
        /// 两人聊天 userName,otherUserName,message
        /// </summary>
        ChatToUser = 1,
        /// <summary>
        /// 登陆 userName,ipEndPoint
        /// </summary>
        Login = 2,
        /// <summary>
        /// 登出 name
        /// </summary>
        LogOut = 3,
        /// <summary>
        /// 在线人员列表 name,ipEndPoint,x,y;name1,ipEndPoint1,x1,y1;...
        /// </summary>
        UserList = 4,
        /// <summary>
        /// 每个用户的位置
        /// </summary>
        Location = 5,
    }
}
