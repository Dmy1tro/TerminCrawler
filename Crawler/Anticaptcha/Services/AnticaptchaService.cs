using Anticaptcha.Configs;
using Anticaptcha.Enums;
using Anticaptcha.Interfaces;
using Anticaptcha.Models;
using Anticaptcha.Response;
using Newtonsoft.Json;
using Shared.Helpers;
using System;
using System.Threading.Tasks;

namespace Anticaptcha.Services
{
    public class AnticaptchaService : IAnticaptchaService
    {
        private readonly AnticaptchaConfig _config;

        public AnticaptchaService(AnticaptchaConfig config)
        {
            _config = config;
        }

        public string Process(AnticaptchaInfoBase model)
        {
            model.ClientKey = _config.ClientKey;

            var taskId = CreateTask(model);

            var result = WaitForResult(taskId, model.ClientKey);

            return result.Solution.Text;
        }

        private int CreateTask(AnticaptchaInfoBase model)
        {
            var jsonPostData = model.ToJson();

            var response = PostRequest<CreateTaskResponse>(ApiMethod.CreateTask, jsonPostData);

            if (response is null || response.ErrorId is null || response.ErrorId != 0)
                throw new Exception("API error");

            return response.TaskId.Value;
        }

        private TaskResultResponse WaitForResult(int taskId, string clientKey, int maxSeconds = 60, int currentSeconds = 0)
        {
            if (currentSeconds >= maxSeconds)
            {
                throw new Exception("Time is out");
            }

            if (currentSeconds == 0)
                Task.Delay(3000).GetAwaiter().GetResult();
            else
                Task.Delay(1000).GetAwaiter().GetResult();

            var jsonData = JsonConvert.SerializeObject(new { taskId, clientKey });

            var taskResultInfo = PostRequest<TaskResultResponse>(ApiMethod.GetTaskResult, jsonData);

            if (taskResultInfo is null || taskResultInfo.ErrorId is null)
                throw new Exception("API error");

            if (taskResultInfo.ErrorId != 0)
            {
                throw new Exception($"API error {taskResultInfo.ErrorId}: {taskResultInfo.ErrorDescription}");
            }

            if (taskResultInfo.Status == StatusType.Processing)
            {
                return WaitForResult(taskId, clientKey, maxSeconds, currentSeconds + 1);
            }

            return taskResultInfo;
        }

        private T PostRequest<T>(ApiMethod method, string jsonData) where T : class
        {
            var methodName = char.ToLowerInvariant(method.ToString()[0]) + method.ToString().Substring(1);

            var response = HttpHelper.Post<T>(
                new Uri(AnticaptchaInfoBase.Scheme + "://" + AnticaptchaInfoBase.Host + "/" + methodName),
                jsonData);

            return response;
        }
    }
}
