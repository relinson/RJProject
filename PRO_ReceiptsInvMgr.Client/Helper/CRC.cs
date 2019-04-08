using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Client.Helper
{

    static class Crc
    {
        public static ulong CRC_any(byte[] blk_adr, ulong ulPoly, ulong ulInit, ulong ulXorOut, ulong ulMask)
        {
            ulong crc = ulInit;
            byte ucByte;
            int blk_len = blk_adr.Length;
            int i;
            bool iTopBitCRC;
            bool iTopBitByte;
            ulong ulTopBit;
            if (ulMask > 0xffff)
            {
                ulTopBit = 0x80000000;
            }
            else
            {
                ulTopBit = ((ulMask + 1) >> 1);
            }
               
            for (int j = 0; j < blk_len; j++)
            {
                ucByte = blk_adr[j];
                for (i = 0; i < 8; i++)
                {
                    iTopBitCRC = (crc & ulTopBit) != 0;
                    iTopBitByte = (ucByte & 0x80) != 0;
                    if (iTopBitCRC != iTopBitByte)
                    {
                        crc = (crc << 1) ^ ulPoly;
                    }
                    else
                    {
                        crc = (crc << 1);
                    }
                    ucByte <<= 1;
                }
            }
            return (crc ^ ulXorOut) & ulMask;
        }
    }
}
