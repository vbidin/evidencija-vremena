//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EvidencijaVremena
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipAktivnosti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipAktivnosti()
        {
            this.Aktivnost = new HashSet<Aktivnost>();
            this.Opterecenje = new HashSet<Opterecenje>();
        }
    
        public int ID { get; set; }
        public string Ime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Aktivnost> Aktivnost { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Opterecenje> Opterecenje { get; set; }
    }
}
