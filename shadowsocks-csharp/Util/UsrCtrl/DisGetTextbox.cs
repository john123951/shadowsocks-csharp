using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shadowsocks.Util.UsrCtrl
{
    public class DisGetTextBox : TextBox
    {
        protected override void WndProc(ref Message m)
        {
            //吃掉消息，禁止星号密码查看器
            if (m.Msg == (int)WinMsg.WM_GETTEXT)
            {
                //本窗口的消息还是接收
                if (m.HWnd == this.Handle)
                {
                    base.WndProc(ref m);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}
