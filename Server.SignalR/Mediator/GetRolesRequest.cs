//using Calabonga.AspNetCore.Controllers.Base;
//using MediatR;
//using MediatR.Wrappers;
//using Microsoft.AspNetCore.Http;

//using System.Threading;
//using System.Threading.Tasks;

//namespace Server.SignalR.Mediator
//{
//    public record GetRolesRequest : RequestBase<string>;
//    public class GetRolesRequestHandler : RequestHandler<GetRolesRequest, string>
//    {
//        private readonly IHttpContextAccessor _contextAccessor;

//        public GetRolesRequestHandler(IHttpContextAccessor contextAccessor)
//        {
//            _contextAccessor = contextAccessor;
//        }

//        public Task<string> Handle(GetRolesRequest request, CancellationToken cancellationToken)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}
