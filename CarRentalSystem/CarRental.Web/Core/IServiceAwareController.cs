using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Web.Core
{
    interface IServiceAwareController
    {
        void RegisterDisposableServices(List<IServiceContract> disposableServices);

        List<IServiceContract> DisposableServices { get; }
    }
}
