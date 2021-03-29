using FuncTests.Requests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuncTests.Suites
{
    class Projects
    {
        private ProjectsRequests requests;

        [OneTimeSetUp]
        public void SetUp()
        {
            requests = new ProjectsRequests();
        }
    }
}
