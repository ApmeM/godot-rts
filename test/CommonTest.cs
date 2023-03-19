namespace GodotRts.Tests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    public interface ITest
    {
        public void Method();
    }

    public struct Test : ITest
    {
        public void Method()
        {
            Console.WriteLine("Self");
        }
    }

    public class TestHandler<T> where T : ITest
    {
        private readonly T test;

        public TestHandler(T test)
        {
            this.test = test;
        }

        public void Do()
        {
            this.test.Method();
        }
    }

    [TestFixture]
    public class CommonTest
    {
        [Test]
        public void Test()
        {
            var test = new Test();
            var handler1 = new TestHandler<Test>(test);
            var handler2 = new TestHandler<ITest>(test);
            var handler3 = new TestHandler<ITest>((ITest)test);

            handler1.Do();
            handler2.Do();
            handler3.Do();
        }
    }
}
