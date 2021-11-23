using ByteFormatter.Runtime;

namespace Sample
{
    public class HeroInfo
    {
        public string Name;
        public short Strength;
        public short Agility;
        public short Intelligence;

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Strength)}: {Strength}, {nameof(Agility)}: {Agility}, {nameof(Intelligence)}: {Intelligence}";
        }

        public readonly struct Reader
        {
            public HeroInfo Read(ByteReader reader)
            {
                var heroInfo = new HeroInfo
                {
                    Name = reader.ReadString(),
                    Strength = reader.ReadInt16(),
                    Agility = reader.ReadInt16(),
                    Intelligence = reader.ReadInt16()
                };
                return heroInfo;
            }
        }
    }
}