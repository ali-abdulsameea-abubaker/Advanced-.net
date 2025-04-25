using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using System.Linq.Expressions;

namespace TestingExpression
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAddition()
        {
            // Arrange
            var expressionTree = RPNExpressionBuilder.BuildTree("5 3 +");

            // Act
            var result = Expression.Lambda<Func<double>>(expressionTree).Compile()();

            // Assert
            Assert.AreEqual(8, result);
        }

        [TestMethod]
        public void TestSubtraction()
        {
            // Arrange
            var expressionTree = RPNExpressionBuilder.BuildTree("10 4 -");

            // Act
            var result = Expression.Lambda<Func<double>>(expressionTree).Compile()();

            // Assert
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void TestMultiplication()
        {
            // Arrange
            var expressionTree = RPNExpressionBuilder.BuildTree("5 3 *");

            // Act
            var result = Expression.Lambda<Func<double>>(expressionTree).Compile()();

            // Assert
            Assert.AreEqual(15, result);
        }

        [TestMethod]
        public void TestPower()
        {
            // Arrange
            var expressionTree = RPNExpressionBuilder.BuildTree("2 3 ^");

            // Act
            var result = Expression.Lambda<Func<double>>(expressionTree).Compile()();

            // Assert
            Assert.AreEqual(8, result);
        }

        [TestMethod]
        public void TestComplexExpression()
        {
            // Arrange
            var expressionTree = RPNExpressionBuilder.BuildTree("15 7 1 1 + - / 3 * 2 1 1 + + -");

            // Act
            var result = Expression.Lambda<Func<double>>(expressionTree).Compile()();

            // Assert
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInvalidOperator()
        {
            // Act
            var expressionTree = RPNExpressionBuilder.BuildTree("5 3 %");
        }


        [TestMethod]
        public void TestDivisionByZero()
        {
            // Act & Assert
            Assert.ThrowsException<DivideByZeroException>(() =>
            {
                var expressionTree = RPNExpressionBuilder.BuildTree("5 0 /");
                var compiledExpression = Expression.Lambda<Func<double>>(expressionTree).Compile();
                compiledExpression();
            });
        }
    }
}
