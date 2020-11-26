using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicJournal.Application.Dto
{
    public interface IResult
    {
        bool IsSuccessed { get; }
        IReadOnlyList<ErrorResult> Errors { get; }
    }
    public interface IResult<TResult> : IResult
    {
        TResult Value { get; }
    }
}
