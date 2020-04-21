using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AviFav_.Config
{
    public class SavedAvi
    {
        private string name = "";
        private string avatarID = "";
        private string thumbnailImageUrl = "";

        public string ThumbnailImageUrl { get => thumbnailImageUrl; set => thumbnailImageUrl = value; }
        public string AvatarID { get => avatarID; set => avatarID = value; }
        public string Name { get => name; set => name = value; }
    }
}
