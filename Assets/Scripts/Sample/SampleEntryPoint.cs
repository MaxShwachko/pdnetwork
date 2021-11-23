using PdNetwork.Client.Impl;
using PdNetwork.Engine;
using PdNetwork.Engine.Impl;
using PdNetwork.Exchange;
using PdNetwork.Exchange.Responses;
using PdNetwork.Sockets.Impl;
using Sample.Requests;
using Sample.Responses;
using UnityEngine;

namespace Sample
{
    public class SampleEntryPoint : MonoBehaviour
    {
        public string authToken;
        public string version;
        public string host;
        public int port;
        public int reconnectionSeconds;
        public bool secureConnection;
        
        private IPdNetEngine _pdNetEngine;
        
        private void Awake()
        {
            var exchangeHandler = new SampleAExchangeManager();
            exchangeHandler.Initialize();

            // var socketLayer = new TcpSocketLayer();
            var socketLayer = new WebSocketLayer(secureConnection);
            var socketClient = new SocketClient(socketLayer, exchangeHandler);
            _pdNetEngine = new PdNetEngine();
            _pdNetEngine.Configure(authToken, version, socketClient, reconnectionSeconds);
            _pdNetEngine.PingMonitor = new EmptyPingMonitor();

            _pdNetEngine.OnConnected += () =>
            {
                Debug.Log("OnConnected");
            };
            
            _pdNetEngine.OnConnectError += error =>
            {
                Debug.Log("OnConnectError: " + error);
            };
            
            _pdNetEngine.OnConnectionResume += () =>
            {
                Debug.Log("OnConnectionResume");
            };

            _pdNetEngine.OnReconnectionTry += attemptingReconnection =>
            {
                Debug.Log("attemptingReconnection: " + attemptingReconnection);
            };

            _pdNetEngine.OnDisconnected += error =>
            {
                Debug.Log("OnDisconnect, error: " + error);
                Debug.Log("Connected: " + _pdNetEngine.IsConnected);
            };

            var counter = 0;
            
            _pdNetEngine.OnResponse += response =>
            {
                Debug.Log("response header: " + response.GetHeader());

                if (response is PingPongResp)
                {
                    Debug.Log("average round ping Ms: " + _pdNetEngine.AverageRoundPing);   
                }

                if (response is HeroesInfoResp r)
                {
                //     Debug.Log("heroes: " + r.HeroInfos.Count);
                //     foreach (var heroInfo in r.HeroInfos)
                //     {
                //         Debug.Log("hero: " + heroInfo);
                //     }
                //     Debug.Log("=====");
                    counter++;
                    Debug.Log("HeroesInfoResp count: " + counter);
                }
            };
            
            _pdNetEngine.Connect(host, port);
        }

        private void OnDestroy()
        {
            _pdNetEngine.Disconnect();
            _pdNetEngine.Destroy();
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(Screen.width - 300, 5, 150, 30), "Kill Connection"))
            {
                _pdNetEngine.KillConnection();
            }
            
            if (GUI.Button(new Rect(Screen.width - 300, 50, 150, 30), "Get Heroes info"))
            {
                _pdNetEngine.Send(new HeroesInfoReq());
            }       
            
            if (GUI.Button(new Rect(Screen.width - 300, 95, 150, 30), "Get 500 Heroes info"))
            {
                for (var i = 0; i < 40; i++)
                {
                    _pdNetEngine.Send(new HeroesInfoReq());   
                }
            }   
        }

        private class SampleAExchangeManager : AExchangeManager
        {
            public override void Initialize()
            {
                RegisterResponseReader((byte) Header.Handshake, new HandshakeResp.Reader());
                RegisterResponseReader((byte) Header.PingPong, new PingPongResp.Reader());
                RegisterResponseReader((byte) SampleHeader.HeroesInfo, new HeroesInfoResp.Reader());
            }
        }
    }
}