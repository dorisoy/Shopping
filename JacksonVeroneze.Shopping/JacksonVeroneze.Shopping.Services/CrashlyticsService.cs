using JacksonVeroneze.Shopping.Services.Interfaces;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;

namespace JacksonVeroneze.Shopping.Services
{
    //
    // Summary:
    //     /// Class responsible for the service. ///
    //
    public class CrashlyticsService : ICrashlyticsService
    {

        //
        // Summary:
        //     Method responsible for recording a certain error/event in the App-Center.
        // 
        // Parameters:
        //   e:
        //     The e param.
        //
        public void TrackError(Exception e)
        {
            try
            {
                Crashes.TrackError(e);
            }
            catch (Exception) { }
        }

        //
        // Summary:
        //     Method responsible for recording a certain error/event in the App-Center.
        // 
        // Parameters:
        //   eventName:
        //     The eventName param.
        //
        //   dictionary:
        //     The dictionary param.
        //
        public void TrackEvent(string eventName, IDictionary<string, string> dictionary)
        {
            try
            {
                Analytics.TrackEvent(eventName, dictionary);
            }
            catch (Exception) { }
        }
    }
}