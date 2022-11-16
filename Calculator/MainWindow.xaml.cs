using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace Calculator
{
    public partial class MainWindow : Window
    {
        public string WorkString
        {
            get
            {
                return FormulaBlock.Text;
            }
            set
            {
                FormulaBlock.Text = value;
            }
        }
        public bool IsRad = true;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void LeftBracket_Click(object sender, RoutedEventArgs e)
        {
            WorkString+="(";
            CheckFormula();
        }
        private void RightBracket_Click(object sender, RoutedEventArgs e)
        {
            WorkString+= ")";
            CheckFormula();
        }
        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            WorkString+= "7";
            CheckFormula();
        }
        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            WorkString+= "8";
            CheckFormula();
        }
        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            WorkString+= "9";
            CheckFormula();
        }
        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            WorkString+= "4";
            CheckFormula();
        }
        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            WorkString+= "5";
            CheckFormula();
        }
        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            WorkString+= "6";
            CheckFormula();
        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            WorkString+= "1";
            CheckFormula();
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            WorkString+= "2";
            CheckFormula();
        }
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            WorkString+= "3";
            CheckFormula();
        }
        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            WorkString+= "0";
            CheckFormula();
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (WorkString.Length > 0)
            {
                 WorkString = WorkString.Substring(0,WorkString.Length-1);
            }
            CheckFormula();
        }
        void CheckFormula()
        {
            bool isFine = true;
            if(CountChars(WorkString,"(")!= CountChars(WorkString, ")"))
            {
                isFine = false;
            }
            if (WorkString.Length == 0)
            {
                isFine = false;
                ResultBlock.Text = "";
                return;
            }
            if (isFine)
            {
                try
                {
                    ResultBlock.Foreground = new SolidColorBrush(Colors.Black);
                    ResultBlock.Text = Calculate(WorkString).ToString();
                }catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    ResultBlock.Foreground = new SolidColorBrush(Colors.Red);
                    ResultBlock.Text = "ERROR";
                }
            }
            else
            {
                ResultBlock.Foreground = new SolidColorBrush(Colors.Red);

                ResultBlock.Text = "ERROR";
            }
            if (IsRad)
            {
                RadButton.Content = "Radian";
            }
            else
            {
                RadButton.Content = "Degree";
            }
        }
        int CountChars(string Word,string SubWord)
        {
            int v = 0;
            for(int i = 0; i < Word.Length; i++)
            {
                if (Word[i] == SubWord[0])
                {
                    string t=Word.Substring(i, Math.Min(SubWord.Length, Word.Length - i));
                    if (t == SubWord)
                    {
                        v++;
                    }
                }
            }
            return v;
        }
        private void FormulaBlock_TextInput(object sender, TextCompositionEventArgs e)
        {
            CheckFormula();
        }
        private void FormulaBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckFormula();
        }
        string CalculatePart(string Part)
        {
            int StartPoint = 0;
            List<string> Parts = new List<string>(10);
            bool WorkingPart = false;
            int BracketCount = 0;
            for(int i = 0; i < Part.Length; i++)
            {
                if (Part[i] == '(')
                {
                    if (!WorkingPart)
                    {
                        StartPoint = i+1;
                    }
                    BracketCount++;
                    WorkingPart = true;
                }
                if (Part[i] == ')')
                {
                    BracketCount--;
                    if (BracketCount == 0)
                    {
                        Parts.Add(CalculatePart(Part.Substring(StartPoint,i-StartPoint)));
                        WorkingPart = false;
                    }
                }
                if ((Part[i] == '0' || Part[i] == '1' || Part[i] == '2' || Part[i] == '3' || Part[i] == '4' || Part[i] == '5' || Part[i] == '6' || Part[i] == '7' || Part[i] == '8' || Part[i] == '9') && !WorkingPart)
                {
                    if (!WorkingPart)
                    {
                        StartPoint = i;

                        while ((Part[i] == '0' || Part[i] == '1' || Part[i] == '2' || Part[i] == '3' || Part[i] == '4' || Part[i] == '5' || Part[i] == '6' || Part[i] == '7' || Part[i] == '8' || Part[i] == '9' || Part[i] == '.' || Part[i] == ','))
                        {
                            i++;
                            if (i == Part.Length)
                            {
                                break;
                            }
                        }
                        Parts.Add(Part.Substring(StartPoint, i - StartPoint));
                    }
                }
                if (i == Part.Length)
                {
                    break;
                }
                if (!WorkingPart && (Part[i] == '/'|| Part[i] == '*'|| Part[i] == '+'|| Part[i] == '-'))
                {
                    Parts.Add(Part[i].ToString());
                }
                if (char.IsLetter(Part[i]) && !WorkingPart)
                {
                    StartPoint = i;
                    while (char.IsLetter(Part[i]))
                    {
                        i++;
                        if (i == Part.Length)
                        {
                            break;
                        }
                    }
                    Parts.Add(Part.Substring(StartPoint, i - StartPoint));
                    if (i >= Part.Length)
                    {
                        break;
                    }
                    i--;
                }
            }
            for (int i = 0; i < Parts.Count; i++)
            {
                if (Parts[i].Contains(')'))
                {
                    Parts[i] = CalculatePart(Parts[i]);
                }
            }
            int Length = Parts.Count;
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "PI")
                {
                    Parts[i] = Math.PI.ToString();
                    i = 0;
                    Length = Parts.Count;
                }
            }
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "EXP")
                {
                    if (i + 1 >= Parts.Count)
                    {
                        throw new Exception();
                    }
                    double v2 = double.Parse(Parts[i + 1]);
                    Parts[i] = ((Math.Exp(v2))).ToString();
                    Parts.RemoveAt(i + 1);
                    i = 0;
                    Length = Parts.Count;
                }
            }
            Length = Parts.Count;
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "SIN")
                {
                    if (i + 1 >= Parts.Count)
                    {
                        throw new Exception();
                    }
                    double v2 = double.Parse(Parts[i + 1]);
                    double RadFix = 1;
                    if (!IsRad)
                    {
                        RadFix = 57.2958f;
                    }
                    Parts[i] = ((Math.Sin(v2/RadFix))).ToString();
                    Parts.RemoveAt(i + 1);
                    i = 0;
                    Length = Parts.Count;
                }
            }
             Length = Parts.Count;
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "COS")
                {
                    if (i + 1 >= Parts.Count)
                    {
                        throw new Exception();
                    }
                    double v2 = double.Parse(Parts[i + 1]);
                    double RadFix = 1;
                    if (!IsRad)
                    {
                        RadFix = 57.2958f;
                    }
                    Parts[i] = ((Math.Cos(v2 / RadFix))).ToString();
                    Parts.RemoveAt(i + 1);
                    i = 0;
                    Length = Parts.Count;
                }
            }
             Length = Parts.Count;
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "TG")
                {
                    if (i + 1 >= Parts.Count)
                    {
                        throw new Exception();
                    }
                    double v2 = double.Parse(Parts[i + 1]);
                    double RadFix = 1;
                    if (!IsRad)
                    {
                        RadFix = 57.2958f;
                    }
                    Parts[i] = ((Math.Tan(v2 / RadFix))).ToString();
                    Parts.RemoveAt(i + 1);
                    i = 0;
                    Length = Parts.Count;
                }
            }
             Length = Parts.Count;
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "CTG")
                {
                    if (i + 1 >= Parts.Count)
                    {
                        throw new Exception();
                    }
                    double v2 = double.Parse(Parts[i + 1]);
                    double RadFix = 1;
                    if (!IsRad)
                    {
                        RadFix = 57.2958f;
                    }
                    Parts[i] = (1/(Math.Tan(v2 / RadFix))).ToString();
                    Parts.RemoveAt(i + 1);
                    i = 0;
                    Length = Parts.Count;
                }
            }
             Length = Parts.Count;
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "ATAN")
                {
                    if (i + 1 >= Parts.Count)
                    {
                        throw new Exception();
                    }
                    double v2 = double.Parse(Parts[i + 1]);
                    double RadFix = 1;
                    if (!IsRad)
                    {
                        RadFix = 57.2958f;
                    }
                    Parts[i] = ((Math.Atan(v2))*RadFix).ToString();
                    Parts.RemoveAt(i + 1);
                    i = 0;
                    Length = Parts.Count;
                }
            }
             Length = Parts.Count;
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "SQRT")
                {
                    if (i + 1 >= Parts.Count)
                    {
                        throw new Exception();
                    }
                    double v2 = double.Parse(Parts[i + 1]);
                    Parts[i] = ((Math.Sqrt(v2))).ToString();
                    Parts.RemoveAt(i + 1);
                    i = 0;
                    Length = Parts.Count;
                }
            }
             Length = Parts.Count;
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "LN")
                {
                    if (i + 1 >= Parts.Count)
                    {
                        throw new Exception();
                    }
                    double v2 = double.Parse(Parts[i + 1]);
                    Parts[i] = ((Math.Log(v2))).ToString();
                    Parts.RemoveAt(i + 1);
                    i = 0;
                    Length = Parts.Count;
                }
            }
             Length = Parts.Count;
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "LOG")
                {
                    if (i + 1 >= Parts.Count)
                    {
                        throw new Exception();
                    }
                    double v2 = double.Parse(Parts[i + 1]);
                    Parts[i] = ((Math.Log10(v2))).ToString();
                    Parts.RemoveAt(i + 1);
                    i = 0;
                    Length = Parts.Count;
                }
            }
             Length = Parts.Count;
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "/")
                {
                    if (i - 1 < 0)
                    {
                        throw new Exception();
                    }else
                    if (i + 1 >= Parts.Count)
                    {
                        throw new Exception();
                    }
                    double v=double.Parse(Parts[i-1]);
                    double v2= double.Parse(Parts[i+1]);
                    Parts[i] = (v/v2).ToString();
                    Parts.RemoveAt(i + 1);
                    Parts.RemoveAt(i -1);
                    i = 0;
                    Length = Parts.Count;
                }
            }
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "*")
                {
                    if (i - 1 < 0)
                    {
                        throw new Exception();
                    }
                    else
                    if (i + 1 >= Parts.Count)
                    {
                        throw new Exception();
                    }
                    double v = double.Parse(Parts[i - 1]);
                    double v2 = double.Parse(Parts[i + 1]);
                    Parts[i] = (v * v2).ToString();
                    Parts.RemoveAt(i + 1);
                    Parts.RemoveAt(i - 1);
                    i = 0;
                    Length = Parts.Count;
                }
            }
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "-")
                {
                    if (i - 1 < 0)
                    {
                        throw new Exception();
                    }
                    else
                    if (i + 1 >= Parts.Count)
                    {
                        throw new Exception();
                    }

                    double v = double.Parse(Parts[i - 1]);
                    double v2 = double.Parse(Parts[i + 1]);
                    Parts[i] = (v - v2).ToString();
                    Parts.RemoveAt(i + 1);
                    Parts.RemoveAt(i - 1);
                    i = 0;
                    Length = Parts.Count;
                }
            }
            for (int i = 0; i < Length; i++)
            {
                if (Parts[i] == "+")
                {
                    if (i - 1 < 0)
                    {
                        throw new Exception();
                    }
                    else
                    if (i + 1 >= Parts.Count)
                    {
                        throw new Exception();
                    }

                    double v = double.Parse(Parts[i - 1]);
                    double v2 = double.Parse(Parts[i + 1]);
                    Parts[i] = (v + v2).ToString();
                    Parts.RemoveAt(i + 1);
                    Parts.RemoveAt(i - 1);
                    i = 0;
                    Length = Parts.Count;
                }
            }
            if (Parts.Count > 1)
            {
                throw new Exception();
            }
            return Parts[0];
        }
        double Calculate(string formula)
        {
            formula=formula.Replace('.', ',');
            try
            {
                return double.Parse(CalculatePart(formula.ToUpper()));
            }catch
            {
                formula = formula.Replace(',', '.');
                return double.Parse(CalculatePart(formula.ToUpper()));
            }
        }
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            IsRad = !IsRad;
            CheckFormula();
        }
    }
}
