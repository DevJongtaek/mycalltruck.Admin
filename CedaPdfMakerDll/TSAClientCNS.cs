using System;
using System.IO;
using System.Net;
using System.util;
using System.Text;
using CedaCommonDll;

namespace CedaPdfMakerDll
{
    internal class TSAClientCNS : iTextSharp.text.pdf.security.ITSAClient
    {
        private string _DigestAlgorithm { get; } = "SHA-256";
        private int _TokenSizeEstimate { get; set; } = 4096;

        public Org.BouncyCastle.Crypto.IDigest GetMessageDigest()
        {
            return iTextSharp.text.pdf.security.DigestAlgorithms.GetMessageDigest(this._DigestAlgorithm);
        }
        
        public int GetTokenSizeEstimate()
        {
            return this._TokenSizeEstimate;
        }

        public virtual byte[] GetTimeStampToken(byte[] imprint)
        {
            byte[] lResponseBytes = null;
            Org.BouncyCastle.Tsp.TimeStampRequestGenerator lTimeStampRequestGenerator = new Org.BouncyCastle.Tsp.TimeStampRequestGenerator();
            lTimeStampRequestGenerator.SetCertReq(true);
            lTimeStampRequestGenerator.SetReqPolicy("1.2.840.113549.1.1.11");
            Org.BouncyCastle.Math.BigInteger lNonce = Org.BouncyCastle.Math.BigInteger.ValueOf(DateTime.Now.Ticks + Environment.TickCount);
            Org.BouncyCastle.Tsp.TimeStampRequest lTimeStampRequest = lTimeStampRequestGenerator.Generate(iTextSharp.text.pdf.security.DigestAlgorithms.GetAllowedDigests(this._DigestAlgorithm), imprint, lNonce);
            byte[] lRequestBytes = lTimeStampRequest.GetEncoded();
            lResponseBytes = GetTSAResponse(lRequestBytes);
            Org.BouncyCastle.Tsp.TimeStampResponse lTimeStampResponse = new Org.BouncyCastle.Tsp.TimeStampResponse(lResponseBytes);
            lTimeStampResponse.Validate(lTimeStampRequest);
            Org.BouncyCastle.Asn1.Cmp.PkiFailureInfo lPkiFailureInfo = lTimeStampResponse.GetFailInfo();
            int lResultValue = (lPkiFailureInfo == null) ? 0 : lPkiFailureInfo.IntValue;
            if (lResultValue != 0) { throw new IOException(iTextSharp.text.error_messages.MessageLocalization.GetComposedMessage("invalid.tsa.1.lTimeStampResponse.code.2", CedaConsts._TSA_URL, lResultValue)); }
            Org.BouncyCastle.Tsp.TimeStampToken lTimeStampToken = lTimeStampResponse.TimeStampToken;
            if (lTimeStampToken == null) { throw new IOException(iTextSharp.text.error_messages.MessageLocalization.GetComposedMessage("tsa.1.failed.to.return.time.stamp.token.2", CedaConsts._TSA_URL, lTimeStampResponse.GetStatusString())); }
            Org.BouncyCastle.Tsp.TimeStampTokenInfo lTimeStampTokenInfo = lTimeStampToken.TimeStampInfo;
            byte[] lTimeStampTokenBytes = lTimeStampToken.GetEncoded();
            this._TokenSizeEstimate = lTimeStampTokenBytes.Length + 32;
            return lTimeStampTokenBytes;
        }

        private byte[] GetTSAResponse(byte[] pRequestBytes)
        {
            HttpWebRequest lHttpWebRequest = (HttpWebRequest)WebRequest.Create(CedaConsts._TSA_URL);
            lHttpWebRequest.ContentLength = pRequestBytes.Length;
            lHttpWebRequest.ContentType = "application/timestamp-query";
            lHttpWebRequest.Method = "POST";
            lHttpWebRequest.Headers["companyCd"] = "lgupluse01";
            Stream lRequestStream = lHttpWebRequest.GetRequestStream();
            lRequestStream.Write(pRequestBytes, 0, pRequestBytes.Length);
            lRequestStream.Close();
            HttpWebResponse lHttpWebResponse = (HttpWebResponse)lHttpWebRequest.GetResponse();
            if (lHttpWebResponse.StatusCode != HttpStatusCode.OK) { throw new IOException(iTextSharp.text.error_messages.MessageLocalization.GetComposedMessage("invalid.http.lHttpWebResponse.1", (int)lHttpWebResponse.StatusCode)); }
            Stream lResponseStream = lHttpWebResponse.GetResponseStream();
            MemoryStream lTSAResponseStream = new MemoryStream();
            byte[] lBuffer = new byte[1024];
            int lReadLength;
            while ((lReadLength = lResponseStream.Read(lBuffer, 0, lBuffer.Length)) > 0) { lTSAResponseStream.Write(lBuffer, 0, lReadLength); }
            lResponseStream.Close();
            lHttpWebResponse.Close();
            byte[] lResponseBytes = lTSAResponseStream.ToArray();
            string lContentEncoding = lHttpWebResponse.ContentEncoding;
            if (lContentEncoding != null && Util.EqualsIgnoreCase(lContentEncoding, "base64")) { lResponseBytes = Convert.FromBase64String(Encoding.ASCII.GetString(lResponseBytes)); }
            return lResponseBytes;
        }
    }
}
