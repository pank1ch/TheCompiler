namespace compiler_theory.app.model.polish_notation;

class RPN
{
    public string Output = "";
    public double Result;
    public List<string> Errors = new List<string>();
    
    public string Calculate(string input)
    {
        Output = GetExpression(input); 
        Result = Counting(Output); 
        return Output;
    }

    private string GetExpression(string input)
    {
        var output = string.Empty; 
        var operatorStack = new Stack<char>(); 

        for (int i = 0; i < input.Length; i++) 
        {
            if (IsSeparator(input[i]))
                continue; 
            
            if (char.IsDigit(input[i])) 
            {
                while (!IsSeparator(input[i]) && !IsOperator(input[i]))
                {
                    output += input[i]; 
                    i++; 

                    if (i == input.Length) break; 
                }

                output += " "; 
                i--; 
            }
            
            if (IsOperator(input[i])) 
            {
                if (input[i] == '(') 
                    operatorStack.Push(input[i]);
                else if (input[i] == ')') 
                {
                    bool foundOpeningBracket = false;
                    while (operatorStack.Count > 0 && operatorStack.Peek() != '(')
                    {
                        output += operatorStack.Pop() + " ";
                    }

                    if (operatorStack.Count > 0 && operatorStack.Peek() == '(')
                    {
                        operatorStack.Pop();
                        foundOpeningBracket = true;
                    }

                    if (!foundOpeningBracket)
                    {
                        Errors.Add("Ошибка: Отсутствует открывающая скобка для закрывающей скобки");
                        return output;
                    }
                }
                else 
                {
                    if (operatorStack.Count > 0) 
                        if (GetPriority(input[i]) <= GetPriority(operatorStack.Peek()))
                            output += operatorStack.Pop().ToString() + " "; 

                    operatorStack.Push(char.Parse(input[i].ToString()));
                }
            }
        }
        
        while (operatorStack.Count > 0)
        {
            if (operatorStack.Peek() == '(')
            {
                Errors.Add("Ошибка: Отсутствует закрывающая скобка для открывающей скобки");
                operatorStack.Pop();
            }
            else
                output += operatorStack.Pop() + " ";
        }

        return output;
    }

    private double Counting(string input)
    {
        double result = 0;
        var temp = new Stack<double>();

        for (int i = 0; i < input.Length; i++) 
        {
            if (char.IsDigit(input[i])) 
            {
                var a = string.Empty;

                while (!IsSeparator(input[i]) && !IsOperator(input[i]))
                {
                    a += input[i]; 
                    i++;
                    if (i == input.Length) break;
                }
                temp.Push(double.Parse(a)); 
                i--;
            }
            else if (IsOperator(input[i])) 
            {
                if (temp.Count < 2)
                {
                    Errors.Add("Ошибка: Недостаточно операндов для данного оператора");
                    return 0;
                }

                var a = temp.Pop(); 
                var b = temp.Pop();

                result = input[i] switch
                {
                    '+' => b + a,
                    '-' => b - a,
                    '*' => b * a,
                    '/' => b / a,
                    '^' => double.Parse(Math.Pow(double.Parse(b.ToString()), double.Parse(a.ToString())).ToString()),
                    _ => result
                };
                temp.Push(result); 
            }
        }
        return temp.Peek(); 
    }
    
    private static bool IsSeparator(char c)
    {
        return " =".Contains(c);
    }
    
    private static bool IsOperator(char с)
    {
        return "+-/*^()".Contains(с);
    }
    
    private static byte GetPriority(char s)
    {
        return s switch
        {
            '(' => 0,
            ')' => 1,
            '+' => 2,
            '-' => 3,
            '*' => 4,
            '/' => 4,
            '^' => 5,
            _ => 6
        };
    }
    
    public string GetNormalExpression(string rpnExpression)
    {
        var stack = new Stack<string>();
        var tokens = rpnExpression.Split(' ');

        foreach (var token in tokens)
        {
            if (IsOperator(token[0]))
            {
                if (stack.Count < 2)
                {
                    Errors.Add("Ошибка: Недостаточно операндов для данного оператора");
                    return "";
                }

                var operand2 = stack.Pop();
                var operand1 = stack.Pop();
                var expression = $"({operand1} {token} {operand2})";
                stack.Push(expression);
            }
            else
            {
                stack.Push(token);
            }
        }

        if (stack.Count != 1)
        {
            Errors.Add("Ошибка: Неверное количество операндов в обратной польской нотации");
            return "";
        }

        return stack.Pop();
    }
}