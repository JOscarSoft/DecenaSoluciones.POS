﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Dtos
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public required string Password { get; set; }
    }
}
