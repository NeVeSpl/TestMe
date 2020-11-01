using System;

namespace TestMe.BuildingBlocks.App
{
    public class Result<T> : IResult
    {
        private static readonly Result<T> OkResult = new Result<T>(ResultStatus.Ok);
        private static readonly Result<T> UnauthorizedResult = new Result<T>(ResultStatus.Unauthorized);
        private static readonly Result<T> ErrorResult = new Result<T>(ResultStatus.Error);
        private static readonly Result<T> NotFoundResult = new Result<T>(ResultStatus.NotFound);
        private static readonly Result<T> ConflictResult = new Result<T>(ResultStatus.Conflict);

        public ResultStatus Status { get; }
        public T Value { get; }


        internal protected Result(ResultStatus status, T value = default)
        {
            Status = status;
            Value = value;
        }



        public object? GetValue()
        {
            return Value;
        }
        public bool HasValue()
        {
            return Value != null;
        }
        public virtual string GetError()
        {
            return String.Empty;
        }

        public static implicit operator Result<T>(Result result)
        {
            switch (result.Status)
            {
                case ResultStatus.Ok:
                    return OkResult;
                case ResultStatus.Error:
                    if (result is ErrorResult errorResult)
                    {
                        return new ErrorResult<T>(result.Status, errorResult.GetError());
                    }
                    return ErrorResult;
                case ResultStatus.Unauthorized:
                    return UnauthorizedResult;
                case ResultStatus.NotFound:
                    return NotFoundResult;
                case ResultStatus.Conflict:
                    return ConflictResult;
                default:
                    throw new NotImplementedException();
            }
        }
        public static implicit operator bool(Result<T> result)
        {
            return result.Status == ResultStatus.Ok;
        }       
    }

    internal class ErrorResult<T> : Result<T>
    {
        public string Error;


        public ErrorResult(ResultStatus status, string error) : base(status)
        {
            Error = error;
        }


        public override string GetError()
        {
            return Error;
        }
    }

    public class Result : IResult
    {
        private static readonly Result OkResult = new Result(ResultStatus.Ok );
        private static readonly Result UnauthorizedResult = new Result(ResultStatus.Unauthorized);
        private static readonly Result ErrorResult = new Result(ResultStatus.Error);
        private static readonly Result NotFoundResult = new Result(ResultStatus.NotFound);
        private static readonly Result ConflictResult = new Result(ResultStatus.Conflict);

        public ResultStatus Status { get;  }


        internal protected Result(ResultStatus status)
        {
            Status = status;
        }


        public bool HasValue()
        {
            return false;
        }
        public object? GetValue()
        {
            return null;
        }
        public virtual string GetError()
        {
            return String.Empty;
        }

        public static Result Ok()
        {
            return OkResult;
        }
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(ResultStatus.Ok, value);
        }
        public static Result Unauthorized()
        {
            return UnauthorizedResult;
        }
        public static Result Error()
        {
            return ErrorResult;
        }
        public static Result Error(string error)
        {
            return new ErrorResult(ResultStatus.Error, error);
        }
        public static Result Error(IResult result)
        {
            return new ErrorResult(result.Status, result.GetError());
        }
        public static Result NotFound()
        {
            return NotFoundResult;
        }
        public static Result Conflict()
        {
            return ConflictResult;
        }
        public static Result<T> Conflict<T>(T value)
        {
            return new Result<T>(ResultStatus.Conflict, value);
        }       
    }

    internal class ErrorResult : Result
    {
        private readonly string error;


        public ErrorResult(ResultStatus status, string error) : base(status)
        {
            this.error = error;
        }


        public override string GetError()
        {
            return error;
        }
    }
}