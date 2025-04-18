﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Server;

namespace WPLBlazor.AuthenticationStateSyncer
{
    public class PersistentAuthenticationStateProvider(PersistentComponentState persistentState) : AuthenticationStateProvider
    {
        private static readonly Task<AuthenticationState> _unauthenticatedTask =
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!persistentState.TryTakeFromJson<Auth0UserProfile>(nameof(UserInfo), out var userInfo) || userInfo is null)
            {
                return _unauthenticatedTask;
            }

            Claim[] claims = [
                new Claim(ClaimTypes.NameIdentifier, ClaimTypes.Email , userInfo.UserId),
                new Claim(ClaimTypes.Name, userInfo.Name ?? string.Empty),
                new Claim(ClaimTypes.Email, userInfo.Email ?? string.Empty)];


            return Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,
                    authenticationType: nameof(PersistentAuthenticationStateProvider)))));
        }
    }

}
