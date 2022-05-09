using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using EscPosServer.Check;

namespace EscPosServer
{
    internal class EscPosServer
    {
        private readonly TcpListener _tcpListener;
        private readonly List<TcpClient> _connectedClients = new();
        private readonly object _clientsLock = new();

        private CancellationTokenSource _cancellationTokenSource;
        private Task _waitConnectionTask;
        private bool _isServerStarted = false;

        public EscPosServer(int port)
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
        }

        public bool IsWriterBusy { get; private set; }

        public bool IsHaveClients
        {
            get
            {
                lock (_clientsLock)
                {
                    return _connectedClients.Count > 0;
                }
            }
        }

        public void StartServer()
        {
            if (_isServerStarted)
                return;

            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
            _tcpListener.Start();
            _waitConnectionTask = WaitForClientsAsync(cancellationToken);
            _isServerStarted = true;
        }

        public void StopServer()
        {
            if (!_isServerStarted)
                return;

            _cancellationTokenSource.Cancel();
            _waitConnectionTask.Wait();
            lock (_clientsLock)
            {
                foreach (var client in _connectedClients)
                {
                    client.Close();
                }
            }

            _tcpListener.Stop();
            _isServerStarted = false;
        }

        public async Task WriteCheckAsync(EscPosCheck check)
        {
            IsWriterBusy = true;

            var cancellationToken = _cancellationTokenSource.Token;
            TcpClient[] clientsCopy;
            lock (_clientsLock)
            {
                clientsCopy = _connectedClients.ToArray();
            }

            foreach (var client in clientsCopy)
            {
                await WriteToStreamAsync(client, check, cancellationToken);
            }

            IsWriterBusy = false;
        }

        private async Task WaitForClientsAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var tcpClient = await _tcpListener.AcceptTcpClientAsync(cancellationToken);
                lock (_clientsLock)
                {
                    _connectedClients.Add(tcpClient);
                }
            }
        }

        private async Task WriteToStreamAsync(TcpClient client, EscPosCheck check, CancellationToken cancellationToken)
        {
            try
            {
                var stream = client.GetStream();
                var checkBytes = check.GetBytes();
                await stream.WriteAsync(checkBytes, cancellationToken);

#if DEBUG
                var remoteEndPoint = ((IPEndPoint) client.Client.RemoteEndPoint);
                var remoteAddress = remoteEndPoint?.Address.ToString() ?? "UNKNOWN";
                var remotePort = remoteEndPoint?.Address.ToString() ?? "UNKNOWN";
                Console.WriteLine($"{checkBytes.Length} bytes is written to the {remoteAddress}:{remotePort}");
#endif
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e);
#endif

                lock (_clientsLock)
                {
                    _connectedClients.Remove(client);
                    client.Close();
                }
            }
        }
    }
}