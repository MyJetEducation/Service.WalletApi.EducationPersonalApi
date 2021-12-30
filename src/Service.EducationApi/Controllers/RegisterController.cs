﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Core.Domain.Extensions;
using Service.Core.Grpc.Models;
using Service.EducationApi.Constants;
using Service.EducationApi.Models;
using Service.EducationApi.Services;
using Service.PasswordRecovery.Grpc;
using Service.PasswordRecovery.Grpc.Models;
using Service.Registration.Grpc;
using Service.Registration.Grpc.Models;
using Service.UserInfo.Crud.Grpc;

namespace Service.EducationApi.Controllers
{
	[Route("/api/register/v1")]
	public class RegisterController : BaseController
	{
		private readonly ILoginRequestValidator _loginRequestValidator;
		private readonly IPasswordRecoveryService _passwordRecoveryService;
		private readonly IRegistrationService _registrationService;

		public RegisterController(IUserInfoService userInfoService,
			ILoginRequestValidator loginRequestValidator,
			IPasswordRecoveryService passwordRecoveryService, IRegistrationService registrationService) : base(userInfoService)
		{
			_loginRequestValidator = loginRequestValidator;
			_passwordRecoveryService = passwordRecoveryService;
			_registrationService = registrationService;
		}

		[HttpPost("create")]
		public async ValueTask<IActionResult> RegisterAsync([FromBody] LoginRequest request)
		{
			int? validationResult = _loginRequestValidator.ValidateRequest(request);
			if (validationResult != null)
			{
				WaitFakeRequest();
				return StatusResponse.Error(validationResult.Value);
			}

			Guid? userId = await GetUserIdAsync(request.UserName);
			if (userId != null)
				return StatusResponse.Error(ResponseCode.UserAlreadyExists);

			CommonGrpcResponse response = await _registrationService.RegistrationAsync(new RegistrationGrpcRequest
			{
				UserName = request.UserName,
				Password = request.Password
			});

			return Result(response?.IsSuccess);
		}

		[HttpPost("confirm")]
		public async ValueTask<IActionResult> ConfirmRegisterAsync([FromBody, Required] string hash)
		{
			if (hash.IsNullOrWhiteSpace())
			{
				WaitFakeRequest();
				return StatusResponse.Error(ResponseCode.NoRequestData);
			}

			CommonGrpcResponse response = await _registrationService.ConfirmRegistrationAsync(new ConfirmRegistrationGrpcRequest {Hash = hash});

			return Result(response?.IsSuccess);
		}

		[HttpPost("recovery")]
		public async ValueTask<IActionResult> PasswordRecoveryAsync([FromBody, Required] string email)
		{
			if (email.IsNullOrWhiteSpace())
			{
				WaitFakeRequest();
				return StatusResponse.Error(ResponseCode.NoRequestData);
			}

			CommonGrpcResponse response = await _passwordRecoveryService.Recovery(new RecoveryPasswordGrpcRequest {Email = email});

			return Result(response?.IsSuccess);
		}

		[HttpPost("change")]
		public async ValueTask<IActionResult> ChangePasswordAsync([FromBody, Required] ChangePasswordRequest request)
		{
			string hash = request.Hash;
			if (hash.IsNullOrWhiteSpace())
			{
				WaitFakeRequest();
				return StatusResponse.Error(ResponseCode.NoRequestData);
			}

			string password = request.Password;
			int? validationResult = _loginRequestValidator.ValidatePassword(password);
			if (validationResult != null)
			{
				WaitFakeRequest();
				return StatusResponse.Error(validationResult.Value);
			}

			CommonGrpcResponse response = await _passwordRecoveryService.Change(new ChangePasswordGrpcRequest {Password = password, Hash = hash});

			return Result(response?.IsSuccess);
		}
	}
}