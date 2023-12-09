using System;
using System.Net.Http;
using System.Threading.Tasks;
using Crawler.Anticaptcha.Configs;
using Crawler.Anticaptcha.Enums;
using Crawler.Anticaptcha.Interfaces;
using Crawler.Anticaptcha.Models;
using Crawler.Anticaptcha.Response;
using Crawler.Services.Helpers;

namespace Crawler.Anticaptcha.Services
{
    public class AnticaptchaService : IAnticaptchaService
    {
        private readonly AnticaptchaConfig _config;
        private readonly HttpClient _httpClient;

        public AnticaptchaService(AnticaptchaConfig config, IHttpClientFactory factory)
        {
            _config = config;
            _httpClient = factory.CreateClient("anticaptcha_client");
        }

        public async Task<string> Process(ImageToTextInfo model)
        {
            var request = CreateTaskRequest.From(model, _config.ClientKey);
            var taskId = await CreateTask(request);
            var result = await WaitForResult(taskId, _config.ClientKey);

            return result.Solution.Text;
        }

        private async Task<int> CreateTask(CreateTaskRequest request)
        {
            var response = await PostRequest<CreateTaskResponse>(ApiMethod.CreateTask, request);

            if (response is null || response.ErrorId is null || response.ErrorId != 0)
                throw new Exception("API error");

            return response.TaskId.Value;
        }

        private async Task<TaskResultResponse> WaitForResult(int taskId, string clientKey, int maxAttempts = 60, int currentAttempt = 0)
        {
            if (currentAttempt >= maxAttempts)
            {
                throw new Exception("Time is out while wating for anticaptcha result");
            }

            // First attempt wait 3sec then wait 1sec.
            if (currentAttempt == 0)
                await Task.Delay(3000);
            else
                await Task.Delay(1000);

            var taskResultRequest = new GetTaskResultRequest { ClientKey = clientKey, TaskId = taskId };
            var taskResultInfo = await PostRequest<TaskResultResponse>(ApiMethod.GetTaskResult, taskResultRequest);

            if (taskResultInfo is null || taskResultInfo.ErrorId is null)
                throw new Exception("API error");

            if (taskResultInfo.ErrorId != 0)
            {
                throw new Exception($"API error {taskResultInfo.ErrorId}: {taskResultInfo.ErrorDescription}");
            }

            if (taskResultInfo.Status == StatusType.Processing)
            {
                return await WaitForResult(taskId, clientKey, maxAttempts, currentAttempt + 1);
            }

            return taskResultInfo;
        }

        private async Task<T> PostRequest<T>(ApiMethod method, object request) where T : class
        {
            var methodName = char.ToLowerInvariant(method.ToString()[0]) + method.ToString().Substring(1);
            var host = "api.anti-captcha.com";
            var scheme = SchemeType.Https;

            var response = await _httpClient.Post<T>(new Uri($"{scheme}://{host}/{methodName}"), request);

            return response;
        }
    }
}
