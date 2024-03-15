using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naami.SuiNet.Apis.Event;
using Naami.SuiNet.Apis.Event.Query;
using Naami.SuiNet.Apis.Read;
using Naami.SuiNet.JsonRpc;
using Naami.SuiNet.Types;
using Naami.SuiNet.Apis.Quorum;
using Naami.SuiNet.Apis.TransactionBuilder;
using Naami.SuiNet.JsonRpc;
using Naami.SuiNet.Signer;
using Naami.SuiNet.Types;


namespace WeatherBackend.MyServices
{
    internal class QueryClient
    {
        private const string ModuleName = "weather";

        private readonly Ed25519KeyPair _keyPair;
        private readonly SuiAddress _signerAddress;

        private readonly ObjectId _packageId;

        private readonly ITransactionSigner _signer = new TransactionSigner();
        private readonly ITransactionBuilderApi _builderApi;
        private readonly IQuorumApi _quorumApi;

        public QueryClient(IJsonRpcClient jsonRpcClient, Ed25519KeyPair keyPair, ObjectId packageId,
            ObjectId capyPostObjectId, ObjectId capyRegistryObjectId, SuiAddress signerAddress)
        {
            _builderApi = new TransactionBuilderApi(jsonRpcClient);
            _quorumApi = new QuorumApi(jsonRpcClient);

            _keyPair = keyPair;
            _packageId = packageId;
            _signerAddress = signerAddress;
        }


        public Task UpdateWeatherCityOracle(
            ObjectId oracle, 
            int geoname_id, 
            int weather_id, 
            int pressure, 
            int hunidity,
            int visibility, 
            int wind_speed, 
            int wind_deg, 
            int cloud,
            int dt
       )  => ExecuteTransaction("update", Array.Empty<SuiObjectType>(),
           new[] { (object)oracle, geoname_id, weather_id, pressure, hunidity, visibility, wind_speed, wind_deg, cloud, dt });

        private async Task ExecuteTransaction(string function, SuiObjectType[] typeArgs, object[] callArgs)
        {
            var builder = await _builderApi.MoveCall(
                _signerAddress,
                _packageId,
                ModuleName,
                function,
                typeArgs,
                callArgs,
                10000
            );

            var signature = _signer.SignTransaction(
                new Intent(Scope.TransactionData),
                Convert.FromBase64String(builder.TxBytes),
                _keyPair
            );

            _ = await _quorumApi.ExecuteTransaction(
                builder.TxBytes,
                SignatureScheme.ED25519,
                signature,
                Convert.ToBase64String(_keyPair.PublicKey),
                ExecuteTransactionRequestType.WaitForEffectsCert
            );
        }

    }
}
