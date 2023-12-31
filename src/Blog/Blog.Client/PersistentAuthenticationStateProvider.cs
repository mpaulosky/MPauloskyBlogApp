// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     PersistentAuthenticationStateProvider.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : MPauloskyBlogApp
// Project Name :  Blog.Client
// =============================================

using System.Security.Claims;
using Microsoft.AspNetCore.Components;

namespace Blog.Client;

public class PersistentAuthenticationStateProvider
	(PersistentComponentState persistentState) : AuthenticationStateProvider
{
	private static readonly Task<AuthenticationState> _unauthenticatedTask =
		Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

	public override Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		if (!persistentState.TryTakeFromJson<UserInfo>(nameof(UserInfo), out var userInfo) || userInfo is null)
		{
			return _unauthenticatedTask;
		}

		Claim[] claims =  [
			new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),

		new Claim(ClaimTypes.Name, userInfo.Email),
		new Claim(ClaimTypes.Email, userInfo.Email)];

		return Task.FromResult(
			new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,
				authenticationType: nameof(PersistentAuthenticationStateProvider)))));
	}
}
