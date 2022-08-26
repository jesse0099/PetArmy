using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PetArmy.Helpers
{
    public static class Commons
    {
        public static string GetAdoptionRequest = @"query GetAdoptionRequests($uid: String!) {
          solicitudes_adopcion(where: {_and: [{ aprobacion: {_eq: false}, 
    																		        fecha_revision: {_eq: ""01/01/0001""},
  																			        refugio: {administrador: {_eq: $uid}}}]}) {
            adoptante
            aprobacion
            fecha_revision
            fecha_solicitud
            id_mascota
            id_refugio
            adoptanteInfo {
              correo
              nombre
            }
            mascota:mascotum {
              nombre
            }
          }
        }";

        public static byte[] StreamToByteArray(Stream input)
        {
            try
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static Stream GetImageSourceStream(ImageSource imgSource)
        {
            if (imgSource is StreamImageSource)
            {
                try
                {
                    StreamImageSource strImgSource = (StreamImageSource)imgSource;
                    System.Threading.CancellationToken cToken = System.Threading.CancellationToken.None;
                    Task<Stream> returned = strImgSource.Stream(cToken);
                    return returned.Result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
