using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using OcelotApiGateway.DTOs;
using System.Net;
using System.Net.Http.Headers;

namespace OcelotApiGateway.Aggregators;

public class TestAggregator : IDefinedAggregator
{

    //https://arbems.com/en/building-api-gateway-on-net-with-ocelot/#Request-Aggregation-in-API-gateways-with-Ocelot

    public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
    {
        var breeds = await responses[0].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<BreedDto>>();
        var animals = await responses[1].Items.DownstreamResponse().Content.ReadFromJsonAsync<TableResponseDto<AnimalDto>>();

        var tests = new List<TestDto>();

        if (breeds != null)
        {
            foreach (var breed in breeds)
            {
                int count = animals != null ? animals.Rows.Where(x => x.BreedId == breed.Id).Count() : 0;
                var test = new TestDto(
                    (int)breed.Id!,
                    breed.Name,
                    count
                );
                tests.Add(test);
            }
        }

        var jsonString = JsonConvert.SerializeObject(tests, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });

        var stringContent = new StringContent(jsonString)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };

        return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");

    }
}
