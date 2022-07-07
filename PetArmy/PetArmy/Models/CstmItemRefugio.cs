using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PetArmy.Models
{
    public class CstmItemRefugio
    {
        public Refugio refugio { get; set; }
        public List<Imagen_refugio> imagenes_Refugio { get; set; }
        public List<MediaFileModel> media_Refugios { get; set; }
        public MediaFileModel defaultImg { get; set; }

        public CstmItemRefugio()
        {
        }
   
    }
}
