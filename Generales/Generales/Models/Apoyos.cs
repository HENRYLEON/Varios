using System;
using System.Collections.Generic;


namespace Generales.Models
{
  public static class Apoyos
  {

    #region Servicios
    #region Generales
    public static string GetAssemblyDirectory() => Utilidades.GetAssemblyDirectory();
    public static Boolean DescomprimirArchivo(string rutaComprimido, string rutaDescompresion) => Utilidades.DescomprimirArchivo(rutaComprimido, rutaDescompresion);
    public static Boolean DescomprimirUnicoArchivo(string rutaComprimido, string rutaArchivo) => Utilidades.DescomprimirUnicoArchivo(rutaComprimido, rutaArchivo);
    public static Boolean ComprimirDirectorio(string rutaDirectorio, string rutaArchivo) => Utilidades.ComprimirDirectorio(rutaDirectorio, rutaArchivo);
    public static Boolean ComprimirArchivo(string rutaArchivoOriginal, string rutaArchivoNew) => Utilidades.ComprimirArchivo(rutaArchivoOriginal, rutaArchivoNew);
    public static Boolean EliminarArchivo(string RutaArchivo) => Utilidades.EliminarArchivo(RutaArchivo);
    public static Boolean EliminarArchivosDirectorio(string RutaDirectorio, List<string> Excluidos = null) => Utilidades.EliminarArchivosDirectorio(RutaDirectorio, Excluidos);
    public static Boolean EliminarDirectorio(string RutaDirectorio) => Utilidades.EliminarDirectorio(RutaDirectorio);
    public static Boolean CrearDirectorio(string RutaDirectorio) => Utilidades.CrearDirectorio(RutaDirectorio);
    public static Boolean CopiarArchivo(string RutaOrigen, string RutaDestino) => Utilidades.CopiarArchivo(RutaOrigen, RutaDestino);
    public static Boolean MoverArchivo(string RutaOrigen, string RutaDestino) => Utilidades.MoverArchivo(RutaOrigen, RutaDestino);
    public static long ObtenerPesoArchivo(string RutaArchivo) => Utilidades.ObtenerPesoArchivo(RutaArchivo);
    public static Boolean EsCorreoValido(string Correo) => Utilidades.EsCorreoValido(Correo);
    public static Byte[] SerializarBytes(string RutaArchivo) => Utilidades.SerializarBytes(RutaArchivo);
    public static Boolean DeserializarBytes(Byte[] Datos, string RutaArchivo) => Utilidades.DeserializarBytes(Datos, RutaArchivo);
    #endregion

    #endregion
  }
}
