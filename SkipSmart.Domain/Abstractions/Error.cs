﻿namespace SkipSmart.Domain.Abstractions;

public record Error(string Code, string Name) {
    public static Error None = new(string.Empty, string.Empty);
    
    public static Error NullValue = new("Error.NullValue", "The value cannot be null.");
}