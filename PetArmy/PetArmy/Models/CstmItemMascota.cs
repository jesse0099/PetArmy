using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class CstmItemMascota
    {
        public Mascota mascota { get; set; }

        public byte[] Image { get; set; }
        public Imagen_Mascota imgMascota { get; set; }

        public CstmItemMascota()
        { 
        }

        public CstmItemMascota(Mascota mascota, byte[] image)
        {
            this.mascota = mascota;
            Image = image;
        }
    }
}
