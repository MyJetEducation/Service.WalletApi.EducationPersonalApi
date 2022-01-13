﻿using Autofac;
using Microsoft.Extensions.Logging;
using Service.EducationApi.Services;
using Service.EducationRetry.Client;
using Service.KeyValue.Client;
using Service.PasswordRecovery.Client;
using Service.Registration.Client;
using Service.TutorialPersonal.Client;
using Service.UserInfo.Crud.Client;
using Service.UserInfo.Crud.Grpc;
using Service.UserProfile.Client;

namespace Service.EducationApi.Modules
{
	public class ServiceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterUserInfoCrudClient(Program.Settings.UserInfoCrudServiceUrl);
			builder.RegisterKeyValueClient(Program.Settings.KeyValueServiceUrl);
			builder.RegisterUserProfileClient(Program.Settings.UserProfileServiceUrl);
			builder.RegisterPasswordRecoveryClient(Program.Settings.PasswordRecoveryServiceUrl);
			builder.RegisterRegistrationClient(Program.Settings.RegistrationServiceUrl);
			builder.RegisterTutorialPersonalClient(Program.Settings.TutorialPersonalServiceUrl);
			builder.RegisterEducationRetryClient(Program.Settings.EducationRetryServiceUrl);

			builder.RegisterType<LoginRequestValidator>().AsImplementedInterfaces();

			builder.Register(context =>
				new TokenService(
					context.Resolve<IUserInfoService>(),
					Program.JwtSecret,
					Program.Settings.JwtTokenExpireMinutes,
					Program.Settings.RefreshTokenExpireMinutes,
					context.Resolve<ILogger<TokenService>>()))
				.As<ITokenService>()
				.SingleInstance();
		}
	}
}