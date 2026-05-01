using BTCPayServer.Hosting;
using BTCPayServer.Payments;
using BTCPayServer.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using NBitcoin;

namespace BTCPayServer.Plugins.Altcoins;
public partial class AltcoinsPlugin
{
    public void InitRaptoreum(IServiceCollection services)
    {
        var nbxplorerNetwork = NBXplorerNetworkProvider.GetFromCryptoCode("RTM");
        var network = new BTCPayNetwork()
        {
            CryptoCode = nbxplorerNetwork.CryptoCode,
            DisplayName = "Raptoreum",
            NBXplorerNetwork = nbxplorerNetwork,
            DefaultRateRules = new[]
                {
                    "RTM_X = RTM_BTC * BTC_X",
                    "RTM_BTC = coingecko(raptoreum_BTC)"
                },
            CryptoImagePath = "imlegacy/raptoreum.png",
            DefaultSettings = BTCPayDefaultSettings.GetDefaultSettings(ChainName),
            //https://github.com/satoshilabs/slips/blob/master/slip-0044.md
            // RTM uses coin type 200' (registered)
            CoinType = ChainName == ChainName.Mainnet ? new KeyPath("200'")
                : new KeyPath("1'")
        }.SetDefaultElectrumMapping(ChainName);

        var blockExplorerLink = ChainName == ChainName.Mainnet
                ? "https://explorer.raptoreum.com/tx/{0}"
                : "https://explorer.raptoreum.com/tx/{0}";
        services.AddBTCPayNetwork(network)
                .AddTransactionLinkProvider(PaymentTypes.CHAIN.GetPaymentMethodId(nbxplorerNetwork.CryptoCode), new DefaultTransactionLinkProvider(blockExplorerLink));
    }
}
