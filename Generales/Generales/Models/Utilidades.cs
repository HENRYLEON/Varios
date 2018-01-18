/*
 HENRY LEONARDO LEON FAJARDO 
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Generales.Models
{
  internal static class Utilidades
  {
    #region servicios
    internal static string GetAssemblyDirectory()
    {
      string codebase = Assembly.GetExecutingAssembly().CodeBase;
      UriBuilder uri1 = new UriBuilder(codebase);
      string path2 = Uri.UnescapeDataString(uri1.Path);
      path2 = path2.Replace(@"/", @"\");
      return path2.Substring(0, path2.IndexOf(@"\") + 1);
    }
    /// <summary>
    /// Descomprime el contenido de un archivo
    /// </summary>
    /// <param name="rutaComprimido">Ruta del archivo comprimido</param>
    /// <param name="rutaDescompresion">Ruta donde se quiere descomprimir</param>
    /// <Autor>Henry Leonardo Leon Fajardo</Autor>
    /// <returns>Estado Boolean</returns>
    internal static Boolean DescomprimirArchivo(string rutaComprimido, string rutaDescompresion)
    {
      int count=1;
      Boolean estado;
      try
      {
        estado = true;
        if (File.Exists(rutaComprimido))
        {
          if (!(Directory.Exists(rutaDescompresion)))
          {
            Directory.CreateDirectory(rutaDescompresion);
          }
          using (ZipArchive archivo = ZipFile.OpenRead(rutaComprimido))
            foreach (ZipArchiveEntry entry in archivo.Entries) {
              //if (!(File.Exists(RutaDescompresion + "/" + entry.FullName)))
              count = 1;
              if (File.Exists(rutaDescompresion + "/" + entry.FullName))
              {
                do { count++; } while (File.Exists(rutaDescompresion + "/" + entry.FullName.Replace(Path.GetExtension(entry.FullName), $"({count}){Path.GetExtension(entry.FullName)}")));
                entry.ExtractToFile(Path.Combine(rutaDescompresion, entry.FullName.Replace(Path.GetExtension(entry.FullName), $"({count}){Path.GetExtension(entry.FullName)}")));
              }
              else
              {
                entry.ExtractToFile(Path.Combine(rutaDescompresion, entry.FullName));
              }
              
            }
        }
        else
        {
          estado = false;
          throw new Exception("El archivo no existe");
        }

      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;
    }
    /// <summary>
    /// Descomprime el contenido de un archivo en una ruta especifica
    /// </summary>
    /// <param name="rutaComprimido">Ruta del archivo comprimido</param>
    /// <param name="rutaArchivo">Ruta del archivo donde descomprimir</param>
    /// <Autor>Henry Leonardo Leon Fajardo</Autor>
    /// <returns>Estado Boolean</returns>
    internal static Boolean DescomprimirUnicoArchivo(string rutaComprimido, string rutaArchivo)
    {
      Boolean estado;
      string nombreArchivo;
      try
      {
        estado = true;
        nombreArchivo = Path.GetFileName(rutaArchivo);
        if (File.Exists(rutaComprimido))
        {
          using (ZipArchive archivo = ZipFile.Open(rutaComprimido, ZipArchiveMode.Update))
          {
            ZipArchiveEntry entry = archivo.GetEntry(nombreArchivo);

            if (File.Exists(rutaArchivo))
            {
              File.Delete(rutaArchivo);
            }
            entry.ExtractToFile(Path.Combine(Path.GetDirectoryName(rutaArchivo) + "/", entry.FullName), true);
          }
        }
        else
        {
          estado = false;
          throw new Exception("El archivo no existe");
        }
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;
    }
    /// <summary>
    /// Comprime un directorio en un archivo
    /// </summary>
    /// <param name="rutaDirectorio">Ruta del directorio</param>
    /// <param name="rutaArchivo">Ruta del archivo comprimido</param>
    /// <Autor>Henry Leonardo Leon Fajardo</Autor>
    /// <returns>Estado Boolean</returns>
    internal static Boolean ComprimirDirectorio(string rutaDirectorio, string rutaArchivo)
    {
      Boolean estado;
      int count = 1;
      try
      {
        estado = true;
        if (Directory.Exists(rutaDirectorio))
        {
          if (File.Exists(rutaArchivo))
          {
            do { count++; } while (File.Exists(rutaArchivo.Replace(Path.GetExtension(rutaArchivo), $"({count}){Path.GetExtension(rutaArchivo)}")));
            rutaArchivo = rutaArchivo.Replace(Path.GetExtension(rutaArchivo), $"({count}){Path.GetExtension(rutaArchivo)}");
          }
          ZipFile.CreateFromDirectory(rutaDirectorio, rutaArchivo);
                    
                              
        }
        else
        {
          estado = false;
          throw new Exception("El directorio " + rutaDirectorio + " no existe");
        }
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;
    }
        internal static Boolean ComprimirArchivo (string rutaArchivoOriginal, string rutaArchivoNew)
        {
            Boolean estado;
            try
            {
                estado = true;
                if (File.Exists(rutaArchivoOriginal))
                {
  
                        using (ZipArchive archivo =  ZipFile.Open(rutaArchivoNew, ZipArchiveMode.Create))
                        {
                        archivo.CreateEntryFromFile(rutaArchivoOriginal, rutaArchivoNew);
                        }
                }
                else
                {
                    estado = false;
                    throw new Exception("El directorio " + rutaArchivoOriginal + " no existe");
                }
            }
            catch (InvalidCastException ex)
            {
                estado = false;
                throw ex;
            }
            return estado;
        }
    /// <summary>
    /// Eliminar un archivo especifico
    /// </summary>
    /// <param name="RutaArchivo">Ruta del archivo a eliminar</param>
    /// <Autor>Henry Leonardo Leon Fajardo</Autor>
    /// <returns>Estado Boolean</returns>
    internal static Boolean EliminarArchivo(string RutaArchivo)
    {
      Boolean estado;
      try
      {
        estado = true;
        if (File.Exists(RutaArchivo))
        {
          File.Delete(RutaArchivo);
        }
        else
        {
          estado = false;
          throw new Exception("El archivo " + RutaArchivo + " no existe");
        }
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;
    }
    /// <summary>
    /// Elimina algun(nos) arvhivo(s) en un directorio especifico
    /// </summary>
    /// <param name="RutaDirectorio">Ruta del directorio</param>
    /// <param name="Excluidos">Archivo excluidos de la eliminacion</param>
    /// <Autor>Henry Leonardo Leon Fajardo</Autor>
    /// <returns>Estado Boolean</returns>
    internal static Boolean EliminarArchivosDirectorio(string RutaDirectorio, List<string> Excluidos = null)
    {
      Boolean estado;
      try
      {
        estado = true;
        if (Directory.Exists(RutaDirectorio))
        {
          foreach (string archivo in Directory.GetFiles(RutaDirectorio))
          {
            if ((Excluidos != null) || (Excluidos.Contains(archivo)))
            {
              File.Delete(archivo);
            }
          }
        }
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;
    }
    /// <summary>
    /// Elimina un directorio
    /// </summary>
    /// <param name="RutaDirectorio">Ruta del directorio a eliminar</param>
    /// <Autor>Henry Leonardo Leon Fajardo</Autor>
    /// <returns>Estado Boolean</returns>
    internal static Boolean EliminarDirectorio(string RutaDirectorio)
    {
      Boolean estado;
      try
      {
        estado = true;
        if (Directory.Exists(RutaDirectorio))
        {
          Directory.Delete(RutaDirectorio);
        }
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;
    }
    /// <summary>
    /// Crea un directorio 
    /// </summary>
    /// <param name="RutaDirectorio">Ruta del directorio a crear</param>
    /// <Autor>Henry Leonardo Leon Fajardo</Autor>
    /// <returns>Estado Boolean</returns>
    internal static Boolean CrearDirectorio(string RutaDirectorio)
    {
      Boolean estado;
      try
      {
        estado = true;
        if (!(Directory.Exists(RutaDirectorio)))
        {
          Directory.CreateDirectory(RutaDirectorio);
        }
      }
      catch (UnauthorizedAccessException exU)
      {
        estado = false;
        throw exU;
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;
    }
    /// <summary>
    /// Copia un archivo
    /// </summary>
    /// <param name="RutaOrigen">Ruta del archivo</param>
    /// <param name="RutaDestino">Ruta destino del archivo</param>
    /// <Autor>Henry Leonardo Leon Fajardo</Autor>
    /// <returns>Estado Boolean</returns>
    internal static Boolean CopiarArchivo(string RutaOrigen, string RutaDestino)
    {
      Boolean estado;
      string rutaDestino;
      try
      {
        rutaDestino = RutaDestino;
        estado = true;
        if (File.Exists(RutaOrigen))
        {
          rutaDestino = RutaDestino.Split('/')[0] + '/' + Path.GetFileName(RutaDestino);
          File.Copy(RutaOrigen, rutaDestino);
        }
        else
        {
          estado = false;
          throw new Exception("El archivo " + RutaOrigen + " no existe");
        }
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;
    }
    /// <summary>
    /// Mueve un archivo
    /// </summary>
    /// <param name="RutaOrigen">Ruta del archivo</param>
    /// <param name="RutaDestino">Nueva ruta para el archivo</param>
    /// <Autor>Henry Leonardo Leon Fajardo</Autor>
    /// <returns>Estado Boolean</returns>
    internal static Boolean MoverArchivo(string RutaOrigen, string RutaDestino)
    {
      Boolean estado;
      try
      {
        estado = true;
        if (File.Exists(RutaOrigen) & !(File.Exists(RutaDestino)))
        {
          File.Move(RutaOrigen, RutaDestino);
        }
        else
        {
          estado = false;
          throw new Exception("El archivo " + RutaOrigen + " no existe o Existe en: " + RutaDestino);
        }
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;
    }
    /// <summary>
    /// Obtiene el tamaño del archivo
    /// </summary>
    /// <param name="RutaArchivo">Ruta del archivo</param>
    /// <Autor>Henry Leonardo Leon Fajardo</Autor>
    /// <returns>Peso Log</returns>
    internal static long ObtenerPesoArchivo(string RutaArchivo)
    {
      FileInfo fileInfo;
      long peso = 0L;
      try
      {
        if (File.Exists(RutaArchivo))
        {
          fileInfo = new FileInfo(RutaArchivo);
          peso = fileInfo.Length;
        }
        else
        {
          throw new Exception("El archivo " + RutaArchivo + " no existe ");
        }
      }
      catch (InvalidCastException ex)
      {
        throw ex;
      }
      return peso;
    }
    /// <summary>
    /// alida si un texto es una direccion de correo valida
    /// </summary>
    /// <param name="Correo">Correo para validar</param>
    /// <Autor>Henry Leonardo Leon Fajardo</Autor>
    /// <returns>Verdadero si es valido, falso en caso contrario</returns>
    internal static Boolean EsCorreoValido(string Correo)
    {
      if (string.IsNullOrEmpty(Correo))
        return true;
      try
      {
        return Regex.IsMatch(Correo,
              @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
              @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
              RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
      }
      catch (RegexMatchTimeoutException)
      {
        return false;
      }
    }
    /// <summary>
    /// Convierte un archivo a un arreglo de bytes
    /// </summary>
    /// <param name="RutaArchivo">Ruta del archivo a serializar</param>
    /// <Autor>Henry Leonardo Leon Fajardo</Autor>
    /// <returns>Arreglo de bytes del archivo</returns>
    internal static Byte[] SerializarBytes(string RutaArchivo)
    {
      Byte[] datos = null;
      try
      {
        if (File.Exists(RutaArchivo))
        {
          datos = File.ReadAllBytes(RutaArchivo);
        }
      }
      catch (InvalidCastException ex)
      {
        throw ex;
      }
      return datos;
    }
    /// <summary>
    /// Convierte el contenido de un arreglo de bytes en un archivo en una ruta
    /// </summary>
    /// <param name="Datos">Arreglo de bytes</param>
    /// <param name="RutaArchivo">Ruta del archivo</param>
    /// <Autor>Henry Leonardo Leon Fajardo</Autor>
    /// <returns>estado boolean</returns>
    internal static Boolean DeserializarBytes(Byte[] Datos, string RutaArchivo)
    {
      Boolean estado;
      try
      {
        estado = true;
        if (!(Directory.Exists(Path.GetDirectoryName(RutaArchivo))))
        {
          Directory.CreateDirectory(Path.GetDirectoryName(RutaArchivo));
        }
        File.WriteAllBytes(RutaArchivo, Datos);
      }
      catch (InvalidCastException ex)
      {
        estado = false;
        throw ex;
      }
      return estado;
    }
    #endregion
  }
}
