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

namespace WeatherBackend.MyServices
{
    internal class QueryClient
    {
        private readonly IReadApi _readApi;
        private readonly IEventApi _eventApi;

        private readonly ObjectId _packageId;

        private const string CapyModuleName = "capy";
        private const string ItemModuleName = "capy_item";

        public QueryClient(IJsonRpcClient jsonRpcClient, ObjectId packageId)
        {
            _packageId = packageId;
            _readApi = new ReadApi(jsonRpcClient);
            _eventApi = new EventApi(jsonRpcClient);
        }

        public async Task<Weather> GetCapy(ObjectId id)
        {
            var response = await _readApi.GetObject<Weather>(id);
            return response.ExistsResult.Data.Fields!;
        }

        public async Task<CapyItem[]> GetCapyItems(ObjectId capyObjectId)
        {
            var dynamicCapyObjectIds = new List<ObjectId>();

            await foreach (var dynamicFieldInfos in _readApi.GetDynamicFieldsStream(capyObjectId))
            {
                dynamicCapyObjectIds.AddRange(
                    dynamicFieldInfos
                        .Where(x => x.ObjectType.Package == _packageId)
                        .Where(x => x.ObjectType.Module == ItemModuleName)
                        .Where(x => x.ObjectType.Struct == "CapyItem")
                        .Select(x => x.ObjectId)
                );
            }

            var tasks = dynamicCapyObjectIds
                .Select(id => _readApi.GetObject<CapyItem>(id))
                .ToArray();

            await Task.WhenAll(tasks);

            return tasks.Select(x => x.Result.ExistsResult!.Data.Fields!).ToArray();
        }

    }
}
