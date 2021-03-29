using IqOption.BackEnd.DTO;
using IqOptionApi;
using IqOptionApi.Models;
using IqOptionApi.Models.BinaryOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IqOption.BackEnd
{
    public class Core
    {
        public Dictionary<string, IqOptionClient> Followers { get; }

        private IqOptionClient _trader;

        public Core(LoginIqOptionDTO mainAccount, IEnumerable<LoginIqOptionDTO> followersAccount = null)
        {
            _trader = new IqOptionClient(mainAccount.Email, mainAccount.Password);
            _trader.OnBuy += FollowerBuyAsync;

            Followers = followersAccount
                ?.ToDictionary(x => x.Email, x => new IqOptionClient(x.Email, x.Password));          
        }
        public async Task<bool> ConnectMainAccountAsync()
        {
            return await _trader.ConnectAsync();
        }
        public async Task<bool> ConnectFollowersAsync()
        {
            if (Followers is null) return true;

            foreach (var account in Followers.Values)
            {
                var isConnected = await account.ConnectAsync();

                if (isConnected)
                    continue;

                return false;
            }
            return true;
        }
        public async Task<decimal> GetBalanceMainAccountAsync() => await _trader.GetBalanceAsync();

        public async Task<IDictionary<string, decimal>> GetBalanceFollowersAccountAsync() 
        {
            if (Followers is null) return null;
            var result = new Dictionary<string, decimal>();

            foreach (var follower in Followers)
            {
                var balance = await follower.Value.GetBalanceAsync();
                result.Add(follower.Key, balance);
            }

            return result;
        }
        public async Task<BinaryOptionsResult> BuyAsync(
            ActivePair pair,
            int size,
            OrderDirection direction,
            DateTimeOffset expiration)
        {
            if (pair == pair) //TODO: Verificar se é digital ou nn
            {
                return await _trader.BuyBinaryAsync(
                    pair,
                    size,
                    direction,
                    expiration);
            }

            return null;
        }
        private async Task FollowerBuyAsync(BuyModel buyModel)
        {
            if (Followers is null) return;

            foreach (var follower in Followers.Values)
            {
                if (!follower.IsConnected) continue;

                await follower.BuyBinaryAsync(buyModel);
            }
        }
    }
}
