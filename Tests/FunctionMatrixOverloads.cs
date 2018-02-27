﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionZero.ExpressionParserZero;
using FunctionZero.ExpressionParserZero.Exceptions;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Operators;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.ExpressionParserZero.Variables;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionParserUnitTests
{
	[TestClass]
	public class FunctionMatrixOverloads
	{
		[TestMethod]
		public void TestOverloadAddLongToString()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Age", 42);
			variables.RegisterDouble("Opacity", 0.23);
			variables.RegisterString("Name", "Valerie");

			e.RegisterOverload("+", OperandType.Long, OperandType.String, 
				(leftOperand, rightOperand) => new Operand(OperandType.String, (string)leftOperand.GetValue() + (long)rightOperand.GetValue()));

			string expectedResult = "Valerie42";

			var result = e.Parse("Name + Age");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			var actualResult = (string)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestRegisterUnaryOverloadToDoubleOperand()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Age", 42);
			variables.RegisterDouble("Opacity", 0.23);
			variables.RegisterString("Name", "Valerie");

			try
			{
				e.RegisterOverload("+", OperandType.String,
					(operand) => new Operand(OperandType.String, (string)"!" + (string)operand.GetValue()));
			}
			catch (ExpressionParserException ex)
			{
				Assert.AreEqual(-1, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.UnaryOperatorNotFound, ex.Cause);
			}
		}

		[TestMethod]
		public void TestOverloadNotString()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Age", 42);
			variables.RegisterDouble("Opacity", 0.23);
			variables.RegisterString("Name", "Valerie");

			e.RegisterOverload("!", OperandType.String,
				(operand) => new Operand(OperandType.String, (string)"!"+(string)operand.GetValue()));

			string expectedResult = "!Valerie";

			var result = e.Parse("!Name");

			var evalResult = ev.Evaluate(result, variables);
			Assert.AreEqual(1, evalResult.Count);
			var actualResult = (string)evalResult.Pop().GetValue();
			Assert.AreEqual(expectedResult, actualResult);
		}

		[TestMethod]
		public void TestRegisterMultiOverloadToUnaryOperand()
		{
			ExpressionParser e = new ExpressionParser();
			ExpressionEvaluator ev = new ExpressionEvaluator();
			VariableSet variables = new VariableSet();

			variables.RegisterLong("Age", 42);
			variables.RegisterDouble("Opacity", 0.23);
			variables.RegisterString("Name", "Valerie");

			try
			{
				e.RegisterOverload("!", OperandType.Long, OperandType.String,
					(leftOperand, rightOperand) => new Operand(OperandType.String, (string)leftOperand.GetValue() + (long)rightOperand.GetValue()));
			}
			catch(ExpressionParserException ex)
			{
				Assert.AreEqual(-1, ex.Offset);
				Assert.AreEqual(ExpressionParserException.ExceptionCause.DoubleOperandOperatorNotFound, ex.Cause);
			}
		}
	}
}