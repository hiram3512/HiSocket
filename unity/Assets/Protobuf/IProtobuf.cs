using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HiSocket.Protobuf
{
    public interface IProtobuf
    {
        byte[] Serialize<T>(T param);

        T Deserialize<T>(byte[] param);
    }
}
