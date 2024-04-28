using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tic.Shared.ApiDTOs
{
    public class MarksDTOs
    {
        public int MarkId { get; set; }

        public string MarkName { get; set; } = null!;

    }
}
