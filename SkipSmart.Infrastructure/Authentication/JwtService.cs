﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Users.Shared;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Users;

namespace SkipSmart.Infrastructure.Authentication;

internal sealed class JwtService : IJwtService {
    private readonly string _secret;
    private readonly int _tokenLifeTime;
    private readonly AuthenticationOptions _authenticationOptions;
    
    public JwtService(IOptions<AuthenticationOptions> authenticationOptions) {
        _secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new ApplicationException("Jwt secret is missing.");
        _tokenLifeTime = int.Parse(Environment.GetEnvironmentVariable("JWT_TOKEN_LIFETIME") ?? throw new ApplicationException("Jwt token lifetime is missing."));
        _authenticationOptions = authenticationOptions.Value;
    }
    
    public Result<AccessTokenResponse> CreateToken(User user) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secret);

        var claims = new List<Claim> {
            new("user_id", user.Id.ToString()),
            new("user_email", user.Email.Value),
            new("email_verified", user.IsEmailVerified.ToString().ToLower()),
            new("group_id", user.GroupId.ToString())
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_tokenLifeTime),
            Issuer = _authenticationOptions.Issuer,
            Audience = _authenticationOptions.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var expirationDate = DateOnly.FromDateTime(tokenDescriptor.Expires.Value);
        
        var token = tokenHandler.CreateToken(tokenDescriptor);

        var jwt = tokenHandler.WriteToken(token);

        return new AccessTokenResponse(jwt, expirationDate);
    }
}