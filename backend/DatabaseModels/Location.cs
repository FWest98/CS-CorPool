﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CorPool.BackEnd.DatabaseModels {
    public class Location {
        [BsonRequired]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
