﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebApi.Custom.Throttle
{
    public class Throttler
    {
        public int RequestLimit { get; private set; }
        public int RequestsRemaining { get; private set; }
        public DateTime WindowResetDate { get; private set; }
        private static readonly ConcurrentDictionary<string, ThrottleInfo> Cache =
            new ConcurrentDictionary<string, ThrottleInfo>();

        public string ThrottleGroup { get; set; }
        private readonly int _timeoutInSeconds;

        public Throttler(string key, int requestLimit = 5, int timeoutInSeconds = 10)
        {
            RequestLimit = requestLimit;
            _timeoutInSeconds = timeoutInSeconds;
            ThrottleGroup = key;
        }

        private ThrottleInfo GetThrottleInfoFromCache()
        {
            ThrottleInfo throttleInfo =
                Cache.ContainsKey(ThrottleGroup) ? Cache[ThrottleGroup] : null;

            if (throttleInfo == null || throttleInfo.ExpiresAt <= DateTime.Now)
            {
                throttleInfo = new ThrottleInfo
                {
                    ExpiresAt = DateTime.Now.AddSeconds(_timeoutInSeconds),
                    RequestCount = 0
                };
            };

            return throttleInfo;
        }

        public bool RequestShouldBeThrottled
        {
            get
            {
                ThrottleInfo throttleInfo = GetThrottleInfoFromCache();
                WindowResetDate = throttleInfo.ExpiresAt;
                RequestsRemaining = Math.Max(RequestLimit - throttleInfo.RequestCount, 0);
                return (throttleInfo.RequestCount > RequestLimit);
            }
        }

        public void IncrementRequestCount()
        {
            Cache.AddOrUpdate(ThrottleGroup, new ThrottleInfo
            {
                ExpiresAt = DateTime.Now.AddSeconds(_timeoutInSeconds),
                RequestCount = 1
            }, (retrievedKey, throttleInfo) =>
            {
                var throttleInfoRequestCount = throttleInfo.RequestCount;
                Interlocked.Increment(ref throttleInfoRequestCount);
                return throttleInfo;
            });
        }

        private class ThrottleInfo
        {
            public DateTime ExpiresAt { get; set; }
            public int RequestCount { get; set; }
        }

        public Dictionary<string, string> GetRateLimitHeaders()
        {
            ThrottleInfo throttleInfo = GetThrottleInfoFromCache();

            int requestsRemaining = Math.Max(RequestLimit - throttleInfo.RequestCount, 0);

            var headers = new Dictionary<string, string>
            {
                {"X-RateLimit-Limit", RequestLimit.ToString()},
                {"X-RateLimit-Remaining", RequestsRemaining.ToString()},
                {"X-RateLimit-Reset", throttleInfo.ExpiresAt.ToString(CultureInfo.InvariantCulture)}
            };
            return headers;
        }

        private long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }
    }
}