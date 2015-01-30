using Sweet.LoveWinne.Model;
using System.Collections.Generic;
using System.Drawing;
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
            InitQuestion();
        }

        protected void InitQuestion()
        {
            if (_getQuestionListResponse != null && _getQuestionListResponse.IsSuccess)
            {
                var list = _getQuestionListResponse.QuestionList;

                labelTitle.Text = _getQuestionListResponse.QuestionTitle;
                labelTitle.MinimumSize = new Size(labelTitle.Parent.Width - 1, labelTitle.Parent.Height - 1);

                foreach (var item in list)
                {
                    var textBox = new TextBox();
                    var label = new Label();
                    var childLayout = new Panel();

                    label.Text = item.QuestionContent;
                    textBox.Tag = item;
                    textBox.Dock = DockStyle.Right;
                    label.Dock = DockStyle.Fill;
                    childLayout.Dock = DockStyle.Top;
                    //childLayout.AutoSizeMode = AutoSizeMode.GrowOnly;

                    childLayout.Controls.Add(label);
                    childLayout.Controls.Add(textBox);

                    mainLayout.Controls.Add(childLayout);
                }
            }
        }

        private void btn_ok_Click(object sender, System.EventArgs e)
        {
            AnswerQuestionItems = GetAnswer(mainLayout.Controls);
        }

        private List<AnswerQuestionItem> GetAnswer(Control.ControlCollection parent)
        {
            var result = new List<AnswerQuestionItem>();

            if (parent == null)
            {
                return result;
            }
            foreach (Control control in parent)
            {
                var textBox = control as TextBox;

                if (textBox != null)
                {
                    var questionDto = textBox.Tag as QuestionDto;

                    if (questionDto != null)
                    {
                        result.Add(new AnswerQuestionItem()
                        {
                            QuestionId = questionDto.Id,
                            AnswerContent = textBox.Text.Trim()
                        });
                    }
                }

                var childAnswer = GetAnswer(control.Controls);

                result.AddRange(childAnswer);
            }

            return result;
        }
    }
}