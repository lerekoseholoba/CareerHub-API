using System.Net;
using System.Text;
using System.Net.Http.Json;
using CareerHub_API.DTOs;
using CareerHub_API.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace CareerHub_API.Tests.Integration;

public class JobsControllerTests(WebApplicationFactoryFixture factory)
    : IClassFixture<WebApplicationFactoryFixture>
{
    private readonly HttpClient _client = factory.CreateClient();
    [Fact]
    public async Task GetJobs_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/v1/jobs");
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetJobs_ResponseIsPagedEnvelope()
    {
        var response = await _client.GetAsync("/api/v1/jobs?page=1&pageSize=5");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PagedResponse<JobResponse>>(json);

        Assert.NotNull(result);
        Assert.Equal(1, result.Page);
        Assert.Equal(5, result.PageSize);
        Assert.True(result.TotalCount >= 0);
    }

    [Fact]
    public async Task GetJobs_ResponseIncludesXTotalCountHeader()
    {
        var response = await _client.GetAsync("/api/v1/jobs");
        response.EnsureSuccessStatusCode();

        Assert.True(response.Headers.Contains("X-Total-Count"));
    }

    [Fact]
    public async Task GetJobs_WithoutVersion_ReturnsSameStatusAsV1()
    {
        var v1Response = await _client.GetAsync("/api/v1/jobs");
        var noVersionResponse = await _client.GetAsync("/api/jobs");

        Assert.Equal(v1Response.StatusCode, noVersionResponse.StatusCode);
    }

    [Fact]
    public async Task GetJobs_ResponseIncludesApiSupportedVersionsHeader()
    {
        var response = await _client.GetAsync("/api/v1/jobs");
        response.EnsureSuccessStatusCode();

        Assert.True(response.Headers.Contains("api-supported-versions"));
        var versions = response.Headers.GetValues("api-supported-versions");
        Assert.Contains("1.0", versions);
    }

    [Fact]
    public async Task PostJob_WithoutToken_Returns401()
    {
        var requestBody = new CreateJobRequest
        {
            Title = "Test Job",
            CompanyId = Guid.NewGuid(),
            Location = "Remote",
            Description = "Test description for job",
            ClosingDate = DateTime.UtcNow.AddDays(10)
        };

        var content = new StringContent(
            JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json");

        var response = await _client.PostAsync("/api/v1/jobs", content);

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PostApplication_WithoutToken_Returns401()
    {
        var requestBody = new
        {
            ApplicantId = Guid.NewGuid(),
            JobListingId = Guid.NewGuid()
        };

        var content = new StringContent(
            JsonConvert.SerializeObject(requestBody),
            Encoding.UTF8,
            "application/json");

        var response = await _client.PostAsync("/api/v1/applications", content);

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetJobById_WithValidId_DoesNotReturn500()
    {
        var getAll = await _client.GetAsync("/api/v1/jobs");
        getAll.EnsureSuccessStatusCode();

        var json = await getAll.Content.ReadAsStringAsync();
        var jobs = JsonConvert.DeserializeObject<PagedResponse<JobResponse>>(json);

        var id = jobs?.Data.FirstOrDefault()?.Id ?? Guid.NewGuid();
        var response = await _client.GetAsync($"/api/v1/jobs/{id}");

        Assert.NotEqual(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK
                    || response.StatusCode == System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetJobById_ResponseIncludesETagHeader()
    {
        var getAll = await _client.GetAsync("/api/v1/jobs");
        getAll.EnsureSuccessStatusCode();

        var json = await getAll.Content.ReadAsStringAsync();
        var jobs = JsonConvert.DeserializeObject<PagedResponse<JobResponse>>(json);
        var id = jobs!.Data.First().Id;

        var response = await _client.GetAsync($"/api/v1/jobs/{id}");
        response.EnsureSuccessStatusCode();

        Assert.True(response.Headers.ETag != null);
    }

    [Fact]
    public async Task GetJobById_WithMatchingETag_Returns304()
    {
        var getAll = await _client.GetAsync("/api/v1/jobs");
        getAll.EnsureSuccessStatusCode();

        var json = await getAll.Content.ReadAsStringAsync();
        var jobs = JsonConvert.DeserializeObject<PagedResponse<JobResponse>>(json);
        var id = jobs!.Data.First().Id;

        var initial = await _client.GetAsync($"/api/v1/jobs/{id}");
        var etag = initial.Headers.ETag;

        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/jobs/{id}");
        request.Headers.IfNoneMatch.Add(etag!);

        var second = await _client.SendAsync(request);

        Assert.Equal(System.Net.HttpStatusCode.NotModified, second.StatusCode);
    }
}