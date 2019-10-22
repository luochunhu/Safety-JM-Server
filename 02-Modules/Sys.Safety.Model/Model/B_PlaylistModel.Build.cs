using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("RA_PlayListMusicLink")]
    public partial class B_PlaylistModel
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
                public string PlayListCode
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string PlayListName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Bz1
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Bz2
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Bz3
        {
           get;
           set;
        }
            }
}

