using System.Collections.Generic;
using PdNetwork.Exchange;
using PdNetwork.Utils.Pools;

namespace Sample.Responses
{
    public readonly struct HeroesInfoResp : IResponse
    {
        public readonly List<HeroInfo> HeroInfos;

        public HeroesInfoResp(List<HeroInfo> heroInfos)
        {
            HeroInfos = heroInfos;
        }
        
        public byte GetHeader() => (byte) SampleHeader.HeroesInfo;
        
        public readonly struct Reader : IResponseReader
        {
            public IResponse ReadResponse(byte[] body)
            {
                var reader = ByteReaderPool.Instance.GetReader(body);
                var heroInfoReader = new HeroInfo.Reader();

                var res = new List<HeroInfo>();
                do
                {
                    var heroInfo = heroInfoReader.Read(reader);  
                    res.Add(heroInfo);
                } while (reader.Position < reader.Length);
                
                return new HeroesInfoResp(res);
            }
        }
    }
}