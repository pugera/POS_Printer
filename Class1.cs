using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;


using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;


namespace POS_Printer
{
    public class Sender{

        public String Port_Name = "";

        SerialPort Send_Port;
        

        //シリアルポート接続部
        public void Open() {


            Send_Port = new SerialPort();
            Send_Port.BaudRate = 300;
            Send_Port.Parity = Parity.None;
            Send_Port.DataBits = 8;
            Send_Port.StopBits = StopBits.One;
            Send_Port.Handshake = Handshake.None;
            Send_Port.PortName = Port_Name;
            
            try {
                Send_Port.Open();
            }
            catch {
                Console.WriteLine("Error");
                MessageBox.Show("Error");
                return;
            }
            Console.WriteLine("Success");
            
            return;

        }

        //RAW出力部
        public void SendRAW(Byte[] Txt,int a,int b) {
            if (Send_Port != null) {
                Send_Port.Write(Txt, a, b);
                return;
            }
            else {
                MessageBox.Show("ポートがオープンしていません。");
            }
        }


        //String出力部
        public void SendString(String Txt) {
            if (Send_Port != null) {
                Send_Port.Write(Txt);
            }
            else {
                MessageBox.Show("ポートがオープンしていません。");
            }
        }


        //シリアルポート切断部
        public void Close() {
            Send_Port.Close();
        }




        //画像出力部
        public int Speed = 24;


        public void SendImage(String FileName) {


            Bitmap myBitmap = new Bitmap(FileName);

            int a;


            for (a = 0; a < (myBitmap.Height / Speed); a++) {




                byte[] cmd = new byte[65535];
                int n = 0;



                cmd[n++] = 0x1d;//GS
                cmd[n++] = 0x76;//v
                cmd[n++] = 0x30;//0
                cmd[n++] = 0;//m
                cmd[n++] = 48;//xL
                cmd[n++] = 0;//xH
                cmd[n++] = (byte)(Speed % 256);//yL
                cmd[n++] = (byte)(Speed / 256);//yH

                for (int j = 0; j < Speed; j++) {



                    for (int m = 0; m < 48; m++) {


                        byte tis = 0;

                        for (int mx = 0; mx < 8; mx++) {

                            if ((myBitmap.GetPixel(m * 8 + (7 - mx), j + Speed * a).R + myBitmap.GetPixel(m * 8 + (7 - mx), j + Speed * a).G + myBitmap.GetPixel(m * 8 + (7 - mx), j + Speed * a).B) < 384) {

                                tis += (byte)Math.Pow(2, mx);

                            }

                        }




                        cmd[n++] = tis;

                    }
                }


                
                
                SendRAW(cmd,0,n);
                


                System.Threading.Thread.Sleep(10);
            }



            if (Speed * a < myBitmap.Height) {


                byte[] cmd = new byte[65535];
                int n = 0;

                //MessageBox.Show((myBitmap.Height - a*Speed).ToString());

                cmd[n++] = 0x1d;//GS
                cmd[n++] = 0x76;//v
                cmd[n++] = 0x30;//0
                cmd[n++] = 0;//m
                cmd[n++] = 48;//xL
                cmd[n++] = 0;//xH
                cmd[n++] = (byte)((myBitmap.Height - a * Speed) % 256);//yL
                //MessageBox.Show(cmd[n - 1].ToString());
                cmd[n++] = (byte)((myBitmap.Height - a * Speed) / 256);//yH

                for (int j = 0; j < (myBitmap.Height - a * Speed); j++) {

                    //MessageBox.Show(j.ToString());

                    for (int m = 0; m < 48; m++) {


                        byte tis = 0;

                        for (int mx = 0; mx < 8; mx++) {

                            if ((myBitmap.GetPixel(m * 8 + (7 - mx), j + Speed * a).R + myBitmap.GetPixel(m * 8 + (7 - mx), j + Speed * a).G + myBitmap.GetPixel(m * 8 + (7 - mx), j + Speed * a).B) < 384) {

                                tis += (byte)Math.Pow(2, mx);

                            }


                            //Console.WriteLine((n).ToString());

                        }







                        cmd[n++] = tis;

                    }
                }


                //MessageBox.Show(cmd.ToString() + ",");
                //MessageBox.Show(n.ToString());

                SendRAW(cmd, 0, n);



                System.Threading.Thread.Sleep(10);







            }
            



        }







        public void SendImage(Bitmap myBitmap) {

           // MessageBox.Show(myBitmap.Height.ToString());
            
            int a;


            for (a = 0; a < (myBitmap.Height / Speed); a++) {




                byte[] cmd = new byte[65535];
                int n = 0;



                cmd[n++] = 0x1d;//GS
                cmd[n++] = 0x76;//v
                cmd[n++] = 0x30;//0
                cmd[n++] = 0;//m
                cmd[n++] = 48;//xL
                cmd[n++] = 0;//xH
                cmd[n++] = (byte)(Speed % 256);//yL
                cmd[n++] = (byte)(Speed / 256);//yH

                for (int j = 0; j < Speed; j++) {



                    for (int m = 0; m < 48; m++) {


                        byte tis = 0;

                        for (int mx = 0; mx < 8; mx++) {

                            if ((myBitmap.GetPixel(m * 8 + (7 - mx), j + Speed * a).R + myBitmap.GetPixel(m * 8 + (7 - mx), j + Speed * a).G + myBitmap.GetPixel(m * 8 + (7 - mx), j + Speed * a).B) < 384) {

                                tis += (byte)Math.Pow(2, mx);

                            }

                        }



                        //Console.WriteLine((j + Speed * a).ToString());
                        


                        cmd[n++] = tis;

                    }
                }




                SendRAW(cmd, 0, n);



                System.Threading.Thread.Sleep(10);
            }



            if(Speed*a<myBitmap.Height){


                byte[] cmd = new byte[65535];
                int n = 0;

                //MessageBox.Show((myBitmap.Height - a*Speed).ToString());

                cmd[n++] = 0x1d;//GS
                cmd[n++] = 0x76;//v
                cmd[n++] = 0x30;//0
                cmd[n++] = 0;//m
                cmd[n++] = 48;//xL
                cmd[n++] = 0;//xH
                cmd[n++] = (byte)((myBitmap.Height-a*Speed) % 256);//yL
                //MessageBox.Show(cmd[n-1].ToString());
                cmd[n++] = (byte)((myBitmap.Height - a*Speed) / 256);//yH

                for (int j = 0; j < (myBitmap.Height - a*Speed); j++) {

                    //MessageBox.Show(j.ToString());

                    for (int m = 0; m < 48; m++) {


                        byte tis = 0;

                        for (int mx = 0; mx < 8; mx++) {

                            if ((myBitmap.GetPixel(m * 8 + (7 - mx), j + Speed * a).R + myBitmap.GetPixel(m * 8 + (7 - mx), j + Speed * a).G + myBitmap.GetPixel(m * 8 + (7 - mx), j + Speed * a).B) < 384) {

                                tis += (byte)Math.Pow(2, mx);

                            }


                            //Console.WriteLine((n).ToString());

                        }



                        



                        cmd[n++] = tis;

                    }
                }


                //MessageBox.Show(cmd.ToString()+",");
                //MessageBox.Show(n.ToString());

                SendRAW(cmd, 0, n);



                System.Threading.Thread.Sleep(10);






            
            }









        }













    }

    


    public class Create_Receipt {

        public String Port_Name="";


        Bitmap Receipt = new Bitmap(384, 1);
        Graphics rc;


        int Pos_Y = 0;



        public int Txt_Width = 1;//テキストの横倍率
        public int Txt_Height = 1;//テキストの縦倍率


        public int Offset = 0;//テキストのX座標オフセット



        public int LineHeight = 30;

        public int EndHeight = 30;

        public int Speed = 24;

        public Create_Receipt() {
            rc = Graphics.FromImage(Receipt);
            rc.PixelOffsetMode = PixelOffsetMode.Half;
            rc.InterpolationMode = InterpolationMode.NearestNeighbor;
            rc.FillRectangle(Brushes.White, rc.VisibleClipBounds);

        
        }



        void Resize(int y) {

            Bitmap tmp_b = new Bitmap(Receipt.Width, Receipt.Height);//現在のレシートサイズを取得

            Graphics tmp_g = Graphics.FromImage(tmp_b);//現在のレシート状態をキャプチャ


            tmp_g.FillRectangle(Brushes.White, tmp_g.VisibleClipBounds);//白塗りつぶし

            tmp_g.DrawImage(Receipt, 0, 0, Receipt.Width, Receipt.Height);//現在のレシートを貼り付け


            Receipt.Dispose();//メモリ解放
            rc.Dispose();//メモリ解放

            Receipt = new Bitmap(384,y);//レシートのサイズ変更

            
            rc = Graphics.FromImage(Receipt);//貼り付け用レイヤーのメモリ再取得
            rc.PixelOffsetMode = PixelOffsetMode.Half;//アンチエイリアス処理－＞オフ
            rc.InterpolationMode = InterpolationMode.NearestNeighbor;//アンチエイリアス処理－＞オフ
            rc.FillRectangle(Brushes.White, rc.VisibleClipBounds);//白塗りつぶし


            rc.DrawImage(tmp_b,0,0,tmp_b.Width,tmp_b.Height);//レシートに前の状態のレシートを書き込み

            tmp_b.Dispose();//メモリ解放
            tmp_g.Dispose();//メモリ解放




        }

        public void Logo(String Img) {

            

            Bitmap ALine = new Bitmap(Img);//画像読み込み

            
            Resize(Pos_Y+ALine.Height);//リサイズ

            rc.DrawImage(ALine, 0, Pos_Y, (int)(ALine.Width), (int)(ALine.Height));//描画処理反映

            Pos_Y += ALine.Height;//ビットマップの高さの差分をレシートに加算


            ALine.Dispose();
        }


        public void NewLine() {

            Pos_Y += LineHeight;
            Print("");

        }


        public void PutEnd() {

            Pos_Y += EndHeight;
            Print("");

        }



        public void Print (String Txt){

            //1ライン分の画像を生成
            Bitmap ALine = new Bitmap(384, LineHeight);
            Graphics ln = Graphics.FromImage(ALine);

            //フォント設定
            Font fnt = new Font("ＤＦ華康ゴシック体W2", 18);
            ln.DrawString(Txt, fnt, Brushes.Black,new Point(0,0));

            //レシートをリサイズ
            Resize(Pos_Y + LineHeight);

            //倍率適用して貼り付け
            rc.DrawImage(ALine, Offset, Pos_Y, (int)(ALine.Width * 1.0 * Txt_Width), (int)(ALine.Height * 1.0 * Txt_Height));

            ALine.Dispose();
            ln.Dispose();
            fnt.Dispose();


        }

        public void CreateImage() {
            Pos_Y += EndHeight;
            Print("");

            Receipt.Save(@".\\ss.png");
            //MessageBox.Show(Receipt.Height.ToString());

            Pos_Y -= EndHeight;
            Print("");

        }


        public void SendImage() {

            Pos_Y += EndHeight;
            Print("");

            //MessageBox.Show(Receipt.Height.ToString());

            Sender printer = new Sender();

            printer.Port_Name = Port_Name;
            printer.Open();
            printer.Speed=Speed;
            
            printer.SendImage(Receipt);
            printer.SendString("\n\n\n");
            printer.Close();


            Pos_Y -= EndHeight;
            Print("");
           

        }

    
    }



}
