using System;
using Microsoft.Extensions.DependencyInjection;

namespace CorPool.BackEnd.Helpers {
    public class LazyDep<T> : Lazy<T> where T : class {
        public LazyDep(IServiceProvider provider) : base(provider.GetService<T>) { }
    }
}
