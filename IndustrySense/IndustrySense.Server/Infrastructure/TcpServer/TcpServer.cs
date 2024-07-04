﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IndustrySense.Server.Infrastructure.TcpServer
{
    public class TcpServer : ITcpServer
    {
        private readonly TcpListener _listener;
        private bool _isRunning = false;

        public event EventHandler<string>? MessageReceived;

        public TcpServer(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                Task.Run(() => ListenForClients());
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
        }

        private async Task ListenForClients()
        {
            _listener.Start();
            Console.WriteLine($"TCP Server started");

            while (_isRunning)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                Console.WriteLine($"TCP Client connected: {client.Client.RemoteEndPoint}");

                _ = Task.Run(() => HandleClient(client));
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while (
                    (
                        bytesRead = await stream
                            .ReadAsync(buffer, 0, buffer.Length)
                            .ConfigureAwait(false)
                    ) != 0
                )
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {message}");

                    MessageReceived?.Invoke(this, message);

                    //byte[] response = Encoding.UTF8.GetBytes($"Server: {message}");
                    //await stream.WriteAsync(response, 0, response.Length).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in HandleClient: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
