using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ChatServer.IO
{
    public class PacketReader : BinaryReader 
    {
        private NetworkStream _netStream;
        public PacketReader(NetworkStream netStream) : base(netStream) 
        {
            _netStream = netStream;
        }

        public string ReadMesage() 
        {
            byte[] msgBuffer;
            var length = ReadInt32();
            msgBuffer = new byte[length];
            _netStream.Read(msgBuffer, 0, length);

            var msg = Encoding.ASCII.GetString(msgBuffer);
            return msg;
        }

    }
}
