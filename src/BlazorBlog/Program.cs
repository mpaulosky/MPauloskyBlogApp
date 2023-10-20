// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     Program.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : MPauloskyBlogApp
// Project Name :  BlazorBlog
// =============================================

using BlazorBlog.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
	new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAuthentication()
	.AddGoogle(options =>
	{
		IConfigurationSection googleAuthNSection = config.GetSection("Authentication:Google");
		options.ClientId = googleAuthNSection["ClientId"];
		options.ClientSecret = googleAuthNSection["ClientSecret"];
	})
	.AddMicrosoftAccount(microsoftOptions =>
	{
		microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"];
		microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
	});

await builder.Build().RunAsync();
