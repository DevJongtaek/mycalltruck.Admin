using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mycalltruck.Admin.Class.Common;

namespace mycalltruck.Admin.Class
{
    class Filter
    {
        public class PreOrder
        {
            public static List<SelectListItem> FilterTypeList(bool IsClient)
            {
                List<SelectListItem> r = new List<SelectListItem>();
                if (IsClient)
                {
                    r.Add(new SelectListItem { Text = "인근공차", Value = 1 });
                }
                r.Add(new SelectListItem { Text = "행정구역", Value = 2 });
                if (IsClient)
                {
                    r.Add(new SelectListItem { Text = "그룹공차", Value = 3 });
                    r.Add(new SelectListItem { Text = "그룹+인근", Value = 4 });
                    r.Add(new SelectListItem { Text = "개별차량", Value = 5 });
                }
                return r;
            }

            public static List<SelectListItem> RadiusList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "1Km 이내", Value = 1 });
                    r.Add(new SelectListItem { Text = "3Km 이내", Value = 3 });
                    r.Add(new SelectListItem { Text = "5Km 이내", Value = 5 });
                    r.Add(new SelectListItem { Text = "10Km 이내", Value = 10 });
                    r.Add(new SelectListItem { Text = "20Km 이내", Value = 20 });
                    r.Add(new SelectListItem { Text = "30Km 이내", Value = 30 });
                    r.Add(new SelectListItem { Text = "50Km 이내", Value = 50 });

                    return r;
                }
            }

            public static List<SelectListItem> DriverFilterTypeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "차량번호", Value = 1 });
                    r.Add(new SelectListItem { Text = "기사명", Value = 2 });
                    r.Add(new SelectListItem { Text = "핸드폰번호", Value = 3 });
                    r.Add(new SelectListItem { Text = "사업자번호", Value = 4 });
                    return r;
                }
            }

            public static List<SelectListItem> CarTypeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "차종", Value = 0 });
                    r.Add(new SelectListItem { Text = "라보/다마스", Value = 1 });

                    r.Add(new SelectListItem { Text = "카고", Value = 2 });
                    r.Add(new SelectListItem { Text = "윙", Value = 3 });
                    r.Add(new SelectListItem { Text = "일반탑", Value = 4 });
                    r.Add(new SelectListItem { Text = "축", Value = 5 });

                    r.Add(new SelectListItem { Text = "냉동/냉장", Value = 6 });
                    r.Add(new SelectListItem { Text = "로우베드", Value = 7 });
                    r.Add(new SelectListItem { Text = "평판", Value = 8 });
                    r.Add(new SelectListItem { Text = "컨테이너", Value = 9 });
                    return r;
                }
            }

            public static List<SelectListItem> CarSizeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "톤수", Value = 0 });
                    r.Add(new SelectListItem { Text = "1t", Value = 1 });
                    r.Add(new SelectListItem { Text = "1.4t", Value = 2 });
                    r.Add(new SelectListItem { Text = "2.5t", Value = 3 });
                    r.Add(new SelectListItem { Text = "3.5t", Value = 4 });
                    r.Add(new SelectListItem { Text = "5t", Value = 5 });
                    r.Add(new SelectListItem { Text = "8t", Value = 6 });
                    r.Add(new SelectListItem { Text = "11t", Value = 7 });
                    r.Add(new SelectListItem { Text = "14t", Value = 8 });
                    r.Add(new SelectListItem { Text = "15t", Value = 9 });
                    r.Add(new SelectListItem { Text = "18t", Value = 10});
                    r.Add(new SelectListItem { Text = "22t", Value = 11 });
                    r.Add(new SelectListItem { Text = "25t", Value = 12 });
                    r.Add(new SelectListItem { Text = "25t이상", Value = 13 });
                    r.Add(new SelectListItem { Text = "트레일러", Value = 14 });
                    return r;
                }
            }

            public static List<SelectListItem> PreviewTypeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "공차전체", Value = 0 });
                    //r.Add(new SelectListItem { Text = "실시간", Value = 1 });
                    //r.Add(new SelectListItem { Text = "예고", Value = 2 });
                    r.Add(new SelectListItem { Text = "현재공차", Value = 1 });
                    r.Add(new SelectListItem { Text = "예고공차", Value = 2 });
                    return r;
                }
            }

            public static List<SelectListItem> RouteTypeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();

                    r.Add(new SelectListItem { Text = "운행노선", Value = 0 });
                    r.Add(new SelectListItem { Text = "전국", Value = 1 });
                    r.Add(new SelectListItem { Text = "수도권", Value = 2 });
                    r.Add(new SelectListItem { Text = "충청권", Value = 3 });
                    r.Add(new SelectListItem { Text = "강원권", Value = 4 });
                    r.Add(new SelectListItem { Text = "서울경상권", Value = 5 });
                    r.Add(new SelectListItem { Text = "서울전라권", Value = 6 });

                    return r;
                }
            }

            public static String ItemUrl
            {
                get {
                  //  return "http://m.mycalltruck.co.kr/ForAdmin/PreOrder_ItemFromAdmin/";
                    return "http://m.cardpay.kr/ForAdmin/PreOrder_ItemFromAdmin/";


                  //  return "http://localhost/Truck.MVC/ForAdmin/PreOrder_ItemFromAdmin/";

                   
                }
            }

            public static String CustomSelectUrl
            {
                get
                {
                    //return "http://m.mycalltruck.co.kr/ForAdmin/CustomSelect_AddFromAdmin/?ClientId={0}&PreOrderId={1}&IsChecked={2}";
                    return "http://m.cardpay.kr/ForAdmin/CustomSelect_AddFromAdmin/?ClientId={0}&PreOrderId={1}&IsChecked={2}";
                }
            }


        }

        public class PreviewPreOrder
        {
            public static List<SelectListItem> FilterTypeList(bool IsClient)
            {
                List<SelectListItem> r = new List<SelectListItem>();
                if (IsClient)
                {
                    r.Add(new SelectListItem { Text = "인근공차", Value = 1 });
                }
                r.Add(new SelectListItem { Text = "행정구역", Value = 2 });
                if (IsClient)
                {
                    r.Add(new SelectListItem { Text = "그룹공차", Value = 3 });
                    r.Add(new SelectListItem { Text = "그룹+인근", Value = 4 });
                    r.Add(new SelectListItem { Text = "개별차량", Value = 5 });
                }
                return r;
            }

            public static List<SelectListItem> RadiusList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "1Km 이내", Value = 1 });
                    r.Add(new SelectListItem { Text = "3Km 이내", Value = 3 });
                    r.Add(new SelectListItem { Text = "5Km 이내", Value = 5 });
                    r.Add(new SelectListItem { Text = "10Km 이내", Value = 10 });
                    r.Add(new SelectListItem { Text = "20Km 이내", Value = 20 });
                    r.Add(new SelectListItem { Text = "30Km 이내", Value = 30 });
                    r.Add(new SelectListItem { Text = "50Km 이내", Value = 50 });

                    return r;
                }
            }

            public static List<SelectListItem> DriverFilterTypeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "차량번호", Value = 1 });
                    r.Add(new SelectListItem { Text = "기사명", Value = 2 });
                    r.Add(new SelectListItem { Text = "핸드폰번호", Value = 3 });
                    r.Add(new SelectListItem { Text = "사업자번호", Value = 4 });
                    return r;
                }
            }

            public static List<SelectListItem> CarTypeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "차종", Value = 0 });
                    r.Add(new SelectListItem { Text = "라보/다마스", Value = 1 });

                    r.Add(new SelectListItem { Text = "카고", Value = 2 });
                    r.Add(new SelectListItem { Text = "윙", Value = 3 });
                    r.Add(new SelectListItem { Text = "일반탑", Value = 4 });
                    r.Add(new SelectListItem { Text = "축", Value = 5 });

                    r.Add(new SelectListItem { Text = "냉동/냉장", Value = 6 });
                    r.Add(new SelectListItem { Text = "로우베드", Value = 7 });
                    r.Add(new SelectListItem { Text = "평판", Value = 8 });
                    r.Add(new SelectListItem { Text = "컨테이너", Value = 9 });
                    return r;
                }
            }

            public static List<SelectListItem> CarSizeList
            {
                get
                {
                    //List<SelectListItem> r = new List<SelectListItem>();
                    //r.Add(new SelectListItem { Text = "톤수", Value = 0 });
                    //r.Add(new SelectListItem { Text = "1t", Value = 1 });
                    //r.Add(new SelectListItem { Text = "1.4t", Value = 2 });
                    //r.Add(new SelectListItem { Text = "2.5t", Value = 3 });
                    //r.Add(new SelectListItem { Text = "3.5t", Value = 4 });
                    //r.Add(new SelectListItem { Text = "5t", Value = 5 });
                    //r.Add(new SelectListItem { Text = "8t", Value = 12 });
                    //r.Add(new SelectListItem { Text = "11t", Value = 6 });
                    //r.Add(new SelectListItem { Text = "14t", Value = 7 });
                    //r.Add(new SelectListItem { Text = "15t", Value = 8 });
                    //r.Add(new SelectListItem { Text = "18t", Value = 9 });
                    //r.Add(new SelectListItem { Text = "22t", Value = 13 });
                    //r.Add(new SelectListItem { Text = "25t", Value = 10 });
                    //r.Add(new SelectListItem { Text = "25t이상", Value = 14 });
                    //r.Add(new SelectListItem { Text = "트레일러", Value = 11 });
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "톤수", Value = 0 });
                    r.Add(new SelectListItem { Text = "1t", Value = 1 });
                    r.Add(new SelectListItem { Text = "1.4t", Value = 2 });
                    r.Add(new SelectListItem { Text = "2.5t", Value = 3 });
                    r.Add(new SelectListItem { Text = "3.5t", Value = 4 });
                    r.Add(new SelectListItem { Text = "5t", Value = 5 });
                    r.Add(new SelectListItem { Text = "8t", Value = 6 });
                    r.Add(new SelectListItem { Text = "11t", Value = 7 });
                    r.Add(new SelectListItem { Text = "14t", Value = 8 });
                    r.Add(new SelectListItem { Text = "15t", Value = 9 });
                    r.Add(new SelectListItem { Text = "18t", Value = 10 });
                    r.Add(new SelectListItem { Text = "22t", Value = 11 });
                    r.Add(new SelectListItem { Text = "25t", Value = 12 });
                    r.Add(new SelectListItem { Text = "25t이상", Value = 13 });
                    r.Add(new SelectListItem { Text = "트레일러", Value = 14 });
                    return r;
                }
            }


            public static List<SelectListItem> RouteTypeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();

                    r.Add(new SelectListItem { Text = "운행노선", Value = 0 });
                    r.Add(new SelectListItem { Text = "전국", Value = 1 });
                    r.Add(new SelectListItem { Text = "수도권", Value = 2 });
                    r.Add(new SelectListItem { Text = "충청권", Value = 3 });
                    r.Add(new SelectListItem { Text = "강원권", Value = 4 });
                    r.Add(new SelectListItem { Text = "서울경상권", Value = 5 });
                    r.Add(new SelectListItem { Text = "서울전라권", Value = 6 });

                    return r;
                }
            }

            public static String ItemUrl
            {
                get
                {
                   // return "http://m.mycalltruck.co.kr/ForAdmin/PreviewPreOrder_ItemFromAdmin/";
                    return "http://m.cardpay.kr/ForAdmin/PreviewPreOrder_ItemFromAdmin/";
                }
            }

            public static String CustomSelectUrl
            {
                get
                {
                   // return "http://m.mycalltruck.co.kr/ForAdmin/CustomSelect_AddFromAdmin/?ClientId={0}&PreOrderId={1}&IsChecked={2}";
                    return "http://m.cardpay.kr/ForAdmin/CustomSelect_AddFromAdmin/?ClientId={0}&PreOrderId={1}&IsChecked={2}";
                }
            }


        }

        public class CustomSelecte
        {
            public static List<SelectListItem> DriverFilterTypeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "차량번호", Value = 1 });
                    r.Add(new SelectListItem { Text = "기사명", Value = 2 });
                    r.Add(new SelectListItem { Text = "핸드폰번호", Value = 3 });
                    r.Add(new SelectListItem { Text = "사업자번호", Value = 4 });
                    return r;
                }
            }

            public static List<SelectListItem> CarTypeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "차종", Value = 0 });
                    r.Add(new SelectListItem { Text = "라보/다마스", Value = 1 });
                   
                    r.Add(new SelectListItem { Text = "카고", Value = 2 });
                    r.Add(new SelectListItem { Text = "윙", Value = 3 });
                    r.Add(new SelectListItem { Text = "일반탑", Value = 4 });
                    r.Add(new SelectListItem { Text = "축", Value = 5 });
                 
                    r.Add(new SelectListItem { Text = "냉동/냉장", Value = 6 });
                    r.Add(new SelectListItem { Text = "로우베드", Value = 7 });
                    r.Add(new SelectListItem { Text = "평판", Value = 8 });
                    r.Add(new SelectListItem { Text = "컨테이너", Value = 9 });
                    return r;
                }
            }

            public static List<SelectListItem> CarSizeList
            {
                get
                {
                    //List<SelectListItem> r = new List<SelectListItem>();
                    //r.Add(new SelectListItem { Text = "톤수", Value = 0 });
                    //r.Add(new SelectListItem { Text = "1t", Value = 1 });
                    //r.Add(new SelectListItem { Text = "1.4t", Value = 2 });
                    //r.Add(new SelectListItem { Text = "2.5t", Value = 3 });
                    //r.Add(new SelectListItem { Text = "3.5t", Value = 4 });
                    //r.Add(new SelectListItem { Text = "5t", Value = 5 });
                    //r.Add(new SelectListItem { Text = "8t", Value = 12 });
                    //r.Add(new SelectListItem { Text = "11t", Value = 6 });
                    //r.Add(new SelectListItem { Text = "14t", Value = 7 });
                    //r.Add(new SelectListItem { Text = "15t", Value = 8 });
                    //r.Add(new SelectListItem { Text = "18t", Value = 9 });
                    //r.Add(new SelectListItem { Text = "22t", Value = 13 });
                    //r.Add(new SelectListItem { Text = "25t", Value = 10 });
                    //r.Add(new SelectListItem { Text = "25t이상", Value = 14 });
                    //r.Add(new SelectListItem { Text = "트레일러", Value = 11 });
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "톤수", Value = 0 });
                    r.Add(new SelectListItem { Text = "1t", Value = 1 });
                    r.Add(new SelectListItem { Text = "1.4t", Value = 2 });
                    r.Add(new SelectListItem { Text = "2.5t", Value = 3 });
                    r.Add(new SelectListItem { Text = "3.5t", Value = 4 });
                    r.Add(new SelectListItem { Text = "5t", Value = 5 });
                    r.Add(new SelectListItem { Text = "8t", Value = 6 });
                    r.Add(new SelectListItem { Text = "11t", Value = 7 });
                    r.Add(new SelectListItem { Text = "14t", Value = 8 });
                    r.Add(new SelectListItem { Text = "15t", Value = 9 });
                    r.Add(new SelectListItem { Text = "18t", Value = 10 });
                    r.Add(new SelectListItem { Text = "22t", Value = 11 });
                    r.Add(new SelectListItem { Text = "25t", Value = 12 });
                    r.Add(new SelectListItem { Text = "25t이상", Value = 13 });
                    r.Add(new SelectListItem { Text = "트레일러", Value = 14 });
                    return r;
                }
            }


            public static List<SelectListItem> RouteTypeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();

                    r.Add(new SelectListItem { Text = "운행노선", Value = 0 });
                    r.Add(new SelectListItem { Text = "전국", Value = 1 });
                    r.Add(new SelectListItem { Text = "수도권", Value = 2 });
                    r.Add(new SelectListItem { Text = "충청권", Value = 3 });
                    r.Add(new SelectListItem { Text = "강원권", Value = 4 });
                    r.Add(new SelectListItem { Text = "서울경상권", Value = 5 });
                    r.Add(new SelectListItem { Text = "서울전라권", Value = 6 });

                    return r;
                }
            }

            public static String ItemUrl
            {
                get
                {
                    //return "http://m.mycalltruck.co.kr/ForAdmin/CustomSelect_ItemsFromAdmin/";
                    return "http://m.cardpay.kr/ForAdmin/CustomSelect_ItemsFromAdmin/";
                    //return "http://localhost/Manage/CustomSelect/ItemsFromAdmin/";
                }
            }


        }

        public class Order
        {
            public static List<SelectListItem> MinuteList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    for (int i = 0; i < 60; i+=10)
                    {
                        r.Add(new SelectListItem { Text = i.ToString("00")+"분", Value = i });

                    }
                    return r;
                }
            }

            public static List<SelectListItem> HourList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    for (int i = 0; i < 24; i++)
                    {
                        r.Add(new SelectListItem { Text = i.ToString("00")+"시", Value = i });

                    }

                    return r;
                }
            }
            public static List<SelectListItem> IsSharedList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "독차", BoolValue = false });
                    r.Add(new SelectListItem { Text = "혼적", BoolValue = true });
                    return r;
                }
            }

            public static List<SelectListItem> StatusList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "취소", Value = 0 });
                    r.Add(new SelectListItem { Text = "배차전", Value = 1 });
                    r.Add(new SelectListItem { Text = "배차완료", Value = 3 });
                    //r.Add(new SelectListItem { Text = "상차완료", Value = 4 });
                    //r.Add(new SelectListItem { Text = "하차완료", Value = 5 });
                    //r.Add(new SelectListItem { Text = "배차취소", Value = 6 });
                    //r.Add(new SelectListItem { Text = "배차실패", Value = 7 });

                    return r;
                }
            }

            public static List<SelectListItem> StatusListWithAll
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "배송상태", Value = 0 });
                    r.Add(new SelectListItem { Text = "배차전", Value = 1 });
                    r.Add(new SelectListItem { Text = "배차완료", Value = 3 });
                    //r.Add(new SelectListItem { Text = "상차완료", Value = 4 });
                    //r.Add(new SelectListItem { Text = "하차완료", Value = 5 });
                    //r.Add(new SelectListItem { Text = "배차취소", Value = 6 });
                    //r.Add(new SelectListItem { Text = "배차실패", Value = 7 });

                    return r;
                }
            }

            public static List<SelectListItem> CarTypeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    foreach (var optionRow in SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarType").OrderBy(c=>c.Seq))
                    {
                        r.Add(new SelectListItem { Value = optionRow.Value, Text = optionRow.Name });
                    }
                    return r;
                }
            }

            public static List<SelectListItem> CarSizeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    foreach (var optionRow in SingleDataSet.Instance.StaticOptions.Where(c => c.Div == "CarSize").OrderBy(c => c.Seq))
                    {
                        r.Add(new SelectListItem { Value = optionRow.Value, Text = optionRow.Name, Number = optionRow.Number });
                    }
                    return r;
                }
            }

            public static List<SelectListItem> LocationList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "선불", Value = 1 });
                    r.Add(new SelectListItem { Text = "일괄후불", Value = 2 });
                    r.Add(new SelectListItem { Text = "착불", Value = 3 });
                    return r;
                }
            }

            public static List<SelectListItem> FilterTypeList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "행정구역", Value = 1 });
                    //r.Add(new SelectListItem { Text = "인근공차", Value = "2" });
                    r.Add(new SelectListItem { Text = "그룹", Value = 3 });
                    r.Add(new SelectListItem { Text = "그룹+행정구역", Value = 4});
                    return r;
                }
            }

            public static List<SelectListItem> RadiusList
            {
                get
                {
                    List<SelectListItem> r = new List<SelectListItem>();
                    r.Add(new SelectListItem { Text = "1Km 이내", Value = 1 });
                    r.Add(new SelectListItem { Text = "3Km 이내", Value = 3 });
                    r.Add(new SelectListItem { Text = "5Km 이내", Value = 5 });
                    r.Add(new SelectListItem { Text = "10Km 이내", Value = 10 });
                    r.Add(new SelectListItem { Text = "20Km 이내", Value = 20 });
                    r.Add(new SelectListItem { Text = "30Km 이내", Value = 30 });
                    r.Add(new SelectListItem { Text = "50Km 이내", Value = 50 });

                    return r;
                }
            }

        }

        public class SelectListItem
        {
            public int Value { get; set; }
            public bool BoolValue { get; set; }
            public string Text { get; set; }
            public double Number { get; set; }
        }
        public class SelectListItem2
        {
            public string Value { get; set; }
            public bool BoolValue { get; set; }
            public string Text { get; set; }
        }
        public class Bank
        {
            public static List<SelectListItem2> BankList
            {
                get
                {

                    List<SelectListItem2> r = new List<SelectListItem2>();
                    r.Add(new SelectListItem2 { Text = "은행선택", Value = "" });
                    r.Add(new SelectListItem2 { Text = "기업은행", Value = "003"});
                    r.Add(new SelectListItem2 { Text = "외한은행", Value = "005" });
                    r.Add(new SelectListItem2 { Text = "국민은행", Value = "004" });
                    r.Add(new SelectListItem2 { Text = "농협", Value = "011" });
                    //r.Add(new SelectListItem2 { Text = "단위농협", Value = "012" });
                    r.Add(new SelectListItem2 { Text = "우리은행", Value = "020" });
                    r.Add(new SelectListItem2 { Text = "신한은행", Value = "088" });
                    r.Add(new SelectListItem2 { Text = "SC제일", Value = "023" });
                    r.Add(new SelectListItem2 { Text = "시티은행", Value = "027" });
                    r.Add(new SelectListItem2 { Text = "부산은행", Value = "032" });
                    r.Add(new SelectListItem2 { Text = "경남은행", Value = "039" });
                    r.Add(new SelectListItem2 { Text = "대구은행", Value = "031" });
                    r.Add(new SelectListItem2 { Text = "우체국", Value = "071" });
                    r.Add(new SelectListItem2 { Text = "광주은행", Value = "034" });
                    r.Add(new SelectListItem2 { Text = "수협", Value = "007" });
                    r.Add(new SelectListItem2 { Text = "상업은행", Value = "022" });
                    r.Add(new SelectListItem2 { Text = "대동은행", Value = "030" });
                    r.Add(new SelectListItem2 { Text = "충청은행", Value = "033" });
                    r.Add(new SelectListItem2 { Text = "제주은행", Value = "035" });
                    r.Add(new SelectListItem2 { Text = "경기은행", Value = "036" });
                    r.Add(new SelectListItem2 { Text = "전북은행", Value = "037" });
                    r.Add(new SelectListItem2 { Text = "강원은행", Value = "038" });
                    r.Add(new SelectListItem2 { Text = "충북은행", Value = "040" });
                    r.Add(new SelectListItem2 { Text = "하나은행", Value = "081" });
                    r.Add(new SelectListItem2 { Text = "보람은행", Value = "082" });
                    r.Add(new SelectListItem2 { Text = "산업은행", Value = "002" });
                    r.Add(new SelectListItem2 { Text = "새마을금고", Value = "045" });
                    r.Add(new SelectListItem2 { Text = "HSBC은행", Value = "052" });    
                    r.Add(new SelectListItem2 { Text = "신협", Value = "048" });
                    r.Add(new SelectListItem2 { Text = "카카오뱅크", Value = "090" });
                    r.Add(new SelectListItem2 { Text = "케이뱅크", Value = "089" });

                    r.Add(new SelectListItem2 { Text = "유안타증권", Value = "S0" });
                    r.Add(new SelectListItem2 { Text = "미래에셋", Value = "S1" });
                    r.Add(new SelectListItem2 { Text = "신한금융투자", Value = "S2" });
                    r.Add(new SelectListItem2 { Text = "삼성증권", Value = "S3" });
                    r.Add(new SelectListItem2 { Text = "한국투자증권", Value = "S4" });
                    r.Add(new SelectListItem2 { Text = "한화증권", Value = "S5" });

                   
                    //r.Add(new SelectListItem2 { Text = "산림조합", Value = "064" });
                    
                   
                   
                    //r.Add(new SelectListItem2 { Text = "동양종금증권", Value = "209" });
                    //r.Add(new SelectListItem2 { Text = "한국투자증권", Value = "243" });
                    //r.Add(new SelectListItem2 { Text = "삼성증권", Value = "240" });
                    //r.Add(new SelectListItem2 { Text = "미래에셋", Value = "230" });
                    //r.Add(new SelectListItem2 { Text = "우리투자증권", Value = "247" });
                    //r.Add(new SelectListItem2 { Text = "현대증권", Value = "218" });
                    //r.Add(new SelectListItem2 { Text = "SK증권", Value = "266" });
                    //r.Add(new SelectListItem2 { Text = "신한금융투자", Value = "278" });
                    //r.Add(new SelectListItem2 { Text = "하이증권", Value = "262" });
                    //r.Add(new SelectListItem2 { Text = "HMC증권", Value = "263" });
                    //r.Add(new SelectListItem2 { Text = "대신증권", Value = "267" });
                    //r.Add(new SelectListItem2 { Text = "하나대투증권", Value = "270" });
                    //r.Add(new SelectListItem2 { Text = "동부증권", Value = "279" });
                    //r.Add(new SelectListItem2 { Text = "유진증권", Value = "280" });
                    //r.Add(new SelectListItem2 { Text = "메리츠증권", Value = "287" });
                    //r.Add(new SelectListItem2 { Text = "신영증권", Value = "291" });
                   
                    return r;
                }
            }
        }

        public class Dealer
        {
            public static List<SelectListItem2> DealerList
            {
                get
                {

                    List<SelectListItem2> r = new List<SelectListItem2>();
                    r.Add(new SelectListItem2 { Text = "추천인", Value = "0" });
                    r.Add(new SelectListItem2 { Text = "본사(추천인 없음)", Value = "92" });
                    r.Add(new SelectListItem2 { Text = "정귀진", Value = "75" });
                    r.Add(new SelectListItem2 { Text = "박동석", Value = "111" });
                    r.Add(new SelectListItem2 { Text = "정상형", Value = "112" });
                    r.Add(new SelectListItem2 { Text = "정균희", Value = "1202" });
                    r.Add(new SelectListItem2 { Text = "양원식", Value = "153" });

                    return r;
                }
            }
        }


        public class Driver
        {
            public static bool IsCarType(int Value)
            {
                return SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarType" && c.Value == Value);
            }
            public static bool IsCarSize(int Value)
            {
                return SingleDataSet.Instance.StaticOptions.Any(c => c.Div == "CarSize" && c.Value == Value);
            }
        }
    }
}
