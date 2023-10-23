// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     ApplicationDbContext.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : MPauloskyBlogApp
// Project Name :  Blog
// =============================================

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Blog.Data;

public class ApplicationDbContext
	(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
}
