using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Autenticacao.Enums
{
    public enum Roles
    {
        Gestor = 0,
        Cliente = 1,
        Atendente = 2,
        ClienteSemCpf = 3,
    }
}
