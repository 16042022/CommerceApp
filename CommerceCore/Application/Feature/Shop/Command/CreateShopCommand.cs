using CommerceCore.Application.Base;
using CommerceCore.Application.Interface;
using CommerceCore.Application.Models;
using CommerceCore.Domain.Entities;
using CommerceCore.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CommerceCore.Application.Feature.Shop.Command
{
    public class CreateShopCommand : IRequest<BaseResultWithData<KeyTokenStore>>
    {
        public string Name { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class CreateShopCommandHandle : 
        IRequestHandler<CreateShopCommand, BaseResultWithData<KeyTokenStore>>
    {
        private readonly ISQLService<ShopExample> _sqlService;
        private IAuthService<ShopExample, KeyTokenStore> authService;
        public CreateShopCommandHandle(ISQLService<ShopExample> sqlService, 
            IAuthService<ShopExample, KeyTokenStore> service)
        {
            _sqlService = sqlService;
            authService = service;
        }

        public async Task<BaseResultWithData<KeyTokenStore>> Handle(CreateShopCommand request, 
            CancellationToken cancellationToken)
        {
            var result = new BaseResultWithData<KeyTokenStore>();
            // Check mail
            var _cursor = await _sqlService.GetAll();
            ShopExample? check = _cursor.FirstOrDefault(x => x.Email == request.Email);
            if (check != null)
            {
                result.StatusCode = (int)TaskStatus.Faulted;
                result.Message = "This email already sign-in";
                result.Data = null;
            } else
            {
                check = new ShopExample()
                {
                    Name = request.Name, Email = request.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
                };
                KeyTokenStore outRes = await authService.SignUpUser(check);
                result.StatusCode = 201;
                result.Message = "Sing-up process successful";
                result.Data = outRes;
            }
            return result;
        }
    }
}
