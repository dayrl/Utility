using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Zdd.Utility
{
    public class Encryption
    {
        /// <summary>
        /// 置换选择1的矩阵
        /// </summary>
        private static int[] iSelePM1 = {
	        57, 49, 41, 33, 25, 17, 09, 01, 
            58, 50, 42, 34, 26, 18, 10, 02, 
            59, 51, 43, 35, 27, 19, 11, 03, 
            60, 52, 44, 36, 63, 55, 47, 39, 
            31, 23, 15, 07, 62, 54, 46, 38, 
            30, 22, 14, 06, 61, 53, 45, 37, 
            29, 21, 13, 05, 28, 20, 12, 04 };

        /// <summary>
        /// 置换选择2的矩阵
        /// </summary>
        private static int[] iSelePM2 = {
	        14, 17, 11, 24, 01, 05, 03, 28, 
            15, 06, 21, 10, 23, 19, 12, 04, 
            26, 08, 16, 07, 27, 20, 13, 02, 
            41, 52, 31, 37, 47, 55, 30, 40, 
            51, 45, 33, 48, 44, 49, 39, 56, 
            34, 53, 46, 42, 50, 36, 29, 32 };

        /// <summary>
        /// 循环左移位数表
        /// </summary>
        private static int[] iROLtime = {
	        1, 1, 2, 2, 2, 2, 2, 2, 
            1, 2, 2, 2, 2, 2, 2, 1 };
        /// <summary>
        /// 初始置换IP(将输入的58位换到第1位,第50位换到第2位...,得到L0表示前32位 R0表示后32位)
        /// </summary>
        private static int[] iInitPM = {
	        58, 50, 42, 34, 26, 18, 10, 02, 
            60, 52, 44, 36, 28, 20, 12, 04, 
            62, 54, 46, 38, 30, 22, 14, 06, 
            64, 56, 48, 40, 32, 24, 16, 08, 
            57, 49, 41, 33, 25, 17, 09, 01, 
            59, 51, 43, 35, 27, 19, 11, 03, 
            61, 53, 45, 37, 29, 21, 13, 05, 
            63, 55, 47, 39, 31, 23, 15, 07 };

        /// <summary>
        /// 初始逆置换
        /// </summary>
        private static int[] iInvInitPM = {
	        40, 08, 48, 16, 56, 24, 64, 32, 
            39, 07, 47, 15, 55, 23, 63, 31, 
            38, 06, 46, 14, 54, 22, 62, 30, 
            37, 05, 45, 13, 53, 21, 61, 29, 
            36, 04, 44, 12, 52, 20, 60, 28, 
            35, 03, 43, 11, 51, 19, 59, 27, 
            34, 02, 42, 10, 50, 18, 58, 26, 
            33, 01, 41, 09, 49, 17, 57, 25 };
        /// <summary>
        /// 选择运算E
        /// </summary>
        private static int[] iEPM = {
	        32, 01, 02, 03, 04, 05, 04, 05, 
            06, 07, 08, 09, 08, 09, 10, 11, 
            12, 13, 12, 13, 14, 15, 16, 17, 
            16, 17, 18, 19, 20, 21, 20, 21, 
            22, 23, 24, 25, 24, 25, 26, 27, 
            28, 29, 28, 29, 30, 31, 32, 01 };
        /// <summary>
        /// 置换运算P
        /// </summary>
        private static int[] iPPM = {
	        16, 07, 20, 21, 29, 12, 28, 17, 
            01, 15, 23, 26, 05, 18, 31, 10, 
            02, 08, 24, 14, 32, 27, 03, 09, 
            19, 13, 30, 06, 22, 11, 04, 25 };

        /// <summary>
        /// 8个S盒
        /// </summary>
        private static int[][] iSPM = new int[8][];

        /// <summary>
        /// ctr.
        /// </summary>
        public Encryption()
        {
            iSPM[0] = new int[]{
                14, 4, 13, 1, 2, 15, 11, 8, 
                3, 10, 6, 12, 5, 9, 0, 7, 
                0, 15, 7,4, 14, 2, 13, 1, 
                10, 6, 12, 11, 9, 5, 3, 8, 
                4, 1, 14, 8,13, 6, 2, 11, 
                15, 12, 9, 7, 3, 10, 5, 0, 
                15, 12, 8, 2, 4,9, 1, 7, 
                5, 11, 3, 14, 10, 0, 6, 13 };

            iSPM[1] = new int[]{
                15, 1, 8, 14, 6, 11, 3, 4, 
                9, 7, 2, 13, 12, 0, 5, 10, 3, 
                13, 4,7, 15, 2, 8, 14, 12, 
                0, 1, 10, 6, 9, 11, 5, 0, 
                14, 7, 11,10, 4, 13, 1, 5, 
                8, 12, 6, 9, 3, 2, 15, 13, 
                8, 10, 1, 3,15, 4, 2, 11, 
                6, 7, 12, 0, 5, 14, 9};
            iSPM[2] = new int[] { 
                10, 0, 9, 14, 6, 3, 15, 5, 
                1, 13, 12, 7, 11, 4, 2, 8, 
                13, 7, 0,9, 3, 4, 6, 10, 
                2, 8, 5, 14, 12, 11, 15, 1, 
                13, 6, 4, 9, 8,15, 3, 0, 
                11, 1, 2, 12, 5, 10, 14, 7, 
                1, 10, 13, 0, 6, 9,8, 7, 
                4, 15, 14, 3, 11, 5, 2, 12};
            iSPM[3] = new int[] {
                7, 13, 14, 3, 0, 6, 9, 10, 
                1, 2, 8, 5, 11, 12, 4, 15, 
                13, 8, 11,5, 6, 15, 0, 3, 
                4, 7, 2, 12, 1, 10, 14, 9, 
                10, 6, 9, 0, 12,11, 7, 13, 
                15, 1, 3, 14, 5, 2, 8, 4, 
                3, 15, 0, 6, 10, 1,13, 8, 
                9, 4, 5, 11, 12, 7, 2, 14 };
            iSPM[4] = new int[] {  
                2, 12, 4, 1, 7, 10, 11, 6, 
                8, 5, 3, 15, 13, 0, 14, 9, 
                14, 11, 2,12, 4, 7, 13, 1, 
                5, 0, 15, 10, 3, 9, 8, 6, 
                4, 2, 1, 11, 10,13, 7, 8, 
                15, 9, 12, 5, 6, 3, 0, 14, 
                11, 8, 12, 7, 1, 14,2, 13, 
                6, 15, 0, 9, 10, 4, 5, 3};
            iSPM[5] = new int[] {
                12, 1, 10, 15, 9, 2, 6, 8, 
                0, 13, 3, 4, 14, 7, 5, 11, 
                10, 15, 4,2, 7, 12, 9, 5, 
                6, 1, 13, 14, 0, 11, 3, 8, 
                9, 14, 15, 5, 2, 8, 12, 3,
                7, 0, 4, 10, 1, 13, 11, 6, 
                4, 3, 2, 12, 9, 5, 15,10, 
                11, 14, 1, 7, 6, 0, 8, 13 };
            iSPM[6] = new int[] {  
                4, 11, 2, 14, 15, 0, 8, 13, 
                3, 12, 9, 7, 5, 10, 6, 1, 
                13, 0, 11,7, 4, 9, 1, 10, 
                14, 3, 5, 12, 2, 15, 8, 6, 
                1, 4, 11, 13,12, 3, 7, 14, 
                10, 15, 6, 8, 0, 5, 9, 2, 
                6, 11, 13, 8, 1, 4,10, 
                7, 9, 5, 0, 15, 14, 2, 3, 12};
            iSPM[7] = new int[] {  
                13, 2, 8, 4, 6, 15, 11, 1, 
                10, 9, 3, 14, 5, 0, 12, 7, 
                1, 15, 13,8, 10, 3, 7, 4, 
                12, 5, 6, 11, 0, 14, 9, 2, 
                7, 11, 4, 1, 9,12, 14, 2, 
                0, 6, 10, 13, 15, 3, 5, 8, 
                2, 1, 14, 7, 4, 10,8, 13, 
                15, 12, 9, 0, 3, 5, 6, 11};
        }

        private int[] iCipherKey = new int[64];
        private int[] iCKTemp = new int[56];
        private int[] iPlaintext = new int[64];
        private int[] iCiphertext = new int[64];
        private int[] iPKTemp = new int[64];
        private int[] iL = new int[32];
        private int[] iR = new int[32];

        /// <summary>
        /// 数组置换 iSource与iDest的大小不一定相等
        /// </summary>
        /// <param name="iSource"></param>
        /// <param name="iDest"></param>
        /// <param name="iPM"></param>
        private void permu(int[] iSource, int[] iDest, int[] iPM)
        {
            if (iDest == null) iDest = new int[iPM.Length];

            for (int i = 0; i < iPM.Length; i++)
                iDest[i] = iSource[iPM[i] - 1];
        }

        /// <summary>
        /// 将字节数组进行 位整数 压缩
        ///  例如：{0x35,0xf3}={0,0,1,1,0,1,0,1,1,1,1,1,0,0,1,1}, bArray-&gt;iArray
        /// </summary>
        /// <param name="bArray"></param>
        /// <param name="iArray"></param>
        private void arrayBitToI(byte[] bArray, int[] iArray)
        {
            for (int i = 0; i < iArray.Length; i++)
            {
                iArray[i] = (int)(bArray[i / 8] >> (7 - i % 8) & 0x01);
            }
        }

        // 将整形数组进行 整数-〉位 压缩
        // arrayBitToI的逆变换,iArray->bArray
        /// <summary>
        /// 将整形数组进行 整数位压缩
        /// </summary>
        /// <param name="bArray"></param>
        /// <param name="iArray"></param>
        private void arrayIToBit(byte[] bArray, int[] iArray)
        {
            for (int i = 0; i < bArray.Length; i++)
            {
                bArray[i] = (byte)iArray[8 * i];
                for (int j = 1; j < 8; j++)
                {
                    bArray[i] = (byte)(bArray[i] << 1);
                    bArray[i] += (byte)iArray[8 * i + j];
                }
            }
        }

        // 数组的逐项模2加
        // array1[i]=array1[i]^array2[i]
        /// <summary>
        /// 数组的逐项模2加
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        private void arrayM2Add(int[] array1, int[] array2)
        {
            for (int i = 0; i < array2.Length; i++)
            {
                array1[i] ^= array2[i];
            }
        }

        /// <summary>
        /// 一个数组等分成两个数组-数组切割
        /// </summary>
        /// <param name="iSource"></param>
        /// <param name="iDest1"></param>
        /// <param name="iDest2"></param>
        private void arrayCut(int[] iSource, int[] iDest1, int[] iDest2)
        {
            int k = iSource.Length;
            for (int i = 0; i < k / 2; i++)
            {
                iDest1[i] = iSource[i];
                iDest2[i] = iSource[i + k / 2];
            }
        }

        /// <summary>
        /// 两个等大的数组拼接成一个arrayCut的逆变换
        /// </summary>
        /// <param name="iDest"></param>
        /// <param name="iSource1"></param>
        /// <param name="iSource2"></param>
        private void arrayComb(int[] iDest, int[] iSource1, int[] iSource2)
        {
            int k = iSource1.Length;
            for (int i = 0; i < k; i++)
            {
                iDest[i] = iSource1[i];
                iDest[i + k] = iSource2[i];
            }
        }

        /// <summary>
        /// 子密钥产生算法中的循环左移
        /// </summary>
        /// <param name="array"></param>
        private void ROL(int[] array)
        {
            int temp = array[0];
            for (int i = 0; i < 27; i++)
            {
                array[i] = array[i + 1];
            }
            array[27] = temp;

            temp = array[28];
            for (int i = 0; i < 27; i++)
            {
                array[28 + i] = array[28 + i + 1];
            }
            array[55] = temp;


        }

        /// <summary>
        /// 16个子密钥完全倒置
        /// </summary>
        /// <param name="iSubKeys"></param>
        /// <returns></returns>
        private int[][] invSubKeys(int[][] iSubKeys)
        {
            int[][] iInvSubKeys = new int[16][];
            for (int i = 0; i < 16; i++)
            {
                iInvSubKeys[i] = new int[48];
                for (int j = 0; j < 48; j++)
                    iInvSubKeys[i][j] = iSubKeys[15 - i][j];
            }
            return iInvSubKeys;
        }

        /// <summary>
        /// S盒代替 输入输出皆为部分数组，因此带偏移量
        /// </summary>
        /// <param name="iInput"></param>
        /// <param name="iOffI"></param>
        /// <param name="iOutput"></param>
        /// <param name="iOffO"></param>
        /// <param name="iSPM"></param>
        private void Sbox(int[] iInput, int iOffI, int[] iOutput, int iOffO,
                int[] iSPM)
        {
            int iRow = iInput[iOffI] * 2 + iInput[iOffI + 5]; // S盒中的行号
            int iCol = iInput[iOffI + 1] * 8 + iInput[iOffI + 2] * 4
                    + iInput[iOffI + 3] * 2 + iInput[iOffI + 4];
            // S盒中的列号
            int x = iSPM[16 * iRow + iCol];
            iOutput[iOffO] = x >> 3 & 0x01;
            iOutput[iOffO + 1] = x >> 2 & 0x01;
            iOutput[iOffO + 2] = x >> 1 & 0x01;
            iOutput[iOffO + 3] = x & 0x01;
        }

        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="iInput"></param>
        /// <param name="iSubKey"></param>
        /// <returns></returns>
        private int[] encFunc(int[] iInput, int[] iSubKey)
        {
            int[] iTemp1 = new int[48];
            int[] iTemp2 = new int[32];
            int[] iOutput = new int[32];
            permu(iInput, iTemp1, iEPM);
            arrayM2Add(iTemp1, iSubKey);
            for (int i = 0; i < 8; i++)
                Sbox(iTemp1, i * 6, iTemp2, i * 4, iSPM[i]);
            permu(iTemp2, iOutput, iPPM);
            return iOutput;
        }

        /// <summary>
        /// 子密钥生成
        /// </summary>
        /// <param name="bCipherKey"></param>
        /// <returns></returns>
        private int[][] makeSubKeys(byte[] bCipherKey)
        {
            int[][] iSubKeys = new int[16][];
            arrayBitToI(bCipherKey, iCipherKey);
            // int[] tmp = iCipherKey;
            permu(iCipherKey, iCKTemp, iSelePM1);
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < iROLtime[i]; j++)
                    ROL(iCKTemp);
                iSubKeys[i] = new int[48];
                permu(iCKTemp, iSubKeys[i], iSelePM2);
            }
            return iSubKeys;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="bPlaintext"></param>
        /// <param name="iSubKeys"></param>
        /// <returns></returns>
        private byte[] encrypt(byte[] bPlaintext, int[][] iSubKeys)
        {
            byte[] bCiphertext = new byte[8];
            arrayBitToI(bPlaintext, iPlaintext);
            permu(iPlaintext, iPKTemp, iInitPM);
            arrayCut(iPKTemp, iL, iR);
            for (int i = 0; i < 16; i++)
            {
                if (i % 2 == 0)
                {
                    arrayM2Add(iL, encFunc(iR, iSubKeys[i]));
                }
                else
                {
                    arrayM2Add(iR, encFunc(iL, iSubKeys[i]));
                }
            }
            arrayComb(iPKTemp, iR, iL);
            permu(iPKTemp, iCiphertext, iInvInitPM);
            arrayIToBit(bCiphertext, iCiphertext);
            return bCiphertext;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="bCiphertext"></param>
        /// <param name="iSubKeys"></param>
        /// <returns></returns>
        private byte[] decrypt(byte[] bCiphertext, int[][] iSubKeys)
        {
            int[][] iInvSubKeys = invSubKeys(iSubKeys);
            return encrypt(bCiphertext, iInvSubKeys);
        }

        /// <summary>
        /// Bit XOR
        /// </summary>
        /// <param name="Data1"></param>
        /// <param name="Data2"></param>
        /// <param name="Len"></param>
        /// <returns></returns>
        private byte[] BitXor(byte[] Data1, byte[] Data2, int Len)
        {
            int i;
            byte[] Dest = new byte[Len];

            for (i = 0; i < Len; i++)
                Dest[i] = (byte)(Data1[i] ^ Data2[i]);

            return Dest;
        }

        /// <summary>
        /// 3DesMac
        /// </summary>
        /// <param name="iSubKeys1"></param>
        /// <param name="iSubKeys2"></param>
        /// <param name="bInit"></param>
        /// <param name="bCiphertext"></param>
        /// <returns></returns>
        private byte[] MAC16(int[][] iSubKeys1, int[][] iSubKeys2, byte[] bInit,  byte[] bCiphertext)
        {
            byte[] pbySrcTemp = new byte[8];
            byte[] pbyInitData = new byte[8];
            byte[] pbyDeaSrc = new byte[8];
            byte[] pbyMac = new byte[4];
            int i, j, n, iAppend;
            int nCur = 0;
            int iSrcLen = bCiphertext.Length;
            n = iSrcLen / 8 + 1;
            iAppend = 8 - (n * 8 - iSrcLen);

            for (nCur = 0; nCur < 8; nCur++)
                pbyInitData[nCur] = bInit[nCur];

            for (i = 0; i < n; i++)
            {
                for (nCur = 0; nCur < 8; nCur++)
                    pbySrcTemp[0] = 0x00;
                if (i == (n - 1))
                {
                    for (nCur = 0; nCur < iAppend; nCur++)
                        pbySrcTemp[nCur] = bCiphertext[i * 8 + nCur];
                    pbySrcTemp[iAppend] = (byte)0x80;
                    for (j = iAppend + 1; j < 8; j++)
                        pbySrcTemp[j] = 0x00;
                }
                else
                {
                    for (nCur = 0; nCur < 8; nCur++)
                        pbySrcTemp[nCur] = bCiphertext[i * 8 + nCur];
                }

                pbyDeaSrc = BitXor(pbySrcTemp, pbyInitData, 8);

                pbyInitData = encrypt(pbyDeaSrc, iSubKeys1);
            }
            pbyDeaSrc = decrypt(pbyInitData, iSubKeys2);
            pbyInitData = encrypt(pbyDeaSrc, iSubKeys1);

            for (nCur = 0; nCur < 4; nCur++)
                pbyMac[nCur] = pbyInitData[nCur];
            return pbyMac;
        }
        /// <summary>
        /// 16进制数组压缩
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private byte[] hex2byte(byte[] b)
        {
            if ((b.Length % 2) != 0)
                throw new Exception("长度不是偶数");
            byte[] b2 = new byte[b.Length / 2];
            for (int n = 0; n < b.Length; n += 2)
            {
                byte[] bs = new byte[2];
                Array.Copy(b, n, bs, 0, 2);
                String item = System.Text.Encoding.Default.GetString(bs);
                b2[n / 2] = (byte)Convert.ToInt16(item, 16);
            }
            return b2;
        }

        /// <summary>
        /// 计算MAC
        /// </summary>
        /// <param name="strKey">密钥,长度必须为32个16进制字符,如:78B49F4BF5B16A17DF4AF5A36E49F4A0</param>
        /// <param name="strInitData">初始因子.长度必须为16,一般为:0000000000000000</param>
        /// <param name="strMacData">加入计算的MAC数据,长度为偶数</param>
        /// <returns></returns>
        public String Str3MAC(String strKey, String strInitData, String strMacData)
        {
            String strKey1;
            String strKey2;
            if ((strKey.Length) != 32)
            {
                throw new Exception("密钥长度不正确,必须为32");
            }
            if ((strInitData.Length) != 16)
            {
                throw new Exception("初始因子长度不正确,必须为16");
            }
            if ((strMacData.Length % 2) != 0)
            {
                throw new Exception("MAC Data长度不是偶数");
            }

            strKey1 = strKey.Substring(0, 16);
            strKey2 = strKey.Substring(16, 16);

            byte[] cipherKey1 = hex2byte(Encoding.Default.GetBytes(strKey1)); // 3DES的密钥K1
            byte[] cipherKey2 = hex2byte(Encoding.Default.GetBytes(strKey2)); // 3DES的密钥K2
            byte[] bInit = hex2byte(Encoding.Default.GetBytes(strInitData)); // 初始因子
            byte[] bCiphertext = hex2byte(Encoding.Default.GetBytes(strMacData)); // MAC数据

            int[][] subKeys1 = new int[16][]; // 用于存放K1产生的子密钥
            int[][] subKeys2 = new int[16][]; // 用于存放K2产生的子密钥
            subKeys1 = makeSubKeys(cipherKey1);
            subKeys2 = makeSubKeys(cipherKey2);

            byte[] byMac = MAC16(subKeys1, subKeys2, bInit, bCiphertext);

            String sRet = byte2hex(byMac);
            //System.out.println("strKey:" + strKey + " strInitData:" + strInitData
            //        + " strMacData:" + strMacData);
            //System.out.println("sRet:" + sRet);
            return sRet;
        }
        /// <summary>
        /// 计算MAC，初始向量默认:0000000000000000
        /// </summary>
        /// <param name="communicationKey"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        public String calculatorMac(String communicationKey, String strData, string iv = "0000000000000000")
        {
            return this.Str3MAC(communicationKey, iv, strData);
        }
        /// <summary>
        /// 3Des加密(双倍)
        /// </summary>
        /// <param name="strKey">密钥长度必须为32</param>
        /// <param name="strEncData">数据密文长度必须为32</param>
        /// <returns></returns>
        public String Str3DES(String strKey, String strEncData)
        {
            String strKey1;
            String strKey2;
            String strTemp1;
            String strTemp2;

            if ((strKey.Length) != 32)
            {
                throw new Exception("密钥长度不正确,必须为32");
            }
            if ((strEncData.Length) != 32)
            {
                throw new Exception("数据密文长度不正确,必须为32");
            }

            strKey1 = strKey.Substring(0, 16);
            strKey2 = strKey.Substring(16, 16);
            strTemp1 = strEncData.Substring(0, 16);
            strTemp2 = strEncData.Substring(16, 16);

            byte[] cipherKey1 = hex2byte(Encoding.Default.GetBytes(strKey1)); // 3DES的密钥K1
            byte[] cipherKey2 = hex2byte(Encoding.Default.GetBytes(strKey2)); // 3DES的密钥K2

            byte[] bCiphertext1 = hex2byte(Encoding.Default.GetBytes(strTemp1)); // 数据1
            byte[] bCiphertext2 = hex2byte(Encoding.Default.GetBytes(strTemp2)); // 数据1

            int[][] subKeys1 = new int[16][]; // 用于存放K1产生的子密钥
            int[][] subKeys2 = new int[16][]; // 用于存放K2产生的子密钥
            subKeys1 = makeSubKeys(cipherKey1);
            subKeys2 = makeSubKeys(cipherKey2);

            byte[] bTemp11 = encrypt(bCiphertext1, subKeys1);
            byte[] bTemp21 = decrypt(bTemp11, subKeys2);
            byte[] bPlaintext11 = encrypt(bTemp21, subKeys1);

            byte[] bTemp12 = encrypt(bCiphertext2, subKeys1);
            byte[] bTemp22 = decrypt(bTemp12, subKeys2);
            byte[] bPlaintext12 = encrypt(bTemp22, subKeys1);

            return byte2hex(bPlaintext11) + byte2hex(bPlaintext12);
        }
        /// <summary>
        /// 3Des解密(双倍)
        /// </summary>
        /// <param name="strKey">密钥长度必须为32</param>
        /// <param name="strEncData">数据密文长度必须为32</param>
        /// <returns></returns>
        public String StrDe3DES(String strKey, String strEncData)
        {
            String strKey1;
            String strKey2;
            String strTemp1;
            String strTemp2;

            if ((strKey.Length) != 32)
            {
                throw new Exception("密钥长度不正确,必须为32");
            }
            if ((strEncData.Length) != 32)
            {
                throw new Exception("数据密文长度不正确,必须为32");
            }

            strKey1 = strKey.Substring(0, 16);
            strKey2 = strKey.Substring(16, 16);
            strTemp1 = strEncData.Substring(0, 16);
            strTemp2 = strEncData.Substring(16, 16);

            byte[] cipherKey1 = hex2byte(Encoding.Default.GetBytes(strKey1)); // 3DES的密钥K1
            byte[] cipherKey2 = hex2byte(Encoding.Default.GetBytes(strKey2)); // 3DES的密钥K2

            byte[] bCiphertext1 = hex2byte(Encoding.Default.GetBytes(strTemp1)); // 数据1
            byte[] bCiphertext2 = hex2byte(Encoding.Default.GetBytes(strTemp2)); // 数据1

            int[][] subKeys1 = new int[16][]; // 用于存放K1产生的子密钥
            int[][] subKeys2 = new int[16][]; // 用于存放K2产生的子密钥
            subKeys1 = makeSubKeys(cipherKey1);
            subKeys2 = makeSubKeys(cipherKey2);

            byte[] bTemp11 = decrypt(bCiphertext1, subKeys1);
            byte[] bTemp21 = encrypt(bTemp11, subKeys2);
            byte[] bPlaintext11 = decrypt(bTemp21, subKeys1);

            byte[] bTemp12 = decrypt(bCiphertext2, subKeys1);
            byte[] bTemp22 = encrypt(bTemp12, subKeys2);
            byte[] bPlaintext12 = decrypt(bTemp22, subKeys1);

            return byte2hex(bPlaintext11) + byte2hex(bPlaintext12);
        }
        /// <summary>
        /// 16进制字符转换为字节数组,长度必须为偶数
        /// </summary>
        /// <param name="hexStr"></param>
        /// <returns></returns>
        private static byte[] hex2byte(string hexStr)
        {
            if ((hexStr.Length % 2) != 0)
                throw new Exception("长度不是偶数");

            int ilen = hexStr.Length / 2;
            byte[] b2 = new byte[ilen];
            for (int n = 0; n < hexStr.Length; n += 2)
            {
                String item = hexStr.Substring(n, 2);
                b2[n / 2] = (byte)Convert.ToInt16(item, 16);
            }
            return b2;
        }
        /// <summary>
        /// 一个字节的数转成16进制字符串
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static String byte2hex(byte[] b)
        {
            String hs = "";
            String stmp = "";
            for (int n = 0; n < b.Length; n++)
            {
                // 整数转成十六进制表示
                stmp = String.Format("{0:X2}", b[n] & 0xFF);// (java.lang.Integer.toHexString(b[n] & 0XFF));
                if (stmp.Length == 1)
                    hs = hs + "0" + stmp;
                else
                    hs = hs + stmp;
            }
            return hs.ToUpper(); // 转成大写
        }
        /// <summary>
        /// TripleDES加密,返回加密过的16进制字符串
        /// </summary>
        /// <param name="strString"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string DES3Encrypt(string strString, string strKey, string sIV = "")
        {
            try
            {
                using (SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider())
                {
                    //MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
                    mCSP.Key = Encoding.Default.GetBytes(strKey);//hashMD5.ComputeHash(hex2byte(strKey));//Convert.FromBase64String(strKey);
                    if (!string.IsNullOrEmpty(sIV))
                    {
                        mCSP.IV = Encoding.Default.GetBytes(sIV);
                    }
                    //指定加密的运算模式
                    mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
                    //获取或设置加密算法的填充模式
                    mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                    using(ICryptoTransform ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV))//创建加密对象
                    {
                        byte[] byt = Encoding.Default.GetBytes(strString);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                            cs.Write(byt, 0, byt.Length);
                            cs.FlushFinalBlock();
                            cs.Close();
                            //return Convert.ToBase64String(ms.ToArray());
                            return ByteUtils.toHexStr(ms.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ("Error in Encrypting " + ex.Message);
            }

        }
        /// <summary>
        /// 3DES解密字符串
        /// </summary>
        /// <param name="Value">加密后的16进制字符串</param>
        /// <param name="sKey">密钥，必须32位</param>
        /// <param name="sIV">向量，必须是12个字符</param>
        /// <returns>解密后字符串</returns>
        public static string DES3Decrypt(string Value, string sKey, string sIV = "")
        {
            try
            {
                //ct 加密转换运算
                //ms内存流
                //cs数据流连接到数据加密转换的流
                using (SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider())
                {
                    //MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
                    //将3DES的密钥转换成byte
                    mCSP.Key = Encoding.Default.GetBytes(sKey);//hashMD5.ComputeHash(hex2byte(sKey));
                    //将3DES的向量转换成byte
                    if (!string.IsNullOrEmpty(sIV))
                    {
                        mCSP.IV = Encoding.Default.GetBytes(sIV);
                    }
                    mCSP.Mode = System.Security.Cryptography.CipherMode.ECB;
                    mCSP.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

                    using(ICryptoTransform ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV))//创建对称解密对象
                    {
                        //byte[] byt = Convert.FromBase64String(Value);
                        byte[] byt = ByteUtils.hexStr2Bytes(Value);
                        using(MemoryStream ms = new MemoryStream())
                        {
                            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                            cs.Write(byt, 0, byt.Length);
                            cs.FlushFinalBlock();
                            cs.Close();
                            return Encoding.Default.GetString(ms.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ("Error in Decrypting " + ex.Message);
            }
        }

        public static string MD5Encrypt(string inputString)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(inputString, "MD5");
            /*
            using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] t = md5.ComputeHash(Encoding.Default.GetBytes(inputString));
                        StringBuilder sb = new StringBuilder(32);
                        for (int i = 0; i < t.Length; i++)
                            sb.Append(t[i].ToString("x").PadLeft(2, '0'));
                        return sb.ToString();
            }*/
        }
        /// <summary>
        /// DES加密,返回加密过16进制字符串
        /// </summary>
        /// <param name="code">源字符串</param>
        /// <param name="key">8字节密钥</param>
        /// <param name="iv">8字节IV</param>
        /// <returns></returns>
        public static string DesEncrypt(string code, string key, string iv = "")
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //des.KeySize = 8;
            byte[] inputByteArray = Encoding.Default.GetBytes(code);
            des.Key = Encoding.Default.GetBytes(key);
            byte[] btIV = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            if (!string.IsNullOrEmpty(iv))
            {
                btIV = Encoding.Default.GetBytes(iv);
            }

            des.IV = btIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ms.Dispose();
            cs.Dispose(); ;
            return ret.ToString();
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="code">加密过的16进制字符串</param>
        /// <param name="key">8字节密钥</param>
        /// <param name="iv">8字节IV</param>
        /// <returns></returns>
        public static string DesDecrypt(string code, string key, string iv = "")
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[code.Length / 2];
            for (int x = 0; x < code.Length / 2; x++)
            {
                int i = (Convert.ToInt32(code.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = Encoding.Default.GetBytes(key);
            byte[] btIV = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            if (!string.IsNullOrEmpty(iv))
            {
                btIV = Encoding.Default.GetBytes(iv);
            }

            des.IV = btIV;
            using (MemoryStream ms = new MemoryStream())
            {
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                cs.Dispose();
                StringBuilder ret = new StringBuilder();
                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
        }
        #region [RSA]
        /// <summary>  
        /// RSA加密（用私钥加密哟）  
        /// </summary>  
        /// <param name="key">私钥 XML字符串</param>  
        /// <param name="data">待加密的数据</param>  
        /// <returns></returns>  
        public static byte[] RSAEncrypt(String key, byte[] data)
        {
            //由密钥xml取得RSA对象  
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);
            //取得加密时使用的2个参数  
            RSAParameters par = rsa.ExportParameters(true);
            BigInteger mod = new BigInteger(par.Modulus);
            BigInteger ep = new BigInteger(par.D);
            //计算填充长度  
            int mLen = par.Modulus.Length;
            int fLen = mLen - data.Length - 3;
            //组建bytes  
            List<byte> lis = new List<byte>();
            lis.Add(0x00);
            lis.Add(0x01);//兼容java  
            for (int i = 0; i < fLen; i++) lis.Add(0xff);
            lis.Add(0x00);
            lis.AddRange(data);
            byte[] bytes = lis.ToArray();
            //加密就这么简单？  
            BigInteger m = new BigInteger(bytes);
            BigInteger c = m.modPow(ep, mod);
            return c.getBytes();
        }


        /// <summary>  
        /// RSA解密（用公钥解密哟）  
        /// </summary>  
        /// <param name="key">公钥</param>  
        /// <param name="data">待解密的数据</param>  
        /// <returns></returns>  
        public static byte[] RSADecrypt(String key, byte[] data)
        {
            //由密钥xml取得RSA对象  
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);
            //取得解密时使用的2个参数  
            RSAParameters par = rsa.ExportParameters(false);
            BigInteger mod = new BigInteger(par.Modulus);
            BigInteger ep = new BigInteger(par.Exponent);
            //解密？  
            BigInteger m = new BigInteger(data);
            BigInteger c = m.modPow(ep, mod);
            byte[] bytes = c.getBytes();
            //去掉填充域（头部可能填充了一段0xff）  
            byte flag = 0;
            for (int i = 1/*我从1开始啦*/; i < bytes.Length; i++)
            {
                if (bytes[i] == flag && i != (bytes.Length - 1))
                {
                    byte[] retBytes = new byte[bytes.Length - i - 1];
                    Array.Copy(bytes, i + 1, retBytes, 0, retBytes.Length);
                    return retBytes;
                }
            }
            return bytes;
        }


        /// <summary>  
        /// 取得证书私钥  
        /// </summary>  
        /// <param name="pfxPath">证书的绝对路径</param>  
        /// <param name="password">访问证书的密码</param>  
        /// <returns></returns>  
        public static String GetPrivateKey(string pfxPath, string password)
        {
            X509Certificate2 pfx = new X509Certificate2(pfxPath, password, X509KeyStorageFlags.Exportable);
            string privateKey = pfx.PrivateKey.ToXmlString(true);
            return privateKey;
        }


        /// <summary>  
        /// 取得证书的公钥  
        /// </summary>  
        /// <param name="cerPath">证书的绝对路径</param>  
        /// <returns></returns>  
        public static String GetPublicKey(string cerPath)
        {
            X509Certificate2 cer = new X509Certificate2(cerPath);
            string publicKey = cer.PublicKey.Key.ToXmlString(false);
            return publicKey;
        }  
        #endregion [End RAS]

        #region SHA1
        /// <summary>
        /// 获取SHA1计算的字符串,16进制格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HashString(string str)
        {
            byte[] buf = System.Text.Encoding.UTF8.GetBytes(str);

            SHA1 sha1 = SHA1.Create();
            byte[] saltedSHA1 = sha1.ComputeHash(buf);
            return ByteUtils.toHexStr(saltedSHA1);
        }

      
        #endregion
    }
}