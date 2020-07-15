using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using Learning.Api.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services.OperationCodes;
using Services.ResultObject;

namespace Learning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string _appId;
        private IConfiguration config;
        private readonly string _key;
        private readonly int _expireTokenDays;
        public AuthController(IConfiguration configuration)
        {
            this.config = configuration;
            _appId = config.GetSection("AppId").Value;
            _key = config.GetSection("Jwt:key").Value;
            _expireTokenDays = int.Parse(config.GetSection("Jwt:ExpireAfterDays").Value);
        }
        [HttpGet("{appid}")]
        public IActionResult VerifyApiClient(string appId)
        {
            if(_appId !=appId)
            {
                return Unauthorized();
            }
            var key = Encoding.ASCII.GetBytes(_key);
          var token =  TokenGenerator.GenerateToken(key,_expireTokenDays);
            var tokenResponse = new AuthTokenResponse
            {
                Token = token,
                TokenExpiresAfter = _expireTokenDays
            };

            return Ok(new SuccessOperationResult<AuthTokenResponse>
            {
                Result = tokenResponse,
                Code = ConstOperationCodes.SUCCESS_OPERATION,
            });
        }
    }
}