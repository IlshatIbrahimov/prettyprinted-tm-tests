using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace UITests.Suites
{
    class SuiteTemplate
    {
        protected Fixture fixture { get; private set; }

        [OneTimeSetUp]
        public virtual void SetUp()
        {
            fixture = new Fixture();
        }

        [OneTimeTearDown]
        public virtual void Dispose()
        {
            fixture.Dispose();
        }

        [TearDown]
        public virtual void TearDown()
        {
            ResultState outcome = TestContext.CurrentContext.Result.Outcome;
            if (outcome != ResultState.Success && outcome != ResultState.Ignored)
            {
                fixture.Driver.AttachScreenToReport();
                fixture.Driver.AttachPageSourceToReport();
            }
        }
    }
}
