using ChatServer.IO;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    internal class Program
    {
        static List<User> users;
        static TcpListener listener;
        static void Main(string[] args)
        {
            users = new List<User>();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            listener.Start();

            while (true)
            {
                var newUser = new User(listener.AcceptTcpClient());
                users.Add(newUser);

                BroadcastConnection();
            }



        }
        static void BroadcastConnection()
        {
            foreach (var user in users)
            {
                foreach (var usr in users)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteMessage(usr.userName);
                    broadcastPacket.WriteMessage(usr.ID.ToString());
                    user.UserSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }
        public static void BroadcastMessage(string message)
        {
            foreach (var user in users)
            {
                var messagePacket = new PacketBuilder();
                messagePacket.WriteOpCode(5);
                messagePacket.WriteMessage(message);
                user.UserSocket.Client.Send(messagePacket.GetPacketBytes());
            }
        }
        public static void BroadcastDisconnect(string ID)
        {
            var disconnectedUser = users.Where(x => x.ID.ToString() == ID).FirstOrDefault();
            users.Remove(disconnectedUser);
            foreach (var user in users)
            {
                var broadcastPakcet = new PacketBuilder();
                broadcastPakcet.WriteOpCode(10);
                broadcastPakcet.WriteMessage(ID);
                user.UserSocket.Client.Send(broadcastPakcet.GetPacketBytes());
            }

        }
    }
}
