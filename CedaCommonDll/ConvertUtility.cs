using System;
using System.IO;
using System.Text;
using System.Drawing;

namespace CedaCommonDll
{
    public sealed class ConvertUtility : IDisposable
    {
        ~ConvertUtility()
        {
            try { this.Dispose(true); }
            catch (Exception) { throw; }
        }

        public void Dispose()
        {
            try { this.Dispose(false); }
            catch (Exception) { throw; }
        }

        private void Dispose(bool pFinalize)
        {
            try { if (pFinalize != true) { GC.SuppressFinalize(this); } }
            catch (Exception) { throw; }
        }

        public byte[] getBytesFromString(string pString)
        {
            try { return Encoding.Default.GetBytes(pString); }
            catch (Exception) { throw; }
        }

        public byte[] getBytesFromHexString(string pHexString)
        {
            try
            {
                int lRest = pHexString.Length % 2;
                pHexString = pHexString.PadLeft(pHexString.Length + lRest, '0');
                using (MemoryStream lReturnMemoryStream = new MemoryStream())
                {
                    using (StringReader lHexStringReader = new StringReader(pHexString))
                    {
                        int lGetLength;
                        char[] lChars = new char[2];
                        while ((lGetLength = lHexStringReader.Read(lChars, 0, lChars.Length)) > 0) { lReturnMemoryStream.WriteByte(Convert.ToByte(new string(lChars, 0, lGetLength), 16)); }
                    }
                    return lReturnMemoryStream.ToArray();
                }
            }
            catch (Exception) { throw; }
        }

        public byte[] getBytesFromBitString(string pBitString)
        {
            try
            {
                int lRest = pBitString.Length % 8;
                pBitString = pBitString.PadLeft(pBitString.Length + lRest, '0');
                using (MemoryStream lReturnMemoryStream = new MemoryStream())
                {
                    using (StringReader lBitStringReader = new StringReader(pBitString))
                    {
                        char[] lChar = new char[8];
                        while (lBitStringReader.Read(lChar, 0, 8) > 0) { lReturnMemoryStream.WriteByte(Convert.ToByte(new string(lChar), 2)); }
                    }
                    return lReturnMemoryStream.ToArray();
                }
            }
            catch (Exception) { throw; }
        }

        public byte[] getBytesFromBase64String(string pBase64String)
        {
            try { return Convert.FromBase64String(pBase64String); }
            catch (Exception) { throw; }
        }

        public byte[] getBytesFromShort(short pShort)
        {
            try { return BitConverter.GetBytes(pShort); }
            catch (Exception) { throw; }
        }
        
        public byte[] getBytesFromInt(int pInt)
        {
            try { return BitConverter.GetBytes(pInt); }
            catch (Exception) { throw; }
        }

        public byte[] getBytesFromLong(long pLong)
        {
            try { return BitConverter.GetBytes(pLong); }
            catch (Exception) { throw; }
        }

        public string getStringFromBytes(byte[] pBytes)
        {
            try { return Encoding.Default.GetString(pBytes); }
            catch (Exception) { throw; }
        }

        public string getStringFromHexString(string pHexString)
        {
            try { return this.getStringFromBytes(this.getBytesFromHexString(pHexString)); }
            catch (Exception) { throw; }
        }

        public string getStringFromBitString(string pBitString)
        {
            try { return this.getStringFromBytes(this.getBytesFromBitString(pBitString)); }
            catch (Exception) { throw; }
        }

        public string getStringFromBase64String(string pBase64String)
        {
            try { return this.getStringFromBytes(this.getBytesFromBase64String(pBase64String)); }
            catch (Exception) { throw; }
        }

        public string getHexStringFromBytes(byte[] pBytes)
        {
            try { return BitConverter.ToString(pBytes).Replace("-", string.Empty).ToLower(); }
            catch (Exception) { throw; }
        }

        public string getHexStringFromString(string pString)
        {
            try { return this.getHexStringFromBytes(this.getBytesFromString(pString)); }
            catch (Exception) { throw; }
        }

        public string getHexStringFromBitString(string pBitString)
        {
            try { return this.getHexStringFromBytes(this.getBytesFromBitString(pBitString)); }
            catch (Exception) { throw; }
        }

        public string getHexStringFromShort(short pShort)
        {
            try { return Convert.ToString(pShort, 16); }
            catch (Exception) { throw; }
        }

        public string getHexStringFromInt(int pInt)
        {
            try { return Convert.ToString(pInt, 16); }
            catch (Exception) { throw; }
        }


        public string getHexStringFromLong(long pLong)
        {
            try { return Convert.ToString(pLong, 16); }
            catch (Exception) { throw; }
        }

        public string getBitStringFromBytes(byte[] pBytes)
        {
            try
            {
                StringBuilder lReturnStringBuilder = new StringBuilder();
                foreach (byte lByte in pBytes) { lReturnStringBuilder.Append(Convert.ToString(lByte, 2).PadLeft(8, '0')); }
                return lReturnStringBuilder.ToString();
            }
            catch (Exception) { throw; }
        }

        public string getBitStringFromString(string pString)
        {
            try { return this.getBitStringFromBytes(this.getBytesFromString(pString)); }
            catch (Exception) { throw; }
        }

        public string getBitStringFromHexString(string pHexString)
        {
            try { return this.getBitStringFromBytes(this.getBytesFromHexString(pHexString)); }
            catch (Exception) { throw; }
        }

        public string getBitStringFromShort(short pShort)
        {
            try { return Convert.ToString(pShort, 2); }
            catch (Exception) { throw; }
        }
        
        public string getBitStringFromInt(int pInt)
        {
            try { return Convert.ToString(pInt, 2); }
            catch (Exception) { throw; }
        }
        
        public string getBitStringFromLong(long pLong)
        {
            try { return Convert.ToString(pLong, 2); }
            catch (Exception) { throw; }
        }
        
        public string getBase64StringFromBytes(byte[] pBytes)
        {
            try { return Convert.ToBase64String(pBytes); }
            catch (Exception) { throw; }
        }

        public string getBase64StringFromString(string pString)
        {
            try { return this.getBase64StringFromBytes(this.getBytesFromString(pString)); }
            catch (Exception) { throw; }
        }

        public short getShortFromBytes(byte[] pBytes)
        {
            try { return BitConverter.ToInt16(pBytes, 0); }
            catch (Exception) { throw; }
        }

        public short getShortFromHexString(string pHexString)
        {
            try { return Convert.ToInt16(pHexString, 16); }
            catch (Exception) { throw; }
        }

        public short getShortFromBitString(string pBitString)
        {
            try { return Convert.ToInt16(pBitString, 2); }
            catch (Exception) { throw; }
        }

        public int getIntFromBytes(byte[] pBytes)
        {
            try { return BitConverter.ToInt32(pBytes, 0); }
            catch (Exception) { throw; }
        }

        public int getIntFromHexString(string pHexString)
        {
            try { return Convert.ToInt32(pHexString, 16); }
            catch (Exception) { throw; }
        }

        public int getIntFromBitString(string pBitString)
        {
            try { return Convert.ToInt32(pBitString, 2); }
            catch (Exception) { throw; }
        }

        public long getLongFromBytes(byte[] pBytes)
        {
            try { return BitConverter.ToInt64(pBytes, 0); }
            catch (Exception) { throw; }
        }
        
        public long getLongFromHexString(string pHexString)
        {
            try { return Convert.ToInt64(pHexString, 16); }
            catch (Exception) { throw; }
        }
        
        public long getLongFromBitString(string pBitString)
        {
            try { return Convert.ToInt64(pBitString, 2); }
            catch (Exception) { throw; }
        }

        public float getPdfLengthFromInch(float pInch)
        {
            try { return pInch * 72F; }
            catch (Exception) { throw; }
        }

        public float getPdfLengthFromMillimeter(float pMillimeter)
        {
            try { return this.getPdfLengthFromInch(this.getInchFromMillimeter(pMillimeter)); }
            catch (Exception) { throw; }
        }

        public float getInchFromMillimeter(float pMillimeter)
        {
            try { return pMillimeter / 10F / 2.54F; }
            catch (Exception) { throw; }
        }

        public float getInchFromPdfLength(float pPdfLength)
        {
            try { return pPdfLength / 72F; }
            catch (Exception) { throw; }
        }

        public float getMillimeterFromInch(float pInch)
        {
            try { return pInch * 2.54F * 10F; }
            catch (Exception) { throw; }
        }

        public float getMillimeterFromPdfLength(float pPdfLength)
        {
            try { return this.getMillimeterFromInch(this.getInchFromPdfLength(pPdfLength)); }
            catch (Exception) { throw; }
        }

        public float getConvertGraphicUnits(Graphics pGraphics, float pValue, GraphicsUnit pFromUnit, GraphicsUnit pToUnit)
        {
            try
            {
                if (pFromUnit == pToUnit) { return pValue; }
                float lFromValue = pValue;
                switch (pFromUnit)
                {
                    case GraphicsUnit.Document:
                        lFromValue = pValue * pGraphics.DpiX / 300;
                        break;
                    case GraphicsUnit.Inch:
                        lFromValue = pValue * pGraphics.DpiX;
                        break;
                    case GraphicsUnit.Millimeter:
                        lFromValue = pValue * pGraphics.DpiX / 25.4F;
                        break;
                    case GraphicsUnit.Pixel:
                        lFromValue = pValue;
                        break;
                    case GraphicsUnit.Point:
                        lFromValue = pValue * pGraphics.DpiX / 72;
                        break;
                    default:
                        lFromValue = pValue;
                        break;
                }
                float lReturnValue = lFromValue;
                switch (pToUnit)
                {
                    case GraphicsUnit.Document:
                        lReturnValue = lFromValue / pGraphics.DpiX / 300;
                        break;
                    case GraphicsUnit.Inch:
                        lReturnValue = lFromValue / pGraphics.DpiX;
                        break;
                    case GraphicsUnit.Millimeter:
                        lReturnValue = lFromValue / pGraphics.DpiX / 25.4F;
                        break;
                    case GraphicsUnit.Pixel:
                        lReturnValue = lFromValue;
                        break;
                    case GraphicsUnit.Point:
                        lReturnValue = lFromValue / pGraphics.DpiX / 72;
                        break;
                    default:
                        break;
                }
                return lReturnValue;
            }
            catch (Exception) { throw; }
        }
    }
}
