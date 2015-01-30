using System.Reflection.Emit;
using Sweet.LoveWinne.Model;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Shadowsocks.View
{
    public partial class ValidateForm : Form
    {
        private readonly GetQuestionListResponse _getQuestionListResponse;

        public List<AnswerQuestionItem> AnswerQuestionItems { get; protected set; }

        protected ValidateForm()
        {
            InitializeComponent();
        }

        public ValidateForm(GetQuestionListResponse listResponse)
            : this()
        {
            _getQuestionListResponse = listResponse;
        }

        protected void InitQuestion()
        {
            if (_getQuestionListResponse != null && _getQuestionListResponse.IsSuccess)
            {
                var list = _getQuestionListResponse.QuestionList;
                foreach (var item in list)
                {
                    var label = new Label();
                    label.Text = item.QuestionContent;
                    var textBox = new TextBox();

                    flowLayout.Controls.Add(label);

                    flowLayout.Controls.Add(textBox);
                }
            }
        }
    }
}