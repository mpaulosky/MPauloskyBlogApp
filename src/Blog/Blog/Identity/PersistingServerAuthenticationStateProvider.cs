// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     PersistingServerAuthenticationStateProvider.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : MPauloskyBlogApp
// Project Name :  Blog
// =============================================

using System.Diagnostics;
using Blog.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;

namespace Blog.Identity;

public class PersistingServerAuthenticationStateProvider : ServerAuthenticationStateProvider, IDisposable
{
	private readonly PersistentComponentState _state;
	private readonly IdentityOptions _options;

	private readonly PersistingComponentStateSubscription _subscription;

	private Task<AuthenticationState>? _authenticationStateTask;

	public PersistingServerAuthenticationStateProvider(PersistentComponentState state, IOptions<IdentityOptions> options)
	{
		_state = state;
		_options = options.Value;

		AuthenticationStateChanged += OnAuthenticationStateChanged;
		_subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
	}

	private void OnAuthenticationStateChanged(Task<AuthenticationState> authenticationStateTask)
	{
		_authenticationStateTask = authenticationStateTask;
	}

	private async Task OnPersistingAsync()
	{
		if (_authenticationStateTask is null)
		{
			throw new UnreachableException(
				$"Authentication state not set in {nameof(RevalidatingServerAuthenticationStateProvider)}.{nameof(OnPersistingAsync)}().");
		}

		var authenticationState = await _authenticationStateTask;
		var principal = authenticationState.User;

		if (principal.Identity?.IsAuthenticated == true)
		{
			var userId = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;
			var email = principal.FindFirst(_options.ClaimsIdentity.EmailClaimType)?.Value;

			if (userId != null && email != null)
			{
				_state.PersistAsJson(nameof(UserInfo), new UserInfo
				{
					UserId = userId,
					Email = email,
				});
			}
		}
	}

	public void Dispose()
	{
		_subscription.Dispose();
		AuthenticationStateChanged -= OnAuthenticationStateChanged;
	}
}
