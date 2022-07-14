using System;
using System.Collections.Generic;
using System.Text;

namespace PetArmy.Models
{
    public class Default_Images
    {
        public string imageName { get; set; }
        public string image { get; set; }

        public Default_Images()
        {
        }

        public Default_Images(string imageName, string image)
        {
            this.imageName = imageName;
            this.image = image;
        }
    }
}
