using System;
using System.Collections.Generic;
using System.Text;

namespace IzmPortal.Application.DTOs.MenuDocument;

public class UpdateMenuDocumentDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
}
