using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

public class StringTypeReader : TypeReader
{
    public override Task<TypeReaderResult> Read(ICommandContext context, string input, IServiceProvider services)
    {
        return Task.FromResult(TypeReaderResult.FromSuccess(input));
    }
}