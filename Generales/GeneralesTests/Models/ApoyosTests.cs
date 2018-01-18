using Microsoft.VisualStudio.TestTools.UnitTesting;
using Generales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generales.Models.Tests
{
  [TestClass()]
  public class ApoyosTests
  {
    [TestMethod()]
    public void GetAssemblyDirectoryTest()
    {
      string path;
      path = Generales.Models.Apoyos.GetAssemblyDirectory();
      Assert.AreEqual(path, @"D:\");
    }
  }
}