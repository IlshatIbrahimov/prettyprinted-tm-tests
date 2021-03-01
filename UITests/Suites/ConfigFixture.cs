using NUnit.Framework;
using System.IO;
using UITests.Requests;

namespace UITests.Suites
{
    [SetUpFixture]
    class ConfigFixture
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            Clients.Init();

            Utilities.BaseUrl = File.ReadAllText("../../../InputData/StandUrl.txt");

            DirectoryInfo di = Directory.CreateDirectory("../../../results");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
