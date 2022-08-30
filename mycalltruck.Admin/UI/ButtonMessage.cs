using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycalltruck.Admin.UI
{
    class ButtonMessage
    { /// <summary>
        /// 추가,수정,삭제 버튼 다이알로그 메시지
        /// </summary>
        /// <param name="mType"></param>
        /// <param name="iDiv"></param>
        /// <param name="iCount">
        /// 처음 ~건의 라는 말이 안나오게 하려면 -1값입력함.
        /// </param>
        /// <returns></returns>
        public static string GetMessage(MessageType mType, string iDiv, int iCount)
        {
            string rMessage = string.Empty;
            string sCount = string.Empty;
            if (iCount < 0)
            {
                sCount = "";
            }
            else if (iCount == 0)
            {
                sCount = "";
            }
            else
            {
                sCount = string.Format("{0}건의 ", iCount);
            }
            switch (mType)
            {
                case MessageType.추가성공:
                    rMessage = string.Format("{0}{1}정보가 추가 되었습니다.", sCount, iDiv);
                    break;
                case MessageType.수정성공:
                    rMessage = string.Format("{0}{1}정보가 수정 되었습니다.", "", iDiv);
                    break;
                case MessageType.삭제성공:
                    rMessage = string.Format("{0}{1}정보가 삭제 되었습니다.", sCount, iDiv);
                    break;
                case MessageType.삭제성공2:
                    rMessage = string.Format("{0}정보가 삭제 되었습니다.", iDiv);
                    break;
                case MessageType.추가질문:
                    rMessage = string.Format("{0}{1}정보를 추가하시겠습니까?", sCount, iDiv);
                    break;
                case MessageType.수정질문:
                    rMessage = string.Format("{0}{1}정보를 수정하시겠습니까?", "", iDiv);
                    break;
                case MessageType.삭제질문:
                    rMessage = string.Format("{0}{1}정보를 삭제하시겠습니까?", sCount, iDiv);
                    break;
                case MessageType.삭제질문2:
                    rMessage = string.Format("{0}정보를 삭제하시겠습니까?",iDiv);
                    break;
                case MessageType.필수항목누락:
                    rMessage = "필수 항목이 누락되었습니다. 확인해 주십시오.";
                    break;
                case MessageType.청구성공:
                    rMessage = string.Format("{0}{1}정보가 청구 되었습니다.", sCount, iDiv);
                    break;
                case MessageType.취소질문:
                    rMessage = string.Format("{0}{1}정보를 취소하시겠습니까?", "", iDiv);
                    break;
                default:
                    break;
            }
            return rMessage;
        }
    }
    enum MessageType
    {
        추가성공,
        수정성공,
        삭제성공,
        삭제성공2,
        추가질문,
        수정질문,
        삭제질문,
        삭제질문2,
        필수항목누락,
        청구성공,
        취소질문
    }
}
