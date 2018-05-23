using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;


using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace POS_Printer
{
    public class Sender{

        public String Port_Name = "";

        SerialPort Send_Port;
        

        //シリアルポート接続部
        public void Open() {


            SerialPort serialPort1 = new SerialPort();
            serialPort1.BaudRate = 300;
            serialPort1.Parity = Parity.None;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = StopBits.One;
            serialPort1.Handshake = Handshake.None;
            serialPort1.PortName = Port_Name;
            
            try {
                serialPort1.Open();
            }
            catch {
                Console.WriteLine("Error");
                MessageBox.Show("Error");
                return;
            }
            Console.WriteLine("Success");
            Send_Port = serialPort1;
            return;

        }

        //RAW出力部
        public void SendRAW(Byte[] Txt) {
            if (Send_Port != null) {
                Send_Port.Write(Txt, 0, Txt.Length);
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



            for (int a = 0; a < (myBitmap.Height / Speed); a++) {




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


                
                
                SendRAW(cmd);



                System.Threading.Thread.Sleep(10);
            }

            




        }





















    }



}
