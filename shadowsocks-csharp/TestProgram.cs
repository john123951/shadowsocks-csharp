using Shadowsocks.View;
using Sweet.LoveWinne.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Shadowsocks
{
    internal static class TestProgram
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main3()
        {
            var form = new ValidateForm(new GetQuestionListResponse
            {
                IsSuccess = true,
                QuestionTitle = "11122222222233333333333344444",
                QuestionList = new List<QuestionDto>()
                {
                    new QuestionDto() {Id = 1, QuestionContent = "11111111111", QuestionType = 1},
                    new QuestionDto() {Id = 2, QuestionContent = "222222222222", QuestionType = 1},
                    new QuestionDto() {Id = 3, QuestionContent = "3333333333333", QuestionType = 1},
                }
            });

            form.ShowDialog();

            var result = form.AnswerQuestionItems;

            Application.Run();
        }
    }
}