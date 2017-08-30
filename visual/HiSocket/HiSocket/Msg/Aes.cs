//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************
using System;

namespace HiSocket.Msg
{
    class Aes
    {
        private string _key;

        public Aes(string key)
        {
            if (_key.Length != 32)
                throw new Exception("please check key, current length is: " + _key.Length);
            _key = key;
        }

        ///<summary>
        ///加密
        ///</summary>
        ///<param name="toEncrypt">需要被加密的数据</param>
        ///<returns></returns>
        public byte[] Encrypt(byte[] toEncrypt)
        {
            Byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(_key);
            System.Security.Cryptography.RijndaelManaged aes = new System.Security.Cryptography.RijndaelManaged();
            aes.Key = keyArray;
            aes.Mode = System.Security.Cryptography.CipherMode.ECB;
            aes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            System.Security.Cryptography.ICryptoTransform transform = aes.CreateEncryptor();
            Byte[] resultArray = transform.TransformFinalBlock(toEncrypt, 0, toEncrypt.Length);
            return resultArray;
        }

        ///<summary>
        ///解密
        ///</summary>
        ///<param name="toDecrypt">需要被解密的数据</param>
        ///<returns></returns>
        public byte[] Decrypt(byte[] toDecrypt)
        {
            Byte[] keyArray = System.Text.UTF8Encoding.UTF8.GetBytes(_key);
            System.Security.Cryptography.RijndaelManaged aes = new System.Security.Cryptography.RijndaelManaged();
            aes.Key = keyArray;
            aes.Mode = System.Security.Cryptography.CipherMode.ECB;
            aes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            System.Security.Cryptography.ICryptoTransform transform = aes.CreateDecryptor();
            Byte[] resultArray = transform.TransformFinalBlock(toDecrypt, 0, toDecrypt.Length);
            return resultArray;
        }
    }
}