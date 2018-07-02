using FabricWCF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Services
{
    internal class SetupService : WcfServiceBase, ISetupService
    {
        public Task<string> Home()
        {
            return Task.FromResult("Welcome to Core!");
        }
    }
}
