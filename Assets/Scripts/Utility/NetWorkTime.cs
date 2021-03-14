using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 网络时间
/// </summary>
public class NetworkTime : MonoSingleton<NetworkTime>
{
    
    /// <summary>
    /// ntp服务器
    /// </summary>
    public string TIME_SERVER_URL = "ntp7.aliyun.com";
    /// <summary>
    /// 当前时间
    /// </summary>
    public DateTime CurrentTime;
    /// <summary>
    /// 是否连上网
    /// </summary>
    public bool Connect { get; private set; }

    private void Awake()
    {
        StartCoroutine(TimeUpdate());
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            GetNetworkTime();
            yield return new WaitForSeconds(1);
        }
    }

    public DateTime GetNetworkTime()
    {
        try
        {
            //ntp服务器地址
            string server = TIME_SERVER_URL;

            var ntpData = new byte[48];
            ntpData[0] = 0x1B;
            //网络链接
            var addresses = Dns.GetHostEntry(server).AddressList;
            var ipEndPoint = new IPEndPoint(addresses[0], 123); // https port : 443
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.ReceiveTimeout = 5000;
            socket.Connect(ipEndPoint);
            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();

            ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
            ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            var networkDataTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Local)).AddMilliseconds((long)milliseconds);

            TimeZone localzone = TimeZone.CurrentTimeZone;
            TimeSpan currentOffset = localzone.GetUtcOffset(networkDataTime);
            CurrentTime = networkDataTime + currentOffset;
            Connect = true;
            return CurrentTime;

        }
        catch
        {
            TimeZone localzone_ = TimeZone.CurrentTimeZone;
            TimeSpan currentOffset_ = localzone_.GetUtcOffset(DateTime.UtcNow);
            CurrentTime = DateTime.UtcNow + currentOffset_;
            Connect = false;
            return CurrentTime;
        }

    }

}