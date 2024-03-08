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
    public class GetAllShop : IRequest<IEnumerable<ShopExample>>
    {
    }

    public class GetAllSHopHandle : IRequestHandler<GetAllShop, IEnumerable<ShopExample>>
    {
        private readonly ISQLService<ShopExample> service;

        public GetAllSHopHandle(ISQLService<ShopExample> service)
        {
            this.service = service;
        }

        public async Task<IEnumerable<ShopExample>> Handle(GetAllShop request, CancellationToken cancellationToken)
        {
            var _cursor = await service.GetAll();
            var listResult = _cursor.ToList();
            return listResult == null ? throw new AggregateException("Empty return object list") 
                : (IEnumerable<ShopExample>)listResult.AsReadOnly();
        }
    }
}
