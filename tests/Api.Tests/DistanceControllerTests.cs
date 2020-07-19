using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using CTeleport.Client.Interfaces;
using CTeleport.Client.Models;
using DistanceService.Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Api.Tests
{
    public class DistanceControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _fixture;
        private readonly Mock<ICTeleportClient> _cTeleportClientMock;
        private readonly HttpClient _client;

        private static (string Request, AirportInfo Response)[] ClientSuccessData =>
            new[]
            {
                ("AMS", new AirportInfo {
                    IataCode = "AMS",
                    Location = new LocationPoint
                    {
                        Lat = 52.374030,
                        Lon = 4.889690
                    }
                }),
                ("BKK", new AirportInfo {
                    IataCode = "BKK",
                    Location = new LocationPoint
                    {
                        Lat = 13.600000,
                        Lon = 100.716700
                    }
                })
            };

        public DistanceControllerTests(WebApplicationFactory<Startup> fixture)
        {
            _fixture = fixture;
            _cTeleportClientMock = new Mock<ICTeleportClient>();
            foreach (var data in ClientSuccessData)
            {
                var (request, response) = data;
                _cTeleportClientMock
                    .Setup(action => action.GetAirportInfo(It.Is<string>(x => x == request)))
                    .Returns(Task.FromResult(response));
            }

            _client = CreateClient(_cTeleportClientMock.Object);
        }

        [Fact]
        public async Task GetDistance_SendCorrectRequest_ReturnCorrectResponse()
        {
            var response = await _client.GetAsync("/api/distance/calculate?from=AMS&to=BKK");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            _cTeleportClientMock.VerifyAll();

            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<Result<double>>(body);

            content.IsSuccess.Should().BeTrue();
            content.IsFailure.Should().BeFalse();
            content.Value.Should().Be(5718.2989789776648);
        }

        [Theory]
        [InlineData("AMS", "")]
        [InlineData("", "BKK")]
        [InlineData("AMS", null)]
        [InlineData(null, "BKK")]
        [InlineData("AMSB", "BKK")]
        [InlineData("AMS", "BBKK")]
        [InlineData("AM", "BKK")]
        [InlineData("AMS", "BK")]
        public async Task GetDistance_SendIncorrectRequest_ReturnBadRequest(string from, string to)
        {
            var response = await _client.GetAsync($"/api/distance/calculate?from={from}&to={to}");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public static IEnumerable<object[]> ClientFailureData =>
            new List<object[]>
            {
                new object[] { "AMS", null },
                new object[] { "AMS", new AirportInfo { IataCode = "AMS", Location = null }}
            };

        [Theory]
        [MemberData(nameof(ClientFailureData))]
        public async Task GetDistance_SendCorrectRequest_ClientBrokeDown_ReturnFailure(string requestCode,
            AirportInfo failedResponse)
        {
            var mock = new Mock<ICTeleportClient>();
            mock
                .Setup(action => action.GetAirportInfo(It.Is<string>(x => x == requestCode)))
                .Returns(Task.FromResult(failedResponse));

            var client = CreateClient(mock.Object);

            var response = await client.GetAsync($"/api/distance/calculate?from={requestCode}&to=BKK");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            mock.VerifyAll();

            var body = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<Result<double>>(body);

            content.IsSuccess.Should().BeFalse();
            content.IsFailure.Should().BeTrue();
            content.Error.Should().NotBeNullOrWhiteSpace();
        }

        private HttpClient CreateClient(ICTeleportClient cTeleportClient) =>
            _fixture.WithWebHostBuilder(builder =>
                builder.ConfigureTestServices(services =>
                    services.AddTransient(provider => cTeleportClient))).CreateClient();
    }
}
