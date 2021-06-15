//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi2.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    
    public partial class Composition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Composition()
        {
            this.Playlists = new HashSet<Playlist>();
        }
    
        public int ID { get; set; }
        public string CompositionName { get; set; }
        public int GenreID { get; set; }
        public int PerformerID { get; set; }
        public int CompositorID { get; set; }
        public int RecordCompanyID { get; set; }
        public string MediaType { get; set; }
        public System.DateTime RecordDate { get; set; }
        public string Duration { get; set; }
        public string Views { get; set; }
        public string ImgUrl { get; set; }
    
        public virtual Compositor Compositor { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Performer Performer { get; set; }
        public virtual RecordCompany RecordCompany { get; set; }
        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}
