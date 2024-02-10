using System;

namespace Domain.EmailTemplates;

public class EmailTemplateId
{
    public Guid Value { get; set; }

    public EmailTemplateId(Guid value)
    {
        Value = value;
    }
}