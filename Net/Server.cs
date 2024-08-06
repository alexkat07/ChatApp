using ChatClient.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net
{
    class Server
    {
        TcpClient client;
        PacketBuilder packetBuilder;
        public PacketReader packetReader;

        public event Action connectedEvent;
        public event Action msgReceivedEvent;
        public event Action userDisconnectEvent;
        public Server() 
        {
            client = new TcpClient();
        }
        public void ConnectToServer(string userName) 
        {
            if (!client.Connected) 
            {
                client.Connect("127.0.0.1", 7891);
                packetReader = new PacketReader(client.GetStream());

                if (!string.IsNullOrEmpty(userName)) 
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteMessage(userName);
                    client.Client.Send(connectPacket.GetPacketBytes());

                }
                ReadPackets();
        

            }
        }
        private void ReadPackets() 
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            msgReceivedEvent?.Invoke();
                            break;
                        case 10:
                            userDisconnectEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("ah yes..");
                            break;
                    }

                }
            });
        }
        public void SendMessageToServer(string message) 
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            client.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
