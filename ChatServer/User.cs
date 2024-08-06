using ChatServer.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class User
    {
        public string userName { get; set; }
        public Guid ID { get; set; }
        public TcpClient UserSocket { get; set; }

        PacketReader _packetReader;
        public User(TcpClient user)   
        { 
            UserSocket = user;
            ID = Guid.NewGuid();
            _packetReader = new PacketReader(UserSocket.GetStream());

            var opcode = _packetReader.ReadByte();
            userName = _packetReader.ReadMesage();

            Console.WriteLine($"{DateTime.Now}: A new user has connected with the username: {userName}");

            Task.Run(() => Process() );
        }
        void Process() 
        {
            while ( true ) 
            {
                try 
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode ) 
                    {
                        case 5:
                            var msg = _packetReader.ReadMesage();
                            Console.WriteLine($"{DateTime.Now}: Message received! {msg}");
                            Program.BroadcastMessage( msg );
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"[{ID.ToString()}]: Disconnected!");
                    UserSocket.Close();
                    throw;
                }
            }
        }
    }
}
