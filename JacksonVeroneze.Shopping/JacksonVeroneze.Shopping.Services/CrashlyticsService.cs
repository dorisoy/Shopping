using JacksonVeroneze.Shopping.Common;
using JacksonVeroneze.Shopping.Domain.Entities;
using JacksonVeroneze.Shopping.Domain.Interface.Repositories;
using JacksonVeroneze.Shopping.Domain.Interface.Services;
using JacksonVeroneze.Shopping.Services.Interfaces;
using System.Threading.Tasks;

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
        //     Method responsible for recording a certain error / event in the App-Center.
        // 
        // Parameters:
        //   e:
        //     The e param.
        //
        public async void TrackErrorAsync(Exception e)
        {
            try
            {
                Crashes.TrackError(e);
            }
            catch (Exception) { }
        }

        //
        // Summary:
        //     Method responsible for recording a certain error / event in the App-Center.
        // 
        // Parameters:
        //   eventName:
        //     The eventName param.
        //
        //   dictionary:
        //     The dictionary param.
        //
        public void TrackEventAsync(string eventName, IDictionary<string, string> dictionary)
        {
            try
            {
                Analytics.TrackEvent(eventName, dictionary);
            }
            catch (Exception) { }
        }
    }
}