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
    public class CreateShopCommand : IRequest<BaseResultWithData<Keys>>
    {
        public string Name { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class CreateShopCommandHandle : 
        IRequestHandler<CreateShopCommand, BaseResultWithData<Keys>>
    {
        private readonly ISQLService<Domain.Entities.Shop> _sqlService;
        private IAuthService<Domain.Entities.Shop, Keys> authService;
        public CreateShopCommandHandle(ISQLService<Domain.Entities.Shop> sqlService, 
            IAuthService<Domain.Entities.Shop, Keys> service)
        {
            _sqlService = sqlService;
            authService = service;
        }

        public async Task<BaseResultWithData<Keys>> Handle(CreateShopCommand request, 
            CancellationToken cancellationToken)
        {
            var result = new BaseResultWithData<Keys>();
            // Check mail
            var _cursor = await _sqlService.GetAll();
            Domain.Entities.Shop? check = _cursor.FirstOrDefault(x => x.Email == request.Email);
            if (check != null)
            {
                result.StatusCode = (int)TaskStatus.Faulted;
                result.Message = "This email already sign-in";
                result.Data = null;
            } else
            {
                check = new Domain.Entities.Shop()
                {
                    Name = request.Name, Email = request.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
                };
                Keys outRes = await authService.SignUpUser(check);
                result.StatusCode = 201;
                result.Message = "Sing-up process successful";
                result.Data = outRes;
            }
            return result;
        }
    }
}
