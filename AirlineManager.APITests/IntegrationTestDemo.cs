
namespace AirlineManager.APITests
{
    public partial class TestApi
    {
        public class IntegrationTestDemo
        {
            [SetUp]
            public void Setup()
            {
            }

            [Test]
            public async Task TestWithAuth()
            {
                var api = new TestApi();
                var client = api.CreateClient();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
                //var response = await client.GetAsync("/weatherforecast");
                Assert.Pass();
                //response.EnsureSuccessStatusCode();
            }

            [Test]
            public async Task StartWithNoneAddOneGetOneBack()
            {
                var api = new TestApi();
                var client = api.CreateClient();

                Assert.Pass();
            }

            [Test]
            public async Task StartWithNoneAddTwoGetTwoBack()
            {
                var api = new TestApi();
                var client = api.CreateClient();
                Assert.Pass();
            }
        }
    }
}