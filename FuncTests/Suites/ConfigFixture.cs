using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FuncTests.Suites
{
    [SetUpFixture]
    class ConfigFixture
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            DirectoryInfo di = Directory.CreateDirectory("../../../results");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
