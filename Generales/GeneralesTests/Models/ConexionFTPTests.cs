using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Generales.Models.Tests
{
  [TestClass()]
  public class ConexionFTPTests
  {
    #region atributos
    private const string server = "";
    private const string user = "";
    private const string password = "";
    private const string port = "";
    #endregion
    [TestMethod()]
    public void SubirArchivoTest()
    {
      Boolean estado;
      estado = subirArchivosftp();
      Assert.IsTrue(estado);
    }

    [TestMethod()]
    public void DescargarArchivoTest()
    {
      Boolean estado;
      estado = DescargarArchivoFPT();
      Assert.IsTrue(estado);
    }

    [TestMethod()]
    public void SubirArchivoAsync()
    {
      Boolean estado;
      estado = subirArchivosftpAsync().Result;
      Assert.IsTrue(estado);
    }

    [TestMethod()]
    public void DescargarArchivoAsyncTest()
    {
      Boolean estado;
      estado = DescargarArchivoAsync().Result;
      Assert.IsTrue(estado);
    }

    public static Boolean subirArchivosftp()
    {
      Generales.Models.ConexionFTP oftp;

      Boolean estado = false;
      try
      {
        oftp = new Generales.Models.ConexionFTP(server, user, password, Generales.Models.TipoFtp.FTP, 300);
        oftp.SubirArchivo("eula.1028.txt", @"D:\Proyectos hl\pruebas\eula.1028.txt");
        estado = true;
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;

    }


    public static Boolean DescargarArchivoFPT()
    {
      Generales.Models.ConexionFTP oftp;

      Boolean estado = false;
      try
      {
        oftp = new Generales.Models.ConexionFTP(server, user, password, Generales.Models.TipoFtp.FTP, 300);
        oftp.DescargarArchivo("eula.1028.txt", @"D:\Proyectos hl\pruebas\do\eula.1028.txt");
        estado = true;
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;

    }

    public static async Task<Boolean> subirArchivosftpAsync()
    {
      Generales.Models.ConexionFTP oftp;

      Boolean estado = false;
      try
      {
        oftp = new Generales.Models.ConexionFTP(server, user, password, Generales.Models.TipoFtp.FTP, 300);
        await oftp.SubirArchivoAsync("eula.1031.txt", @"D:\Proyectos hl\pruebas\eula.1031.txt");
        estado = true;

      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;

    }

    public static async Task<Boolean> DescargarArchivoAsync()
    {
      Generales.Models.ConexionFTP oftp;

      Boolean estado = false;
      try
      {
        oftp = new Generales.Models.ConexionFTP(server, user, password, Generales.Models.TipoFtp.FTP, 300);
        await oftp.DescargarArchivoAsync("eula.1031.txt", @"D:\Proyectos hl\pruebas\do\eula.1031.txt");
        estado = true;

      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;

    }
  }
}