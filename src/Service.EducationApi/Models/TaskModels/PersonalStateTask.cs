﻿namespace Service.EducationApi.Models.TaskModels
{
	public class PersonalStateTask
	{
		public int Task { get; set; }

		public int TestScore { get; set; }

		public bool CanRetry { get; set; }
	}
}