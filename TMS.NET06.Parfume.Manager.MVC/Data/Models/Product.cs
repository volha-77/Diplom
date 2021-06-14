using System.Collections.Generic;

namespace TMS.NET06.Parfume.Manager.MVC.Data.Models
{
    public class Product
    {
        public Product()
        {
            if (Images == null)
                Images = new List<string>();

            if (ImagesSmall == null)
                ImagesSmall = new List<string>();
        }

        public int ProductId { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public Gender? Gender { get; set; }

        public int Volume { get; set; }

        public string Overview { get; set; }

        public List<string> Images { get; set; }

        public List<string> ImagesSmall { get; set; }


    }

    public enum Gender
    {
        men = 1,
        women
    }
}
