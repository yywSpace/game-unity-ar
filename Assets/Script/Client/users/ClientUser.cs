using System.Net;

namespace Script.Client.users
{
    public class ClientUser
    {
        public string UserName { get; set; }
        public IPEndPoint UserEndPoint { get; set; }

        public Location MapLocation = new Location() {X = 0, Y = 0};

        public override string ToString()
        {
            return UserName + "," + UserEndPoint + "," + MapLocation;
        }
    }
}