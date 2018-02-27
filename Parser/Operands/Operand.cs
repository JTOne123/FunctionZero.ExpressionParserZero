﻿#region License
// Author: Keith Pickford
// 
// MIT License
// 
// Copyright (c) 2016 FunctionZero Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion
using System;
using System.Diagnostics;
using FunctionZero.ExpressionParserZero.Tokens;
using FunctionZero.ExpressionParserZero.Variables;

namespace FunctionZero.ExpressionParserZero.Operands
{
	public class Operand : IOperand, IToken
	{
	    private const long Bogey = -1;

        public Operand(OperandType operandType, object operandValue) : this(Bogey, operandType, operandValue)
	    {

	    }

        public Operand(long parserPosition, OperandType operandType, object operandValue)
		{
			Debug.Assert(CheckValueType(operandType, operandValue) == true);

			Type = operandType;
			OperandValue = operandValue;
			ParserPosition = parserPosition;
		}
		
		public long ParserPosition { get; }
		public OperandType Type { get; }
		private object OperandValue { get; }

		public bool IsNumber => (Type == Operands.OperandType.Double) || (Type == OperandType.Long);

        public TokenType TokenType => TokenType.Operand;

        public object GetValue()
		{
			Debug.Assert(CheckValueType(Type, OperandValue) == true);

			return OperandValue;
		}

		private bool CheckValueType(OperandType tokenType, object tokenValue)
		{
			switch(tokenType)
			{
				case OperandType.Long:
					return tokenValue is long;

				case OperandType.Double:
					return tokenValue is double;

				case OperandType.String:
					return tokenValue is string;

				case OperandType.Variable:                // TODO: Make this a reference to a variable? Possible? It might not resolve.
					return tokenValue is string;

				case OperandType.Bool:
					return tokenValue is bool;

                case OperandType.NullableBool:
                    return tokenValue == null || tokenValue is bool;

                case OperandType.VSet:
                    return tokenValue is VariableSet;

				case OperandType.Object:
					return true;

                case OperandType.Null:
                    return tokenValue == null;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public override string ToString()
		{
			return OperandValue.ToString();
		}
	}
}