using NUnit.Framework;
using System.IO;

namespace UITests.Suites
{
    [SetUpFixture]
    class ConfigFixture
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            Utilities.BaseUrl = File.ReadAllText("../../../InputData/StandUrl.txt");

            DirectoryInfo di = Directory.CreateDirectory("../../../results");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
