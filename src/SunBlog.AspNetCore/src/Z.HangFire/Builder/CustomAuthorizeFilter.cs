﻿using Hangfire.Dashboard;
using System.Diagnostics.CodeAnalysis;

namespace Z.HangFire.Builder;

public class CustomAuthorizeFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context)
    {
        return true;
    }
    public CustomAuthorizeFilter()
    {
    }
}
