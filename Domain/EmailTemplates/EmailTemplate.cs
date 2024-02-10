using System;

namespace Domain.EmailTemplates;

public class EmailTemplate
{
    public EmailTemplateId EmailTemplateId { get; set; }
    public string Code { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
