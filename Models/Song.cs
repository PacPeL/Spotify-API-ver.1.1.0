using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify_API.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }

        // Los archivos ahora se guardan como texto Base64
        public string AudioBase64 { get; set; }
        public string ImageBase64 { get; set; }
    }
}

