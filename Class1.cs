﻿using System;
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


        Bitmap Receipt = new Bitmap(384, 2000);


        Bitmap Canvas = new Bitmap(384, 2000);
        Graphics rcc;
        
        int Pos_Y = 0;

        Graphics rc;


        public int LineHeight = 30;

        public int EndHeight = 30;

        public int Speed = 24;

        public Create_Receipt() {
            rc = Graphics.FromImage(Receipt);
            rc.PixelOffsetMode = PixelOffsetMode.Half;
            rc.InterpolationMode = InterpolationMode.NearestNeighbor;
            rc.FillRectangle(Brushes.White, rc.VisibleClipBounds);


            rcc = Graphics.FromImage(Canvas);
            rcc.PixelOffsetMode = PixelOffsetMode.Half;
            rcc.InterpolationMode = InterpolationMode.NearestNeighbor;
            rcc.FillRectangle(Brushes.White, rc.VisibleClipBounds);




        
        }



        void Resize(int y) {
            Receipt = new Bitmap(384, y);
            rc = Graphics.FromImage(Receipt);
            rc.PixelOffsetMode = PixelOffsetMode.Half;
            rc.InterpolationMode = InterpolationMode.NearestNeighbor;
            rc.FillRectangle(Brushes.White, rc.VisibleClipBounds);
        }

        public void Logo(String Img) {

            Bitmap ALine = new Bitmap(Img);

            rcc.DrawImage(ALine, 0, Pos_Y, (int)(ALine.Width), (int)(ALine.Height));

            Pos_Y += ALine.Height;

            Resize(Pos_Y);

            rc.DrawImage(Canvas, 0, 0, (int)(Canvas.Width), (int)(Canvas.Height));

            

        }


        public void NewLine() {

            Pos_Y += LineHeight;
            Print("");

        }


        public void PutEnd() {

            Pos_Y += EndHeight;
            Print("");

        }

        public int Pow = 1;

        public int Offset = 0;

        public void Print (String Txt){



            Bitmap ALine = new Bitmap(384, 25);
            Graphics ln = Graphics.FromImage(ALine);

            Font fnt = new Font("ＭＳ ゴシック", 10);
            ln.DrawString(Txt, fnt, Brushes.Black,new Point(0,0));


            //ln.PixelOffsetMode = PixelOffsetMode.Half;
            //ln.InterpolationMode = InterpolationMode.NearestNeighbor;
            //Bitmap logo = new Bitmap(@".\\logo.png");



            rcc.DrawImage(ALine, Offset, Pos_Y, (int)(ALine.Width * 2 * Pow), (int)(ALine.Height * 2));

            Resize(Pos_Y+LineHeight);

            rc.DrawImage(Canvas, 0, 0, (int)(Canvas.Width), (int)(Canvas.Height));



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
