using System;
using System.Collections.Generic;

namespace JacksonVeroneze.Shopping.Services.Interfaces
{
    //
    // Summary:
    //     /// Interface responsible for the contract. ///
    //
    public interface ICrashlyticsService
    {
        void TrackEvent(string eventName, IDictionary<string, string> dictionary);

        void TrackError(Exception e);
    }
}