using shadowsocks_csharp.model.Response;
using System.Collections.Generic;

namespace Sweet.LoveWinne.Model
{
    public class GetQuestionListResponse : BaseResponse
    {
        /// <summary>
        /// Question title.
        /// </summary>
        /// <value>The question title.</value>
        public string QuestionTitle { get; set; }

        /// <summary>
        /// The validate question list.
        /// </summary>
        /// <value>The question list.</value>
        public List<QuestionDto> QuestionList { get; set; }
    }
}