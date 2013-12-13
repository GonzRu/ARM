using System;
using System.Drawing;

namespace LibraryElements.Sources
{
    /// <summary>
    /// Данные изображения
    /// </summary>
    public class ImageData
    {
        public ImageData( Image image, String path )
        {
            this.Image = image;
            this.Path = path;
        }
        internal ImageData( ImageData original )
        {
            if ( original == null )
                return;

            this.Image = (Image)original.Image.Clone();
            this.Path = original.Path;
        }
        public Image Image { get; private set; }
        public String Path { get; private set; }
    }
}