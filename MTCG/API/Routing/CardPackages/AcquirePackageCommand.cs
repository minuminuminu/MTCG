using MTCG.BLL;
using MTCG.HttpServer.Response;
using MTCG.HttpServer.Routing;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.API.Routing.CardPackages
{
    internal class AcquirePackageCommand : IRouteCommand
    {
        private readonly IPackageManager _packageManager;
        private readonly IUserManager _userManager;
        private readonly string _authToken;

        public AcquirePackageCommand(IPackageManager packageManager, IUserManager userManager, string authToken)
        {
            _authToken = authToken;
            _userManager = userManager;
            _packageManager = packageManager;
        }

        public HttpResponse Execute()
        {
            HttpResponse response;

            try
            {
                if (!_packageManager.IsPackageAvailable())
                {
                    throw new NoPackageAvailableException();
                }

                _userManager.WithdrawCoinsForPackage(_authToken);
                _packageManager.AcquirePackage(_authToken);

                response = new HttpResponse(StatusCode.Ok);
            } 
            catch(NotEnoughCoinsException)
            {
                response = new HttpResponse(StatusCode.Forbidden);
            }
            catch (NoPackageAvailableException)
            {
                response = new HttpResponse(StatusCode.NotFound);
            }

            return response;
        }
    }
}
