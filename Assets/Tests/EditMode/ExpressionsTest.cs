using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ExpressionsTest
    {
        ExpressionHandler calc = new ExpressionHandler();
        [Test]
        public void AddSub()
        {
            Assert.AreEqual(expected: 10, calc.HandleExpression("1+1+1+1+1+1+1+1+1+1")[0]);
            Assert.AreEqual(expected: -10, calc.HandleExpression("-1-1-1-1-1-1-1-1-1-1")[0]);
            Assert.AreEqual(expected: 0, calc.HandleExpression("1-1+1-1+1-1+1-1+1-1")[0]);
            Assert.AreEqual(expected: -2, calc.HandleExpression("-1-1")[0]);
            Assert.AreEqual(expected: 2, calc.HandleExpression("+1+1")[0]);
            Assert.AreEqual(expected: -1, calc.HandleExpression("-1+1-1")[0]);
            Assert.AreEqual(expected: 20, calc.HandleExpression("10+10")[0]);
            Assert.AreEqual(expected: 0, calc.HandleExpression("10-10")[0]);
            Assert.AreEqual(expected: 5, calc.HandleExpression("20-15")[0]);
        }
        [Test]
        public void diceRoll()
        {
            List<float> trail = calc.HandleExpression("d20");
            Assert.AreEqual(expected: true , (trail[0] <= 20) && trail[0] >= 1);
        }
    }
}
