using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq.Expressions;
using myExpression;

namespace Calculator
{
    public partial class myCalculator : Form
    {

        public myCalculator()
        {
            InitializeComponent();
        }

        #region 定义变量
        Regex RegNum = new Regex(@"^[0-9\.]*$");    //数字的正则表达式
        Regex RegBegin = new Regex(@"^[0-9\-(]");   //第一个字符的正则表达式
        Regex RegMark = new Regex(@"[-+*/^\()=.]");     //运算符的正则表达式
        Regex RegAll = new Regex(@"[0-9\-(\-+*/^\()=.SCT]");//所有运算符、数字的正则表达式

        bool Start = true;      //判断是否为第一个字符
        bool IsMark = true;     //判断是否可以输入数字，为真，允许输入数字
        bool IsRight = false;   //判断最后一个字符是否为右括号
        bool IsDot = false;     //判断是否可以输入小数点
        int BeforeLen = 0;       //判断输入运算符之前的字符串长度
        List<bool> LeftList = new List<bool>(); //存储左括号个数
        string RealStr = string.Empty;
        string FinalResult = string.Empty;
        #endregion

        #region 数字键处理
        private void PressNumBtn(string NumTxt)     
        {
            if (IsRight == false)//数字键输入之前紧前不能有右括号
            {
                if (!this.InputText.Text.Contains("="))
                {
                    this.InputText.Text += NumTxt;
                    IsMark = false;//输入数字后，则最后一个字符不是运算符
                    //IsDot = false;  
                }
                else
                {
                    this.InputText.Text = string.Empty;
                    this.InputText.Text = this.FinalResult + NumTxt;
                    IsMark = false;
                }
            }
            this.GetFocus();
        }
        #endregion

        #region 符号键处理
        private void PressMathBtn(string MarkTxt)       
        {
            if (MarkTxt != "." && IsMark == false)
            {
                if (FinalResult == string.Empty)
                {
                    this.InputText.Text += MarkTxt;
                }
                else
                {
                    if (this.InputText.Text.Contains("="))
                    {
                        this.InputText.Text = FinalResult;
                    }
                    else
                    {
                        this.InputText.Text += MarkTxt;
                    }
                }
                IsMark = true;//输入了运算符，最后一个字符就是运算符了
                IsDot = false;//输入了运算符，最后一个字符就不是小数点了
                IsRight = false;//输入了运算符，最后一个字符就不是右括号了
                Start = false;
                this.ZeroTwoChk(this.InputText.Text.Trim(), BeforeLen);//处理0
            }
            else
            {
                if (IsMark == false)
                {
                    if (FinalResult == string.Empty)
                    {
                        this.InputText.Text += MarkTxt;
                    }
                    else
                    {
                        if (this.InputText.Text.Contains("="))
                        {
                            this.InputText.Text = FinalResult;
                        }
                        else
                        {
                            this.InputText.Text += MarkTxt;
                        }
                    }
                    IsMark = true;//输入了运算符，最后一个字符就是运算符了
                    IsDot = false;//输入了运算符，最后一个字符就不是小数点了
                    IsRight = false;//输入了运算符，最后一个字符就不是右括号了
                    Start = false;
                }
            }
            this.GetFocus();
        }
        #endregion

        #region 等于号处理
        private void Equal()        
        {
            try
            {
                if (this.InputText.Text.Contains("/0"))   //除数不能为0
                {
                    if (this.InputText.Text.Contains("/0.")) { }     //去掉除号后为小数的情况
                    else
                    {
                        this.OutputText.Text = "除数不能为零,请重新输入!";
                        this.InputText.Text = "";
                    }
                }
                else if (this.InputText.Text.Contains("="))
                {
                    this.OutputText.Text = FinalResult;
                }
                else if (IsMark == false)
                {
                    RealStr = this.InputText.Text.Trim() + "=";
                    this.ZeroTwoChk(RealStr, BeforeLen);
                    RealStr = this.InputText.Text.Trim();//移除前面和后面空白字符
                    if (RealStr.StartsWith("-"))        //开始为负号时
                    {
                        RealStr = "0" + RealStr;//前面加个0
                    }
                    this.Calculate();
                    IsRight = false;
                }
                this.GetFocus();
            }
            catch
            {
                this.OutputText.Text = "Error!";
            }
        }
        #endregion

        #region 退格键处理
        private void Del()     
        {
            string Txt = this.InputText.Text;
            if (Txt.Length >= 1)
            {
                string L = Txt.Substring(Txt.Length - 1, 1);    //取前一个字符
                if (L == "(")
                {
                    this.LeftList.Remove(true);     //左括号列表的记录删除一个
                    this.InputText.Text = Txt.Remove(Txt.Length - 1, 1);
                }
                else if (L == ")")
                {
                    this.LeftList.Add(true);    //左括号列表的记录增加一个
                    this.InputText.Text = Txt.Remove(Txt.Length - 1, 1);
                    IsRight = false;
                }
                else
                    this.InputText.Text = Txt.Remove(Txt.Length - 1, 1); //删除最后一个字符
                IsMark = false;
            }
        }
        #endregion

        #region 点击数字键
        private void btn1_Click(object sender, EventArgs e)
        {
            PressNumBtn(btn1.Text);
            this.OutputText.Text = "";
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            PressNumBtn(btn2.Text);
            this.OutputText.Text = "";
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            PressNumBtn(btn3.Text);
            this.OutputText.Text = "";
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            PressNumBtn(btn4.Text);
            this.OutputText.Text = "";
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            PressNumBtn(btn5.Text);
            this.OutputText.Text = "";
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            PressNumBtn(btn6.Text);
            this.OutputText.Text = "";
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            PressNumBtn(btn7.Text);
            this.OutputText.Text = "";
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            PressNumBtn(btn8.Text);
            this.OutputText.Text = "";
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            PressNumBtn(btn9.Text);
            this.OutputText.Text = "";
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            PressNumBtn(btn0.Text);
            this.OutputText.Text = "";
        }
        #endregion

        #region 点击运算符键
        private void btnAc_Click(object sender, EventArgs e)     //清空
        {
            this.InputText.Text = string.Empty.Trim();
            this.OutputText.Text = string.Empty.Trim();
            Start = true;
            IsMark = true;
            IsRight = false;
            IsDot = false;
            BeforeLen = 0;
            LeftList.Clear();
            RealStr = string.Empty;
            FinalResult = string.Empty;

        }
        private void btnDot_Click(object sender, EventArgs e)       //点
        {
            string Txt = this.InputText.Text;
            if (IsDot == false &&  Txt != "")
            {
                this.PressMathBtn(btnDot.Text);
                IsDot = true;
            }
            else
            {
                this.OutputText.Text = "非法输入!";
            }
            this.GetFocus();
        }
       
        private void btnDel_Click(object sender, EventArgs e)   //退格
        {
            Del();
        }
        private void btnDs_Click(object sender, EventArgs e)   //倒数
        {
            try
            {
                if (this.InputText.Text != "0")
                {
                    double a = Convert.ToDouble(this.InputText.Text);
                    this.InputText.Text = (1 / a).ToString();
                }
                else
                {
                    this.OutputText.Text = "非法输入!";
                }
            }
            catch
            {
                this.OutputText.Text = "Error!";
            }
        }
        private void btnSqrt_Click(object sender, EventArgs e)      // 开方 
        {
            try
            { 
                double a = Convert.ToDouble(this.InputText.Text);
                if (a >= 0)
                    this.InputText.Text = (Math.Sqrt(a)).ToString();
                else
                    this.OutputText.Text = "输入的数字应为非负数!";
            }
            catch
            {
                this.OutputText.Text = "非法输入!";
            }

        }
        private void btnJc_Click(object sender, EventArgs e)     //   阶乘  
        {
            try
            {
                if (this.InputText.Text.Contains("."))
                {
                    this.OutputText.Text = "格式错误!";
                }
                else if (this.InputText.Text.Contains("-"))
                {
                    this.OutputText.Text = "格式错误!";
                }
                else
                {
                    int a = int.Parse(this.InputText.Text);
                    long result = 1;
                    for (int i = 1; i <= a; i++)
                    {
                        result = result * i;
                    }
                    this.InputText.Text = result.ToString();
                }
            }
            catch
            {
                this.OutputText.Text = "Error!";
            }

        }
        private void btnPow2_Click(object sender, EventArgs e)//平方
        {
            PressMathBtn("^");
            PressNumBtn("2");

        }
        private void btnPow3_Click(object sender, EventArgs e)//立方
        {
            PressMathBtn("^");
            PressNumBtn("3");
        }
        private void btnPow_Click(object sender, EventArgs e)//   乘方
        {
            PressMathBtn(btnPow.Text);
        }
        private void btnLeft_Click(object sender, EventArgs e)      //左括号
        {
            string Txt = this.InputText.Text;
            if (Txt.Trim() == string.Empty || (!RegNum.IsMatch(Txt.Substring(Txt.Length - 1, 1)) && Txt.Substring(Txt.Length - 1, 1) != ")" && IsDot == false) )
            {
                this.InputText.Text = this.InputText.Text + "(";
                IsMark = true;//输入了左括号，最后一个字符就是运算符了
                this.LeftList.Add(true);
            }
            this.GetFocus();
        }
        private void btnRight_Click(object sender, EventArgs e)     //右括号
        {
            try
            {
                string Txt = this.InputText.Text;
                if (LeftList.Contains(true) && (RegNum.IsMatch(Txt.Substring(Txt.Length - 1, 1)) || Txt.Substring(Txt.Length - 1, 1) == ")"))
                {
                    this.PressMathBtn(")");
                    this.LeftList.Remove(true);//配对去掉一个左括号
                    IsMark = false;//输入了右括号，最后一个字符不是运算符了
                    IsRight = true;//输入了右括号，最后一个字符是右括号
                }
                this.GetFocus();
            }
            catch
            {
                this.OutputText.Text = "Error!";
            }
        }
        private void btnEqual_Click(object sender, EventArgs e)//  等于号
        {
            this.Equal();
        }
        private void btnDiv_Click(object sender, EventArgs e)//除
        {
            PressMathBtn(btnDiv.Text);
        }
        private void btnMul_Click(object sender, EventArgs e)//乘
        {
            PressMathBtn(btnMul.Text);
        }
        private void btnSub_Click(object sender, EventArgs e)//减
        {
            PressMathBtn(btnSub.Text);
        }
        private void btnPlus_Click(object sender, EventArgs e)//加
        {
            PressMathBtn(btnPlus.Text);
        }
        private void btnFu_Click(object sender, EventArgs e)//负数
        {
            this.InputText.Text += "-";
        }
        private void btnMod_Click(object sender, EventArgs e)//取模
        {
            PressMathBtn(btnMod.Text);
        }
        #endregion

        #region 输入合法性检验
        private string CheckLegal(string StrInput, bool IsStart, List<bool> List)
        {
            if (RegAll.IsMatch(StrInput))
            {
                if (IsStart == true)
                {
                    if (RegBegin.IsMatch(StrInput))
                    {
                        Start = false;
                        return "GoOn";      //输入的第一个字符合格，继续
                    }
                    else
                    {
                        return "RollBack";      //输入的第一个字符不合格则，输入无效
                    }

                }
                else
                {
                    if (StrInput.Contains("="))
                    {
                        if (List.Contains(true))
                        {
                            return "LeftProblem";   //括号没有完全，则返回“左括号问题”，应用于等号按键的按下
                        }
                        else
                        {
                            return "Over";      //算式合符规范，返回计算结束
                        }
                    }
                    else
                    {
                        return "GoOn";      //不是等号，则继续
                    }
                }
            }
            else
            {
                return "RollBack";      //其他情况，输入无效
            }
        }
        #endregion

        #region 计算
        private void Calculate()
        {
            if (this.CheckLegal(RealStr, Start, LeftList) == "Over")
            {
                myExp Exp = new myExp();
                FinalResult = Exp.GetExpression(RealStr.Replace("=", ""));//获得最终结果
                if (FinalResult == "Error")
                {
                    FinalResult = "Error";
                }
                else
                {
                    this.InputText.Text = FinalResult;           //显示最后结果
                    if (FinalResult.Contains("."))
                    {
                        IsDot = true;
                    }
                    if (RealStr.StartsWith("0-"))
                    {
                        this.InputText.Text = this.InputText.Text.TrimStart('0');
                    }
                    this.InputText.Refresh();
                }
            }
            else if (this.CheckLegal(RealStr, Start, LeftList) == "RollBack")
            {
                FinalResult = "Error";
            }
            else if (this.CheckLegal(RealStr, Start, LeftList) == "LeftProblem")
            {
                FinalResult = "Error";
            }
        }
        #endregion

        #region 处理0的问题
        private void ZeroTwoChk(string inPutTxt, int beforeLen)    //处理0的问题，如两个0和6.30等
        {
            try
            {
                string lastMark = inPutTxt.Trim().Substring(inPutTxt.Trim().Length - 1, 1);
                if (beforeLen != 0)
                {
                    string beforeStr = inPutTxt.Trim().Substring(0, beforeLen);
                    string nowStr = inPutTxt.Trim().Substring(beforeLen, inPutTxt.Length - beforeLen - 1);
                    if (nowStr.Contains("."))
                    {
                        nowStr = nowStr.Trim('0');
                        if (nowStr.StartsWith(".") && !nowStr.EndsWith("."))
                        {
                            nowStr = "0" + nowStr;
                        }
                        if (nowStr == ".")
                        {
                            nowStr = "0";
                        }
                        if (nowStr.EndsWith("."))
                        {
                            nowStr = nowStr.TrimEnd('.');
                        }
                    }
                    else
                    {
                        if (nowStr != string.Empty)
                        {
                            nowStr = nowStr.TrimStart('0');
                            if (nowStr == string.Empty)
                            {
                                nowStr = "0";
                            }
                        }
                    }
                    this.InputText.Text = beforeStr + nowStr + lastMark;
                }
                else
                {
                    inPutTxt = inPutTxt.Trim().Substring(0, inPutTxt.Length - 1);
                    if (inPutTxt.Contains("."))
                    {
                        inPutTxt = inPutTxt.Trim('0');
                        if (inPutTxt.StartsWith(".") && !inPutTxt.EndsWith("."))
                        {
                            inPutTxt = "0" + inPutTxt;
                        }
                        if (inPutTxt == ".")
                        {
                            inPutTxt = "0";
                        }
                        if (inPutTxt.EndsWith("."))
                        {
                            inPutTxt = inPutTxt.TrimEnd('.');
                        }
                    }
                    else
                    {
                        inPutTxt = inPutTxt.TrimStart('0');
                        if (inPutTxt == string.Empty)
                        {
                            inPutTxt = "0";
                        }
                    }
                    this.InputText.Text = inPutTxt + lastMark;
                }
            }
            catch { }
            BeforeLen = this.InputText.Text.Trim().Length;
        }
        #endregion

        #region 将焦点定位在最后一个字符之后
        private void GetFocus()         
        {
            if (this.InputText.ContainsFocus == false)
            {
                this.InputText.Focus();
                this.InputText.ScrollToCaret();
                this.InputText.SelectionStart = this.InputText.TextLength;
            }
        }
        #endregion


    }
}
