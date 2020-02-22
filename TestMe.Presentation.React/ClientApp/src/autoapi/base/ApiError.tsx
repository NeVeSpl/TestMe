import {ErrorCode, ProblemDetails } from '.';


export class ApiError extends Error
{
    readonly errorCode: ErrorCode;
    readonly traceId: string | undefined;
    readonly detail: string | undefined;
    readonly errors: object & { [key: string]: string };

    constructor(errorCode: ErrorCode, problem?: ProblemDetails, ...params: any[]) 
    {       
        super(...params);

        // Maintains proper stack trace for where our error was thrown (only available on V8)
        if (Error.captureStackTrace) 
        {
            Error.captureStackTrace(this, ApiError);
        }

        this.errors = {};

        this.errorCode = errorCode;
        if (problem !== undefined)
        {
            this.traceId = problem!.traceId;
            this.detail = problem!.detail;

            if (this.errorCode === ErrorCode.Conflict)
            {
                this.detail = "Someone has done changes before you. The conflict was automatically resolved. Please review changes and redo operation.";
            }

            if (problem.errors !== undefined)
            {
                for (const key in problem.errors)
                {
                    this.errors[this.camalize(key)] = problem.errors[key][0];
                }
            }
        }
    }

    camalize(str : string)
    {
        return str.charAt(0).toLowerCase() + str.slice(1);
    }
}