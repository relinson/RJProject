using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;

namespace PRO_ReceiptsInvMgr.WebService
{
    /// <summary>
    /// SOCKS代理类
    /// </summary>
    public class SocksProxy
    {
        public SocksProxy()
        {
        }

        #region ErrorMessages
        private static string[] errorMsgs = {
                                        "Operation completed successfully.",
                                        "General SOCKS server failure.",
                                        "Connection not allowed by ruleset.",
                                        "Network unreachable.",
                                        "Host unreachable.",
                                        "Connection refused.",
                                        "TTL expired.",
                                        "Command not supported.",
                                        "Address type not supported.",
                                        "Unknown error."
                                    };
        #endregion

        /// <summary>
        /// 异常类
        /// </summary>
        public class ConnectionException : ApplicationException
        {
            public ConnectionException(string message)
                : base(message)
            {
            }

            protected ConnectionException(SerializationInfo info, StreamingContext context)
            {
            }

            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }

        /// <summary>
        /// 测试SOCKS5代理是否可用
        /// </summary>
        /// <param name="proxyAdress">代理地址</param>
        /// <param name="proxyPort">代理端口好号</param>
        /// <param name="destAddress">目的地址</param>
        /// <param name="destPort">目的端口号</param>
        /// <param name="userName">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static bool CheckSocks5Proxy(string proxyAdress, int proxyPort, string destAddress, int destPort,
            string userName, string password)
        {
            bool ret = false;

            try
            {
                Uri uri = new Uri(destAddress);
                ConnectSocks5Server(proxyAdress, proxyPort, uri.Host, uri.Port, userName, password);
                ret = true;
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(SocksProxy), ex);
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 连接SOCKS5代理服务器
        /// </summary>
        /// <param name="proxyAdress">代理地址</param>
        /// <param name="proxyPort">代理端口好号</param>
        /// <param name="destAddress">目的地址</param>
        /// <param name="destPort">目的端口号</param>
        /// <param name="userName">账号</param>
        /// <param name="password">密码</param>
        /// <returns>TCP连接的套接字</returns>
        public static Socket ConnectSocks5Server(string proxyAdress, int proxyPort, string destAddress, int destPort,
            string userName, string password)
        {
            IPAddress proxyIP = null;
            IPAddress destIP = null;
            byte[] request = new byte[257];
            byte[] response = new byte[257];
            ushort nIndex;

            try
            {
                proxyIP = IPAddress.Parse(proxyAdress);
            }
            catch (FormatException)
            {	// get the IP address
                proxyIP = Dns.GetHostEntry(proxyAdress).AddressList[0];
            }

            // Parse destAddress (assume it in string dotted format "212.116.65.112" )
            try
            {
                destIP = IPAddress.Parse(destAddress);
            }
            catch (FormatException)
            {
                destIP = Dns.GetHostEntry(destAddress).AddressList[0];
                // wrong assumption its in domain name format "www.microsoft.com"
            }

            IPEndPoint proxyEndPoint = new IPEndPoint(proxyIP, proxyPort);

            // open a TCP connection to SOCKS server...
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(proxyEndPoint);

            nIndex = 0;
            request[nIndex] = 0x05; // Version 5.
            nIndex++;
            request[nIndex] = 0x02; // 2 Authentication methods are in packet...
            nIndex++;
            request[nIndex] = 0x00; // NO AUTHENTICATION REQUIRED
            nIndex++;
            request[nIndex] = 0x02; // USERNAME/PASSWORD
            nIndex++;
            // Send the authentication negotiation request...
            s.Send(request, nIndex, SocketFlags.None);

            // Receive 2 byte response...
            int nGot = s.Receive(response, 2, SocketFlags.None);
            if (nGot != 2)
            {
                throw new ConnectionException("Bad response received from proxy server.");
            }

            if (response[1] == 0xFF)
            {	// No authentication method was accepted close the socket.
                s.Close();
                throw new ConnectionException("None of the authentication method was accepted by proxy server.");
            }

            byte[] rawBytes;

            if (response[1] == 0x02)//Username/Password Authentication protocol
            {
                nIndex = 0;
                request[nIndex] = 0x05; // Version 5.
                nIndex++;
                // add user name
                request[nIndex] = (byte)userName.Length;
                nIndex++;
                rawBytes = Encoding.Default.GetBytes(userName);
                rawBytes.CopyTo(request, nIndex);
                nIndex += (ushort)rawBytes.Length;

                // add password
                request[nIndex] = (byte)password.Length;
                nIndex++;
                rawBytes = Encoding.Default.GetBytes(password);
                rawBytes.CopyTo(request, nIndex);
                nIndex += (ushort)rawBytes.Length;

                // Send the Username/Password request
                s.Send(request, nIndex, SocketFlags.None);
                // Receive 2 byte response...
                nGot = s.Receive(response, 2, SocketFlags.None);
                if (nGot != 2)
                {
                    throw new ConnectionException("Bad response received from proxy server.");
                }
                if (response[1] != 0x00)
                {
                    throw new ConnectionException("Bad Usernaem/Password.");
                }
            }
            // This version only supports connect command. 
            // UDP and Bind are not supported.

            // Send connect request now...
            nIndex = 0;
            request[nIndex] = 0x05;	// version 5.
            nIndex++;
            request[nIndex] = 0x01;	// command = connect.
            nIndex++;
            request[nIndex] = 0x00;	// Reserve = must be 0x00
            nIndex++;
            if (destIP != null)// Destination adress in an IP.
            {
                switch (destIP.AddressFamily)
                {
                    case AddressFamily.InterNetwork:
                        // Address is IPV4 format
                        request[nIndex++] = 0x01;
                        rawBytes = destIP.GetAddressBytes();
                        rawBytes.CopyTo(request, nIndex);
                        nIndex += (ushort)rawBytes.Length;
                        break;
                    case AddressFamily.InterNetworkV6:
                        // Address is IPV6 format
                        request[nIndex++] = 0x04;
                        rawBytes = destIP.GetAddressBytes();
                        rawBytes.CopyTo(request, nIndex);
                        nIndex += (ushort)rawBytes.Length;
                        break;
                    default:
                        break;
                }
            }
            else // Dest. address is domain name.
            {
                request[nIndex] = 0x03;   // Address is full-qualified domain name.
                nIndex++;
                request[nIndex] = Convert.ToByte(destAddress.Length); // length of address.
                nIndex++;
                rawBytes = Encoding.Default.GetBytes(destAddress);
                rawBytes.CopyTo(request, nIndex);
                nIndex += (ushort)rawBytes.Length;
            }

            // using big-edian byte order
            byte[] portBytes = BitConverter.GetBytes((ushort)destPort);
            for (int i = portBytes.Length - 1; i >= 0; i--)
            {
                request[nIndex++] = portBytes[i];
            }

            // send connect request.
            s.Send(request, nIndex, SocketFlags.None);
            //int nRece = s.Receive(response)	// Get variable length response...
            s.Receive(response);	// Get variable length response...
            if (response[1] != 0x00)
            {
                throw new ConnectionException(errorMsgs[response[1]]);
            }
            // Success Connected...
            return s;
        }

        /// <summary>
        /// 测试SOCKS4代理是否可用
        /// </summary>
        /// <param name="proxyAdress">代理地址</param>
        /// <param name="proxyPort">代理端口好号</param>
        /// <param name="destAddress">目的地址</param>
        /// <param name="destPort">目的端口号</param>
        /// <returns></returns>
        public static bool CheckSocks4Proxy(string proxyAdress, int proxyPort, string destAddress, int destPort)
        {
            bool ret = false;

            try
            {
                Uri uri = new Uri(destAddress);
                ConnectSocks4Server(proxyAdress, proxyPort, uri.Host, uri.Port);
                ret = true;
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(typeof(SocksProxy), ex);
                throw;
            }

            return ret;
        }

        /// <summary>
        /// 连接SOCKS4代理服务器
        /// </summary>
        /// <param name="proxyAdress">代理地址</param>
        /// <param name="proxyPort">代理端口好号</param>
        /// <param name="destAddress">目的地址</param>
        /// <param name="destPort">目的端口号</param>
        /// <returns>TCP连接的套接字</returns>
        public static Socket ConnectSocks4Server(string proxyAdress, int proxyPort, string destAddress, int destPort)
        {
            IPAddress proxyIP = null;
            IPAddress destIP = null;
            try
            {
                proxyIP = IPAddress.Parse(proxyAdress);
            }
            catch (FormatException)
            {	// get the IP address
                proxyIP = Dns.GetHostEntry(proxyAdress).AddressList[0];
            }

            // Parse destAddress (assume it in string dotted format "212.116.65.112" )
            try
            {
                destIP = IPAddress.Parse(destAddress);
            }
            catch (FormatException)
            {
                destIP = Dns.GetHostEntry(destAddress).AddressList[0];
                // wrong assumption its in domain name format "www.microsoft.com"
            }

            IPEndPoint proxyEndPoint = new IPEndPoint(proxyIP, proxyPort);

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(proxyEndPoint);

            //构造Socks4代理服务器第一连接头(无用户名密码) 
            byte[] request = new byte[10];
            byte[] response = new byte[10];
            request[0] = 4;
            request[1] = 1;
            request[2] = 0;

            //发送Socks4代理第一次连接信息 
            s.Send(request, 3, SocketFlags.None);
            int nGot = s.Receive(response, response.Length, SocketFlags.None);
            if (nGot < 2)
            {
                s.Close();
                throw new Exception("不能获得代理服务器正确响应。");
            }

            if (response[0] != 4 || (response[1] != 0 && response[1] != 2))
            {
                s.Close();
                throw new Exception("代理服务其返回的响应错误。");
            }

            request[0] = 4;
            request[1] = 1;
            request[2] = 0;
            request[3] = 1;

            string strIp = destIP.ToString();
            string[] strAryTemp = strIp.Split(new char[] { '.' });
            request[4] = Convert.ToByte(strAryTemp[0]);
            request[5] = Convert.ToByte(strAryTemp[1]);
            request[6] = Convert.ToByte(strAryTemp[2]);
            request[7] = Convert.ToByte(strAryTemp[3]);

            request[8] = (byte)(destPort / 256);
            request[9] = (byte)(destPort % 256);

            // send connect request.
            s.Send(request, request.Length, SocketFlags.None);
            //int iRecCount = s.Receive(response, response.Length, SocketFlags.None)
            s.Receive(response, response.Length, SocketFlags.None);
            if (response[1] != 0)
            {
                s.Close();
                throw new Exception("第二次连接Socks4代理返回数据出错。");
            }
            // Success Connected...
            return s;
        }
    }
}
