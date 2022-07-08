using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PetArmy.Models
{
    public class CstmItemRefugio
    {
        public Refugio refugio { get; set; }
        public byte[] Image { get; set; }

        public CstmItemRefugio()
        {
        }

        public CstmItemRefugio(Refugio refugio, byte[] image)
        {
            this.refugio = refugio;
            Image = image;
        }
    }
}
