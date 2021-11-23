using PdNetwork.Client.Impl;
using UnityEngine;

namespace Sample
{
    public class SampleUdpClient : MonoBehaviour
    {
        public string host;
        public int outPort;
        public int inPort;

        private PdUdpClient _udpClient;
        
        private void Awake()
        {
            _udpClient = new PdUdpClient();
            _udpClient.Configure(host, outPort, inPort);

            _udpClient.OnResponse += Response;
        }

        private void Response(byte[] data)
        {
            Debug.Log("response: " + data);
        }

        private void OnDestroy()
        {
        }
        
        private void OnGUI()
        {
            if (GUI.Button(new Rect(Screen.width - 300, 5, 150, 30), "Send bytes"))
            {
                _udpClient.Send(new byte[]{1, 1, 1, 1, 1});
            }
        }
        
    }
}