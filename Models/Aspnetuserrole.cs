﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Models;

[PrimaryKey("Userid", "Roleid")]
[Table("aspnetuserroles")]
[Index("Userid", Name = "fki_fk_UserId")]
public partial class Aspnetuserrole
{
    [Key]
    [Column("userid")]
    [StringLength(128)]
    public string Userid { get; set; } = null!;

    [Key]
    [Column("roleid")]
    [StringLength(128)]
    public string Roleid { get; set; } = null!;

    [ForeignKey("Userid")]
    [InverseProperty("Aspnetuserroles")]
    public virtual Aspnetuser User { get; set; } = null!;
}
