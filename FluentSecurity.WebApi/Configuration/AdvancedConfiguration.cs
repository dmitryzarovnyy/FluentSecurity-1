﻿using System.Linq;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.WebApi.Configuration
{
	public class AdvancedConfiguration : AdvancedConfigurationBase<WebApiSecurityRuntime>
	{
		internal AdvancedConfiguration(WebApiSecurityRuntime runtime) : base(runtime)
		{
			if (!Runtime.Conventions.Any())
			{
				Conventions(conventions =>
				{
					conventions.Add(new FindByPolicyNameConvention());
					conventions.Add(new FindDefaultPolicyViolationHandlerByNameConvention());
				});
			}
		}
	}
}