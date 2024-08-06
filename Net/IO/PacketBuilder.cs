using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net.IO
{
    class PacketBuilder
    {
        MemoryStream ms;
        public PacketBuilder() 
        {
            ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode) 
        {
            ms.WriteByte(opcode);
        }

        public void WriteMessage(string msg) 
        {
            var msgLength= ms.Length;
            ms.Write(BitConverter.GetBytes(msg.Length));
            ms.Write(Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPacketBytes() 
        {
            return ms.ToArray();
        }
    }
}
