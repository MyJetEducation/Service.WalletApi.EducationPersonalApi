﻿using System.ComponentModel.DataAnnotations;
using System.Linq;
using Service.EducationApi.Constants;
using Service.EducationApi.Extensions;
using Service.EducationApi.Models;

namespace Service.EducationApi.Services
{
	public class LoginRequestValidator : ILoginRequestValidator
	{
		private const int PasswordMinLength = 8;
		private const int PasswordMaxLength = 31;

		public int? ValidateRequest(LoginRequest request)
		{
			int? loginValidationResult = ValidateLogin(request.UserName);
			if (loginValidationResult != null)
				return loginValidationResult.Value;

			int? passwordValidationResult = ValidatePassword(request.Password);
			return passwordValidationResult;
		}

		public int? ValidateLogin(string value)
		{
			if (value.IsNullOrWhiteSpace())
				return ResponseCode.NoRequestData;

			if (!new EmailAddressAttribute().IsValid(value))
				return LoginRequestValidationResponseCode.NotValidEmail;

			return null;
		}

		private static int? ValidatePassword(string value)
		{
			if (value.IsNullOrWhiteSpace())
				return ResponseCode.NoRequestData;

			if (value.Length < PasswordMinLength || value.Length > PasswordMaxLength)
				return LoginRequestValidationResponseCode.PasswordInvalidLength;

			if (!value.Any(char.IsDigit))
				return LoginRequestValidationResponseCode.PasswordContainsNoDigit;

			if (!value.Any(char.IsLetter))
				return LoginRequestValidationResponseCode.PasswordContainsNoLetter;

			return null;
		}

		public bool ValidateRequired(LoginRequest request) => request.UserName.IsNullOrWhiteSpace() || request.Password.IsNullOrWhiteSpace();
	}
}