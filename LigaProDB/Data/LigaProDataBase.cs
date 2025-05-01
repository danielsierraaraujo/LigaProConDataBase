using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LigaProDB.Models;

    public class LigaProDataBase : DbContext
    {
        public LigaProDataBase (DbContextOptions<LigaProDataBase> options)
            : base(options)
        {
        }

        public DbSet<LigaProDB.Models.Equipo> Equipo { get; set; } = default!;

public DbSet<LigaProDB.Models.Jugador> Jugador { get; set; } = default!;
    }
