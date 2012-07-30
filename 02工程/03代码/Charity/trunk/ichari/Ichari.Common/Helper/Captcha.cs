using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Web;
using System.IO;

namespace Ichari.Common
{
    public class Captcha
    {
        #region 验证码长度(默认6个验证码的长度)
        private int length = 4;
        public int Length
        {
            get { return length; }
            set { length = value; }
        }
        #endregion

        #region 验证码字体大小(为了显示扭曲效果，默认40像素，可以自行修改)
        int fontSize = 20;
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
        #endregion

        #region 边框补(默认1像素)
        int padding = 2;
        public int Padding
        {
            get { return padding; }
            set { padding = value; }
        }
        #endregion

        #region 是否输出燥点(默认不输出)
        bool chaos = true;
        public bool Chaos
        {
            get { return chaos; }
            set { chaos = value; }
        }
        #endregion

        #region 输出燥点的颜色(默认灰色)
        Color chaosColor = Color.LightGray;
        public Color ChaosColor
        {
            get { return chaosColor; }
            set { chaosColor = value; }
        }
        #endregion

        #region 自定义背景色(默认白色)
        Color backgroundColor = Color.White;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }
        #endregion

        #region 自定义随机颜色数组
        Color[] colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
        public Color[] Colors
        {
            get { return colors; }
            set { colors = value; }
        }
        #endregion

        #region 自定义字体数组
        string[] fonts = { "Arial", "Georgia" };
        public string[] Fonts
        {
            get { return fonts; }
            set { fonts = value; }
        }
        #endregion

        #region 自定义随机码字符串序列(使用逗号分隔)
        string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        //string codeSerial = "0,1,2,3,4,5,6,7,8,9,的,一,是,在,不,了,有,和,人,这,中,大,为,上,个,国,我,以,要,他,时,来,用,们,生,到,作,地,于,出,就,分,对,成,会,可, 主,发,年,动,同,工,也,能,下,过,子,说,产,种,面,而,方,后,多,定,行,学,法,所,民,得,经,十,三,之,进,着,等,部,度,家, 电,力,里,如,水,化,高,自,二,理,起,小,物,现,实,加,量,都,两,体,制,机,当,使,点,从,业,本,去,把,性,好,应,开,它,合, 还,因,由,其,些,然,前,外,天,政,四,日,那,社,义,事,平,形,相,全,表,间,样,与,关,各,重,新,线,内,数,正,心,反,你,明, 看,原,又,么,利,比,或,但,质,气,第,向,道,命,此,变,条,只,没,结,解,问,意,建,月,公,无,系,军,很,情,者,最,立,代,想, 已,通,并,提,直,题,党,程,展,五,果,料,象,员,革,位,入,常,文,总,次,品,式,活,设,及,管,特,件,长,求,老,头,基,资,边, 流,路,级,少,图,山,统,接,知,较,将,组,见,计,别,她,手,角,期,根,论,运,农,指,几,九,区,强,放,决,西,被,干,做,必,战, 先,回,则,任,取,据,处,队,南,给,色,光,门,即,保,治,北,造,百,规,热,领,七,海,口,东,导,器,压,志,世,金,增,争,济,阶, 油,思,术,极,交,受,联,什,认,六,共,权,收,证,改,清,己,美,再,采,转,更,单,风,切,打,白,教,速,花,带,安,场,身,车,例, 真,务,具,万,每,目,至,达,走,积,示,议,声,报,斗,完,类,八,离,华,名,确,才,科,张,信,马,节,话,米,整,空,元,况,今,集, 温,传,土,许,步,群,广,石,记,需,段,研,界,拉,林,律,叫,且,究,观,越,织,装,影,算,低,持,音,众,书,布,复,容,儿,须,际, 商,非,验,连,断,深,难,近,矿,千,周,委,素,技,备,半,办,青,省,列,习,响,约,支,般,史,感,劳,便,团,往,酸,历,市,克,何, 除,消,构,府,称,太,准,精,值,号,率,族,维,划,选,标,写,存,候,毛,亲,快,效,斯,院,查,江,型,眼,王,按,格,养,易,置,派, 层,片,始,却,专,状,育,厂,京,识,适,属,圆,包,火,住,调,满,县,局,照,参,红,细,引,听,该,铁,价,严";

        public string CodeSerial
        {
            get { return codeSerial; }
            set { codeSerial = value; }
        }
        #endregion

        #region 产生波形滤镜效果

        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;

        /// <summary>
        /// 正弦曲线Wave扭曲图片（Edit By 51aspx.com）
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }
        #endregion

        #region 生成校验码图片
        public Bitmap CreateImageCode(string code)
        {
            int fSize = FontSize;
            int fWidth = fSize + Padding;

            int imageWidth = (int)(code.Length * fWidth) + 4 + Padding * 2 + 10;
            int imageHeight = fSize * 2 + Padding;

            Bitmap image = new Bitmap(imageWidth, imageHeight);

            Graphics g = Graphics.FromImage(image);

            g.Clear(BackgroundColor);

            Random rand = new Random();

            //给背景添加随机生成的燥点
            if (this.Chaos)
            {
                int Chaoscindex = 0;

                int c = Length * 10;

                for (int i = 0; i < c; i++)
                {
                    Chaoscindex = rand.Next(Colors.Length - 1);
                    Pen pen = new Pen(Colors[Chaoscindex], 0);
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);

                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }

            int left = 0, top = 0, top1 = 1, top2 = 1;

            int n1 = (imageHeight - FontSize - Padding * 2);
            int n2 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;

            Font f;
            Brush b;

            int cindex, findex;

            //随机字体和颜色的验证码字符
            for (int i = 0; i < code.Length; i++)
            {
                cindex = rand.Next(Colors.Length - 1);
                findex = rand.Next(Fonts.Length - 1);

                f = new System.Drawing.Font(Fonts[findex], fSize, System.Drawing.FontStyle.Bold);
                b = new System.Drawing.SolidBrush(Colors[cindex]);

                if (i % 2 == 1)
                {
                    top = top2;
                }
                else
                {
                    top = top1;
                }

                left = i * fWidth;

                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }

            //画一个边框 边框颜色为Color.Gainsboro
            g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();

            //产生波形（Add By 51aspx.com）
            image = TwistImage(image, true, 8, 4);

            return image;
        }
        #endregion

        //#region 将创建好的图片输出到页面
        //public void CreateImageOnPage(string code, HttpContext context)
        //{
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //    Bitmap image = this.CreateImageCode(code);

        //    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

        //    context.Response.ClearContent();
        //    context.Response.ContentType = "image/Jpeg";
        //    context.Response.BinaryWrite(ms.GetBuffer());

        //    ms.Close();
        //    ms = null;
        //    image.Dispose();
        //    image = null;
        //}
        //#endregion

        #region 生成随机字符码
        public string CreateVerifyCode(int codeLen)
        {
            if (codeLen == 0)
            {
                codeLen = Length;
            }

            string[] arr = CodeSerial.Split(',');

            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }
        public string CreateVerifyCode()
        {
            return CreateVerifyCode(0);
        }
        public MemoryStream GetJPEG(string code)
        {
            Bitmap b = CreateImageCode(code);
            MemoryStream m = new MemoryStream();
            b.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
            return m;
        }
        #endregion
    }
}
