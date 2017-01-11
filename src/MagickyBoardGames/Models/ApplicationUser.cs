using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MagickyBoardGames.Models {
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IEntity {
        public virtual ICollection<GameOwner> GameOwners { get; set; }
    }
}