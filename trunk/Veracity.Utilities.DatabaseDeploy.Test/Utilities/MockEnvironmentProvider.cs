using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Veracity.Utilities.DatabaseDeploy.Utilities;

namespace Veracity.Utilities.DatabaseDeploy.Test.Utilities
{
    class MockEnvironmentProvider : EnvironmentProvider
    {
        private string userName;
        private string currentDirectory;
        private string currentExecutingAssemblyDirectory;

        public MockEnvironmentProvider()
        {
            userName = Environment.UserName;
            currentDirectory = Environment.CurrentDirectory;
            currentExecutingAssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public override string CurrentDirectory
        {
            get { return currentDirectory; }
        }

        public override string UserName
        {
            get { return userName; }
        }

        public override string ExecutingAssemblyDirectory
        {
            get { return currentExecutingAssemblyDirectory; }
        }

        public void SetUserName(string username)
        {
            userName = username;
        }
        public void SetCurrentDirectory(string currentDir)
        {
            currentDirectory = currentDir;
        }
        public void SetCurrentExecutingAssemblyDirectory(string dir)
        {
            currentExecutingAssemblyDirectory = dir;
        }
    }
}
