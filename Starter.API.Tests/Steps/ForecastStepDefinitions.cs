using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Starter.API.Weather.Domain.Models;
using Starter.API.Weather.Domain.Repositories;
using Starter.API.Weather.Resources;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Starter.API.Tests.Steps;

[Binding]
public class ForecastStepDefinitions {
  private const string endpoint = "api/v1/forecast";
  private readonly WebApplicationFactory<Program> factory;
  private readonly IForecastRepository repository;
  private readonly IUnitOfWork unitOfWork;
  private HttpClient client = null!;
  private HttpResponseMessage response = null!;
  private ForecastResource? entity;
  private IEnumerable<ForecastResource>? entities;

  public ForecastStepDefinitions(
    WebApplicationFactory<Program> factory,
    IForecastRepository repository,
    IUnitOfWork unitOfWork
  ) {
    this.factory = factory;
    this.repository = repository;
    this.unitOfWork = unitOfWork;
  }

  [Given(@"I am a client")]
  public void GivenIAmAClient() {
    client = factory.CreateDefaultClient();
  }

  [Given(@"the repository has data")]
  public async Task GivenTheRepositoryHasData(Table table) {
    var entries = table.CreateSet<Forecast>();
    foreach (var entry in entries) {
      await repository.Add(entry);
      await unitOfWork.Complete();
    }
  }

  [When(@"a GET request is sent")]
  public async Task WhenAGetRequestIsSent() {
    response = await client.GetAsync(endpoint);
  }

  [When(@"a POST request is sent")]
  public async Task WhenAPostRequestIsSent(Table table) {
    var data = table.CreateInstance<ForecastRequest>();
    var json = JsonConvert.SerializeObject(data);
    var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
    response = await client.PostAsync(endpoint, content);
  }

  [Then(@"a response with status (.*) is received")]
  public void ThenAResponseWithStatusIsReceived(int status) {
    var expected = (HttpStatusCode) status;
    Assert.Equal(expected, response.StatusCode);
  }

  [Then(@"a list of Forecast resources is included in the body")]
  public async Task ThenAListOfForecastResourcesIsIncludedInTheBody(Table table) {
    entities = await response.Content.ReadFromJsonAsync<List<ForecastResource>>();
    table.CompareToSet(entities);
  }

  [Then(@"a Forecast resource is included in the body")]
  public async Task ThenAForecastResourceIsIncludedInTheBody(Table table) {
    entity = await response.Content.ReadFromJsonAsync<ForecastResource>();
    table.CompareToInstance(entity);
  }
}
