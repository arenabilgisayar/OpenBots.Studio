﻿using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using Xunit;

namespace OpenBots.Commands.Data.Test
{
    public class MathCalculationCommandTests
    {
        private MathCalculationCommand _mathCalculation;
        private AutomationEngineInstance _engine;

        [Theory]
        [InlineData("1","+","1", "2.00")]
        [InlineData("2","-","1","1.00")]
        [InlineData("2","*","2","4.00")]
        [InlineData("3","/","2","1.50")]
        public void PerformsCalculationCorrectly(string num1, string operation, string num2, string expectedOutput)
        {
            _mathCalculation = new MathCalculationCommand();
            _engine = new AutomationEngineInstance(null);

            num1.CreateTestVariable(_engine, "num1");
            num2.CreateTestVariable(_engine, "num2");
            "unassigned".CreateTestVariable(_engine, "output", typeof(string));

            _mathCalculation.v_MathExpression = "{num1}" + operation + "{num2}";
            _mathCalculation.v_OutputUserVariableName = "{output}";

            _mathCalculation.RunCommand(_engine);

            Assert.Equal(expectedOutput, _mathCalculation.v_OutputUserVariableName.ConvertUserVariableToString(_engine));
        }

        [Fact]
        public void HandlesThousandSeparator()
        {
            _mathCalculation = new MathCalculationCommand();
            _engine = new AutomationEngineInstance(null);
            string num1 = "10.000";
            string num2 = "1.000";
            string thouSeparator = ".";
            num1.CreateTestVariable(_engine, "num1");
            num2.CreateTestVariable(_engine, "num2");
            thouSeparator.CreateTestVariable(_engine, "thouSeparator", typeof(string));
            "unassigned".CreateTestVariable(_engine, "output", typeof(string));

            string mathExpression = "{num1} + {num2}";

            _mathCalculation.v_MathExpression = mathExpression;
            _mathCalculation.v_ThousandSeparator = "{thouSeparator}";
            _mathCalculation.v_OutputUserVariableName = "{output}";

            _mathCalculation.RunCommand(_engine);

            Assert.Equal("11.000.00", _mathCalculation.v_OutputUserVariableName.ConvertUserVariableToString(_engine));
        }

        [Fact]
        public void HandlesDecimalSeparator()
        {
            _mathCalculation = new MathCalculationCommand();
            _engine = new AutomationEngineInstance(null);

            string num1 = "1:10";
            string num2 = "0:50";
            string decSeparator = ":";
            num1.CreateTestVariable(_engine, "num1", typeof(string));
            num2.CreateTestVariable(_engine, "num2", typeof(string));
            decSeparator.CreateTestVariable(_engine, "decSeparator", typeof(string));
            "unassigned".CreateTestVariable(_engine, "output", typeof(string));

            string mathExpression = "{num1} + {num2}";

            _mathCalculation.v_MathExpression = mathExpression;
            _mathCalculation.v_DecimalSeparator = "{decSeparator}";
            _mathCalculation.v_OutputUserVariableName = "{output}";

            _mathCalculation.RunCommand(_engine);

            Assert.Equal("1:60", _mathCalculation.v_OutputUserVariableName.ConvertUserVariableToString(_engine));
        }
    }
}
