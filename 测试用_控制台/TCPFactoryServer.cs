﻿using dotNetLab.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotNetLab.Common;


namespace shikii.VisionJob 
{
    public class TCPFactoryServer : XTCPServer
    {
        readonly string TCPTABLENAME = "TCP";
         
        public delegate void TCPClientConnectedInvokeCallback(int nExecuteCode);
        public TCPClientConnectedInvokeCallback tcpClientConnectedInvoke;
        // For Decoding Hex String
        byte[] byt_Arr = null;
        public TCPFactoryServer()
        {
            byt_Arr = new byte[256];
            this.TextEncode = Encoding.ASCII;
            
          
        }
        public bool Send_Mill(String strClientID, byte[] byt_Content)
        {
            try
            {
                int n = lstStrArr_ClientID.IndexOf(strClientID);
                int nResult = this.lst_Clients[n].Send(byt_Content);
                if (nResult > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                // Tipper.Ask = "是否未连接到指定的客户端？\r\n建议重新启动本程序重试。";       
                return false;
            }

        }
        public bool Send_Mill( int ClientIndex, byte[] byt_Content)
        {
            try
            {
                int n = ClientIndex;
                int nResult = this.lst_Clients[n].Send(byt_Content);
                if (nResult > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {


            //Tipper.Ask = "是否未连接到指定的客户端？\r\n建议重新启动本程序重试。";

                return false;
            }

        }
        public bool Send_Mill(String strClientID, String strWord)
        {
            byte[] bytArr = this.TextEncode.GetBytes(strWord);
            return Send_Mill(strClientID, bytArr);
        }
        //Decode That String To Byte(0-255)
        void DecodeHexString(String str)
        {
            String[] temp = str.Split(new char[] { ' ' });
            int nIndex = 0;
            foreach (var item in temp)
            {
                this.byt_Arr[nIndex++] =
                    byte.Parse(item,
                    System.Globalization.NumberStyles.HexNumber);
            }
        }
        public virtual bool SendHexStr(String strClientID, String strContent)
        {
            DecodeHexString(strContent);
            return Send_Mill(strClientID, byt_Arr);
        }
        protected override void ImplementClientCon_DisCon_Delegate()
        {
            this.ClientConnected += (nIndex) =>
            {
                try
                {
                   
                 dotNetLab.Common.R.Pipe.Info(String.Format("已经连接到客户端：{0}",
                                    this.GetClientIP(nIndex)));
                }
                catch (Exception ex)
                {

                }
               
                 
            };
            this.ClientDisconnected += (ClientIP) =>
            {
                try
                {
                   
                     dotNetLab.Common.R.Pipe.Info(String.Format("客户端：{0}已经断开",
                                        ClientIP));
                }
                catch (Exception ex)
                {

                }
               

            };
        }
        public Type GetRouteMessageType()
        {
            return typeof(RouteMessageCallback);
        }
        public Type GetTCPBaseType()
        {
            return typeof(TCPBase);
        }
        public String GetIPByIndex(int nIndex)
        {
            return lstStrArr_ClientID[nIndex];
        }
        //public void SetMessageRouteEvent(Delegate routeDel)
        //{
        //    this.Route = (RouteMessageCallback)routeDel;
        //}
    }
}
