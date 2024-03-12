using CommerceCore.Application.Interface;
using CommerceCore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceCore.Application.Feature.Shop.Query
{
    public class GetAllShop : IRequest<IEnumerable<Domain.Entities.Shop>>
    {
    }

    public class GetAllSHopHandle : IRequestHandler<GetAllShop, IEnumerable<Domain.Entities.Shop>>
    {
        private readonly ISQLService<Domain.Entities.Shop> service;

        public GetAllSHopHandle(ISQLService<Domain.Entities.Shop> service)
        {
            this.service = service;
        }

        public async Task<IEnumerable<Domain.Entities.Shop>> Handle(GetAllShop request, 
            CancellationToken cancellationToken)
        {
            var _cursor = await service.GetAll();
            var listResult = _cursor.ToList();
            return listResult == null ? throw new AggregateException("Empty return object list") 
                : (IEnumerable<Domain.Entities.Shop>)listResult.AsReadOnly();
        }
    }
}
