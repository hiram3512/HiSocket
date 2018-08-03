/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using HiSocket.Message;

namespace HiSocket.Example
{
    class MessageRegistExample
    {
        void Main()
        {
            MsgRegister.Regist("PlayDie", OnPlayerDie);

            //when get bytes from socket(OnReceive)
            {
                string name = "PlayerDie";
                IByteArray byteArray = new ByteArray();
                byteArray.Write(new byte[10]);
                MsgRegister.Dispatch(name, byteArray);
            }
        }

        void OnPlayerDie(IByteArray byteArray)
        {
            var msg = new MsgBytes(byteArray);
            var playerName = msg.Read<string>(10);//Get player name
            var gold = msg.Read<int>();//Get player's gold
            //player die logic
        }
    }
}
