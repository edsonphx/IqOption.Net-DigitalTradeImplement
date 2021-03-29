using System;

namespace IqOptionApi.Models
{
    public class BuyModel
    {
        public BuyModel(
            ActivePair pair,
            int size,
            OrderDirection direction,
            DateTimeOffset expiration)
        {
            Pair = pair;
            Size = size;
            Direction = direction;
            Expiration = expiration;
        }

        public ActivePair Pair { get; }
        public int Size { get; }
        public OrderDirection Direction { get; }
        public DateTimeOffset Expiration { get; }
    }
}
