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
        public void AdditionSubtractions()
        {
            Assert.AreEqual(expected: 20, calc.HandleExpression("10+10")[0]);
            Assert.AreEqual(expected: 3, calc.HandleExpression("1+2")[0]);
            Assert.AreEqual(expected: 10, calc.HandleExpression("1+1+1+1+1+1+1+1+1+1")[0]);
            Assert.AreEqual(expected: -10, calc.HandleExpression("-1-1-1-1-1-1-1-1-1-1")[0]);
            Assert.AreEqual(expected: 0, calc.HandleExpression("1-1+1-1+1-1+1-1+1-1")[0]);
            Assert.AreEqual(expected: -2, calc.HandleExpression("-1-1")[0]);
            Assert.AreEqual(expected: 2, calc.HandleExpression("+1+1")[0]);
            Assert.AreEqual(expected: -1, calc.HandleExpression("-1+1-1")[0]);
            Assert.AreEqual(expected: 0, calc.HandleExpression("10-10")[0]);
            Assert.AreEqual(expected: 5, calc.HandleExpression("20-15")[0]);
        }
        [Test]
        public void Multiplications()
        {
            Assert.AreEqual(expected: 1f, calc.HandleExpression("1*1")[0]);
            Assert.AreEqual(expected: 2f, calc.HandleExpression("1*2")[0]);
            Assert.AreEqual(expected: 1f, calc.HandleExpression("1*1*1")[0]);
            Assert.AreEqual(expected: 10f, calc.HandleExpression("10*1")[0]);
            Assert.AreEqual(expected: 100f, calc.HandleExpression("10*10")[0]);
            Assert.AreEqual(expected: 1000f, calc.HandleExpression("10*10*10")[0]);
        }
        [Test]
        public void MixedOperations()
        {
            Assert.AreEqual(expected: 60f, calc.HandleExpression("5+10*5+5")[0]);
            Assert.AreEqual(expected: 2f, calc.HandleExpression("1+1*1")[0]);
            Assert.AreEqual(expected: 2f, calc.HandleExpression("1*1+1")[0]);
            Assert.AreEqual(expected: 10f, calc.HandleExpression("5+1*5")[0]);
            Assert.AreEqual(expected: 30.5f, calc.HandleExpression("5/5*1+1-5+1+1/2*2+45-54/4")[0]);
            Assert.AreEqual(expected: 15f, calc.HandleExpression("5+1*5+5")[0]);
            Assert.AreEqual(expected: 636f, calc.HandleExpression("1+5*5*5*5+5+5")[0]);
            Assert.AreEqual(expected: true, calc.HandleExpression("125*125*5+65*8+4*8+5*8*8*45*554*5")[0] > 0); // Needs an nE+m operation to work
        }
        [Test]
        public void Division()
        {
            Assert.AreEqual(expected: 10f, calc.HandleExpression("10/1")[0]);
            Assert.AreEqual(expected: 2f, calc.HandleExpression("20/10")[0]);
            Assert.AreEqual(expected: 1f, calc.HandleExpression("1/1")[0]);
            Assert.AreEqual(expected: 1.5f, calc.HandleExpression("3/2")[0]);
        }
        [Test]
        public void diceRoll()
        {
            List<float> trail = calc.HandleExpression("d20");
            Assert.AreEqual(expected: true , (trail[0] <= 20) && trail[0] >= 1);
        }
        [Test]
        public void Undefinables()
        {
            Assert.AreEqual(float.NaN, calc.HandleExpression("1/0")[0]);
            Assert.AreEqual(float.NaN, calc.HandleExpression("0/0")[0]);
        }
    }
}
