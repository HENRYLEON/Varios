/*
 
 */
using System;
using System.Threading.Tasks;
using System.Net.FtpClient;
using System.IO;
using System.Net;

namespace Generales.Models
{
  public class ConexionFTP
  {
    #region Atributos
    private FtpClient FTPClient;
    #endregion

    #region Constructor
    public ConexionFTP(string servidor, string usuario, string clave, TipoFtp TipoFtp, int timeOut,string puerto = "")
    {
      CrearConexion(servidor, usuario, clave, TipoFtp, timeOut, puerto);
    }
    #endregion

    #region Servicios
    /// <summary>
    /// Descarga un archivo del FTP
    /// </summary>
    /// <param name="rutaRemota">Ruta del archivo en el FTP</param>
    /// <param name="rutaLocal">Ruta del archivo local</param>
    public void DescargarArchivo(string rutaRemota, string rutaLocal)
    {
      byte[] buffer;
      FileStream fileOutput;
      int len;
      try
      {
        // Conectarse al ftp.
        Conectarse();

        // Verificar la conexion con el FTP.
        if (!estaConectado()) throw new Exception("Se ha presentado un error al descargar el archivo del ftp");

        // Verificar la existencia de la carpeta local.
        if (!String.IsNullOrWhiteSpace(Path.GetDirectoryName(rutaLocal)) && !Directory.Exists(Path.GetDirectoryName(rutaLocal)))
          Directory.CreateDirectory(Path.GetDirectoryName(rutaLocal));

        // Obtener el stream en modo lectura.
        using (Stream inputStream = FTPClient.OpenRead(rutaRemota))
        {
          fileOutput = new FileStream(rutaLocal, FileMode.Create, FileAccess.ReadWrite);
          try
          {
            buffer = new Byte[8 * 1024 - 1];
            // Escribir el archivo remoto en el local.
            do
            {
              len = inputStream.Read(buffer, 0, buffer.Length);
              fileOutput.Write(buffer, 0, len);

            } while (len > 0);

          }
          finally
          {
            fileOutput.Close();
            inputStream.Close();
          }

        }
        Desconectarse();
      }
      catch (InvalidCastException ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// Descarga un archivo del FTP de manera asincrona
    /// </summary>
    /// <param name="rutaRemota">Ruta del archivo en el FTP</param>
    /// <param name="rutaLocal">Ruta del archivo en el FTP</param>
    /// <returns></returns>
    public async Task<Boolean> DescargarArchivoAsync(string rutaRemota, string rutaLocal)
    {
      string nombreArchivo;
      string nombreDirectorioFTP;
      Boolean estado;
      try
      {
        // Conectarse al ftp.
        await ConectarseAsync().ConfigureAwait(false);

        // Verificar la conexion con el FTP.
        if (!estaConectado()) throw new Exception("Se ha presentado un error al descargar el archivo del ftp");

        // Verificar la existencia de la carpeta local.
        if (!String.IsNullOrWhiteSpace(Path.GetDirectoryName(rutaLocal)) && !Directory.Exists(Path.GetDirectoryName(rutaLocal)))
          Directory.CreateDirectory(Path.GetDirectoryName(rutaLocal));

        nombreArchivo = Path.GetFileName(rutaRemota);
        nombreDirectorioFTP = Path.GetDirectoryName(rutaRemota);
        //establece conexion
        await Task.Factory.FromAsync(FTPClient.BeginConnect, FTPClient.EndConnect, state: null).ConfigureAwait(false);
        // Valida que el directorio exista en el FTP.
        bool doesDirectoryExist = await Task<bool>.Factory.FromAsync<string>(FTPClient.BeginDirectoryExists, FTPClient.EndDirectoryExists, nombreDirectorioFTP, state: null).ConfigureAwait(false);
        if (doesDirectoryExist == true)
        {
          await Task.Factory.FromAsync<string>(FTPClient.BeginSetWorkingDirectory, FTPClient.EndSetWorkingDirectory, nombreDirectorioFTP, state: null).ConfigureAwait(false);
          // Valida que el archivo exista en el FTP.
          bool doesFileExist = await Task<bool>.Factory.FromAsync<string>(FTPClient.BeginFileExists, FTPClient.EndFileExists, nombreArchivo, state: null).ConfigureAwait(false);
          if (doesFileExist == true)
          {
            // Realiza  la copia del archivo. 
            using (Stream streamToRead = await Task<Stream>.Factory.FromAsync<string>(FTPClient.BeginOpenRead, FTPClient.EndOpenRead, nombreArchivo, state: null).ConfigureAwait(false))
            using (Stream streamToWrite = File.Open(rutaLocal, FileMode.Append))
            {
              await streamToRead.CopyToAsync(streamToWrite).ConfigureAwait(false);
            }
          }
        }

        await DesconectarseAsync().ConfigureAwait(false);

        estado = true;
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;
    }
    /// <summary>
    /// Sube un archivo al FTP
    /// </summary>
    /// <param name="rutaRemota">Ruta del archivo remoto</param>
    /// <param name="rutaLocal">Ruta del archivo local</param>
    /// <returns>Exito de la operacion</returns>
    public Boolean SubirArchivo(string rutaRemota, string rutaLocal)
    {
      FileStream fileInput;
      Byte[] buffer;
      int len;
      Boolean estado;
      try
      {
        Conectarse();

        if (!estaConectado()) throw new Exception("Se ha presentado un error al descargar el archivo del ftp");

        if (!(File.Exists(rutaLocal))) throw new Exception("No existe el archivo local " + rutaLocal);

        // Obtener el stream remoto en modo escritura.

        using (Stream outputStream = FTPClient.OpenWrite(rutaRemota))
        {
          fileInput = new FileStream(rutaLocal, FileMode.Open, FileAccess.Read);

          buffer = new Byte[8 * 1024 - 1];

          // Escribir el archivo local en el remoto.
          try
          {
            do
            {
              len = fileInput.Read(buffer, 0, buffer.Length);
              outputStream.Write(buffer, 0, len);
            } while (len > 0);
          }
          finally
          {
            fileInput.Close();
            outputStream.Close();
          }
        }
        Desconectarse();

        estado = true;
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      finally
      {
      }
      return estado;
    }

    public async Task<Boolean> SubirArchivoAsync(string rutaRemota, string rutaLocal)
    {
      string nombreArchivo;
      string nombreDirectorioFTP;
      Boolean estado;
      try
      {
        // Conectarse al ftp.
        await ConectarseAsync().ConfigureAwait(false);

        // Verificar la conexion con el FTP.
        if (!estaConectado()) throw new Exception("Se ha presentado un error al descargar el archivo del ftp");

        // Valida que exista la ruta local.
        if (!(File.Exists(rutaLocal))) throw new Exception("No existe el archivo local " + rutaLocal);

        nombreArchivo = Path.GetFileName(rutaRemota);
        nombreDirectorioFTP = Path.GetDirectoryName(rutaRemota);
        //establece conexion
        await Task.Factory.FromAsync(FTPClient.BeginConnect, FTPClient.EndConnect, state: null).ConfigureAwait(false);
        // Valida que el directorio exista en el FTP.
        bool doesDirectoryExist = await Task<bool>.Factory.FromAsync<string>(FTPClient.BeginDirectoryExists, FTPClient.EndDirectoryExists, nombreDirectorioFTP, state: null).ConfigureAwait(false);
        if (doesDirectoryExist == true)
        {
          await Task.Factory.FromAsync<string>(FTPClient.BeginSetWorkingDirectory, FTPClient.EndSetWorkingDirectory, nombreDirectorioFTP, state: null).ConfigureAwait(false);
          // Realiza  la copia del archivo. 
          using (FileStream streamToRead = new FileStream(rutaLocal, FileMode.Open, FileAccess.Read))
          using (Stream streamToWrite = await Task<Stream>.Factory.FromAsync<string>(FTPClient.BeginOpenWrite, FTPClient.EndOpenWrite, nombreArchivo, state: null).ConfigureAwait(false))
          {
            await streamToRead.CopyToAsync(streamToWrite).ConfigureAwait(false);
          }

        }
        await DesconectarseAsync().ConfigureAwait(false);

        estado = true;
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;
    }
    #endregion

    #region Funciones
    /// <summary>
    /// Crea un nuevo cliente de conexion Ftp
    /// </summary>
    /// <param name="servidor">ip o nombre del servidor ftp</param>
    /// <param name="usuario">Usuario para conexion al ftp</param>
    /// <param name="clave">Clave para conexion al ftp</param>
    /// <param name="tipoFtp">Tipo de ftp</param>
    private void CrearConexion(string servidor, string usuario, string clave, TipoFtp tipoFtp, int timeOut, string puerto = "")
    {
      try
      {
        FTPClient = new FtpClient();
        // Establece configuracion el servidor.
        FTPClient.Host = servidor;
        FTPClient.Credentials = new NetworkCredential(usuario, clave);
        FTPClient.DataConnectionType = FtpDataConnectionType.PASV;
        if (!puerto.Equals(""))
          FTPClient.Port = Convert.ToInt32(puerto, 16);

        // Establece la configuracion SSL.
        if (tipoFtp == TipoFtp.FTPS || tipoFtp == TipoFtp.FTPES)
        {
          FTPClient.EncryptionMode = (tipoFtp == TipoFtp.FTPS) ? FtpEncryptionMode.Implicit : FtpEncryptionMode.Explicit;
          if (tipoFtp == TipoFtp.FTPS) FTPClient.Port = 990;
          FTPClient.DataConnectionEncryption = true;
          FTPClient.ValidateCertificate += delegate (FtpClient control, FtpSslValidationEventArgs e) { e.Accept = true; };
        }
        // Metodo para verificar la validez de los certificados.


        //Establece el timeout de conexion.
        FTPClient.ConnectTimeout = timeOut * 1000;
        FTPClient.DataConnectionConnectTimeout = timeOut * 1000;
        FTPClient.DataConnectionReadTimeout = timeOut * 1000;
        FTPClient.ReadTimeout = timeOut * 1000;

      }
      catch (InvalidCastException ex)
      {
        throw ex;
      }
    }


    /// <summary>
    /// Realiza la coneccion al FTP
    /// </summary>
    /// <returns>estado boolean</returns>
    private Boolean Conectarse()
    {
      Boolean estado;
      try
      {
        estado = true;
        if ((FTPClient) != null) FTPClient.Connect();
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;
    }

    /// <summary>
    /// Indica si el cliente FTP esta conectado.
    /// </summary>
    /// <returns>Verdadero si esta conectado, falso en caso contrario</returns>
    private Boolean estaConectado()
    {
      return (((FTPClient) != null) && (FTPClient.IsConnected));
    }

    /// <summary>
    /// Desconecta la conexion al FTP.
    /// </summary>
    /// <returns></returns>
    private Boolean Desconectarse()
    {
      try
      {
        if (((FTPClient) != null) && (FTPClient.IsConnected))
          FTPClient.Disconnect();
        return true;
      }
      catch (InvalidCastException ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// Indica si el cliente FTP esta conectado de manera asincrona.
    /// </summary>
    /// <returns>Exito de la conexion</returns>
    private async Task<Boolean> ConectarseAsync()
    {
      try
      {
        if ((FTPClient) != null)
        {
          await Task.Factory.FromAsync(FTPClient.BeginConnect, FTPClient.EndConnect, state: null).ConfigureAwait(false);
        }
        return true;
      }
      catch (InvalidCastException ex)
      {
        return false;
        throw ex;
      }
    }
    /// <summary>
    /// Desconecta la conexion al FTP de manera asincrona
    /// </summary>
    /// <returns></returns>
    private async Task<Boolean> DesconectarseAsync()
    {
      try
      {
        if ((FTPClient) != null)
        {
          await Task.Factory.FromAsync(FTPClient.BeginDisconnect, FTPClient.EndDisconnect, state: null).ConfigureAwait(false);
        }
        return true;
      }
      catch (InvalidCastException ex)
      {
        return false;
        throw ex;
      }
    }
    #endregion
  }
}
