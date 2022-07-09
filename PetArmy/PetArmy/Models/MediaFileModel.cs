using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;
using Xamarin.Forms;

namespace PetArmy.Models
{
    public class MediaFileModel
    {

        public string PreviewPath { get; set; }
        public ImageSource Path { get; set; }
        public MetafileType Type { get; set; }
        public byte[] imgByte { get; set; }


        public MediaFileModel()
        {
        }

        public MediaFileModel(string previewPath, ImageSource path, MetafileType type, byte[] imgByte)
        {
            PreviewPath = previewPath;
            Path = path;
            Type = type;
            this.imgByte = imgByte;
        }
    }
}
