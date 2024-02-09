using System.Text.Json;
using Api.FunctionalTests.Contracts;
using Newtonsoft.Json;
using SharedKernel;

namespace Api.FunctionalTests.Extensions;

internal static class HttpResponseMessageExtensions
{
    internal static async Task<CustomProblemDetails> GetProblemDetails(
        this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Successful response");
        }

        string result = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            IncludeFields = true
        };

        CustomProblemDetails problemDetails = JsonConvert.DeserializeObject<CustomProblemDetails>(result);

        Ensure.NotNull(problemDetails);

        return problemDetails;
    }
}
