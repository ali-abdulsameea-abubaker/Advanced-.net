using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
/// <summary>
///  <Authorship>I, Ali Abdulsameea, 000857347 certify that this material is my original work.
/// No other person's work has been used without due acknowledgement.</Authorship>
/// Main program class to execute the RPN expression evaluator.
/// 
/// <author>Ali Abubaker - 000857347</author>
/// </summary>
/// 

namespace ExpressionTreeAssignment
{
    public class Program
    {
        /// <summary>
        /// Main entry point of the program.
        /// </summary>
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Enter an RPN expression (e.g., '3 4 + 2 *'):");
                string input = Console.ReadLine();

                if (!RPNExpressionBuilder.IsValidRPN(input))
                {
                    Console.WriteLine("Invalid RPN expression. Please try again.");
                    continue;
                }

                try
                {
                    Expression expressionTree = RPNExpressionBuilder.BuildTree(input);
                    double result = Expression.Lambda<Func<double>>(expressionTree).Compile()();
                    Console.WriteLine("Result: " + Math.Round(result, 2));
                    break;
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("Error: Division by zero is not allowed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }

  
    }

/// <summary>
/// Class to build and evaluate expression trees from Reverse Polish Notation (RPN) expressions.
/// </summary>
public static class RPNExpressionBuilder
{
    /// <summary>
    /// Builds an expression tree from an RPN expression.
    /// </summary>
    /// <param name="rpnExpression">The RPN expression as a string.</param>
    /// <returns>An Expression representing the parsed RPN.</returns>
    public static Expression BuildTree(string rpnExpression)
    {
        Stack<Expression> stack = new Stack<Expression>();
        string[] tokens = rpnExpression.Split(' ');

        foreach (string token in tokens)
        {
            if (double.TryParse(token, out double number))
            {
                stack.Push(Expression.Constant(number));
            }
            else
            {
                if (stack.Count < 2) throw new InvalidOperationException("Invalid RPN expression.");

                Expression right = stack.Pop();
                Expression left = stack.Pop();

                if (token == "/" && right is ConstantExpression rightConst && (double)rightConst.Value == 0)
                {
                    throw new DivideByZeroException();
                }

                stack.Push(GetBinaryExpression(token, left, right));
            }
        }

        return stack.Pop();
    }

    /// <summary>
    /// Determines the expression type based on the operator.
    /// </summary>
    /// <param name="operator">The mathematical operator (+, -, *, /, ^).</param>
    /// <param name="left">Left operand expression.</param>
    /// <param name="right">Right operand expression.</param>
    /// <returns>An Expression representing the operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when an invalid operator is encountered.</exception>
    private static Expression GetBinaryExpression(string token, Expression left, Expression right)
    {
        switch (token)
        {
            case "+": return Expression.Add(left, right);
            case "-": return Expression.Subtract(left, right);
            case "*": return Expression.Multiply(left, right);
            case "/": return Expression.Divide(left, right);
            case "^": return Expression.Call(typeof(Math).GetMethod("Pow", new[] { typeof(double), typeof(double) }), left, right);
            default:
                throw new NotSupportedException($"Operator '{token}' is not supported.");
        }
    }
    /// <summary>
    /// Validates whether a given RPN expression is well-formed.
    /// </summary>
    /// <param name="rpnExpression">The RPN expression to validate.</param>
    /// <returns>True if the expression is valid; otherwise, false.</returns>
    public static bool IsValidRPN(string rpnExpression)
    {
        if (string.IsNullOrWhiteSpace(rpnExpression)) return false;

        int operandCount = 0;
        string[] tokens = rpnExpression.Split(' ');

        foreach (string token in tokens)
        {
            if (double.TryParse(token, out _))
            {
                operandCount++;
            }
            else
            {
                if (operandCount < 2) return false;
                operandCount--;
            }
        }

        return operandCount == 1;
    }
}
