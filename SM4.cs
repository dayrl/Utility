using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zdd.Utility
{
    public class SM4Encryption
    {
        static readonly byte[] sbox = {
	        0xD6, 0x90, 0xE9, 0xFE, 0xCC, 0xE1, 0x3D, 0xB7, 0x16, 0xB6, 0x14, 0xC2, 0x28, 0xFB, 0x2C, 0x05, 
	        0x2B, 0x67, 0x9A, 0x76, 0x2A, 0xBE, 0x04, 0xC3, 0xAA, 0x44, 0x13, 0x26, 0x49, 0x86, 0x06, 0x99, 
	        0x9C, 0x42, 0x50, 0xF4, 0x91, 0xEF, 0x98, 0x7A, 0x33, 0x54, 0x0B, 0x43, 0xED, 0xCF, 0xAC, 0x62, 
	        0xE4, 0xB3, 0x1C, 0xA9, 0xC9, 0x08, 0xE8, 0x95, 0x80, 0xDF, 0x94, 0xFA, 0x75, 0x8F, 0x3F, 0xA6, 
	        0x47, 0x07, 0xA7, 0xFC, 0xF3, 0x73, 0x17, 0xBA, 0x83, 0x59, 0x3C, 0x19, 0xE6, 0x85, 0x4F, 0xA8, 
	        0x68, 0x6B, 0x81, 0xB2, 0x71, 0x64, 0xDA, 0x8B, 0xF8, 0xEB, 0x0F, 0x4B, 0x70, 0x56, 0x9D, 0x35, 
	        0x1E, 0x24, 0x0E, 0x5E, 0x63, 0x58, 0xD1, 0xA2, 0x25, 0x22, 0x7C, 0x3B, 0x01, 0x21, 0x78, 0x87, 
	        0xD4, 0x00, 0x46, 0x57, 0x9F, 0xD3, 0x27, 0x52, 0x4C, 0x36, 0x02, 0xE7, 0xA0, 0xC4, 0xC8, 0x9E, 
	        0xEA, 0xBF, 0x8A, 0xD2, 0x40, 0xC7, 0x38, 0xB5, 0xA3, 0xF7, 0xF2, 0xCE, 0xF9, 0x61, 0x15, 0xA1, 
	        0xE0, 0xAE, 0x5D, 0xA4, 0x9B, 0x34, 0x1A, 0x55, 0xAD, 0x93, 0x32, 0x30, 0xF5, 0x8C, 0xB1, 0xE3, 
	        0x1D, 0xF6, 0xE2, 0x2E, 0x82, 0x66, 0xCA, 0x60, 0xC0, 0x29, 0x23, 0xAB, 0x0D, 0x53, 0x4E, 0x6F, 
	        0xD5, 0xDB, 0x37, 0x45, 0xDE, 0xFD, 0x8E, 0x2F, 0x03, 0xFF, 0x6A, 0x72, 0x6D, 0x6C, 0x5B, 0x51, 
	        0x8D, 0x1B, 0xAF, 0x92, 0xBB, 0xDD, 0xBC, 0x7F, 0x11, 0xD9, 0x5C, 0x41, 0x1F, 0x10, 0x5A, 0xD8, 
	        0x0A, 0xC1, 0x31, 0x88, 0xA5, 0xCD, 0x7B, 0xBD, 0x2D, 0x74, 0xD0, 0x12, 0xB8, 0xE5, 0xB4, 0xB0, 
	        0x89, 0x69, 0x97, 0x4A, 0x0C, 0x96, 0x77, 0x7E, 0x65, 0xB9, 0xF1, 0x09, 0xC5, 0x6E, 0xC6, 0x84, 
	        0x18, 0xF0, 0x7D, 0xEC, 0x3A, 0xDC, 0x4D, 0x20, 0x79, 0xEE, 0x5F, 0x3E, 0xD7, 0xCB, 0x39, 0x48
        };

        static readonly uint[] CK = {
            0x00070e15, 0x1c232a31, 0x383f464d, 0x545b6269, 
            0x70777e85, 0x8c939aa1, 0xa8afb6bd, 0xc4cbd2d9, 
            0xe0e7eef5, 0xfc030a11, 0x181f262d, 0x343b4249, 
            0x50575e65, 0x6c737a81, 0x888f969d, 0xa4abb2b9, 
            0xc0c7ced5, 0xdce3eaf1, 0xf8ff060d, 0x141b2229, 
            0x30373e45, 0x4c535a61, 0x686f767d, 0x848b9299, 
            0xa0a7aeb5, 0xbcc3cad1, 0xd8dfe6ed, 0xf4fb0209, 
            0x10171e25, 0x2c333a41, 0x484f565d, 0x646b7279
        };

        static uint p(uint A)
        {
            uint B;
            byte[] a = new byte[4];
            a[0] = (byte)(A >> 24);
            a[1] = (byte)((A >> 16) & 0xFF);
            a[2] = (byte)((A >> 8) & 0xFF);
            a[3] = (byte)((A) & 0xFF);

            byte[] b = new byte[4];
            b[0] = sbox[a[0]];
            b[1] = sbox[a[1]];
            b[2] = sbox[a[2]];
            b[3] = sbox[a[3]];

            B = (uint)b[0] << 24;
            B += (uint)b[1] << 16;
            B += (uint)b[2] << 8;
            B += (uint)b[3];

            return B;
        }

        static uint Lsr32(uint a, int b)
        {
            return (((a) << (b)) | ((a) >> (32 - (b))));
        }

        static uint L(uint B)
        {
            return B ^ (Lsr32(B, 2)) ^ (Lsr32(B, 10)) ^ (Lsr32(B, 18)) ^ (Lsr32(B, 24));
        }

        static uint L1(uint B)
        {
            return B ^ (Lsr32(B, 13)) ^ Lsr32(B, 23);
        }

        static uint T(uint R)
        {
            return L(p(R));
        }

        static uint T1(uint R)
        {
            return L1(p(R));
        }

        static uint F(uint X0, uint X1, uint X2, uint X3, uint rk)
        {
            return X0 ^ T(X1 ^ X2 ^ X3 ^ rk);
        }

        static uint F1(uint X0, uint X1, uint X2, uint X3, uint rk)
        {
            return X0 ^ T1(X1 ^ X2 ^ X3 ^ rk);
        }

        static readonly uint FK0 = 0xA3B1BAC6;
        static readonly uint FK1 = 0x56AA3350;
        static readonly uint FK2 = 0x677D9197;
        static readonly uint FK3 = 0xB27022DC;

        static void Encrypt(uint K0, uint K1, uint K2, uint K3, uint X0, uint X1, uint X2, uint X3,
                    out uint Y0, out uint Y1, out uint Y2, out uint Y3)
        {
            uint rk;
            int i;
            uint T;

            K0 = K0 ^ FK0;
            K1 = K1 ^ FK1;
            K2 = K2 ^ FK2;
            K3 = K3 ^ FK3;


            for (i = 0; i < 32; i++)
            {
                rk = F1(K0, K1, K2, K3, CK[i]);
                K0 = K1;
                K1 = K2;
                K2 = K3;
                K3 = rk;

                T = F(X0, X1, X2, X3, rk);
                X0 = X1;
                X1 = X2;
                X2 = X3;
                X3 = T;
            }

            Y0 = X3;
            Y1 = X2;
            Y2 = X1;
            Y3 = X0;
        }

        static void Decrypt(uint K0, uint K1, uint K2, uint K3, uint X0, uint X1, uint X2, uint X3,
            out uint Y0, out uint Y1, out uint Y2, out uint Y3)
        {
            uint[] rk = new uint[32];
            int i;
            uint T;

            K0 = K0 ^ FK0;
            K1 = K1 ^ FK1;
            K2 = K2 ^ FK2;
            K3 = K3 ^ FK3;



            for (i = 0; i < 32; i++)
            {
                rk[i] = F1(K0, K1, K2, K3, CK[i]);
                K0 = K1;
                K1 = K2;
                K2 = K3;
                K3 = rk[i];
            }
            for (i = 0; i < 32; i++)
            {
                T = F(X0, X1, X2, X3, rk[31 - i]);
                X0 = X1;
                X1 = X2;
                X2 = X3;
                X3 = T;
            }

            Y0 = X3;
            Y1 = X2;
            Y2 = X1;
            Y3 = X0;
        }

        static uint GET_ULONG_BE(byte[] b, uint i)
        {
            return ((uint)b[i] << 24) | ((uint)b[i + 1] << 16) | ((uint)b[i + 2] << 8) | ((uint)b[i + 3]);
        }

        static void PUT_ULONG_BE(uint n, byte[] b, uint i)
        {
            (b)[(i)] = (byte)((n) >> 24);
            (b)[(i) + 1] = (byte)((n) >> 16);
            (b)[(i) + 2] = (byte)((n) >> 8);
            (b)[(i) + 3] = (byte)((n));
        }
        /// <summary>
        /// SM4解密
        /// </summary>
        /// <param name="bKey">16字节长度密钥</param>
        /// <param name="inData">16字节数据</param>
        /// <returns></returns>
        public static byte[] EncryptB(byte[] bKey, byte[] inData)
        {
            byte[] outData = new byte[16];
            uint K0, K1, K2, K3;
            uint D0, D1, D2, D3;
            uint Y0, Y1, Y2, Y3;
            K0 = GET_ULONG_BE(bKey, 0);
            K1 = GET_ULONG_BE(bKey, 4);
            K2 = GET_ULONG_BE(bKey, 8);
            K3 = GET_ULONG_BE(bKey, 12);
            D0 = GET_ULONG_BE(inData, 0);
            D1 = GET_ULONG_BE(inData, 4);
            D2 = GET_ULONG_BE(inData, 8);
            D3 = GET_ULONG_BE(inData, 12);
            Encrypt(K0, K1, K2, K3, D0, D1, D2, D3, out Y0, out Y1, out Y2, out Y3);

            PUT_ULONG_BE(Y0, outData, 0);
            PUT_ULONG_BE(Y1, outData, 4);
            PUT_ULONG_BE(Y2, outData, 8);
            PUT_ULONG_BE(Y3, outData, 12);
            return outData;
        }
        /// <summary>
        /// sm4加密
        /// </summary>
        /// <param name="bKey">16字节长度密钥</param>
        /// <param name="inData">16字节数据</param>
        /// <returns></returns>
        public static byte[] DecryptB(byte[] bKey, byte[] inData)
        {
            byte[] outData = new byte[16];
            uint K0, K1, K2, K3;
            uint D0, D1, D2, D3;
            uint Y0, Y1, Y2, Y3;
            K0 = GET_ULONG_BE(bKey, 0);
            K1 = GET_ULONG_BE(bKey, 4);
            K2 = GET_ULONG_BE(bKey, 8);
            K3 = GET_ULONG_BE(bKey, 12);
            D0 = GET_ULONG_BE(inData, 0);
            D1 = GET_ULONG_BE(inData, 4);
            D2 = GET_ULONG_BE(inData, 8);
            D3 = GET_ULONG_BE(inData, 12);
            Decrypt(K0, K1, K2, K3, D0, D1, D2, D3, out Y0, out Y1, out Y2, out Y3);

            PUT_ULONG_BE(Y0, outData, 0);
            PUT_ULONG_BE(Y1, outData, 4);
            PUT_ULONG_BE(Y2, outData, 8);
            PUT_ULONG_BE(Y3, outData, 12);

            return outData;
        }
        /// <summary>
        /// 计算MAC
        /// </summary>
        /// <param name="bKey">MAK</param>
        /// <param name="mac">处理过的计算MAC的字符串</param>
        /// <returns></returns>
        public static byte[] GetMac(byte[] bKey, string mac)
        {
            if (string.IsNullOrEmpty(mac) || null == bKey)
                return new byte[32];
            int perBlock = 16;//每个Block的长度
            byte[] srcBuf = Encoding.Default.GetBytes(mac);
            int nBlockCount = srcBuf.Length / perBlock;
            if (srcBuf.Length % perBlock != 0)
            {
                nBlockCount += 1;
            }
            byte[] calBuf = new byte[nBlockCount * perBlock];//参与计算的buffer
            Array.Copy(srcBuf, 0, calBuf, 0, srcBuf.Length);
            byte[] macBuffer = null;

            byte[] tmpBlock = new byte[perBlock];
            Array.Copy(calBuf, 0, tmpBlock, 0, perBlock);
            macBuffer = EncryptB(bKey, tmpBlock);//first

            for (int i = 1; i < nBlockCount; i++)
            {
                Array.Copy(calBuf, i * 16, tmpBlock, 0, perBlock);
                byte[] xorBuf = new byte[perBlock];
                for (int j = 0; j < perBlock; j++)
                {
                    xorBuf[j] = (byte)(tmpBlock[j] ^ macBuffer[j]);
                }
                macBuffer = EncryptB(bKey, xorBuf);
            }
            return macBuffer;
        }
        //[Test]
        static void Main1(string[] args)
        {
            int i, j;

            byte[] indata = { 0X01, 0X23, 0X45, 0X67, 0X89, 0XAB, 0XCD, 0XEF, 0XFE, 0XDC, 0XBA, 0X98, 0X76, 0X54, 0X32, 0X10 };
            byte[] key = { 0X01, 0X23, 0X45, 0X67, 0X89, 0XAB, 0XCD, 0XEF, 0XFE, 0XDC, 0XBA, 0X98, 0X76, 0X54, 0X32, 0X10 };
            //0123456789ABCDEFFEDCBA9876543210
            byte[] bout = new byte[16];

            for (i = 0; i < 10; i++)
            {
                indata = EncryptB(key, indata);
                bout = DecryptB(key, indata);
                for (j = 0; j < indata.Length; j++)
                {
                    Console.Write(indata[j].ToString("X2") + " ");
                }
                Console.WriteLine();
                for (j = 0; j < bout.Length; j++)
                {
                    Console.Write(bout[j].ToString("X2") + " ");
                }
                Console.WriteLine();
            }

            return;
        }
    }
}