using ByteFormatter.Runtime;
using PdNetwork.Exchange;

namespace Sample.Requests
{
    public readonly struct HeroesInfoReq : IRequest
    {
        public byte GetHeader() => (byte) SampleHeader.HeroesInfo;

        public void WriteBody(ByteWriter writer)
        {
            
        }
    }
}