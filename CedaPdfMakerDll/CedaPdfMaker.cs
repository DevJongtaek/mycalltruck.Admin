using System;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Drawing;
using CedaCommonDll;

namespace CedaPdfMakerDll
{
    public class CedaPdfMaker : IDisposable
    {
        ~CedaPdfMaker()
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

        public bool MakePdf(string pTradeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pTradeId) == true) { throw new Exception("TradeId Is Null."); }
                List<string> l10ReadyList = CedaConsts.GetTargetList(CedaConsts.STATUS_CODE._10_ready, CedaConsts.STATUS_CODE._10_prog, pTradeId, 1, 1);
                if (l10ReadyList == null || l10ReadyList.Count < 1) { throw new Exception(string.Concat("DB Data of TradeId ", pTradeId, " Is Null")); }
                if (this.CreatePdf(pTradeId) != true) { return false; }
                CedaConsts.SaveStatusData(pTradeId, CedaConsts.STATUS_CODE._20_prog.ToString(), null);
                return this.CreateSignedPdf(pTradeId);
            }
            catch (Exception ex) { CedaConsts.SaveLog(ex.ToString()); return false; }
        }

        public bool CreatePdf(string pTradeId)
        {
            try
            {
                string lErrorMessage = this.GetImageListData(pTradeId, out string lFileDirectory, out List<string> lImageFileNameList);
                if (string.IsNullOrWhiteSpace(lErrorMessage) != true) { CedaConsts.SaveStatusData(pTradeId, CedaConsts.STATUS_CODE._10_error.ToString(), lErrorMessage); return false; }
                lErrorMessage = this.CheckImageListData(lFileDirectory, lImageFileNameList);
                if (string.IsNullOrWhiteSpace(lErrorMessage) != true) { CedaConsts.SaveStatusData(pTradeId, CedaConsts.STATUS_CODE._10_error.ToString(), lErrorMessage); return false; }
                string lPdfFileName = string.Concat(pTradeId, ".pdf");
                lErrorMessage = this.CreatePdfFile(lFileDirectory, lImageFileNameList, lPdfFileName);
                if (string.IsNullOrWhiteSpace(lErrorMessage) != true) { CedaConsts.SaveStatusData(pTradeId, CedaConsts.STATUS_CODE._10_error.ToString(), lErrorMessage); return false; }
                this.SavePdfData(pTradeId, lPdfFileName);
                return true;
            }
            catch (Exception) { throw; }
        }

        private string GetImageListData(string pTradeId, out string opFileDirectory, out List<string> opImageFileNameList)
        {
            opFileDirectory = string.Empty;
            opImageFileNameList = new List<string>();
            try
            {
                string lErrorMessage = string.Empty;
                using (MsSqlUtility lDbUtil = CedaConsts.GetDBclass())
                {
                    lDbUtil.clearParameterList();
                    StringBuilder lSqlString = new StringBuilder();
                    lSqlString.AppendLine(" SELECT FileDirectory, ");
                    lSqlString.AppendLine("        Image1Name, Image2Name, Image3Name, Image4Name, Image5Name ");
                    lSqlString.AppendLine(" FROM DocuTable ");
                    lSqlString.AppendLine(" WHERE TradeId = @pTradeId ");
                    lDbUtil.addSqlParameter("@pTradeId", int.Parse(pTradeId));
                    lSqlString.AppendLine(" AND   Status = @_10_prog ");
                    lDbUtil.addSqlParameter("@_10_prog", CedaConsts.STATUS_CODE._10_prog.ToString());
                    using (SqlDataReader AdoRs = lDbUtil.selectDB(lSqlString.ToString()))
                    {
                        if (AdoRs != null)
                        {
                            while (AdoRs.Read() == true)
                            {
                                opFileDirectory = lDbUtil.getDbValue(AdoRs["FileDirectory"]);
                                for (int i = 1; i <= 5; i++)
                                {
                                    string lTempName = string.Concat("Image", i.ToString(), "Name");
                                    if (string.IsNullOrWhiteSpace(lDbUtil.getDbValue(AdoRs[lTempName])) != true) { opImageFileNameList.Add(lDbUtil.getDbValue(AdoRs[lTempName])); }
                                }
                            }
                            AdoRs.Close();
                        }
                    }
                }
                if (string.IsNullOrWhiteSpace(opFileDirectory) == true || opImageFileNameList.Count < 1) { lErrorMessage = string.Concat("Confirm DB Data of TradeId ", pTradeId); }
                return lErrorMessage;
            }
            catch (Exception) { throw; }
        }

        private string CheckImageListData(string pFileDir, List<string> pImageFileNameList)
        {
            try
            {
                string lErrorMessage = string.Empty;
                foreach (string lImageFileName in pImageFileNameList)
                {
                    if (File.Exists(Path.Combine(pFileDir, lImageFileName)) != true)
                    {
                        if (string.IsNullOrWhiteSpace(lErrorMessage) != true) { string.Concat(lErrorMessage, Environment.NewLine); }
                        lErrorMessage = string.Concat(lErrorMessage, string.Concat(Path.Combine(pFileDir, lImageFileName), " 를 찾을 수 없습니다."));
                    }
                }
                return lErrorMessage;
            }
            catch (Exception) { throw; }
        }

        private string CreatePdfFile(string pFileDir, List<string> pSouceImageFileNameList, string pTargetPdfFileName)
        {
            try
            {
                iTextSharp.text.Image lImage = iTextSharp.text.Image.GetInstance(Path.Combine(pFileDir, pSouceImageFileNameList[0]));
                using (iTextSharp.text.Document lPdfDocument = new iTextSharp.text.Document(lImage))
                {
                    using (iTextSharp.text.pdf.PdfWriter lPdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(lPdfDocument, new FileStream(Path.Combine(pFileDir, pTargetPdfFileName), FileMode.Create)))
                    {
                        lPdfWriter.SetPdfVersion(iTextSharp.text.pdf.PdfWriter.PDF_VERSION_1_7);
                        lPdfWriter.PdfVersion = iTextSharp.text.pdf.PdfWriter.VERSION_1_7;
                        lPdfDocument.AddCreationDate();
                        lPdfDocument.Open();
                        foreach (string lImageFileName in pSouceImageFileNameList)
                        {
                            lImage = iTextSharp.text.Image.GetInstance(Path.Combine(pFileDir, lImageFileName));
                            lPdfDocument.SetPageSize(lImage);
                            lPdfDocument.NewPage();
                            lImage.SetAbsolutePosition(0, 0);
                            lPdfDocument.Add(lImage);
                        }
                        lPdfDocument.Close();
                    }
                }
                return null;
            }
            catch (Exception) { throw; }
        }

        private void SavePdfData(string pTradeId, string pPdfFileName)
        {
            try
            {
                using (MsSqlUtility lDbUtil = CedaConsts.GetDBclass())
                {
                    lDbUtil.clearParameterList();
                    StringBuilder lSqlString = new StringBuilder();
                    lSqlString.AppendLine(" UPDATE DocuTable ");
                    lSqlString.AppendLine(" SET PdfFileName = @pPdfFileName, ");
                    lDbUtil.addSqlParameter("@pPdfFileName", pPdfFileName);
                    lSqlString.AppendLine("     Status = @_20_ready, ");
                    lDbUtil.addSqlParameter("@_20_ready", CedaConsts.STATUS_CODE._20_ready.ToString());
                    lSqlString.AppendLine("     UpdateDateTime = GETDATE() ");
                    lSqlString.AppendLine(" WHERE TradeId = @pTradeId ");
                    lDbUtil.addSqlParameter("@pTradeId", int.Parse(pTradeId));
                    lSqlString.AppendLine(" AND   Status = @_10_prog ");
                    lDbUtil.addSqlParameter("@_10_prog", CedaConsts.STATUS_CODE._10_prog.ToString());
                    lDbUtil.executeDB(lSqlString.ToString());
                }
            }
            catch (Exception) { throw; }
        }

        public bool CreateSignedPdf(string pTradeId)
        {
            try
            {
                string lErrorMessage = this.GetPdfData(pTradeId, out string lFileDirectory, out string lPdfFileName, out string lSignLocation);
                if (string.IsNullOrWhiteSpace(lErrorMessage) != true) { CedaConsts.SaveStatusData(pTradeId, CedaConsts.STATUS_CODE._20_error.ToString(), lErrorMessage); return false; }
                lErrorMessage = this.CheckPdfData(lFileDirectory, lPdfFileName);
                if (string.IsNullOrWhiteSpace(lErrorMessage) != true) { CedaConsts.SaveStatusData(pTradeId, CedaConsts.STATUS_CODE._20_error.ToString(), lErrorMessage); return false; }
                string lSignedPdfFileName = string.Concat("signed_", lPdfFileName);
                lErrorMessage = this.CreateSignedPdfFile(lFileDirectory, lPdfFileName, lSignedPdfFileName, lSignLocation);
                if (string.IsNullOrWhiteSpace(lErrorMessage) != true) { CedaConsts.SaveStatusData(pTradeId, CedaConsts.STATUS_CODE._20_error.ToString(), lErrorMessage); return false; }
                this.SaveSignedPdfData(pTradeId, lSignedPdfFileName);
                return true;
            }
            catch (Exception) { throw; }
        }

        private string GetPdfData(string pTradeId, out string opFileDirectory, out string opPdfFileName, out string opSignLocation)
        {
            opFileDirectory = string.Empty;
            opPdfFileName = string.Empty;
            opSignLocation = string.Empty;
            try
            {
                string lErrorMessage = string.Empty;
                using (MsSqlUtility lDbUtil = CedaConsts.GetDBclass())
                {
                    lDbUtil.clearParameterList();
                    StringBuilder lSqlString = new StringBuilder();
                    lSqlString.AppendLine(" SELECT FileDirectory, PdfFileName, SignLocation ");
                    lSqlString.AppendLine(" FROM DocuTable ");
                    lSqlString.AppendLine(" WHERE TradeId = @pTradeId ");
                    lDbUtil.addSqlParameter("@pTradeId", int.Parse(pTradeId));
                    lSqlString.AppendLine(" AND   Status = @_20_prog ");
                    lDbUtil.addSqlParameter("@_20_prog", CedaConsts.STATUS_CODE._20_prog.ToString());
                    using (SqlDataReader AdoRs = lDbUtil.selectDB(lSqlString.ToString()))
                    {
                        if (AdoRs != null)
                        {
                            while (AdoRs.Read() == true)
                            {
                                opFileDirectory = lDbUtil.getDbValue(AdoRs["FileDirectory"]);
                                opPdfFileName = lDbUtil.getDbValue(AdoRs["PdfFileName"]);
                                opSignLocation = lDbUtil.getDbValue(AdoRs["SignLocation"]);
                            }
                            AdoRs.Close();
                        }
                    }
                }
                if (string.IsNullOrWhiteSpace(opFileDirectory) == true || string.IsNullOrWhiteSpace(opPdfFileName) == true) { lErrorMessage = string.Concat("Confirm DB Data of TradeId ", pTradeId); }
                return lErrorMessage;
            }
            catch (Exception) { throw; }
        }

        private string CheckPdfData(string pFileDir, string pPdfFileName)
        {
            try
            {
                if (File.Exists(Path.Combine(pFileDir, pPdfFileName)) != true) { return string.Concat(Path.Combine(pFileDir, pPdfFileName), " 를 찾을 수 없습니다."); }
                return null;
            }
            catch (Exception) { throw; }
        }

        private string CreateSignedPdfFile(string pFileDir, string pSourcePdfFileName, string pTargetPdfFileName, string pSignLocation)
        {
            try
            {
                using (iTextSharp.text.pdf.PdfReader lPdfReader = new iTextSharp.text.pdf.PdfReader(Path.Combine(pFileDir, pSourcePdfFileName)))
                {
                    using (iTextSharp.text.pdf.PdfStamper lPdfStamper = iTextSharp.text.pdf.PdfStamper.CreateSignature(lPdfReader, new FileStream(Path.Combine(pFileDir, pTargetPdfFileName), FileMode.Create), '\0'))
                    {
                        iTextSharp.text.pdf.PdfSignatureAppearance lPdfSignatureAppearance = lPdfStamper.SignatureAppearance;
                        lPdfSignatureAppearance.Reason = "타임스탬프";
                        lPdfSignatureAppearance.Location = "docubank.lgcns.com";
                        lPdfSignatureAppearance.SignatureRenderingMode = iTextSharp.text.pdf.PdfSignatureAppearance.RenderingMode.GRAPHIC;
                        lPdfSignatureAppearance.SignatureGraphic = iTextSharp.text.Image.GetInstance(this.GetSignatureImage(), System.Drawing.Imaging.ImageFormat.Png);
                        if (string.IsNullOrWhiteSpace(pSignLocation) == true) { pSignLocation = CedaConsts.SIGN_LOCATION_CODE.LU.ToString(); }
                        float lWidth = 200F;
                        float lHeight = 200F;
                        float lLeft = 0F;
                        float lBottom = 0F;
                        iTextSharp.text.Rectangle lPageSize = lPdfReader.GetPageSize(1);
                        if (pSignLocation.Substring(0, 1).ToUpper().Equals("R") == true) { lLeft = lPageSize.Width - (lPageSize.Width / 20F) - lWidth; }
                        else { lLeft = lPageSize.Width / 20F; }
                        if (pSignLocation.Substring(1, 1).ToUpper().Equals("L") == true) { lBottom = lPageSize.Height / 20F; }
                        else { lBottom = lPageSize.Height - (lPageSize.Height / 20F) - lHeight; }
                        iTextSharp.text.Rectangle lSignLocationRect = new iTextSharp.text.Rectangle(lLeft, lBottom, lLeft + lWidth, lBottom + lHeight);
                        lPdfSignatureAppearance.SetVisibleSignature(lSignLocationRect, 1, "(주) LG CNS TSA");
                        lPdfSignatureAppearance.AddDeveloperExtension(iTextSharp.text.pdf.PdfDeveloperExtension.ESIC_1_7_EXTENSIONLEVEL5);
                        iTextSharp.text.pdf.PdfSignature dic = new iTextSharp.text.pdf.PdfSignature(iTextSharp.text.pdf.PdfName.ADOBE_PPKLITE, iTextSharp.text.pdf.PdfName.ETSI_RFC3161);
                        dic.Put(iTextSharp.text.pdf.PdfName.TYPE, iTextSharp.text.pdf.PdfName.DOCTIMESTAMP);
                        lPdfSignatureAppearance.CryptoDictionary = dic;
                        iTextSharp.text.pdf.security.ITSAClient lTSAClient = new TSAClientCNS();
                        this.Timestamp(lPdfSignatureAppearance, lTSAClient);
                        lPdfStamper.Close();
                    }
                    lPdfReader.Close();
                }
                return null;
            }
            catch (Exception) { throw; }
        }

        private Image GetSignatureImage()
        {
            try
            {
                Image lImage = Image.FromFile(@"D:\EDocument\SignatureImage.png");
                using (Graphics lGraphics = Graphics.FromImage(lImage))
                {
                    using (Font lFont = new Font("맑은 고딕", 36, FontStyle.Bold))
                    {
                        using (SolidBrush lSolidBrush = new SolidBrush(Color.FromArgb(58, 58, 60)))
                        {
                            using (StringFormat lStringFormat = StringFormat.GenericDefault)
                            {
                                lStringFormat.Alignment = StringAlignment.Center;
                                lGraphics.DrawString(string.Concat(DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/"), Environment.NewLine, DateTime.Now.ToString("HH:mm:ss"), Environment.NewLine, "GMT+09:00"), lFont, lSolidBrush, 2012F / 2F, 870F, lStringFormat);
                            }
                        }
                    }
                }
                return lImage;
            }
            catch (Exception) { throw; }
        }

        private void Timestamp(iTextSharp.text.pdf.PdfSignatureAppearance pPdfSignatureAppearance, iTextSharp.text.pdf.security.ITSAClient pTSAClient)
        {
            try
            {
                int lContentEstimated = pTSAClient.GetTokenSizeEstimate();
                Dictionary<iTextSharp.text.pdf.PdfName, int> lExclusionSizes = new Dictionary<iTextSharp.text.pdf.PdfName, int>();
                lExclusionSizes[iTextSharp.text.pdf.PdfName.CONTENTS] = lContentEstimated * 2 + 2;
                pPdfSignatureAppearance.PreClose(lExclusionSizes);
                Stream lStream = pPdfSignatureAppearance.GetRangeStream();
                Org.BouncyCastle.Crypto.IDigest lMessageDigest = pTSAClient.GetMessageDigest();
                byte[] lBuffer = new byte[4096];
                int n;
                while ((n = lStream.Read(lBuffer, 0, lBuffer.Length)) > 0) { lMessageDigest.BlockUpdate(lBuffer, 0, n); }
                byte[] lTsImprint = new byte[lMessageDigest.GetDigestSize()];
                lMessageDigest.DoFinal(lTsImprint, 0);
                byte[] lTsToken = pTSAClient.GetTimeStampToken(lTsImprint);
                if (lContentEstimated + 2 < lTsToken.Length) { throw new Exception("Not enough space"); }
                byte[] lPaddedSig = new byte[lContentEstimated];
                Array.Copy(lTsToken, 0, lPaddedSig, 0, lTsToken.Length);
                iTextSharp.text.pdf.PdfDictionary lPdfDictionary = new iTextSharp.text.pdf.PdfDictionary();
                lPdfDictionary.Put(iTextSharp.text.pdf.PdfName.CONTENTS, new iTextSharp.text.pdf.PdfString(lPaddedSig).SetHexWriting(true));
                pPdfSignatureAppearance.Close(lPdfDictionary);
            }
            catch (Exception) { throw; }
        }

        private void SaveSignedPdfData(string pTradeId, string pSignedPdfFileName)
        {
            try
            {
                using (MsSqlUtility lDbUtil = CedaConsts.GetDBclass())
                {
                    lDbUtil.beginTransaction();
                    try
                    {
                        StringBuilder lSqlString = new StringBuilder();
                        lSqlString.AppendLine(" UPDATE Trades ");
                        lSqlString.AppendLine(" SET PdfFileName = @pSignedPdfFileName, ");
                        lDbUtil.addSqlParameter("@pSignedPdfFileName", pSignedPdfFileName);
                        lSqlString.AppendLine("     UpdateDateTime = GETDATE() ");
                        lSqlString.AppendLine(" WHERE TradeId = @pTradeId ");
                        lDbUtil.addSqlParameter("@pTradeId", int.Parse(pTradeId));
                        lDbUtil.executeDB(lSqlString.ToString());
                        lDbUtil.clearParameterList();
                        lSqlString.Clear();
                        lSqlString.AppendLine(" UPDATE DocuTable ");
                        lSqlString.AppendLine(" SET PdfFileName = @pSignedPdfFileName, ");
                        lDbUtil.addSqlParameter("@pSignedPdfFileName", pSignedPdfFileName);
                        lSqlString.AppendLine("     Status = @_30_ready, ");
                        lDbUtil.addSqlParameter("@_30_ready", CedaConsts.STATUS_CODE._30_ready.ToString());
                        lSqlString.AppendLine("     UpdateDateTime = GETDATE() ");
                        lSqlString.AppendLine(" WHERE TradeId = @pTradeId ");
                        lDbUtil.addSqlParameter("@pTradeId", int.Parse(pTradeId));
                        lSqlString.AppendLine(" AND   Status = @_20_prog ");
                        lDbUtil.addSqlParameter("@_20_prog", CedaConsts.STATUS_CODE._20_prog.ToString());
                        lDbUtil.executeDB(lSqlString.ToString());
                        lDbUtil.commitTransaction();
                    }
                    catch (Exception) { lDbUtil.rollbackTransaction(); throw; }
                }
            }
            catch (Exception) { throw; }
        }
    }   
}