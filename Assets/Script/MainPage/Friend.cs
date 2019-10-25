using System.Collections.Generic;

namespace Script.MainPage
{
    public class Friend
    {
        /// <summary>
        /// 与你通话人的名字
        /// </summary>
        public string UserName;
        /// <summary>
        /// 你们之间的通话记录
        /// </summary>
        public Dictionary<string,string> MessageList = new Dictionary<string, string>();
    }
}