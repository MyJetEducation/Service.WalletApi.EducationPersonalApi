﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using Service.Core.Client.Constants;
using Service.Core.Client.Education;
using Service.Core.Client.Models;
using Service.Core.Client.Services;
using Service.EducationPersonalApi.Mappers;
using Service.EducationPersonalApi.Models;
using Service.TutorialPersonal.Grpc;
using Service.TutorialPersonal.Grpc.Models;
using Service.UserInfo.Crud.Grpc;
using Service.UserReward.Grpc;
using Service.UserReward.Grpc.Models;

namespace Service.EducationPersonalApi.Controllers
{
	[Route("/api/v1/education/personal")]
	public class EducationController : BaseController
	{
		private readonly ITutorialPersonalService _tutorialService;
		private readonly IUserRewardService _userRewardService;

		public EducationController(ITutorialPersonalService tutorialService,
			IUserInfoService userInfoService,
			IUserRewardService userRewardService,
			IEncoderDecoder encoderDecoder, ISystemClock systemClock,
			ILogger<EducationController> logger) : base(systemClock, encoderDecoder, userInfoService, logger)
		{
			_userRewardService = userRewardService;
			_tutorialService = tutorialService;
		}

		[HttpPost("started")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TutorialStateResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> LearningStartedAsync()
		{
			Guid? userId = await GetUserIdAsync();
			if (userId == null)
				return StatusResponse.Error(ResponseCode.UserNotFound);

			CommonGrpcResponse response = await _userRewardService.LearningStartedAsync(new LearningStartedGrpcRequest
			{
				UserId = userId,
				Tutorial = EducationTutorial.PersonalFinance,
				Unit = 1,
				Task = 1
			});

			return StatusResponse.Result(response.IsSuccess);
		}

		[HttpPost("dashboard")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TutorialStateResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> GetDashboardStateAsync() =>
			await Process(userId => _tutorialService.GetDashboardStateAsync(new PersonalSelectTaskUnitGrpcRequest {UserId = userId}), grpc => grpc.ToModel());

		[HttpPost("state")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<FinishUnitResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> GetFinishStateAsync([FromBody] GetFinishStateRequest request)
		{
			if (EducationHelper.GetUnit(EducationTutorial.PersonalFinance, request.Unit) == null)
				return StatusResponse.Error(ResponseCode.NotValidEducationRequestData);

			return await Process(userId => _tutorialService.GetFinishStateAsync(new GetFinishStateGrpcRequest {UserId = userId, Unit = request.Unit}), grpc => grpc.ToModel());
		}

		#region Unit1 (Your income)

		[HttpPost("unit1/text")]
		[OpenApiTag("Unit1")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit1TextAsync([FromBody] TaskTextRequest request) =>
			await ProcessTask(1, 1, request, (userId, timespan) => _tutorialService.Unit1TextAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit1/test")]
		[OpenApiTag("Unit1")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit1TestAsync([FromBody] TaskTestRequest request) =>
			await ProcessTask(1, 2, request, (userId, timespan) => _tutorialService.Unit1TestAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit1/video")]
		[OpenApiTag("Unit1")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit1VideoAsync([FromBody] TaskVideoRequest request) =>
			await ProcessTask(1, 3, request, (userId, timespan) => _tutorialService.Unit1VideoAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit1/case")]
		[OpenApiTag("Unit1")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit1CaseAsync([FromBody] TaskCaseRequest request) =>
			await ProcessTask(1, 4, request, (userId, timespan) => _tutorialService.Unit1CaseAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit1/truefalse")]
		[OpenApiTag("Unit1")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit1TrueFalseAsync([FromBody] TaskTrueFalseRequest request) =>
			await ProcessTask(1, 5, request, (userId, timespan) => _tutorialService.Unit1TrueFalseAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit1/game")]
		[OpenApiTag("Unit1")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit1GameAsync([FromBody] TaskGameRequest request) =>
			await ProcessTask(1, 6, request, (userId, timespan) => _tutorialService.Unit1GameAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		#endregion

		#region Unit2 (Spending money secrets. Instruction. Exercise)

		[HttpPost("unit2/text")]
		[OpenApiTag("Unit2")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit2TextAsync([FromBody] TaskTextRequest request) =>
			await ProcessTask(2, 1, request, (userId, timespan) => _tutorialService.Unit2TextAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit2/test")]
		[OpenApiTag("Unit2")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit2TestAsync([FromBody] TaskTestRequest request) =>
			await ProcessTask(2, 2, request, (userId, timespan) => _tutorialService.Unit2TestAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit2/video")]
		[OpenApiTag("Unit2")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit2VideoAsync([FromBody] TaskVideoRequest request) =>
			await ProcessTask(2, 3, request, (userId, timespan) => _tutorialService.Unit2VideoAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit2/case")]
		[OpenApiTag("Unit2")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit2CaseAsync([FromBody] TaskCaseRequest request) =>
			await ProcessTask(2, 4, request, (userId, timespan) => _tutorialService.Unit2CaseAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit2/truefalse")]
		[OpenApiTag("Unit2")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit2TrueFalseAsync([FromBody] TaskTrueFalseRequest request) =>
			await ProcessTask(2, 5, request, (userId, timespan) => _tutorialService.Unit2TrueFalseAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit2/game")]
		[OpenApiTag("Unit2")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit2GameAsync([FromBody] TaskGameRequest request) =>
			await ProcessTask(2, 6, request, (userId, timespan) => _tutorialService.Unit2GameAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		#endregion

		#region Unit3 (Hidden expenses and lost profits. Instruction. Exercise)

		[HttpPost("unit3/text")]
		[OpenApiTag("Unit3")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit3TextAsync([FromBody] TaskTextRequest request) =>
			await ProcessTask(3, 1, request, (userId, timespan) => _tutorialService.Unit3TextAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit3/test")]
		[OpenApiTag("Unit3")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit3TestAsync([FromBody] TaskTestRequest request) =>
			await ProcessTask(3, 2, request, (userId, timespan) => _tutorialService.Unit3TestAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit3/video")]
		[OpenApiTag("Unit3")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit3VideoAsync([FromBody] TaskVideoRequest request) =>
			await ProcessTask(3, 3, request, (userId, timespan) => _tutorialService.Unit3VideoAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit3/case")]
		[OpenApiTag("Unit3")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit3CaseAsync([FromBody] TaskCaseRequest request) =>
			await ProcessTask(3, 4, request, (userId, timespan) => _tutorialService.Unit3CaseAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit3/truefalse")]
		[OpenApiTag("Unit3")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit3TrueFalseAsync([FromBody] TaskTrueFalseRequest request) =>
			await ProcessTask(3, 5, request, (userId, timespan) => _tutorialService.Unit3TrueFalseAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit3/game")]
		[OpenApiTag("Unit3")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit3GameAsync([FromBody] TaskGameRequest request) =>
			await ProcessTask(3, 6, request, (userId, timespan) => _tutorialService.Unit3GameAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		#endregion

		#region Unit4 (Salary - make sure that it is enough. Instruction. Exercise)

		[HttpPost("unit4/text")]
		[OpenApiTag("Unit4")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit4TextAsync([FromBody] TaskTextRequest request) =>
			await ProcessTask(4, 1, request, (userId, timespan) => _tutorialService.Unit4TextAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit4/test")]
		[OpenApiTag("Unit4")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit4TestAsync([FromBody] TaskTestRequest request) =>
			await ProcessTask(4, 2, request, (userId, timespan) => _tutorialService.Unit4TestAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit4/video")]
		[OpenApiTag("Unit4")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit4VideoAsync([FromBody] TaskVideoRequest request) =>
			await ProcessTask(4, 3, request, (userId, timespan) => _tutorialService.Unit4VideoAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit4/case")]
		[OpenApiTag("Unit4")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit4CaseAsync([FromBody] TaskCaseRequest request) =>
			await ProcessTask(4, 4, request, (userId, timespan) => _tutorialService.Unit4CaseAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit4/truefalse")]
		[OpenApiTag("Unit4")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit4TrueFalseAsync([FromBody] TaskTrueFalseRequest request) =>
			await ProcessTask(4, 5, request, (userId, timespan) => _tutorialService.Unit4TrueFalseAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit4/game")]
		[OpenApiTag("Unit4")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit4GameAsync([FromBody] TaskGameRequest request) =>
			await ProcessTask(4, 6, request, (userId, timespan) => _tutorialService.Unit4GameAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		#endregion

		#region Unit5 (Budget planning in three steps. Modern tools. Instruction. Exercise)

		[HttpPost("unit5/text")]
		[OpenApiTag("Unit5")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit5TextAsync([FromBody] TaskTextRequest request) =>
			await ProcessTask(5, 1, request, (userId, timespan) => _tutorialService.Unit5TextAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit5/test")]
		[OpenApiTag("Unit5")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit5TestAsync([FromBody] TaskTestRequest request) =>
			await ProcessTask(5, 2, request, (userId, timespan) => _tutorialService.Unit5TestAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit5/video")]
		[OpenApiTag("Unit5")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit5VideoAsync([FromBody] TaskVideoRequest request) =>
			await ProcessTask(5, 3, request, (userId, timespan) => _tutorialService.Unit5VideoAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit5/case")]
		[OpenApiTag("Unit5")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit5CaseAsync([FromBody] TaskCaseRequest request) =>
			await ProcessTask(5, 4, request, (userId, timespan) => _tutorialService.Unit5CaseAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit5/truefalse")]
		[OpenApiTag("Unit5")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit5TrueFalseAsync([FromBody] TaskTrueFalseRequest request) =>
			await ProcessTask(5, 5, request, (userId, timespan) => _tutorialService.Unit5TrueFalseAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		[HttpPost("unit5/game")]
		[OpenApiTag("Unit5")]
		[SwaggerResponse(HttpStatusCode.OK, typeof (DataResponse<TestScoreResponse>), Description = "Ok")]
		public async ValueTask<IActionResult> Unit5GameAsync([FromBody] TaskGameRequest request) =>
			await ProcessTask(5, 6, request, (userId, timespan) => _tutorialService.Unit5GameAsync(request.ToGrpcModel(userId, timespan)), grpc => grpc.ToModel());

		#endregion
	}
}