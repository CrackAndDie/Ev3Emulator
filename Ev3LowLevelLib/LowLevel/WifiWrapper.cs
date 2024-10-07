using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Ev3Emulator.LowLevel;

namespace Ev3LowLevelLib.LowLevel
{
	public static class WifiWrapper
	{
		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_wifi_startConnections(reg_w_wifi_startConnectionsAction startConnections);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_wifi_startConnectionsAction();

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_wifi_stopConnections(reg_w_wifi_stopConnectionsAction stopConnections);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void reg_w_wifi_stopConnectionsAction();

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_wifi_readData(reg_w_wifi_readDataAction readData);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int reg_w_wifi_readDataAction(IntPtr buf, int amount);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_wifi_writeData(reg_w_wifi_writeDataAction writeData);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int reg_w_wifi_writeDataAction(IntPtr buf, int amount);

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_wifi_isConnected(reg_w_wifi_isConnectedAction isConnected);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate byte reg_w_wifi_isConnectedAction();

		[DllImport(@"lms2012", CallingConvention = CallingConvention.Cdecl)]
		private extern static void reg_w_wifi_isDataAvailable(reg_w_wifi_isDataAvailableAction isDataAvailable);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate byte reg_w_wifi_isDataAvailableAction();

		public static void Init()
		{
			reg_w_wifi_startConnections(StartConnections);
			reg_w_wifi_stopConnections(StopConnections);
			reg_w_wifi_readData(ReadData);
			reg_w_wifi_writeData(WriteData);
			reg_w_wifi_isConnected(IsConnected);
			reg_w_wifi_isDataAvailable(IsDataAvailable);
		}

		private static void StartConnections()
		{
			if (_currentConnection != null)
				StopConnections();

			_currentConnection = new LabVIEW();
			_currentConnection.SpawnConnectThread();
			_currentConnection.Connect();
		}

		private static void StopConnections()
		{
			if (_currentConnection == null)
				return;

			_currentConnection.DisposeConnectThread();
			_currentConnection.ConnectedSocket?.Close();
			_currentConnection.ConnectedSocket?.Dispose();
			_currentConnection.ConnectedSocket = null;
			_currentConnection.ServerSocket?.Stop();
			_currentConnection.ServerSocket?.Dispose();
			_currentConnection.ServerSocket = null;
			_currentConnection = null;
		}

		private unsafe static int ReadData(IntPtr buf, int amount)
		{
			if (_currentConnection.ConnectedSocket == null) // should not happen
				return 0;

			var dt = (byte*)buf.ToPointer();
			byte[] recvBuff = new byte[amount];
			var reallyRecv = _currentConnection.ConnectedSocket.Receive(recvBuff);
			for (int i = 0; i < reallyRecv; ++i)
			{
				dt[i] = recvBuff[i];
			}
			return reallyRecv;
		}

		private unsafe static int WriteData(IntPtr buf, int amount)
		{
			if (_currentConnection.ConnectedSocket == null) // should not happen
				return 0;

			var dt = (byte*)buf.ToPointer();
			byte[] sendBuff = new byte[amount];
			for (int i = 0; i < amount; ++i)
			{
				sendBuff[i] = dt[i];
			}
			var reallySent = _currentConnection.ConnectedSocket.Send(sendBuff);
			return reallySent;
		}

		private static byte IsConnected()
		{
			return (byte)((_currentConnection.ConnectedSocket != null) ? 1 : 0);
		}

		private static byte IsDataAvailable()
		{
			return (byte)((_currentConnection.ConnectedSocket != null && _currentConnection.ConnectedSocket.Available > 0) ? 1 : 0);
		}

		private static LabVIEW _currentConnection;

		#region Connections
		private class LabVIEW
		{
			public string Serial { get; set; }
			public UInt16 Port { get; set; }
			public string Name { get; set; }
			public string Protocol { get; set; }

			private CancellationTokenSource _connectTaskCts;
			private Task _connectTask;

			private object _connectedSocketLock = new object();
			private Socket _connectedSocket;
			public Socket ConnectedSocket 
			{
				get { lock (_connectedSocketLock) return _connectedSocket; }
				set { lock (_connectedSocketLock) _connectedSocket = value; }
			}

			private object _serverSocketLock = new object();
			private TcpListener _serverSocket;
			public TcpListener ServerSocket
			{
				get { lock (_serverSocketLock) return _serverSocket; }
				set { lock (_serverSocketLock) _serverSocket = value; }
			}

			public LabVIEW()
			{
				Serial = "001612345678";
				Port = 5555;
				Name = "EV3-rcv";
				Protocol = "EV3";
			}

			public void SpawnConnectThread()
			{
				_connectTaskCts = new CancellationTokenSource();

				var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				udpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
				var localIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0);
				udpSocket.Bind(localIP);
				udpSocket.ReceiveTimeout = 10000;
				udpSocket.EnableBroadcast = true;

				_connectTask = Task.Run(() =>
				{
					try
					{
						var sendStr = string.Format(
							"Serial-Number: {0}\r\nPort: {1}\r\nName: {2}\r\nProtocol: {3}\r\n",
							Serial, Port, Name, Protocol
						);
						var remoteIP = new IPEndPoint(IPAddress.Parse("127.255.255.255"), 3015);
						// Console.WriteLine(sendStr);
						while (!_connectTaskCts.IsCancellationRequested)
						{
							byte[] toSend = Encoding.UTF8.GetBytes(sendStr);
							int sentAmount = udpSocket.SendTo(toSend, remoteIP);
							if (sentAmount == toSend.Length)
							{
								byte[] recvBuff = new byte[64];
								udpSocket.Receive(recvBuff, 0, 64, SocketFlags.None, out SocketError err);
								if (err != SocketError.Success)
								{
									continue;
								}

								// Console.WriteLine(Encoding.UTF8.GetString(recvBuff));
							}
							Thread.Sleep(5000);
						}
					}
					catch (Exception ex)
					{
						Debug.WriteLine($"Exception occured in EV3Communicator connection thread: {ex}");
					}
				}, _connectTaskCts.Token);
			}

			public void DisposeConnectThread()
			{
				if (_connectTaskCts != null)
					_connectTaskCts.Cancel();
			}

			public async void Connect()
			{
				IPAddress localAddr = IPAddress.Parse("127.0.0.1");
				ServerSocket = new TcpListener(localAddr, Port);
				ServerSocket.Start();
				var sct = await _serverSocket.AcceptSocketAsync();
				//byte[] recvBuff = new byte[64];
				//sct.Receive(recvBuff);
				//// Console.WriteLine(Encoding.UTF8.GetString(recvBuff));
				//Debug.WriteLine(Encoding.UTF8.GetString(recvBuff));
				//byte[] toSend = Encoding.UTF8.GetBytes("Accept:EV340\r\n\r\n");
				//sct.Send(toSend);
				ConnectedSocket = sct;
				ConnectedSocket.ReceiveTimeout = 5000;
			}
		}
		#endregion
	}
}
