using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("b_playlistmusiclink")]
    public partial class B_PlaylistmusiclinkModel
    {
        	    /// <summary>
        /// 
        /// </summary>
                [Key]
                public string Id
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string PlayListId
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string MusicId
        {
           get;
           set;
        }
            }
}

