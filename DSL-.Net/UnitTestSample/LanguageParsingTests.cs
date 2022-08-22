using NUnit.Framework;
using DomainLanguageInterpreter.Grammar;
using DomainLanguageInterpreter.Arithmetic;
using System;

namespace UnitTestSample
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void DomainLanguageParseTest_Constant()
		{
			Scanner scanner = new Scanner();
			IArithmetic result = scanner.ParseSyntax("42");
			Assert.AreEqual("[Constant(42)]", result.Key);
		}

		[Test]
		public void DomainLanguageParseTest_SimpleArithmetic()
		{
			Scanner scanner = new Scanner();
			IArithmetic result = scanner.ParseSyntax("3 + (2 + 3) * 4");
			Assert.AreEqual("[Addition([Constant(3)],[Multiply([Addition([Constant(2)],[Constant(3)])],[Constant(4)])])]", result.Key);
		}

		[Test]
		public void DomainLanguageParseTest_DomainLangaugeConstructWithArithmetic()
		{
			Scanner scanner = new Scanner();
			IArithmetic result = scanner.ParseSyntax("CLOSE - DELAY(CLOSE,(3 + 2)*1)");
			Assert.AreEqual("[Subtraction([Data(CLOSE)],[Delay([Data(CLOSE)],[Multiply([Addition([Constant(3)],[Constant(2)])],[Constant(1)])])])]", result.Key);
		}

		[Test]
		public void DomainLanguageParseTest_DomainLanguageConstruct()
		{
			Scanner scanner = new Scanner();
			IArithmetic result = scanner.ParseSyntax("OPEN - DELAY(CLOSE,5)");
			Assert.AreEqual("[Subtraction([Data(OPEN)],[Delay([Data(CLOSE)],[Constant(5)])])]", result.Key);
		}

		[Test]
		public void DomainLanguageParseTest_domainLanguageConstructWithPower()
		{
			Scanner scanner = new Scanner();
			IArithmetic result = scanner.ParseSyntax("(HIGH*LOW)^0.5-SMA(CLOSE,5)");
			Assert.AreEqual("[Subtraction([Power([Multiply([Data(HIGH)],[Data(LOW)])],[Constant(0.5)])],[SMA([Data(CLOSE)],[Constant(5)])])]", result.Key);
		}

		[Test]
		public void DomainLanguageParseTest_genericequation()
		{
			Scanner scanner = new Scanner();
			IArithmetic result = scanner.ParseSyntax("(4.5 + 4) * 2 +2*12.5");
			Assert.AreEqual("[Addition([Multiply([Addition([Constant(4.5)],[Constant(4)])],[Constant(2)])],[Multiply([Constant(2)],[Constant(12.5)])])]", result.Key);
			Assert.AreEqual(42, result.GetValue(DateTime.Now));
		}
		
		[Test]
		public void DomainLanguageParseTest_combineddivision()
		{
			Scanner scanner = new Scanner();
			IArithmetic result = scanner.ParseSyntax("((6 - 4)^2) / 2 +2*12.5");
			Assert.AreEqual("[Addition([Divide([Power([Subtraction([Constant(6)],[Constant(4)])],[Constant(2)])],[Constant(2)])],[Multiply([Constant(2)],[Constant(12.5)])])]", result.Key);
			Assert.AreEqual(27, result.GetValue(DateTime.Now));
		}
		
		[Test]
		public void DomainLanguageParseTest_simpledivision()
		{
			Scanner scanner = new Scanner();
			IArithmetic result = scanner.ParseSyntax("84/2");
			Assert.AreEqual("[Divide([Constant(84)],[Constant(2)])]", result.Key);
			Assert.AreEqual(42, result.GetValue(DateTime.Now));
		}
		
		[Test]
		public void DomainLanguageParseTest_simplePower()
		{
			Scanner scanner = new Scanner();
			IArithmetic result = scanner.ParseSyntax("4^3");
			Assert.AreEqual("[Power([Constant(4)],[Constant(3)])]", result.Key);
			Assert.AreEqual(64, result.GetValue(DateTime.Now));
		}
		
		[Test]
		public void DomainLanguageParseTest_volumeAndPlus()
		{
			Scanner scanner = new Scanner();
			IArithmetic result = scanner.ParseSyntax("VOLUME+3");
			Assert.AreEqual("[Addition([Data(VOLUME)],[Constant(3)])]", result.Key);
		}
		
		[Test]
		public void DomainLanguageParseTest_negativeNumber()
		{
			Scanner scanner = new Scanner();
			IArithmetic result = scanner.ParseSyntax("-3");
			Assert.AreEqual("[Subtraction([Constant(0)],[Constant(3)])]", result.Key);
			Assert.AreEqual(-3, result.GetValue(DateTime.Now));
		}
	}
}