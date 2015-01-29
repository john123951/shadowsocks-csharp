using System;

namespace Sweet.LoveWinne.Model
{
	public class AnswerQuestionItem
	{
		public long QuestionId { get; set; }

		public readonly int QuestionType = 1;

		public string AnswerContent { get; set; }
	}
}

