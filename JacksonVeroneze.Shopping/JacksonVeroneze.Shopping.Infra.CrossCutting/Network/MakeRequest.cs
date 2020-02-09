using Akavache;
using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace JacksonVeroneze.Shopping.Infra.CrossCutting.Network
{
    //
    // Summary:
    //     /// Class responsible for network service. ///
    //
    public class MakeRequest
    {
        protected readonly AsyncRetryPolicy _policy = Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: (attempt) => TimeSpan.FromSeconds(2));

        //
        // Summary:
        //     Method responsible for executing the request. ///
        // 
        // Parameters:
        //   task:
        //     The task param.
        //
        public Task<T> Start<T>(Task<T> task)
        {
            return _policy.ExecuteAsync(async () =>
            {
                return await task;
            });
        }

        //
        // Summary:
        //     Method responsible for executing the request. ///
        // 
        // Parameters:
        //   task:
        //     The task param.
        //
        //   cacheKey:
        //     The cacheKey param.
        //
        public async Task<T> StartUseCache<T>(Task<T> task, string cacheKey)
        {
            IObservable<T> cachedConferences = BlobCache.LocalMachine.GetAndFetchLatest(cacheKey, () =>
            {
                return _policy.ExecuteAsync(async () =>
                {
                    return await task;
                });
            },
            offset =>
            {
                TimeSpan elapsed = DateTimeOffset.Now - offset;
                return elapsed > new TimeSpan(hours: 0, minutes: 30, seconds: 0);
            });

            return await cachedConferences.FirstOrDefaultAsync();
        }
    }
}