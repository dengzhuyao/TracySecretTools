using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using TracySecretTool.Server;
using TracySecretTool.Tools;

namespace MessageBomber.Server
{
    public class DuoZhuan
    {
        public bool SendMsg(string phone)
        {
            VerifyCodeHelper codeHelper = new VerifyCodeHelper();
            string code = "";
            HttpStatusCode status = new HttpStatusCode();
            CookieContainer cc = WebRequestHelper.GetCookieContainer("http://www.daichuqu.com");

            bool isOk = false;

            for (int i = 0; i < 50; i++)
            {
                while (true)
                {
                    code = codeHelper.Do("http://www.daichuqu.com/Login/verifycode", cc);
                    if (code.Length == 4)
                        break;
                }

                string sendUrl = "http://www.daichuqu.com/Login/reg_phone_verify";
                object sendParams = new
                  {
                      phone = phone,
                      verifyimg = code
                  };
                string response = WebRequestHelper.HttpPost(sendUrl, cc, sendParams);
                if (response.Contains("\"status\":1"))
                {
                    isOk = true;
                    break;
                }
            }

            return isOk;
        }
    }


    public class VerifyCodeHelper
    {
        /// <summary>
        /// 验证码个数
        /// </summary>
        private int CodeLength = 4;

        //一、获取图像
        public Bitmap GetImg(string imgUrl,CookieContainer cc)
        {
            HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(imgUrl);
           

            wreq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.87 Safari/537.36";
            wreq.CookieContainer = cc;

            HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
            Stream s = wresp.GetResponseStream();

            Image image = Image.FromStream(s);
            Bitmap bitImage = new Bitmap(image);
            //bitImage.Save("D:/code.bmp");
            return bitImage;
        }

        //二、清除边框
        public void ClearBorder(Bitmap bm)
        {
            //去边框 width
            for (int i = 0; i < bm.Width; i++)
            {
                bm.SetPixel(i, 0, Color.White);
                bm.SetPixel(i, bm.Height - 1, Color.White);
            }
            //去边框 height
            for (int j = 0; j < bm.Height; j++)
            {
                bm.SetPixel(0, j, Color.White);
                bm.SetPixel(bm.Width - 1, j, Color.White);
            }
        }

        //三、灰度处理
        public void MakeGray(Bitmap bm)
        {
            for (int i = 0; i < bm.Width; i++)
            {
                for (int j = 0; j < bm.Height; j++)
                {
                    Color c = bm.GetPixel(i, j);//原始背景颜色
                    int gray = (int)(c.R * 0.11 + c.G * 0.59 + c.B * 0.3);//计算灰度
                    bm.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
        }

        //四、二值化处理
        public void MakeBlackWhite(Bitmap bm)
        {
            for (int i = 0; i < bm.Width; i++)
            {
                for (int j = 0; j < bm.Height; j++)
                {
                    Color c = bm.GetPixel(i, j);//背景颜色
                    if (c.R > 200)  //当前像素点与临界值判断
                    {
                        bm.SetPixel(i, j, Color.White);
                    }
                    else
                    {
                        bm.SetPixel(i, j, Color.Black);
                    }
                }
            }
        }

        //4.1 清除横竖的干扰线
        public void ClearDisturbLine(Bitmap bm)
        {
            int avaliWidth = bm.Width / 5;
            int avaliHeight = bm.Height / 4;
            //清除横线
            for (int i = 1; i < bm.Height - 1; i++)
            {
                for (int j = 1; j < bm.Width - 1; j++)
                {
                    //找到起点
                    if (bm.GetPixel(j, i).R == 0)
                    {
                        bool isDisturb = true;
                        for (int z = j; z < (j + avaliWidth > bm.Width ? bm.Width : j + avaliWidth); z++)
                        {
                            Color c = bm.GetPixel(z, i);
                            if (c.R == 255)
                            {
                                isDisturb = false;
                            }
                        }
                        if (isDisturb)
                        {
                            for (int z = j; z < (j + avaliWidth > bm.Width ? bm.Width : j + avaliWidth); z++)
                            {
                                Color cUp = bm.GetPixel(z, i - 1);
                                Color cDown = bm.GetPixel(z, i + 1);
                                if (cUp.R != 0 && cDown.R != 0)
                                {
                                    bm.SetPixel(z, i, Color.White);
                                }
                            }
                        }
                    }
                }
            }


            //清除竖线
            for (int i = 1; i < bm.Width - 2; i++)
            {
                for (int j = 1; j < bm.Height - 1; j++)
                {
                    //找到起点
                    if (bm.GetPixel(i, j).R == 0)
                    {
                        bool isDisturb = true;
                        for (int z = j; z < (j + avaliHeight > bm.Height ? bm.Height : j + avaliHeight); z++)
                        {
                            Color c = bm.GetPixel(i, z);
                            if (c.R == 255)
                            {
                                isDisturb = false;
                            }
                        }
                        if (isDisturb)
                        {
                            for (int z = j; z < (j + avaliHeight > bm.Height ? bm.Height : j + avaliHeight); z++)
                            {
                                Color cLeft = bm.GetPixel(i - 1, z);
                                Color cRight = bm.GetPixel(i + 1, z);
                                if (cLeft.R != 0 && cRight.R != 0)
                                {
                                    bm.SetPixel(i, z, Color.White);
                                }
                            }
                        }
                    }
                }
            }
        }

        //五、噪点处理
        public void ClearPieces(Bitmap bm)
        {
            for (int i = 1; i < bm.Width - 1; i++)
            {
                for (int j = 1; j < bm.Height - 1; j++)
                {
                    Color c = bm.GetPixel(i, j);//原始背景颜色
                    Color cUp = bm.GetPixel(i, j - 1);
                    Color cDown = bm.GetPixel(i, j + 1);
                    Color cLeft = bm.GetPixel(i - 1, j);
                    Color cRight = bm.GetPixel(i + 1, j);
                    //清除单个噪点
                    if (c.R == 0 && cUp.R != 0 && cDown.R != 0 && cLeft.R != 0 && cRight.R != 0)
                    {
                        bm.SetPixel(i, j, Color.White);
                    }
                }
            }
        }

        //六、清除其他干扰
        public List<List<Position>> ClearOtherDisturb(Bitmap bm)
        {
            List<List<Position>> listPosition = new List<List<Position>>();
            for (int i = 1; i < bm.Width - 1; i++)
            {
                for (int j = 1; j < bm.Height - 1; j++)
                {
                    Color c = bm.GetPixel(i, j);
                    if (c.R == 0)
                    {
                        MakeOneFind(bm, new Position { X = i, Y = j }, listPosition);
                    }
                }
            }

            listPosition = listPosition.OrderBy(t => t.FirstOrDefault().X).ToList();
            return listPosition;
        }

        //做一次数字查找
        private void MakeOneFind(Bitmap bm, Position p, List<List<Position>> listPosition)
        {
            List<Position> listOnceTotal = new List<Position>();
            FindNumber(bm, p, listOnceTotal);
            listOnceTotal = listOnceTotal.OrderBy(t => t.X).ThenBy(t => t.Y).ToList();
            int minx = listOnceTotal.Min(t => t.X);
            int maxx = listOnceTotal.Max(t => t.X);
            int miny = listOnceTotal.Min(t => t.Y);
            int maxy = listOnceTotal.Max(t => t.Y);

            if (maxx - minx < 5 || maxy - miny < 5)
            {
                return;
            }
            if (!listPosition.Any(t => t.FirstOrDefault().X == listOnceTotal.FirstOrDefault().X))
            {
                listPosition.Add(listOnceTotal);
            }
        }
        private void FindNumber(Bitmap bm, Position p, List<Position> listPosition)
        {
            listPosition.Add(p);
            if (p.X - 1 > 0 && p.X + 1 < bm.Width - 1 && p.Y - 1 > 0 && p.Y + 1 < bm.Height)
            {
                List<Position> tempList = new List<Position>();
                if (bm.GetPixel(p.X, p.Y - 1).R == 0)
                {
                    if (!listPosition.Any(t => t.X == p.X && t.Y == p.Y - 1))
                    {
                        tempList.Add(new Position { X = p.X, Y = p.Y - 1 });
                    }
                }
                if (bm.GetPixel(p.X - 1, p.Y).R == 0)
                {
                    if (!listPosition.Any(t => t.X == p.X - 1 && t.Y == p.Y))
                    {
                        tempList.Add(new Position { X = p.X - 1, Y = p.Y });
                    }
                }
                if (bm.GetPixel(p.X + 1, p.Y).R == 0)
                {
                    if (!listPosition.Any(t => t.X == p.X + 1 && t.Y == p.Y))
                    {
                        tempList.Add(new Position { X = p.X + 1, Y = p.Y });
                    }
                }
                if (bm.GetPixel(p.X, p.Y + 1).R == 0)
                {
                    if (!listPosition.Any(t => t.X == p.X && t.Y == p.Y + 1))
                    {
                        tempList.Add(new Position { X = p.X, Y = p.Y + 1 });
                    }
                }
                if (bm.GetPixel(p.X + 1, p.Y - 1).R == 0)
                {
                    if (!listPosition.Any(t => t.X == p.X + 1 && t.Y == p.Y - 1))
                    {
                        tempList.Add(new Position { X = p.X + 1, Y = p.Y - 1 });
                    }
                }

                tempList.ForEach(t =>
                {
                    FindNumber(bm, t, listPosition);
                });

            };
        }

        //七、识别单个数字
        public string GetOneNumber(List<Position> listPos)
        {

            int minx = listPos.Min(p => p.X);
            int maxx = listPos.Max(p => p.X);
            List<int> list1 = new List<int>();
            for (int i = minx; i < maxx; i++)
            {
                list1.Add(listPos.Count(p => p.X == i));
            }
            string a = string.Join(",", list1);
            return new TMsg_CodeBLL().GetNum(a);
        }

        //一键执行
        public string Do(string imgUrl,CookieContainer cc)
        {
            Bitmap img = GetImg(imgUrl,cc);
            ClearBorder(img);
            MakeGray(img);
            MakeBlackWhite(img);
            ClearDisturbLine(img);
            ClearPieces(img);

            List<List<Position>> listPos = ClearOtherDisturb(img);

            string result="";
            listPos.ForEach(t =>
                {
                    string temp = GetOneNumber(t);
                    if (temp == "-1")
                    {
                        return;
                    }
                    result += temp;
                });          
            return result;
        }

        public class Position
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}