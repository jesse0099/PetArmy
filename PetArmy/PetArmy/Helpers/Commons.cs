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
        public static string UpdateCampCastraMutation = @"mutation update_an_article($nombre_camp: String!, $descripcion: String!, $direccion: String!, $tel_contacto: String!, $id_campana: Int!) {
          update_camp_castracion_by_pk (
            pk_columns: {id_campana: $id_campana}, 
            _set: { nombre_camp: $nombre_camp
    				        descripcion: $descripcion
    				        direccion: $direccion
    				        tel_contacto: $tel_contacto}
          ) {
            id_campana
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
