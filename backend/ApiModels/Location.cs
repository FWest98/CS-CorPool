using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorPool.BackEnd.ApiModels {
    public class Location {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
