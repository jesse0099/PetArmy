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
    }
}
