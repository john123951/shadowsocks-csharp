using Sweet.LoveWinne.Model;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Shadowsocks.View
{
    public partial class ValidateForm : Form
    {
        public bool IsValidated { get; private set; }

        private List<QuestionDto> _questionList;

        public ValidateForm(List<QuestionDto> questionList)
        {
            InitializeComponent();
            _questionList = questionList;
        }
    }
}