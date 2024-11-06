using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyPlayer.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
