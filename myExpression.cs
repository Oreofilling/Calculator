using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace myExpression
{
    public class myExp
    {

        #region 定义栈
        private Stack<string> OPTR = new Stack<string>();       //数字栈
        private Stack<string> OPND = new Stack<string>();       //符号栈 
        #endregion

        #region 判断是否为数字
        public bool IsNumber(string ch)
        {
            Regex RegInput = new Regex(@"^[0-9\.]");       //用正则表达式判断
            if (RegInput.IsMatch(ch.ToString()))
            {
                return true;
            }
            else
                return false;
        }
        #endregion

        #region 定义运算符优先级
        public static int priority(String Operator)
        {
            if (Operator == ("+") || Operator == ("-"))
            {
                return 1;
            }
            else if (Operator == ("*") || Operator == ("/") || Operator == ("%"))
            {
                return 2;
            }
            else if (Operator == ("^"))
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 比较运算符的优先级
        public int CompareOperate(string ch, string stackCh)
        {
            int intCh = priority(ch.ToString());
            int intSCh = priority(stackCh.ToString());
            if (intCh == intSCh)
            {
                return 0;
            }
            else if (intCh < intSCh)
            {
                return -1;
            }
            else if (intCh > intSCh)
            {
                return 1;
            }
            else
                return -2;
        }
        #endregion

        #region 调用getResult将最后结果显示
        public string GetExpression(string InputString)
        {
            bool Hasop = true;    //这个布尔变量用于判断数字之后是否有符号，作用是用来得到9以上数字
            InputString += "#";     //在字符串最后加上标记符#
            #region 入栈
            for (int i = 0; i < InputString.Length; i++)//遍历每一个字符
            {
                string ch = InputString[i].ToString();
                if (ch == "#")      //判断是否遍历到最后一个字符
                {
                    break;      //是则跳出循环
                }
                else
                {
                    string chNext = InputString[i + 1].ToString();//下一个字符
                    #region "数字的判别与处理"
                    if (IsNumber(ch) == true)  //如果是数字的话，要把数字放入数字栈中
                    {
                        if (Hasop == true)//如果数字之后有符号，意味着个位数,数字栈顶字符串不变更
                        {
                            OPTR.Push(ch);
                            Hasop = false;    //这个数字之后是什么还未知，所以先将其定义为数字
                        }
                        else
                        {
                            OPTR.Push(OPTR.Pop() + ch);//如果数字之后没有符号，则数字栈顶字符串加上新字符串,也就是多位数.
                        }
                    }
                    #endregion

                    #region "符号的判别与处理"
                    else if (ch == "(")
                    {
                        OPND.Push(ch);//左括号压入栈
                    }
                    else if (ch == ")")
                    {
                        while (OPND.Peek() != "(")      //当下一个字符不为左括号
                        {
                            string mark = OPND.Pop().ToString();
                            string op1 = OPTR.Pop().ToString();
                            string op2 = OPTR.Pop().ToString();//相当于计算括号里的内容
                            OPTR.Push(getResult(mark, op1, op2));//调用getResult方法，获取计算结果值，并推入数字栈
                        }
                        OPND.Pop();      //弹出左括号
                    }
                    else if (ch == "+" || ch == "-" || ch == "*" || ch == "/" || ch == "^" || ch == "%")
                    {
                        if (chNext == "-")      //处理负号
                        {
                            OPND.Push(ch);
                            OPTR.Push(chNext);
                            i += 1;
                            Hasop = false;    //输入了符号，Hasop为false
                        }
                        else
                        {
                            if (OPND.Count == 0)
                            {
                                OPND.Push(ch);
                            }
                            else if (OPND.Peek() == "(")
                            {
                                OPND.Push(ch);
                            }
                            else if (CompareOperate(ch, OPND.Peek()) == 1)
                            {
                                OPND.Push(ch);
                            }
                            else if (CompareOperate(ch, OPND.Peek()) == 0)//若优先级一样
                            {
                                string mark = OPND.Pop().ToString();
                                string op1 = OPTR.Pop().ToString();
                                string op2 = OPTR.Pop().ToString();
                                OPTR.Push(getResult(mark, op1, op2));//调用getResult方法，获取计算结果值，并推入数字栈
                                OPND.Push(ch);               //把符号推入符号栈
                            }
                            else if (CompareOperate(ch, OPND.Peek()) == -1)//若优先级较小
                            {
                                int com = -1;
                                while (com == -1 || com == 0)
                                {
                                    string mark = OPND.Pop().ToString();
                                    string op1 = OPTR.Pop().ToString();
                                    string op2 = OPTR.Pop().ToString();
                                    OPTR.Push(getResult(mark, op1, op2));//调用getResult方法，获取计算结果值，并推入数字栈
                                    if (OPND.Count != 0)
                                    {
                                        com = CompareOperate(ch, OPND.Peek());
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                OPND.Push(ch);//把符号推入符号栈
                            }
                            Hasop = true;//输入了运算符，HasMark为true
                        }
                    }
                }
                #endregion
            }
            #endregion
            #region "返回计算结果值"

            for (int i = 0; i < OPND.Count + 1; i++)      //循环得出结果
            {
                try
                {
                    string mark = OPND.Pop().ToString();
                    string op1 = OPTR.Pop().ToString();
                    string op2 = OPTR.Pop().ToString();
                    OPTR.Push(getResult(mark, op1, op2));//调用getResult方法，获取计算结果值，并推入数字栈
                }
                catch
                {
                    return "Error";
                }
            }
            return OPTR.Pop();  //返回最终结果

            #endregion
        }
        #endregion

        #region getResult返回运算结果
        public static string getResult(String Myoperator, String a, String b)
        {
            try
            {
                string op = Myoperator;
                string rs = string.Empty;
                decimal x = System.Convert.ToDecimal(b);
                decimal y = System.Convert.ToDecimal(a);
                decimal result = 0;
                switch (op)
                {
                    case "+": result = x + y; break;
                    case "-": result = x - y; break;
                    case "*": result = x * y; break;
                    case "/": result = x / y; break;
                    case "%": result = x % y; break;
                    case "^": result = Convert.ToDecimal(Math.Pow(Convert.ToDouble(x), Convert.ToDouble(y))); break;
                    default: result = 0; break;
                }
                return rs + result;
            }
            catch (IndexOutOfRangeException)//超出范围
            {
                return "Error!";
            }
        }
        #endregion
    }
}
