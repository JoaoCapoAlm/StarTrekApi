﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public abstract class People
    {
        public string Name { get; set; }
        public DateOnly? DeathDate { get; set; }
    }
}
