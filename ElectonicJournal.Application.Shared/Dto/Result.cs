using System.Collections.Generic;
using System.Linq;

namespace ElectronicJournal.Application.Dto
{
    public struct Result : IResult
    {
        public bool IsSuccessed { get; private set; }
        public IReadOnlyList<ErrorResult> Errors { get; private set; }

        internal Result(bool isSuccesed, List<ErrorResult> errors)
        {
            IsSuccessed = isSuccesed;
            Errors = errors;
        } 
        public static Result Success()
        {
            var result = new Result();
            result.IsSuccessed = true;
            result.Errors = new List<ErrorResult>();
            return result;
        }
        public static Result Failed(List<ErrorResult> errors)
        {
            var result = new Result();
            result.IsSuccessed = false;
            result.Errors = errors;
            return result;
        }
    }
    public struct Result<TResult> : IResult<TResult>
    {
        public bool IsSuccessed { get; private set; }
        public IReadOnlyList<ErrorResult> Errors { get; private set; }
        public TResult Value { get; private set; }
        public static Result<TResult> Success(TResult value)
        {
            var result = new Result<TResult>();
            result.Value = value;
            result.IsSuccessed = true;
            result.Errors = new List<ErrorResult>();
            return result;
        }
        public static Result<TResult> Failed(List<ErrorResult> errors)
        {
            var result = new Result<TResult>();
            result.Value = default;
            result.IsSuccessed = false;
            result.Errors = errors;
            return result;
        }
        public static Result<TResult> Failed(List<ErrorResult> errors, TResult value)
        {
            var result = new Result<TResult>();
            result.Value = value;
            result.IsSuccessed = false;
            result.Errors = errors;
            return result;
        }

        public static implicit operator Result(Result<TResult> resultGeneric)
        {
            var result = new Result(resultGeneric.IsSuccessed, resultGeneric.Errors.ToList());
            return result;
        }
        public static implicit operator Result<TResult>(Result result)
        {
            var genericResult = new Result<TResult>();
            genericResult.IsSuccessed = result.IsSuccessed;
            genericResult.Errors = result.Errors;
            genericResult.Value = default;
            return genericResult;
        }
    }
}
