using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CorPool.Shared.ApiModels;

namespace CorPool.Shared.Hubs {
    public interface IRideHubClient {
        Task RideResult(Offer offer);
    }
}
